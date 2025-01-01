using System.Numerics;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces
{
    public interface IWF_3D : IWF_2D
    {
        Complex this[int x, int y, int z] { get; }

        void SetField(int x, int y, int z, Complex value);

        double getNorm(int x, int y, int z);

        int?[] getNeighborsZ(int i, int positions = 1);

        int? getNeightborZ(int i, int direction);
    }
}