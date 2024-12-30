using JScy.NET.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses;
using JScy.NET.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physik.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class ImaginaryPotential<T> : ImaginaryPotential_Base<T>, IBarrier_X where T : IWF_1D
    {
        public ImaginaryPotential(string name, int xSTART, int xEND, double damping) : base(name, -damping, xSTART, xEND, 0, 1, 0, 1)
        {
        }

        public ImaginaryPotential(string name, int xSTART, int xEND, int ySTART, int yEND, double damping) : base(name, -damping, xSTART, xEND, ySTART, yEND, 0, 1)
        {
        }

        public ImaginaryPotential(string name, int xSTART, int ySTART, int xEND, int yEND, int zSTART, int zEND, double Vmax) : base(name, Vmax, xSTART, xEND, ySTART, yEND, zSTART, zEND)
        {
        }
    }
}