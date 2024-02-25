using JScience.Physik.Simulationen.Wavefunctions.Interfaces;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces
{
    public interface IHamilton<T> where T : IWavefunction
    {
        T HPsi(T psi);

        double E(T psi);
    }
}