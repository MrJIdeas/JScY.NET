using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class BlockPotential : Potential_Base, IBarrier_X
    {
        public BlockPotential(string name, int xSTART, int xEND, double Vmax) : base(name, Vmax, xSTART, xEND, 0, 1, 0, 1)
        {
        }

        public BlockPotential(string name, int xSTART, int ySTART, int xEND, int yEND, double Vmax) : base(name, Vmax, xSTART, xEND, ySTART, yEND, 0, 1)

        {
        }

        public BlockPotential(string name, int xSTART, int ySTART, int xEND, int yEND, int zSTART, int zEND, double Vmax) : base(name, Vmax, xSTART, xEND, ySTART, yEND, zSTART, zEND)

        {
        }
    }
}