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
    public class U_T<T> : IU_T<T> where T : IWavefunction
    {
        public U_T(double t_step)
        {
            t_STEP = t_step;
            hamtodelete = new List<IHamilton<T>>();
        }

        public double t_STEP { get; private set; }

        private List<IHamilton<T>> hamtodelete { get; set; }

        public Orbital Do(Orbital orb, List<IHamilton<T>> Hamiltons)
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
            return new Orbital(WFEnd, orb.Spin, orb.OrbitalBezeichnung);
        }

        protected IWavefunction PsiNTerm(IWavefunction WF, List<IHamilton<T>> Hamiltons, int n)
        {
            IWavefunction WF1 = (T)Activator.CreateInstance(WF.GetType(), WF.WFInfo, WF.CalcMethod);
            hamtodelete.Clear();

            foreach ((IHamilton<T> ham, T hampsi) in from ham in Hamiltons
                                                     let hampsi = ham.HPsi((T)WF)
                                                     select (ham, hampsi))
            {
                if (hampsi.Norm() > double.Epsilon || ham is IPotential<T>)
                    WF1 += hampsi;
                else
                    hamtodelete.Add(ham);
            }

            foreach (IHamilton<T> ham in from ham in hamtodelete
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