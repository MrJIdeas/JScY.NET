using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses
{
    public abstract class Potential_Base<T> : IPotential<T> where T : IWavefunction
    {
        public string Name { get; private set; }

        public decimal Potential { get; private set; }

        protected Potential_Base(string name, decimal Vmax)
        {
            Name = name;
            Potential = Vmax;
        }

        public decimal E(T psi)
        {
            decimal erg = 0;
            T erg2 = (T)((T)psi.Conj() * HPsi(psi));
            for (int i = 0; i < erg2.WFInfo.DimX * erg2.WFInfo.DimY * erg2.WFInfo.DimZ; i++)
                erg += (erg2[i].Conj() * erg2[i]).Real;
            return erg;
        }

        public abstract T HPsi(T psi);
    }
}