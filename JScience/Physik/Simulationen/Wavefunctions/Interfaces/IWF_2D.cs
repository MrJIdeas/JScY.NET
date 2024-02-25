using System.Numerics;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWF_2D : IWF_1D
    {
        int DimY { get; }
        Complex this[int x, int y] { get; set; }

        void SetField(int x, int y, Complex value);

        double getNorm(int x, int y);

        Tuple<int, int> getCoordinates(int i);
    }
}