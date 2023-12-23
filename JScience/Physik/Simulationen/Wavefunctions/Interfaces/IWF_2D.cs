using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWF_2D:IWavefunction
    {
        int DimX { get; }
        int DimY { get; }
        Complex this[int x, int y] { get; }
    }
}
