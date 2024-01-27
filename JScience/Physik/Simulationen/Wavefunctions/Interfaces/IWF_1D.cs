using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWF_1D : IWavefunction
    {
        int DimX { get; }

        Complex this[int x] { get; }

        void SetField(int x, Complex value);

        public static IWF_1D operator +(IWF_1D a, IWF_1D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            for (int i = 0; i < a.DimX; i++)
                a.SetField(i, a[i] + b[i]);
            return a;
        }

        public static IWF_1D operator -(IWF_1D a, IWF_1D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            for (int i = 0; i < a.DimX; i++)
                a.SetField(i, a[i] - b[i]);
            return a;
        }

        public static IWF_1D operator *(IWF_1D a, IWF_1D b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            for (int i = 0; i < a.DimX; i++)
                a.SetField(i, a[i] * b[i]);
            return a;
        }
    }
}