using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class Potential2D<T> : IPotential<T>, IBarrier_X where T : IWF_2D
    {
        public int xStart { get; private set; }

        public int xEnd { get; private set; }

        public int yStart { get; private set; }

        public int yEnd { get; private set; }

        public string Name { get; private set; }

        public decimal Potential { get; private set; }

        public Potential2D(string name, int xSTART, int ySTART, int xEND, int yEND, decimal Vmax)
        {
            xStart = xSTART;
            xEnd = xEND;
            yStart = ySTART;
            yEnd = yEND;
            Name = name;
            Potential = Vmax;
        }

        public decimal E(T psi)
        {
            decimal erg = 0;
            T erg2 = (T)((T)psi.Conj() * HPsi(psi));
            for (int i = 0; i < erg2.DimX; i++)
                for (int j = 0; j < erg2.DimY; j++)
                    erg += (erg2[i, j].Conj() * erg2[i, j]).Real;
            return erg;
        }

        public T HPsi(T psi)
        {
            T psiV = (T)Activator.CreateInstance(psi.GetType(), psi.DimX, psi.DimY, psi.Boundary);
            for (int i = xStart; i < xEnd; i++)
                for (int j = yStart; j < yEnd; j++)
                    psiV.SetField(i, j, Potential * psi[i, j]);
            return psiV;
        }
    }
}