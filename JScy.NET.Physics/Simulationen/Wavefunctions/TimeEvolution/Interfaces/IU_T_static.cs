using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;
using System.Collections.Generic;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.Interfaces
{
    public interface IU_T_static : IU_T
    {
        double t_STEP { get; }

        Orbital Do(ref Orbital WF, List<IHamilton> Hamiltons);
    }
}