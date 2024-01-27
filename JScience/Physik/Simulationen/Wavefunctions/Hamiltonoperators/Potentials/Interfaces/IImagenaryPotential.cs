using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces
{
    public abstract class IImagenaryPotential<T> : IHamilton<T> where T : IWavefunction
    {
        protected IImagenaryPotential(double damping)
        {
            Damping = new Complex(0, -damping);
        }

        public Complex Damping { get; private set; }

        public virtual double E(T psi)
        {
            throw new NotImplementedException();
        }

        public virtual T HPsi(T psi)
        {
            throw new NotImplementedException();
        }
    }
}