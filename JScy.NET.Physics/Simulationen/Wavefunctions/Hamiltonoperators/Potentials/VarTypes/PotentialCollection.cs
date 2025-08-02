using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF;
using System;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class PotentialCollection : Potential_Base
    {
        public PotentialCollection(string name, WFInfo wfinfo) : base(name, 0, 0, 0, 0, 0, 0, 0) => psiV = (IWavefunction)Activator.CreateInstance(wfinfo.Type, wfinfo);

        public void MigratePotential(Potential_Base OPot) => psiV += OPot.psiV;
    }
}