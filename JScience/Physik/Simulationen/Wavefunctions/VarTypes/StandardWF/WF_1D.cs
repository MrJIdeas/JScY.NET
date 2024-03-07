using Cloo;
using JScience.Enums;
using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Classes;
using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using ScottPlot;
using System;
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

        public WF_1D(WFInfo wfinfo, ECalculationMethod Method)
        {
            WFInfo = wfinfo;
            myPlot = new Plot();
            field = new Complex[wfinfo.DimX * wfinfo.DimY * wfinfo.DimZ];
            Boundary = wfinfo.BoundaryInfo;
            rangePartitioner = Partitioner.Create(0, wfinfo.DimX * wfinfo.DimY * wfinfo.DimZ);
            CalcMethod = Method;
            result = new double[field.Length];
            if (wfinfo.CabExits != null && wfinfo.CabExits.Count > 0)
                CabExits = wfinfo.CabExits;
            else
                CabExits = new Dictionary<string, IWavefunction>();
            CabCalc = new Dictionary<string, Complex>();
        }

        public OrderablePartitioner<Tuple<int, int>> rangePartitioner { get; private set; }
        public Complex[] field { get; private set; }

        protected Dictionary<string, IWavefunction> CabExits { get; private set; }

        private Dictionary<string, Complex> CabCalc { get; set; }

        private double[] result { get; set; }

        #region Interface

        public ELatticeBoundary Boundary { get; private set; }

        public Complex this[int i] { get => field[i]; protected set => field[i] = value; }
        public int DimX => field.Length;
        public int Dimensions => 1;

        public ECalculationMethod CalcMethod { get; private set; }

        public double Norm()
        {
            switch (CalcMethod)
            {
                case ECalculationMethod.CPU_Multihreading:
                    return field.ToList().AsParallel().Sum(x => Math.Pow(x.Magnitude, 2));

                case ECalculationMethod.OpenCL:
                    {
                        if (IWavefunction.bBuffer == null)
                            IWavefunction.bBuffer = new ComputeBuffer<Complex>(IWavefunction.context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, field);
                        else
                            IWavefunction.queue.WriteToBuffer(field, IWavefunction.bBuffer, true, null);
                        if (IWavefunction.resultBuffer == null)
                            IWavefunction.resultBuffer = new ComputeBuffer<double>(IWavefunction.context, ComputeMemoryFlags.WriteOnly, result.Length);
                        else
                            IWavefunction.queue.WriteToBuffer(result, IWavefunction.resultBuffer, true, null);

                        IWavefunction.Normkernel.SetMemoryArgument(0, IWavefunction.resultBuffer);
                        IWavefunction.Normkernel.SetMemoryArgument(1, IWavefunction.bBuffer);

                        IWavefunction.queue.Execute(IWavefunction.Normkernel, null, new long[] { field.Length }, null, null);
                        double[] buf = result;
                        IWavefunction.queue.ReadFromBuffer(IWavefunction.resultBuffer, ref buf, true, null);
                        result = buf;
                        return result.Sum();
                    }

                default:
                    return field.ToList().Sum(x => Math.Pow(x.Magnitude, 2));
            }
        }

        protected virtual IWavefunction getEmptyLikeThis() => (IWavefunction)Activator.CreateInstance(GetType(), WFInfo, CalcMethod);

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
            WF_1D neu = new WF_1D(WFInfo, CalcMethod);
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

        public System.Drawing.Image GetCabExitImage(int width, int height)
        {
            myPlot.Clear();
            IWavefunction super = CabExits.Values.First();
            if (super == null)
                return null;
            for (int i = 1; i < CabExits.Count; i++)
                super += CabExits.Values.ElementAt(i);
            List<double> x = new List<double>();
            for (int i = 0; i < DimX; i++)
                x.Add(i);
            List<double> y = new List<double>();
            for (int i = 0; i < DimX; i++)
                y.Add((double)super.getNorm(i));
            myPlot.Add.Bars(x, y);
            var img = System.Drawing.Image.FromStream(new MemoryStream(myPlot.GetImage(width, height).GetImageBytes()));
            return img;
        }

        public void AddCabExitAuto()
        {
            int startx = WFInfo.GetAdditionalInfo<int>("startX");
            if (startx <= 0)
                throw new Exception("Not enough Data to Auto Set Cab Exits!");
            AddCabExit(startx);
            AddCabExit(DimX - startx);
            WFInfo.CabExits = CabExits;
        }

        public void AddExit(string ExitName, IWavefunction exitWF)
        {
            if (!CabExits.ContainsKey(ExitName))
                CabExits.Add(ExitName, exitWF);
        }

        private void AddCabExit(int x)
        {
            IWavefunction clone;
            switch (WFInfo.waveType)
            {
                default:
                    throw new Exception("Not enough Data to Auto Set Cab Exits!");

                case EWaveType.Delta:
                    clone = (WF_1D)WFCreator.CreateDelta(DimX, x, Boundary, CalcMethod);
                    break;

                case EWaveType.Gauß:
                    double k = WFInfo.GetAdditionalInfo<double>("k");
                    double sigma = WFInfo.GetAdditionalInfo<double>("sigma");
                    clone = (WF_1D)WFCreator.CreateGaußWave(k, sigma, DimX, x, Boundary, CalcMethod);
                    break;
            }
            if (clone != null)
                CabExits.Add(x.ToString(), clone);
        }

        public Dictionary<string, Complex> CalcCab()
        {
            CabCalc.Clear();
            foreach (var exit in CabExits)
            {
                Complex calc = new Complex();
                foreach (var item in (exit.Value * this).field)
                    calc += item;
                CabCalc.Add(exit.Key, calc);
            }
            return CabCalc;
        }

        #endregion Interface
    }
}