using Cloo;
using FFmpeg.AutoGen;
using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using ScottPlot;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace JScience.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF
{
    public class WF_1D : IWF_1D
    {
        protected Plot myPlot { get; set; }

        public WFInfo WFInfo { get; private set; }

        public WF_1D(WFInfo wfinfo, bool useGPU)
        {
            WFInfo = wfinfo;
            myPlot = new Plot();
            field = new Complex[wfinfo.DimX * wfinfo.DimY * wfinfo.DimZ];
            Boundary = wfinfo.BoundaryInfo;
            rangePartitioner = Partitioner.Create(0, wfinfo.DimX * wfinfo.DimY * wfinfo.DimZ);
            UseGPU = useGPU;
            result = new double[field.Length];
        }

        public OrderablePartitioner<Tuple<int, int>> rangePartitioner { get; private set; }
        public Complex[] field { get; private set; }

        private double[] result { get; set; }

        #region Interface

        public ELatticeBoundary Boundary { get; private set; }

        public Complex this[int i] { get => field[i]; protected set => field[i] = value; }
        public int DimX => field.Length;
        public int Dimensions => 1;

        public bool UseGPU { get; private set; }

        public double Norm()
        {
            if (!UseGPU)
                return field.ToList().AsParallel().Sum(x => Math.Pow(x.Magnitude, 2));
            else
            {
                if (IWavefunction.bBuffer == null)
                    IWavefunction.bBuffer = new ComputeBuffer<Complex>(IWavefunction.context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, field);
                else
                    IWavefunction.queue.WriteToBuffer(field, IWavefunction.bBuffer, true, null);
                if (IWavefunction.resultBuffer == null)
                    IWavefunction.resultBuffer = new ComputeBuffer<double>(IWavefunction.context, ComputeMemoryFlags.WriteOnly, result.Length);
                else
                    IWavefunction.queue.WriteToBuffer<double>(result, IWavefunction.resultBuffer, true, null);

                IWavefunction.Normkernel.SetMemoryArgument(0, IWavefunction.resultBuffer);
                IWavefunction.Normkernel.SetMemoryArgument(1, IWavefunction.bBuffer);

                IWavefunction.queue.Execute(IWavefunction.Normkernel, null, new long[] { field.Length }, null, null);
                double[] buf = result;
                IWavefunction.queue.ReadFromBuffer(IWavefunction.resultBuffer, ref buf, true, null);
                result = buf;
                return result.Sum();
            }
        }

        protected virtual IWavefunction getEmptyLikeThis() => (IWavefunction)Activator.CreateInstance(GetType(), WFInfo, UseGPU);

        public IWavefunction Conj()
        {
            IWavefunction conj = getEmptyLikeThis();
            Parallel.ForEach(conj.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    conj.SetField(i, Complex.Conjugate(field[i]));
            });
            return conj;
        }

        public IWavefunction GetShift(EShift shift)
        {
            WF_1D neu = new WF_1D(WFInfo, UseGPU);
            switch (shift)
            {
                default:
                    return null;

                case EShift.Xm:
                    Parallel.For(0, neu.DimX - 1, (i) =>
                    {
                        neu.field[i] = field[i + 1];
                    });
                    if (Boundary == ELatticeBoundary.Periodic)
                        neu.field[neu.DimX - 1] = field[0];
                    return neu;

                case EShift.Xp:
                    Parallel.For(1, neu.DimX, (i) =>
                    {
                        neu.field[i] = field[i - 1];
                    });
                    if (Boundary == ELatticeBoundary.Periodic)
                        neu.field[0] = field[DimX - 1];
                    return neu;
            }
        }

        public void SetField(int x, Complex value) => field[x] = value;

        public void SetField(Complex[] field) => this.field = field;

        public void Clear()
        {
            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    field[i] = Complex.Zero;
            });
        }

        public IWavefunction Clone()
        {
            IWavefunction conj = getEmptyLikeThis();
            Parallel.ForEach(conj.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    conj.SetField(i, field[i]);
            });
            return conj;
        }

        public double getNorm(int x) => field[x].Magnitude;

        public System.Drawing.Image GetImage(int width, int height)
        {
            myPlot.Clear();
            List<double> x = new List<double>();
            for (int i = 0; i < DimX; i++)
                x.Add(i);
            List<double> y = new List<double>();
            for (int i = 0; i < DimX; i++)
                y.Add((double)getNorm(i));
            myPlot.Add.Bars(x, y);
            var img = System.Drawing.Image.FromStream(new MemoryStream(myPlot.GetImage(width, height).GetImageBytes()));
            return img;
        }

        #endregion Interface
    }
}