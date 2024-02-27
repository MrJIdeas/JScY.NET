using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class Potential1D<T> : Potential_Base<T>, IBarrier_X where T : IWF_1D
    {
        public Potential1D(string name, int xSTART, int xEND, double Vmax) : base(name, Vmax, xSTART, xEND, 0, 1, 0, 1)
        {
        }
    }
}