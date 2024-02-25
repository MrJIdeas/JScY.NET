using JScience.Mathe.ComplexNumbers.VarTypes;
using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using ScottPlot;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace JScience.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF
{
    public class WF_2D : WF_1D, IWF_2D
    {
        public new int DimX { get; private set; }

        public int DimY { get; private set; }

        public new int Dimensions => 2;

        public WF_2D(WFInfo wfinfo) : base(wfinfo)
        {
            this.DimX = wfinfo.DimX;
            this.DimY = wfinfo.DimY;
        }

        public DecComplex this[int x, int y]
        {
            get => this[x + y * DimX];
            set => this[x + y * DimX] = value;
        }

        public Tuple<int, int> getCoordinates(int i)
        {
            int x = i % DimX;
            return new Tuple<int, int>(x, (i - x) / DimX);
        }

        #region Interface

        public new IWavefunction GetShift(EShift shift)
        {
            WF_2D neu = new WF_2D(WFInfo);
            switch (shift)
            {
                default:
                    return null;

                case EShift.Xm:
                    Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            var coord = getCoordinates(i);
                            if (coord.Item1 < DimX - 1)
                                neu[i] = this[coord.Item1 + 1, coord.Item2];
                            else
                            {
                                if (Boundary == ELatticeBoundary.Periodic)
                                    neu[i] = this[0, coord.Item2];
                            }
                        }
                    });
                    return neu;

                case EShift.Xp:
                    Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            var coord = getCoordinates(i);
                            if (coord.Item1 > 0)
                                neu[i] = this[coord.Item1 - 1, coord.Item2];
                            else
                            {
                                if (Boundary == ELatticeBoundary.Periodic)
                                    neu[i] = this[DimX - 1, coord.Item2];
                            }
                        }
                    });
                    return neu;

                case EShift.Ym:
                    Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            var coord = getCoordinates(i);
                            if (coord.Item2 < DimY - 1)
                                neu[i] = this[coord.Item1, coord.Item2 + 1];
                            else
                            {
                                if (Boundary == ELatticeBoundary.Periodic)
                                    neu[i] = this[coord.Item1, 0];
                            }
                        }
                    });
                    return neu;

                case EShift.Yp:
                    Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            var coord = getCoordinates(i);
                            if (coord.Item2 > 0)
                                neu[i] = this[coord.Item1, coord.Item2 - 1];
                            else
                            {
                                if (Boundary == ELatticeBoundary.Periodic)
                                    neu[i] = this[coord.Item1, DimY - 1];
                            }
                        }
                    });
                    return neu;
            }
        }

        public void SetField(int x, int y, DecComplex value) => this[x, y] = value;

        protected override IWavefunction getEmptyLikeThis() => new WF_2D(WFInfo);

        public decimal getNorm(int x, int y) => getNorm(x + y * DimX);

        public new System.Drawing.Image GetImage(int width, int height)
        {
            myPlot.Clear();
            double[,] data = new double[DimX, DimY];
            for (int i = 0; i < DimX; i++)
                for (int j = 0; j < DimY; j++)
                    data[i, j] = (double)getNorm(i, j);
            var hm1 = myPlot.Add.Heatmap(data);
            hm1.Colormap = new ScottPlot.Colormaps.Turbo();
            myPlot.Add.ColorBar(hm1);
            var img = System.Drawing.Image.FromStream(new MemoryStream(myPlot.GetImage(width, height).GetImageBytes()));
            return img;
        }

        #endregion Interface
    }
}