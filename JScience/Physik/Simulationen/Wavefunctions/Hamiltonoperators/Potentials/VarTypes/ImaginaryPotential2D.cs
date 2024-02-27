using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class ImaginaryPotential2D<T> : ImaginaryPotential_Base<T>, IBarrier_X, IBarrier_Y where T : IWF_2D
    {
        public ImaginaryPotential2D(string name, int xSTART, int xEND, int ySTART, int yEND, double damping) : base(name, -damping, xSTART, xEND, ySTART, yEND, 0, 1)
        {
        }
    }
}