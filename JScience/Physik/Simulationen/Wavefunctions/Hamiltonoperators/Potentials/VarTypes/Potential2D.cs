using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class Potential2D<T> : Potential_Base<T>, IBarrier_X, IBarrier_Y where T : IWF_2D
    {
        public int xStart { get; private set; }

        public int xEnd { get; private set; }

        public int yStart { get; private set; }

        public int yEnd { get; private set; }

        public Potential2D(string name, int xSTART, int ySTART, int xEND, int yEND, double Vmax) : base(name, Vmax)
        {
            xStart = xSTART;
            xEnd = xEND;
            yStart = ySTART;
            yEnd = yEND;
        }

        public override T HPsi(T psi)
        {
            T psiV = (T)Activator.CreateInstance(psi.GetType(), psi.WFInfo);
            for (int i = xStart; i < xEnd; i++)
                for (int j = yStart; j < yEnd; j++)
                    psiV.SetField(i, j, Potential * psi[i, j]);
            return psiV;
        }
    }
}