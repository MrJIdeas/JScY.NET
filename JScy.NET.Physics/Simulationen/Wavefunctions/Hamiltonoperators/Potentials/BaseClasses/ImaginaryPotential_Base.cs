using System.Numerics;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses
{
    public abstract class ImaginaryPotential_Base : Potential_Base, IPotential
    {
        protected Complex ImagPotential { get; private set; }

        protected ImaginaryPotential_Base(string name, double Vmax, int xSTART, int xEND, int ySTART, int yEND, int zSTART, int zEND) : base(name, Vmax, xSTART, xEND, ySTART, yEND, zSTART, zEND) => ImagPotential = Potential * Complex.ImaginaryOne;
    }
}