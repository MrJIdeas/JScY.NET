using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using ScottPlot;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace JScience.Physik.Simulationen.Wavefunctions.VarTypes
{
    public class WF_2D : IWF_2D
    {
        public WF_2D(int DimX, int DimY, ELatticeBoundary boundary)
        {
            Boundary = boundary;
            field = new Complex[DimX, DimY];
            rangePartitioner = Partitioner.Create(0, DimX);
        }

        public OrderablePartitioner<Tuple<int, int>> rangePartitioner { get; private set; }
        public ELatticeBoundary Boundary { get; private set; }

        private Complex[,] field { get; set; }
        public Complex this[int x, int y] => field[x, y];

        public int DimX => field.GetLength(0);

        public int DimY => field.GetLength(1);

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
            Parallel.For(0, conj.DimX, (i) =>
            {
                for (int j = 0; j < conj.DimY; j++)
                    conj.field[i, j] = Complex.Conjugate(conj[i, j]);
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
                            neu.field[i, j] = field[i + 1, j];
                        }
                        if (Boundary == ELatticeBoundary.Periodic)
                            neu.field[neu.DimX - 1, j] = field[0, j];
                        else
                            neu.field[neu.DimX - 1, j] = Complex.Zero;
                    });
                    return neu;

                case EShift.Xp:
                    Parallel.For(0, neu.DimY - 1, (j) =>
                    {
                        for (int i = neu.DimX - 1; i > 0; i--)
                        {
                            neu.field[i, j] = field[i - 1, j];
                        }
                        if (Boundary == ELatticeBoundary.Periodic)
                            neu.field[0, j] = field[DimX - 1, j];
                        else
                            neu.field[0, j] = Complex.Zero;
                    });
                    return neu;

                case EShift.Ym:
                    Parallel.For(0, neu.DimX - 1, (j) =>
                    {
                        for (int i = 0; i < neu.DimY - 1; i++)
                        {
                            neu.field[j, i] = field[j, i + 1];
                        }
                        if (Boundary == ELatticeBoundary.Periodic)
                            neu.field[j, neu.DimY - 1] = field[j, 0];
                        else
                            neu.field[j, neu.DimY - 1] = Complex.Zero;
                    });
                    return neu;

                case EShift.Yp:
                    Parallel.For(0, neu.DimX - 1, (j) =>
                    {
                        for (int i = neu.DimY - 1; i > 0; i--)
                        {
                            neu.field[j, i] = field[j, i - 1];
                        }
                        if (Boundary == ELatticeBoundary.Periodic)
                            neu.field[j, 0] = field[j, DimY - 1];
                        else
                            neu.field[j, 0] = Complex.Zero;
                    });
                    return neu;
            }
        }

        public void SetField(int x, int y, Complex value) => field[x, y] = value;

        public void Clear()
        {
            Parallel.For(0, DimX, (i) =>
            {
                for (int j = 0; j < DimY; j++)
                    field[i, j] = Complex.Zero;
            });
        }

        public IWavefunction Clone()
        {
            WF_2D conj = new WF_2D(DimX, DimY, Boundary);
            Parallel.For(0, conj.DimX, (i) =>
            {
                for (int j = 0; j < conj.DimY; j++)
                    conj.SetField(i, j, field[i, j]);
            });
            return conj;
        }

        public double getNorm(int x, int y) => (Complex.Conjugate(field[x, y]) * field[x, y]).Real;

        public Image GetImage(int width, int height)
        {
            var plt = new Plot();

            double[,] data = new double[DimX, DimY];
            for (int i = 0; i < DimY; i++)
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