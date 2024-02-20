using JScience.Mathe.ComplexNumbers.VarTypes;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class ImaginaryPotential2D<T> : Potential2D<T>, IBarrier_X, IBarrier_Y where T : IWF_2D
    {
        public ImaginaryPotential2D(string name, int xSTART, int xEND, int ySTART, int yEND, decimal damping) : base(name, xSTART, ySTART, xEND, yEND, -damping)
        {
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

        public override T HPsi(T psi)
        {
            T psiV = (T)Activator.CreateInstance(psi.GetType(), psi.DimX, psi.DimY, psi.Boundary);
            for (int i = xStart; i < xEnd; i++)
                for (int j = yStart; j < yEnd; j++)
                    psiV.SetField(i, j, Potential * DecComplex.ImaginaryOne * psi[i, j]);
            return psiV;
        }
    }
}