using System.Numerics;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses
{
    public abstract class ImaginaryPotential_Base(string name, double Vmax, int xSTART, int xEND, int ySTART, int yEND, int zSTART, int zEND) : Potential_Base(name, Vmax, xSTART, xEND, ySTART, yEND, zSTART, zEND), IPotential
    {
        protected Complex ImagPotential => Potential * Complex.ImaginaryOne;
    }
}