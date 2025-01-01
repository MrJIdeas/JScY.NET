using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.Classes
{
    public class U_T : IU_T
    {
        public U_T(double t_step)
        {
            t_STEP = t_step;
            hamtodelete = new List<IHamilton>();
        }

        public double t_STEP { get; private set; }

        private List<IHamilton> hamtodelete { get; set; }

        public Orbital Do(Orbital orb, List<IHamilton> Hamiltons)
        {
            IWavefunction WFEnd = orb.WF.Clone();
            int n = 1;
            IWavefunction WF1 = PsiNTerm(orb.WF, Hamiltons, n);
            WFEnd += WF1;

            while (WF1.Norm() > double.Epsilon)
            {
                n++;
                WF1 = PsiNTerm(WF1, Hamiltons, n);
                WFEnd += WF1;
            }
            orb.WF = WFEnd;
            return orb;
        }

        protected IWavefunction PsiNTerm(IWavefunction WF, List<IHamilton> Hamiltons, int n)
        {
            IWavefunction WF1 = (IWavefunction)Activator.CreateInstance(WF.GetType(), WF.WFInfo, WF.CalcMethod);
            hamtodelete.Clear();

            foreach ((IHamilton ham, IWavefunction hampsi) in from ham in Hamiltons
                                                              let hampsi = ham.HPsi(WF)
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

            WF1 *= -Complex.ImaginaryOne * t_STEP / n;
            return WF1;
        }
    }
}