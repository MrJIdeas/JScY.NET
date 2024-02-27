using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class ImaginaryPotential1D<T> : Potential1D<T>, IBarrier_X where T : IWF_1D
    {
        private Complex ImagPotential { get; set; }

        public ImaginaryPotential1D(string name, int xSTART, int xEND, double damping) : base(name, xSTART, xEND, -damping)
        {
            ImagPotential = Potential * Complex.ImaginaryOne;
        }

        public override T HPsi(T psi)
        {
            T psiV = (T)Activator.CreateInstance(psi.GetType(), psi.WFInfo, psi.CalcMethod);
            for (int i = xStart; i < xEnd; i++)
                psiV.SetField(i, psi[i]);
            return (T)(psiV * ImagPotential);
        }
    }
}