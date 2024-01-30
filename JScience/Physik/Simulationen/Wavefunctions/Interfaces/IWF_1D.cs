using JScience.Mathe.ComplexNumbers.VarTypes;
using System;
using System.Threading.Tasks;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWF_1D : IWavefunction
    {
        int DimX { get; }

        DecComplex this[int x] { get; }

        void SetField(int x, DecComplex value);

        decimal getNorm(int x);

        #region Operatoren WF-DecComplex

        public static IWF_1D operator +(DecComplex b, IWF_1D a)
        {
            Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    a.SetField(i, a[i] + b);
            });
            return a;
        }

        public static IWF_1D operator -(DecComplex b, IWF_1D a)
        {
            Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    a.SetField(i, a[i] - b);
            });
            return a;
        }

        public static IWF_1D operator *(DecComplex b, IWF_1D a)
        {
            Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    a.SetField(i, a[i] * b);
            });
            return a;
        }

        public static IWF_1D operator +(IWF_1D a, DecComplex b) => b + a;

        public static IWF_1D operator -(IWF_1D a, DecComplex b) => b - a;

        public static IWF_1D operator *(IWF_1D a, DecComplex b) => b * a;

        public static IWF_1D operator /(IWF_1D a, DecComplex b)
        {
            Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    a.SetField(i, a[i] / b);
            });
            return a;
        }

        #endregion Operatoren WF-DecComplex

        #region Operatoren WF-decimal

        public static IWF_1D operator +(decimal b, IWF_1D a)
        {
            IWF_1D c = (IWF_1D)a.Clone();
            Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    a.SetField(i, a[i] + b);
            });
            return c;
        }

        public static IWF_1D operator -(decimal b, IWF_1D a)
        {
            IWF_1D c = (IWF_1D)a.Clone();
            Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    a.SetField(i, a[i] - b);
            });
            return c;
        }

        public static IWF_1D operator *(decimal b, IWF_1D a)
        {
            IWF_1D c = (IWF_1D)a.Clone();
            Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    a.SetField(i, a[i] * b);
            });
            return c;
        }

        public static IWF_1D operator +(IWF_1D a, decimal b) => b + a;

        public static IWF_1D operator -(IWF_1D a, decimal b) => b - a;

        public static IWF_1D operator *(IWF_1D a, decimal b) => b * a;

        public static IWF_1D operator /(IWF_1D a, decimal b)
        {
            IWF_1D c = (IWF_1D)a.Clone();
            Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    a.SetField(i, a[i] / b);
            });
            return c;
        }

        #endregion Operatoren WF-decimal

        #region Operatoren WF-WF

        public static IWF_1D operator +(IWF_1D a, IWF_1D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            IWF_1D c = (IWF_1D)a.Clone();
            Parallel.ForEach(c.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    c.SetField(i, c[i] + b[i]);
            });
            return c;
        }

        public static IWF_1D operator -(IWF_1D a, IWF_1D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            IWF_1D c = (IWF_1D)a.Clone();
            Parallel.ForEach(c.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    c.SetField(i, c[i] - b[i]);
            });
            return c;
        }

        public static IWF_1D operator *(IWF_1D a, IWF_1D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            IWF_1D c = (IWF_1D)a.Clone();
            Parallel.ForEach(c.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    c.SetField(i, c[i] * b[i]);
            });
            return c;
        }

        #endregion Operatoren WF-WF
    }
}