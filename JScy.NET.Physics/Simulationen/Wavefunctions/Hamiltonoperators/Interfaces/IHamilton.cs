using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces
{
    public interface IHamilton<T> where T : IWavefunction
    {
        T HPsi(T psi);

        double E(T psi);
    }
}