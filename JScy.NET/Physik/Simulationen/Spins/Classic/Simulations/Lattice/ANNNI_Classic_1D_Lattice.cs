using JScy.NET.Physik.Enums;
using JScy.NET.Physik.Simulationen.Spins.Enums;

namespace JScy.NET.Physik.Simulationen.Spins.Classic.Simulations.Lattice
{
    public class ANNNI_Classic_1D_Lattice : ANNNI_Classic_2D_Lattice
    {
        public ANNNI_Classic_1D_Lattice(double j, double kappa, double b, double t, uint dimX, uint MaxSteps, EParticleType types, ELatticeBoundary boundary, uint StepsPerSaving) :
            base(j, kappa, b, t, dimX, 1, MaxSteps, types, boundary, StepsPerSaving)
        {
        }

        protected override string CONST_FNAME => "ANNNI_CLASSIC_1D";
    }
}