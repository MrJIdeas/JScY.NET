using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.Interfaces
{
    public abstract class ITightBinding<T> : IHamilton<T> where T : IWavefunction
    {
        public double t_Hopping { get; }

        public double E(T psi) => throw new NotImplementedException();

        public T HPsi(T psi) => throw new NotImplementedException();
    }
}