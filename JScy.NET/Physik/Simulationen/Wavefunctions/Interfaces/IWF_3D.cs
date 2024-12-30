using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWF_3D : IWF_2D
    {
        int DimZ { get; }
        Complex this[int x, int y, int z] { get; }

        void SetField(int x, int y, int z, Complex value);

        double getNorm(int x, int y, int z);
    }
}