using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class Potential1D<T> : Potential_Base<T>, IBarrier_X where T : IWF_1D
    {
        public int xStart { get; private set; }

        public int xEnd { get; private set; }

        public Potential1D(string name, int xSTART, int xEND, decimal Vmax) : base(name, Vmax)
        {
            xStart = xSTART;
            xEnd = xEND;
        }

        public override T HPsi(T psi)
        {
            T psiV = (T)Activator.CreateInstance(psi.GetType(), psi.WFInfo);
            for (int i = xStart; i < xEnd; i++)
                psiV.SetField(i, Potential * psi[i]);
            return psiV;
        }
    }
}