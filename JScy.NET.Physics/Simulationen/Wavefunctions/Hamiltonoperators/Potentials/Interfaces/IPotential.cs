using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces
{
    public interface IPotential : IHamilton
    {
        public string Name { get; }
        public double Potential { get; }
    }
}