using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces
{
    public interface IPotential<T> : IHamilton<T> where T : IWavefunction
    {
        public string Name { get; }
        public decimal Potential { get; }
    }
}