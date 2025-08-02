using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;
using System.Collections.Generic;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.BaseClasses
{
    public abstract class U_T_eigen() : IU_T_eigenzeit
    {
        private readonly U_T_flex u_T_Flex = new();
        public double LastTAU { get; protected set; }

        protected abstract void CalcTAU(ref Orbital WF, List<IHamilton> Hamiltons);

        public Orbital Do(ref Orbital WF, List<IHamilton> Hamiltons)
        {
            CalcTAU(ref WF, Hamiltons);
            return u_T_Flex.Do(ref WF, Hamiltons, LastTAU);
        }
    }
}