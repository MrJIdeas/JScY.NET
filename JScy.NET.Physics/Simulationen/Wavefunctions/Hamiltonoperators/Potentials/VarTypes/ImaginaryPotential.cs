using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class ImaginaryPotential : ImaginaryPotential_Base, IBarrier_X
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