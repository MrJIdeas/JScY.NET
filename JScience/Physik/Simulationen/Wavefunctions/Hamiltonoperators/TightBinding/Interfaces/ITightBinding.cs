using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.Interfaces
{
    public interface ITightBinding<T> : IHamilton<T> where T : IWavefunction
    {
    }
}