using JScience.Physik.Simulationen.Wavefunctions.VarTypes;
using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWF_2D : IWavefunction
    {
        int DimX { get; }
        int DimY { get; }
        Complex this[int x, int y] { get; }

        void SetField(int x, int y, Complex value);

        double getNorm(int x, int y);

        #region Operatoren WF-double

        public static IWF_2D operator +(double b, IWF_2D a)
        {
            for (int i = 0; i < a.DimX; i++)
                for (int j = 0; j < a.DimY; j++)
                    a.SetField(i, j, a[i, j] + b);
            return a;
        }

        public static IWF_2D operator -(double b, IWF_2D a)
        {
            for (int i = 0; i < a.DimX; i++)
                for (int j = 0; j < a.DimY; j++)
                    a.SetField(i, j, a[i, j] - b);
            return a;
        }

        public static IWF_2D operator *(double b, IWF_2D a)
        {
            for (int i = 0; i < a.DimX; i++)
                for (int j = 0; j < a.DimY; j++)
                    a.SetField(i, j, a[i, j] * b);
            return a;
        }

        public static IWF_2D operator +(IWF_2D a, double b) => b + a;

        public static IWF_2D operator -(IWF_2D a, double b) => b - a;

        public static IWF_2D operator *(IWF_2D a, double b) => b * a;

        public static IWF_2D operator /(IWF_2D a, double b)
        {
            for (int i = 0; i < a.DimX; i++)
                for (int j = 0; j < a.DimY; j++)
                    a.SetField(i, j, a[i, j] / b);
            return a;
        }

        #endregion Operatoren WF-double

        #region Operatoren WF-WF

        public static IWF_2D operator +(IWF_2D a, IWF_2D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            IWF_2D c = (IWF_2D)a.Clone();
            for (int i = 0; i < c.DimX; i++)
                for (int j = 0; j < c.DimY; j++)
                    c.SetField(i, j, c[i, j] + b[i, j]);
            return c;
        }

        public static IWF_2D operator -(IWF_2D a, IWF_2D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            IWF_2D c = (IWF_2D)a.Clone();
            for (int i = 0; i < c.DimX; i++)
                for (int j = 0; j < c.DimY; j++)
                    c.SetField(i, j, c[i, j] - b[i, j]);
            return c;
        }

        public static IWF_2D operator *(IWF_2D a, IWF_2D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            IWF_2D c = (IWF_2D)a.Clone();
            for (int i = 0; i < c.DimX; i++)
                for (int j = 0; j < c.DimY; j++)
                    c.SetField(i, j, c[i, j] * b[i, j]);
            return c;
        }

        #endregion Operatoren WF-WF
    }
}