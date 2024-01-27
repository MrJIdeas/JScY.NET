using System;
using System.Numerics;
using System.Threading.Tasks;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWF_1D : IWavefunction
    {
        int DimX { get; }

        Complex this[int x] { get; }

        void SetField(int x, Complex value);

        double getNorm(int x);

        #region Operatoren WF-complex

        public static IWF_1D operator +(Complex b, IWF_1D a)
        {
            while (!Parallel.For(0, a.DimX, (i) =>
            {
                a.SetField(i, a[i] + b);
            }).IsCompleted) ;
            return a;
        }

        public static IWF_1D operator -(Complex b, IWF_1D a)
        {
            while (!Parallel.For(0, a.DimX, (i) =>
            {
                a.SetField(i, a[i] - b);
            }).IsCompleted) ;
            return a;
        }

        public static IWF_1D operator *(Complex b, IWF_1D a)
        {
            while (!Parallel.For(0, a.DimX, (i) =>
            {
                a.SetField(i, a[i] * b);
            }).IsCompleted) ;
            return a;
        }

        public static IWF_1D operator +(IWF_1D a, Complex b) => b + a;

        public static IWF_1D operator -(IWF_1D a, Complex b) => b - a;

        public static IWF_1D operator *(IWF_1D a, Complex b) => b * a;

        public static IWF_1D operator /(IWF_1D a, Complex b)
        {
            while (!Parallel.For(0, a.DimX, (i) =>
            {
                a.SetField(i, a[i] / b);
            }).IsCompleted) ;
            return a;
        }

        #endregion Operatoren WF-complex

        #region Operatoren WF-double

        public static IWF_1D operator +(double b, IWF_1D a)
        {
            IWF_1D c = (IWF_1D)a.Clone();
            while (!Parallel.For(0, c.DimX, (i) =>
            {
                c.SetField(i, c[i] + b);
            }).IsCompleted) ;
            return c;
        }

        public static IWF_1D operator -(double b, IWF_1D a)
        {
            IWF_1D c = (IWF_1D)a.Clone();
            while (!Parallel.For(0, c.DimX, (i) =>
            {
                c.SetField(i, c[i] - b);
            }).IsCompleted) ;
            return c;
        }

        public static IWF_1D operator *(double b, IWF_1D a)
        {
            IWF_1D c = (IWF_1D)a.Clone();
            while (!Parallel.For(0, c.DimX, (i) =>
            {
                c.SetField(i, c[i] * b);
            }).IsCompleted) ;
            return c;
        }

        public static IWF_1D operator +(IWF_1D a, double b) => b + a;

        public static IWF_1D operator -(IWF_1D a, double b) => b - a;

        public static IWF_1D operator *(IWF_1D a, double b) => b * a;

        public static IWF_1D operator /(IWF_1D a, double b)
        {
            IWF_1D c = (IWF_1D)a.Clone();
            while (!Parallel.For(0, c.DimX, (i) =>
            {
                c.SetField(i, c[i] / b);
            }).IsCompleted) ;
            return c;
        }

        #endregion Operatoren WF-double

        #region Operatoren WF-WF

        public static IWF_1D operator +(IWF_1D a, IWF_1D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            IWF_1D c = (IWF_1D)a.Clone();
            while (!Parallel.For(0, c.DimX, (i) =>
            {
                c.SetField(i, c[i] + b[i]);
            }).IsCompleted) ;
            return c;
        }

        public static IWF_1D operator -(IWF_1D a, IWF_1D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            IWF_1D c = (IWF_1D)a.Clone();
            while (!Parallel.For(0, c.DimX, (i) =>
            {
                c.SetField(i, c[i] - b[i]);
            }).IsCompleted) ;
            return c;
        }

        public static IWF_1D operator *(IWF_1D a, IWF_1D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            IWF_1D c = (IWF_1D)a.Clone();
            while (!Parallel.For(0, c.DimX, (i) =>
            {
                c.SetField(i, c[i] * b[i]);
            }).IsCompleted) ;
            return c;
        }

        #endregion Operatoren WF-WF
    }
}