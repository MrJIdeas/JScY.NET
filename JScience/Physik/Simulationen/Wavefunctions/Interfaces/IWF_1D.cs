using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWF_1D:IWavefunction
    {
        int DimX { get; }

        Complex this[int x] { get; }
    }
}
