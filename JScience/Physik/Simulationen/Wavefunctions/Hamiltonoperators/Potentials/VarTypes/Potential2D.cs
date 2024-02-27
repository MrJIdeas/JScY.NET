using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class Potential2D<T> : Potential_Base<T>, IBarrier_X, IBarrier_Y where T : IWF_2D
    {
        public Potential2D(string name, int xSTART, int ySTART, int xEND, int yEND, double Vmax) : base(name, Vmax, xSTART, xEND, ySTART, yEND, 0, 1)

        {
        }
    }
}