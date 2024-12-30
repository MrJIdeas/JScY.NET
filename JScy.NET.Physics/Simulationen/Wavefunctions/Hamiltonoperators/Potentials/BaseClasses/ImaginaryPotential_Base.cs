using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using System.Numerics;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses
{
    public abstract class ImaginaryPotential_Base<T> : Potential_Base<T>, IPotential<T> where T : IWavefunction
    {
        private Complex ImagPotential { get; set; }

        protected ImaginaryPotential_Base(string name, double Vmax, int xSTART, int xEND, int ySTART, int yEND, int zSTART, int zEND) : base(name, Vmax, xSTART, xEND, ySTART, yEND, zSTART, zEND)
        {
            ImagPotential = Potential * Complex.ImaginaryOne;
        }

        public override T HPsi(T psi) => (T)(getPsiV(psi) * ImagPotential);
    }
}