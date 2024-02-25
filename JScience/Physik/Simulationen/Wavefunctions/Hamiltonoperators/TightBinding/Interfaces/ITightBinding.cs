using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.Interfaces
{
    public abstract class ITightBinding<T> : IHamilton<T> where T : IWavefunction
    {
        protected ITightBinding(double t_hop)
        {
            t_Hopping = t_hop;
        }

        public double t_Hopping { get; }

        public double E(T psi)
        {
            double erg = 0;
            T erg2 = (T)((T)psi.Conj() * HPsi(psi));
            for (int i = 0; i < erg2.WFInfo.DimX * erg2.WFInfo.DimY * erg2.WFInfo.DimZ; i++)
                erg += (Complex.Conjugate(erg2[i]) * erg2[i]).Real;
            return erg;
        }

        public virtual T HPsi(T psi) => throw new NotImplementedException();
    }
}