using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using ScottPlot;
using System.Collections.Generic;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.VarTypes
{
    public class WF_1D : IWF_1D
    {
        public WF_1D(int DimX)
        {
            field = new Complex[DimX];
        }

        private Complex[] field { get; set; }

        #region Interface

        public Complex this[int x] => field[x];
        public int DimX => field.Length;
        public int Dimensions => 1;

        public double Norm()
        {
            double erg = 0;
            for (int i = 0; i < field.Length; i++)
            {
                erg += (field[i] * Complex.Conjugate(field[i])).Real;
            }
            return erg;
        }

        public IWavefunction Conj()
        {
            WF_1D conj = new WF_1D(DimX);
            for (int i = 0; i < conj.DimX; i++)
                conj.field[i] = Complex.Conjugate(field[i]);
            return conj;
        }

        public IWavefunction GetShift(EShift shift)
        {
            WF_1D neu = new WF_1D(DimX);
            switch (shift)
            {
                default:
                    return null;

                case EShift.Xm:
                    for (int i = 0; i < neu.DimX - 1; i++)
                    {
                        neu.field[i] = field[i + 1];
                    }
                    neu.field[neu.DimX - 1] = Complex.Zero;
                    return neu;

                case EShift.Xp:
                    for (int i = neu.DimX - 1; i > 0; i--)
                    {
                        neu.field[i] = field[i - 1];
                    }
                    neu.field[0] = Complex.Zero;
                    return neu;
            }
        }

        public void SetField(int x, Complex value) => field[x] = value;

        public void Clear()
        {
            for (int i = 0; i < field.Length; i++)
                field[i] = Complex.Zero;
        }

        public IWavefunction Clone()
        {
            WF_1D conj = new WF_1D(DimX);
            for (int i = 0; i < conj.DimX; i++)
                conj.field[i] = field[i];
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

            myPlot.Add.Scatter(x.ToArray(), y.ToArray());
            return myPlot.GetImage(width, height);
        }

        #endregion Interface
    }
}