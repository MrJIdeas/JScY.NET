using JScy.NET.Physik.Enums;
using JScy.NET.Physik.Simulationen.Spins.Enums;

namespace JScy.NET.Physik.Simulationen.Spins.Classic.Simulations.Lattice
{
    public class Ising_Classic_2D_Lattice : Ising_Classic_3D_Lattice
    {
        protected override string CONST_FNAME => "ISING_2D_CLASSIC";

        public Ising_Classic_2D_Lattice(double j, double b, double t, uint dimX, uint dimY, uint MaxSteps, EParticleType types, ELatticeBoundary boundary, uint StepsPerSaving) :
            base(j, b, t, dimX, dimY, 1, MaxSteps, types, boundary, StepsPerSaving)
        {
        }
    }
}