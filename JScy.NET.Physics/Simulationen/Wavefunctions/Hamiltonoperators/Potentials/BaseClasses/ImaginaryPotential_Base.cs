using System.Numerics;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses
{
    public abstract class ImaginaryPotential_Base : Potential_Base, IPotential
    {
        private Complex ImagPotential { get; set; }

        protected ImaginaryPotential_Base(string name, double Vmax, int xSTART, int xEND, int ySTART, int yEND, int zSTART, int zEND) : base(name, Vmax, xSTART, xEND, ySTART, yEND, zSTART, zEND) => ImagPotential = Potential * Complex.ImaginaryOne;

        public override IWavefunction HPsi(IWavefunction psi) => (getPsiV(psi) * ImagPotential);
    }
}