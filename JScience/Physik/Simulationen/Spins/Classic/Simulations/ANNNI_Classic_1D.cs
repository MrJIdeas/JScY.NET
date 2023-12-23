using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Enums;

namespace JScience.Physik.Simulationen.Spins.Classic.Simulations
{
    public class ANNNI_Classic_1D : ANNNI_Classic_2D
    {
        public ANNNI_Classic_1D(double j, double kappa, double b, double t, uint dimX, uint MaxSteps, EParticleType types, ELatticeBoundary boundary, uint StepsPerSaving) :
            base(j, kappa, b, t, dimX, 1, MaxSteps, types, boundary, StepsPerSaving)
        {
        }

        protected override string CONST_FNAME => "ANNNI_CLASSIC_1D";
    }
}