using System;
using System.Numerics;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces
{
    public interface IWF_2D : IWF_1D
    {
        Complex this[int x, int y] { get; set; }

        void SetField(int x, int y, Complex value);

        double getNorm(int x, int y);

        Tuple<int, int> getCoordinatesXY(int i);

        int?[] getNeighborsY(int i, int positions = 1);

        int? getNeightborY(int i, int direction);
    }
}