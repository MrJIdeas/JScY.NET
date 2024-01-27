using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces
{
    public abstract class IImagenaryPotential<T> : IPotential<T> where T : IWavefunction
    {
        protected IImagenaryPotential(string name, double damping)
        {
            Name = name;
            Potential = damping;
            Damping = new Complex(0, -damping);
        }

        public double Potential { get; private set; }

        public Complex Damping { get; private set; }

        public string Name { get; private set; }

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