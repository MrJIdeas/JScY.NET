using System.Collections.Generic;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.Interfaces
{
    public interface IU_T<T> where T : IWavefunction
    {
        double t_STEP { get; }

        Orbital Do(Orbital WF, List<IHamilton<T>> Hamiltons);
    }
}