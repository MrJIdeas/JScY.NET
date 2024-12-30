using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Enums;

namespace JScience.Physik.Simulationen.Spins.Classic.Simulations.Lattice
{
    public class Ising_Classic_1D_Lattice : Ising_Classic_2D_Lattice
    {
        protected override string CONST_FNAME => "ISING_1D_CLASSIC";

        public Ising_Classic_1D_Lattice(double j, double b, double t, uint dimX, uint MaxSteps, EParticleType types, ELatticeBoundary boundary, uint StepsPerSaving) :
            base(j, b, t, dimX, 1, MaxSteps, types, boundary, StepsPerSaving)
        {
        }
    }
}