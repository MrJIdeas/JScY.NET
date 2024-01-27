using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWF_3D : IWavefunction
    {
        int DimX { get; }
        int DimY { get; }
        int DimZ { get; }
        Complex this[int x, int y, int z] { get; }

        void SetField(int x, int y, int z, Complex value);

        #region Operatoren WF-double

        public static IWF_3D operator +(double b, IWF_3D a)
        {
            for (int i = 0; i < a.DimX; i++)
                for (int j = 0; j < a.DimY; j++)
                    for (int k = 0; k < a.DimZ; k++)
                        a.SetField(i, j, k, a[i, j, k] + b);
            return a;
        }

        public static IWF_3D operator -(double b, IWF_3D a)
        {
            for (int i = 0; i < a.DimX; i++)
                for (int j = 0; j < a.DimY; j++)
                    for (int k = 0; k < a.DimZ; k++)
                        a.SetField(i, j, k, a[i, j, k] - b);
            return a;
        }

        public static IWF_3D operator *(double b, IWF_3D a)
        {
            for (int i = 0; i < a.DimX; i++)
                for (int j = 0; j < a.DimY; j++)
                    for (int k = 0; k < a.DimZ; k++)
                        a.SetField(i, j, k, a[i, j, k] * b);
            return a;
        }

        public static IWF_3D operator +(IWF_3D a, double b) => b + a;

        public static IWF_3D operator -(IWF_3D a, double b) => b - a;

        public static IWF_3D operator *(IWF_3D a, double b) => b * a;

        public static IWF_3D operator /(IWF_3D a, double b)
        {
            for (int i = 0; i < a.DimX; i++)
                for (int j = 0; j < a.DimY; j++)
                    for (int k = 0; k < a.DimZ; k++)
                        a.SetField(i, j, k, a[i, j, k] / b);
            return a;
        }

        #endregion Operatoren WF-double

        #region Operatoren WF-WF

        public static IWF_3D operator +(IWF_3D a, IWF_3D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            IWF_3D c = (IWF_3D)a.Clone();
            for (int i = 0; i < c.DimX; i++)
                for (int j = 0; j < c.DimY; j++)
                    for (int k = 0; k < c.DimZ; k++)
                        c.SetField(i, j, k, c[i, j, k] + b[i, j, k]);
            return c;
        }

        public static IWF_3D operator -(IWF_3D a, IWF_3D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            IWF_3D c = (IWF_3D)a.Clone();
            for (int i = 0; i < c.DimX; i++)
                for (int j = 0; j < c.DimY; j++)
                    for (int k = 0; k < c.DimZ; k++)
                        c.SetField(i, j, k, c[i, j, k] - b[i, j, k]);
            return c;
        }

        public static IWF_3D operator *(IWF_3D a, IWF_3D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            IWF_3D c = (IWF_3D)a.Clone();
            for (int i = 0; i < c.DimX; i++)
                for (int j = 0; j < c.DimY; j++)
                    for (int k = 0; k < c.DimZ; k++)
                        c.SetField(i, j, k, c[i, j, k] * b[i, j, k]);
            return c;
        }

        #endregion Operatoren WF-WF
    }
}