using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using ScottPlot;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Threading.Tasks;

namespace JScience.Physik.Simulationen.Wavefunctions.VarTypes
{
    public class WF_1D : IWF_1D
    {
        public WF_1D(int DimX, ELatticeBoundary boundary)
        {
            field = new Complex[DimX];
            Boundary = boundary;
            rangePartitioner = Partitioner.Create(0, DimX);
        }

        public OrderablePartitioner<Tuple<int, int>> rangePartitioner { get; private set; }
        private Complex[] field { get; set; }

        #region Interface

        public ELatticeBoundary Boundary { get; private set; }

        public Complex this[int x] => field[x];
        public int DimX => field.Length;
        public int Dimensions => 1;

        public double Norm() => field.ToList().AsParallel().Sum(x => (x * Complex.Conjugate(x)).Real);

        public IWavefunction Conj()
        {
            WF_1D conj = new WF_1D(DimX, Boundary);
            Parallel.ForEach(conj.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    conj.SetField(i, Complex.Conjugate(field[i]));
            });
            return conj;
        }

        public IWavefunction GetShift(EShift shift)
        {
            WF_1D neu = new WF_1D(DimX, Boundary);
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
                        neu.field[neu.field.Length - 1] = field[0];
                    else
                        neu.field[neu.field.Length - 1] = Complex.Zero;
                    return neu;

                case EShift.Xp:
                    Parallel.For(1, neu.DimX, (i) =>
                    {
                        neu.field[i] = field[i - 1];
                    });
                    if (Boundary == ELatticeBoundary.Periodic)
                        neu.field[0] = field[field.Length - 1];
                    else
                        neu.field[0] = Complex.Zero;
                    return neu;
            }
        }

        public void SetField(int x, Complex value) => field[x] = value;

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
            WF_1D conj = new WF_1D(DimX, Boundary);
            Parallel.ForEach(conj.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    conj.field[i] = field[i];
            });
            return conj;
        }

        public double getNorm(int x) => (Complex.Conjugate(field[x]) * field[x]).Real;

        public Image GetImage(int width, int height)
        {
            List<double> x = new List<double>();
            for (int i = 0; i < DimX; i++)
                x.Add(i);
            List<double> y = new List<double>();
            for (int i = 0; i < DimX; i++)
                y.Add(getNorm(i));
            Plot myPlot = new Plot();

            myPlot.Add.SignalXY(x.ToArray(), y.ToArray());
            return myPlot.GetImage(width, height);
        }

        #endregion Interface
    }
}