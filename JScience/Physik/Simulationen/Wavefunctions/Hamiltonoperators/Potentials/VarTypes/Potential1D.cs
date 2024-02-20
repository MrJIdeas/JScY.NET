using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class Potential1D<T> : IPotential<T>, IBarrier_X where T : IWF_1D
    {
        public int xStart { get; private set; }

        public int xEnd { get; private set; }

        public string Name { get; private set; }

        public decimal Potential { get; private set; }

        public Potential1D(string name, int xSTART, int xEND, decimal Vmax)
        {
            xStart = xSTART;
            xEnd = xEND;
            Name = name;
            Potential = Vmax;
        }

        public decimal E(T psi)
        {
            decimal erg = 0;
            T erg2 = (T)((T)psi.Conj() * HPsi(psi));
            for (int i = 0; i < erg2.DimX; i++)
                erg += (erg2[i].Conj() * erg2[i]).Real;
            return erg;
        }

        public virtual T HPsi(T psi)
        {
            T psiV = (T)Activator.CreateInstance(psi.GetType(), psi.DimX, psi.Boundary);
            for (int i = xStart; i < xEnd; i++)
                psiV.SetField(i, Potential * psi[i]);
            return psiV;
        }
    }
}