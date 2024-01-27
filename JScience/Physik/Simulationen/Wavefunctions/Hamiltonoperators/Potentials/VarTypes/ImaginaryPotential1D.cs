using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class ImaginaryPotential1D<T> : IImagenaryPotential<T>, IBarrier_X where T : IWF_1D
    {
        public int xStart { get; private set; }

        public int xEnd { get; private set; }

        public ImaginaryPotential1D(int xSTART, int xEND, double damping) : base(damping)
        {
            xStart = xSTART;
            xEnd = xEND;
        }

        public override double E(T psi)
        {
            double erg = 0;
            T erg2 = (T)((T)psi.Conj() * HPsi(psi));
            for (int i = 0; i < erg2.DimX; i++)
                erg += (Complex.Conjugate(erg2[i]) * erg2[i]).Real;
            return erg;
        }

        public override T HPsi(T psi)
        {
            T psiV = (T)Activator.CreateInstance(psi.GetType(), psi.DimX);
            for (int i = xStart; i < xEnd; i++)
                psiV.SetField(i, Damping * psi[i]);
            return psiV;
        }
    }
}