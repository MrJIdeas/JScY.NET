using JScience.Mathe.ComplexNumbers.VarTypes;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces
{
    public abstract class IImagenaryPotential<T> : IPotential<T> where T : IWavefunction
    {
        protected IImagenaryPotential(string name, decimal damping)
        {
            Name = name;
            Potential = damping;
            Damping = new DecComplex(0, -damping);
        }

        public decimal Potential { get; private set; }

        public DecComplex Damping { get; private set; }

        public string Name { get; private set; }

        public virtual decimal E(T psi)
        {
            throw new NotImplementedException();
        }

        public virtual T HPsi(T psi)
        {
            throw new NotImplementedException();
        }
    }
}