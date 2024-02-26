using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class ImaginaryPotential2D<T> : Potential2D<T>, IBarrier_X, IBarrier_Y where T : IWF_2D
    {
        public ImaginaryPotential2D(string name, int xSTART, int xEND, int ySTART, int yEND, double damping) : base(name, xSTART, ySTART, xEND, yEND, -damping)
        {
        }

        public override T HPsi(T psi)
        {
            T psiV = (T)Activator.CreateInstance(psi.GetType(), psi.WFInfo, psi.UseGPU);
            for (int i = xStart; i < xEnd; i++)
                for (int j = yStart; j < yEnd; j++)
                    psiV.SetField(i, j, psi[i, j]);
            return (T)(psiV * (Complex.ImaginaryOne * Potential));
        }
    }
}