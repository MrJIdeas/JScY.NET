using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses
{
    public abstract class Potential_Base<T> : IPotential<T> where T : IWavefunction
    {
        public string Name { get; private set; }

        public double Potential { get; private set; }

        protected Potential_Base(string name, double Vmax)
        {
            Name = name;
            Potential = Vmax;
        }

        public double E(T psi)
        {
            double erg = 0;
            T erg2 = (T)((T)psi.Conj() * HPsi(psi));
            for (int i = 0; i < erg2.WFInfo.DimX * erg2.WFInfo.DimY * erg2.WFInfo.DimZ; i++)
                erg += (Complex.Conjugate(erg2[i]) * erg2[i]).Real;
            return erg;
        }

        public abstract T HPsi(T psi);
    }
}