using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.BaseClasses;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses
{
    public abstract class Potential_Base<T> : Hamilton_Base<T>, IPotential<T> where T : IWavefunction
    {
        public string Name { get; private set; }

        public double Potential { get; private set; }

        protected Potential_Base(string name, double Vmax)
        {
            Name = name;
            Potential = Vmax;
        }
    }
}