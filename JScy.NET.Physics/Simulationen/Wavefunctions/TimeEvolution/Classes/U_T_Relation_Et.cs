using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;
using System;
using System.Collections.Generic;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.Classes
{
    public class U_T_Relation_Et : U_T_eigen
    {
        protected override void CalcTAU(ref Orbital WF, List<IHamilton> Hamiltons)
        {
            double energy = 0d, vari = 0d;
            IWavefunction wf1 = WF.WF;
            foreach (var ham in Hamiltons)
            {
                IWavefunction hampsi = ham.HPsi(ref wf1);
                IWavefunction hampsi2 = ham.HPsi(ref hampsi);
                energy += hampsi.Norm();
                vari += hampsi2.Norm();
            }
            double varianz = vari - Math.Pow(energy, 2);
            LastTAU = 0.5 / Math.Sqrt(varianz);
        }
    }
}