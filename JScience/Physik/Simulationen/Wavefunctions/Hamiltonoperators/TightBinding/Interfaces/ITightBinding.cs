using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.Interfaces
{
    public abstract class ITightBinding<T> : IHamilton<T> where T : IWavefunction
    {
        protected ITightBinding(decimal t_hop)
        {
            t_Hopping = t_hop;
        }

        public decimal t_Hopping { get; }

        public virtual decimal E(T psi) => throw new NotImplementedException();

        public virtual T HPsi(T psi) => throw new NotImplementedException();
    }
}