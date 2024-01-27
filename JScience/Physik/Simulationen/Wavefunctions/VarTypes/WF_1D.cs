using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
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
                erg += field[i].Magnitude;
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
            Complex buf;
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

        #endregion Interface
    }
}