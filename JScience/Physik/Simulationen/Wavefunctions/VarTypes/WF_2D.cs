using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.VarTypes
{
    public class WF_2D : IWF_2D
    {
        public WF_2D(int DimX, int DimY)
        {
            field = new Complex[DimX, DimY];
        }

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
                    erg += field[i, j].Magnitude;
            return erg;
        }

        public IWavefunction Conj()
        {
            WF_2D conj = this;
            for (int i = 0; i < conj.DimX; i++)
                for (int j = 0; j < conj.DimY; j++)
                    conj.field[i, j] = Complex.Conjugate(conj[i, j]);
            return conj;
        }

        public IWavefunction GetShift(EShift shift)
        {
            WF_2D neu = this;
            Complex[] buf;
            switch (shift)
            {
                default:
                    return null;

                case EShift.Xm:
                    buf = new Complex[neu.DimY];
                    for (int i = 0; i < neu.DimY; i++)
                        buf[i] = neu.field[0, i];
                    for (int j = 0; j < neu.DimY; j++)
                    {
                        for (int i = 0; i < neu.DimX - 1; i++)
                        {
                            neu.field[0, i] = neu.field[i + 1, j];
                        }
                        neu.field[neu.DimX - 1, j] = buf[j];
                    }
                    return neu;

                case EShift.Xp:
                    buf = new Complex[neu.DimY];
                    for (int i = 0; i < neu.DimY; i++)
                        buf[i] = neu.field[DimX - 1, i];
                    for (int j = 0; j < neu.DimY; j++)
                    {
                        for (int i = neu.DimX - 1 - 1; i >= 0; i--)
                        {
                            neu.field[i, j] = neu.field[i - 1, j];
                        }
                        neu.field[0, j] = buf[j];
                    }
                    return neu;

                case EShift.Ym:
                    buf = new Complex[neu.DimX];
                    for (int i = 0; i < neu.DimX; i++)
                        buf[i] = neu.field[i, 0];
                    for (int i = 0; i < neu.DimX; i++)
                    {
                        for (int j = 0; j < neu.DimY - 1; j++)
                        {
                            neu.field[i, j] = neu.field[i, j + 1];
                        }
                        neu.field[i, neu.DimY - 1] = buf[i];
                    }
                    return neu;

                case EShift.Yp:
                    buf = new Complex[neu.DimX];
                    for (int i = 0; i < neu.DimX; i++)
                        buf[i] = neu.field[i, DimY - 1];
                    for (int i = 0; i < neu.DimX; i++)
                    {
                        for (int j = neu.DimY - 1 - 1; j >= 0; j--)
                        {
                            neu.field[i, j] = neu.field[i, j - 1];
                        }
                        neu.field[i, 0] = buf[i];
                    }
                    return neu;
            }
        }

        public void SetField(int x, int y, Complex value) => field[x, y] = value;

        public void Clear()
        {
            for (int i = 0; i < field.Length; i++)
                for (int j = 0; j < DimY; j++)
                    field[i, j] = Complex.Zero;
        }

        public IWavefunction Clone()
        {
            WF_2D conj = new WF_2D(DimX, DimY);
            for (int i = 0; i < conj.DimX; i++)
                for (int j = 0; j < conj.DimY; j++)
                    conj.SetField(i, j, field[i, j]);
            return conj;
        }

        #endregion Interface

        #region Operatoren

        public static WF_2D operator +(WF_2D A, WF_2D B)
        {
            if (A.DimX != B.DimX)
                return null;
            for (int i = 0; i < A.DimX; i++)
                for (int j = 0; j < A.DimY; j++)
                    A.field[i, j] = Complex.Add(A[i, j], B[i, j]);
            return A;
        }

        public static WF_2D operator +(WF_2D A, IWavefunction B)
        {
            if (B is WF_2D)
                return A + (WF_2D)B;
            throw new Exception("Invalid Type");
        }

        public static WF_2D operator +(IWavefunction B, WF_2D A) => A + B;

        public static WF_2D operator -(WF_2D A, WF_2D B)
        {
            if (A.DimX != B.DimX)
                return null;
            for (int i = 0; i < A.DimX; i++)
                for (int j = 0; j < A.DimY; j++)
                    A.field[i, j] = Complex.Subtract(A[i, j], B[i, j]);
            return A;
        }

        public static WF_2D operator -(WF_2D A, IWavefunction B)
        {
            if (B is WF_2D)
                return A - (WF_2D)B;
            throw new Exception("Invalid Type");
        }

        public static WF_2D operator -(IWavefunction B, WF_2D A)
        {
            if (B is WF_2D)
                return (WF_2D)B - A;
            throw new Exception("Invalid Type");
        }

        public static WF_2D operator *(WF_2D A, WF_2D B)
        {
            if (A.DimX != B.DimX)
                return null;
            for (int i = 0; i < A.DimX; i++)
                for (int j = 0; j < A.DimY; j++)
                    A.field[i, j] = Complex.Multiply(A[i, j], B[i, j]);
            return A;
        }

        public static WF_2D operator *(WF_2D A, IWavefunction B)
        {
            if (B is WF_2D)
                return A * (WF_2D)B;
            throw new Exception("Invalid Type");
        }

        public static WF_2D operator *(IWavefunction B, WF_2D A) => A * B;

        public static WF_2D operator *(WF_2D A, double B)
        {
            for (int i = 0; i < A.field.Length; i++)
                for (int j = 0; j < A.DimY; j++)
                    A.field[i, j] = Complex.Multiply(A[i, j], B);
            return A;
        }

        public static WF_2D operator *(double B, WF_2D A) => A * B;

        public static WF_2D operator *(WF_2D A, Complex B)
        {
            for (int i = 0; i < A.field.Length; i++)
                for (int j = 0; j < A.DimY; j++)
                    A.field[i, j] = Complex.Multiply(A[i, j], B);
            return A;
        }

        public static WF_2D operator *(Complex B, WF_2D A) => A * B;

        public static WF_2D operator /(WF_2D A, double B)
        {
            for (int i = 0; i < A.field.Length; i++)
                for (int j = 0; j < A.DimY; j++)
                    A.field[i, j] = Complex.Divide(A[i, j], B);
            return A;
        }

        public static WF_2D operator /(WF_2D A, Complex B)
        {
            for (int i = 0; i < A.field.Length; i++)
                for (int j = 0; j < A.DimY; j++)
                    A.field[i, j] = Complex.Divide(A[i, j], B);
            return A;
        }

        #endregion Operatoren
    }
}