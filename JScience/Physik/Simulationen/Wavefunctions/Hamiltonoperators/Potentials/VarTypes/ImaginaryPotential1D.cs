using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class ImaginaryPotential1D<T> : ImaginaryPotential_Base<T>, IBarrier_X where T : IWF_1D
    {
        public ImaginaryPotential1D(string name, int xSTART, int xEND, double damping) : base(name, -damping, xSTART, xEND, 0, 1, 0, 1)
        {
        }
    }
}