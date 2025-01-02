using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces
{
    public interface IHamilton
    {
        IWavefunction HPsi(ref IWavefunction psi);

        double E(ref IWavefunction psi);
    }
}