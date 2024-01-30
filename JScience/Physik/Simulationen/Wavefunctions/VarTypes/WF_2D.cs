using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using ScottPlot;
using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Threading.Tasks;

namespace JScience.Physik.Simulationen.Wavefunctions.VarTypes
{
    public class WF_2D : IWF_2D
    {
        public WF_2D(int DimX, int DimY, ELatticeBoundary boundary)
        {
            Boundary = boundary;
            this.DimX = DimX;
            this.DimY = DimY;
            field = new Complex[DimX * DimY];
            rangePartitioner = Partitioner.Create(0, field.Length);
        }

        public OrderablePartitioner<Tuple<int, int>> rangePartitioner { get; private set; }
        public ELatticeBoundary Boundary { get; private set; }

        private Complex[] field { get; set; }

        public Complex this[int x, int y]
        {
            get => field[x + y * DimX];
            set => field[x + y * DimX] = value;
        }

        public Complex this[int i] { get => field[i]; set => field[i] = value; }
        public int DimX { get; private set; }

        public int DimY { get; private set; }

        #region Interface

        public int Dimensions => 1;

        public double Norm()
        {
            double erg = 0;
            for (int i = 0; i < DimX; i++)
                for (int j = 0; j < DimY; j++)
                    erg += getNorm(i, j);
            return erg;
        }

        public IWavefunction Conj()
        {
            WF_2D conj = (WF_2D)Clone();
            Parallel.ForEach(conj.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    conj.field[i] = Complex.Conjugate(conj.field[i]);
            });
            return conj;
        }

        public IWavefunction GetShift(EShift shift)
        {
            WF_2D neu = new WF_2D(DimX, DimY, Boundary);
            Complex[] buf;
            switch (shift)
            {
                default:
                    return null;

                case EShift.Xm:
                    Parallel.For(0, neu.DimY - 1, (j) =>
                    {
                        for (int i = 0; i < neu.DimX - 1; i++)
                        {
                            neu[i, j] = this[i + 1, j];
                        }
                        if (Boundary == ELatticeBoundary.Periodic)
                            neu[neu.DimX - 1, j] = this[0, j];
                        else
                            neu[neu.DimX - 1, j] = Complex.Zero;
                    });
                    return neu;

                case EShift.Xp:
                    Parallel.For(0, neu.DimY - 1, (j) =>
                    {
                        for (int i = neu.DimX - 1; i > 0; i--)
                        {
                            neu[i, j] = this[i - 1, j];
                        }
                        if (Boundary == ELatticeBoundary.Periodic)
                            neu[0, j] = this[DimX - 1, j];
                        else
                            neu[0, j] = Complex.Zero;
                    });
                    return neu;

                case EShift.Ym:
                    Parallel.For(0, DimX, (j) =>
                    {
                        for (int i = 0; i < neu.DimY - 1; i++)
                        {
                            neu[j, i] = this[j, i + 1];
                        }
                        if (Boundary == ELatticeBoundary.Periodic)
                            neu[j, neu.DimY - 1] = this[j, 0];
                        else
                            neu[j, neu.DimY - 1] = Complex.Zero;
                    });
                    return neu;

                case EShift.Yp:
                    Parallel.For(0, DimX, (j) =>
                    {
                        for (int i = neu.DimY - 1; i > 0; i--)
                        {
                            neu[j, i] = this[j, i - 1];
                        }
                        if (Boundary == ELatticeBoundary.Periodic)
                            neu[j, 0] = this[j, DimY - 1];
                        else
                            neu[j, 0] = Complex.Zero;
                    });
                    return neu;
            }
        }

        public void SetField(int x, int y, Complex value) => this[x, y] = value;

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
            WF_2D conj = new WF_2D(DimX, DimY, Boundary);
            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    conj.field[i] = field[i];
            });
            return conj;
        }

        public double getNorm(int x, int y) => (Complex.Conjugate(this[x, y]) * this[x, y]).Real;

        public Image GetImage(int width, int height)
        {
            var plt = new Plot();

            double[,] data = new double[DimX, DimY];

            for (int i = 0; i < DimX; i++)
                for (int j = 0; j < DimY; j++)
                    data[i, j] = getNorm(i, j);

            var hm1 = plt.Add.Heatmap(data);
            hm1.Colormap = new ScottPlot.Colormaps.Turbo();

            plt.Add.ColorBar(hm1);
            return plt.GetImage(width, height);
        }

        #endregion Interface
    }
}