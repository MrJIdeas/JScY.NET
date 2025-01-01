using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces
{
    public interface IHamilton
    {
        IWavefunction HPsi(IWavefunction psi);

        double E(IWavefunction psi);
    }
}