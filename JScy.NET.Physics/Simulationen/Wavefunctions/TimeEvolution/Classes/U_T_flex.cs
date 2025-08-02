using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.Classes
{
    public class U_T_flex() : IU_T_flex
    {
        private List<IHamilton> hamtodelete { get; set; } = [];

        public Orbital Do(ref Orbital orb, List<IHamilton> Hamiltons, double t_step)
        {
            IWavefunction WFEnd = orb.WF.Clone();
            int n = 1;
            IWavefunction WF1 = PsiNTerm(orb.WF, Hamiltons, n, t_step);
            WFEnd += WF1;

            while (WF1.Norm() >= double.Epsilon)
            {
                n++;
                WF1 = PsiNTerm(WF1, Hamiltons, n, t_step);
                WFEnd += WF1;
            }
            orb.WF = WFEnd;
            return orb;
        }

        protected IWavefunction PsiNTerm(IWavefunction WF, List<IHamilton> Hamiltons, int n, double t_step)
        {
            IWavefunction WF1 = (IWavefunction)Activator.CreateInstance(WF.GetType(), WF.WFInfo);
            hamtodelete.Clear();

            foreach ((IHamilton ham, IWavefunction hampsi) in from ham in Hamiltons
                                                              let hampsi = ham.HPsi(ref WF)
                                                              select (ham, hampsi))
            {
                if (hampsi.Norm() > double.Epsilon || ham is IPotential)
                    WF1 += hampsi;
                else
                    hamtodelete.Add(ham);
            }

            foreach (IHamilton ham in from ham in hamtodelete
                                      where Hamiltons.Contains(ham)
                                      select ham)
            {
                Hamiltons.Remove(ham);
            }

            WF1 *= -Complex.ImaginaryOne * t_step / n;
            return WF1;
        }
    }
}