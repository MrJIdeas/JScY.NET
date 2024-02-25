using System.Numerics;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class ImaginaryPotential1D<T> : Potential1D<T>, IBarrier_X where T : IWF_1D
    {
        public ImaginaryPotential1D(string name, int xSTART, int xEND, double damping) : base(name, xSTART, xEND, -damping)
        {
        }

        public override T HPsi(T psi)
        {
            for (int i = xStart; i < xEnd; i++)
                psiV.SetField(i, psi[i]);
            return (T)(psiV * (Complex.ImaginaryOne * Potential));
        }
    }
}