using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Enums;

namespace JScience.Physik.Simulationen.Spins.Classic.Simulations
{
    public class ANNNI_Classic_2D : ANNNI_Classic_3D
    {
        public ANNNI_Classic_2D(double j, double kappa, double b, double t, uint dimX, uint dimY, uint MaxSteps, EParticleType types, ELatticeBoundary boundary, uint StepsPerSaving) :
            base(j, kappa, b, t, dimX, dimY, 1, MaxSteps, types, boundary, StepsPerSaving)
        {
        }

        protected override string CONST_FNAME => "ANNNI_CLASSIC_2D";
    }
}