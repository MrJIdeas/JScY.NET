using JScy.NET.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses;
using JScy.NET.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physik.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class BlockPotential<T> : Potential_Base<T>, IBarrier_X where T : IWavefunction
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