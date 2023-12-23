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
            WF_1D conj = this;
            for (int i = 0; i < conj.DimX; i++)
                conj.field[i] = Complex.Conjugate(conj[i]);
            return conj;
        }

        public IWavefunction GetShift(EShift shift)
        {
            WF_1D neu = this;
            Complex buf;
            switch (shift)
            {
                default:
                    return null;

                case EShift.Xm:
                    buf = neu.field[0];
                    for (int i = 0; i < neu.DimX - 1; i++)
                    {
                        neu.field[i] = neu.field[i + 1];
                    }
                    neu.field[neu.DimX - 1] = buf;
                    return neu;

                case EShift.Xp:
                    buf = neu.field[DimX - 1];
                    for (int i = neu.DimX - 1; i > 0; i--)
                    {
                        neu.field[i] = neu.field[i - 1];
                    }
                    neu.field[0] = buf;
                    return neu;
            }
        }

        #endregion Interface

        #region Operatoren

        public static WF_1D operator +(WF_1D A, WF_1D B)
        {
            if (A.DimX != B.DimX)
                return null;
            for (int i = 0; i < A.field.Length; i++)
                A.field[i] = Complex.Add(A[i], B[i]);
            return A;
        }

        public static WF_1D operator +(WF_1D A, IWavefunction B)
        {
            if (B is WF_1D)
                return A + (WF_1D)B;
            throw new Exception("Invalid Type");
        }

        public static WF_1D operator +(IWavefunction B, WF_1D A) => A + B;

        public static WF_1D operator -(WF_1D A, WF_1D B)
        {
            if (A.DimX != B.DimX)
                return null;
            for (int i = 0; i < A.field.Length; i++)
                A.field[i] = Complex.Subtract(A[i], B[i]);
            return A;
        }

        public static WF_1D operator -(WF_1D A, IWavefunction B)
        {
            if (B is WF_1D)
                return A - (WF_1D)B;
            throw new Exception("Invalid Type");
        }

        public static WF_1D operator -(IWavefunction B, WF_1D A)
        {
            if (B is WF_1D)
                return (WF_1D)B - A;
            throw new Exception("Invalid Type");
        }

        public static WF_1D operator *(WF_1D A, WF_1D B)
        {
            if (A.DimX != B.DimX)
                return null;
            for (int i = 0; i < A.field.Length; i++)
                A.field[i] = Complex.Multiply(A[i], B[i]);
            return A;
        }

        public static WF_1D operator *(WF_1D A, IWavefunction B)
        {
            if (B is WF_1D)
                return A * (WF_1D)B;
            throw new Exception("Invalid Type");
        }

        public static WF_1D operator *(IWavefunction B, WF_1D A) => A * B;

        public static WF_1D operator *(WF_1D A, double B)
        {
            for (int i = 0; i < A.field.Length; i++)
                A.field[i] = Complex.Multiply(A[i], B);
            return A;
        }

        public static WF_1D operator *(double B, WF_1D A) => A * B;

        public static WF_1D operator *(WF_1D A, Complex B)
        {
            for (int i = 0; i < A.field.Length; i++)
                A.field[i] = Complex.Multiply(A[i], B);
            return A;
        }

        public static WF_1D operator *(Complex B, WF_1D A) => A * B;

        public static WF_1D operator /(WF_1D A, double B)
        {
            for (int i = 0; i < A.field.Length; i++)
                A.field[i] = Complex.Divide(A[i], B);
            return A;
        }

        public static WF_1D operator /(WF_1D A, Complex B)
        {
            for (int i = 0; i < A.field.Length; i++)
                A.field[i] = Complex.Divide(A[i], B);
            return A;
        }

        #endregion Operatoren
    }
}