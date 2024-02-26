using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.BaseClasses;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.Interfaces
{
    public abstract class ITightBinding<T> : Hamilton_Base<T> where T : IWavefunction
    {
        protected ITightBinding(double t_hop)
        {
            t_Hopping = t_hop;
        }

        public double t_Hopping { get; }
    }
}