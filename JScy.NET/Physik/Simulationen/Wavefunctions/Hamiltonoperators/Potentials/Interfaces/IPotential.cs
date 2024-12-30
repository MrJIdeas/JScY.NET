using JScy.NET.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScy.NET.Physik.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces
{
    public interface IPotential<T> : IHamilton<T> where T : IWavefunction
    {
        public string Name { get; }
        public double Potential { get; }
    }
}