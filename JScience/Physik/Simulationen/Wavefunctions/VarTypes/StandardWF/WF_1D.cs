using JScience.Mathe.ComplexNumbers.VarTypes;
using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using ScottPlot;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JScience.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF
{
    public class WF_1D : IWF_1D
    {
        protected Plot myPlot { get; set; }

        public WFInfo WFInfo { get; private set; }

        public WF_1D(WFInfo wfinfo)
        {
            WFInfo = wfinfo;
            myPlot = new Plot();
            field = new DecComplex[wfinfo.DimX * wfinfo.DimY * wfinfo.DimZ];
            Boundary = wfinfo.BoundaryInfo;
            rangePartitioner = Partitioner.Create(0, wfinfo.DimX * wfinfo.DimY * wfinfo.DimZ);
        }

        public OrderablePartitioner<Tuple<int, int>> rangePartitioner { get; private set; }
        private DecComplex[] field { get; set; }

        #region Interface

        public ELatticeBoundary Boundary { get; private set; }

        public DecComplex this[int i] { get => field[i]; protected set => field[i] = value; }
        public int DimX => field.Length;
        public int Dimensions => 1;

        public decimal Norm() => field.ToList().AsParallel().Sum(x => x.Magnitude);

        protected virtual IWavefunction getEmptyLikeThis() => (IWavefunction)Activator.CreateInstance(GetType(), WFInfo);

        public IWavefunction Conj()
        {
            IWavefunction conj = getEmptyLikeThis();
            Parallel.ForEach(conj.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    conj.SetField(i, field[i].Conj());
            });
            return conj;
        }

        public IWavefunction GetShift(EShift shift)
        {
            WF_1D neu = new WF_1D(WFInfo);
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

        public void SetField(int x, DecComplex value) => field[x] = value;

        public void Clear()
        {
            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    field[i] = DecComplex.Zero;
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

        public decimal getNorm(int x) => field[x].Magnitude;

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