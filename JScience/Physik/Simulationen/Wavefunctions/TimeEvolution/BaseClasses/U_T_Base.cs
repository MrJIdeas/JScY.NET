using JScience.Mathe.ComplexNumbers.VarTypes;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.BaseClasses
{
    public abstract class U_T_Base<T> : IU_T<T> where T : IWavefunction
    {
        protected U_T_Base(decimal t_step)
        {
            t_STEP = t_step;
        }

        public decimal t_STEP { get; private set; }

        public T Do(T WF, List<IHamilton<T>> Hamiltons)
        {
            T WFEnd = (T)WF.Clone();
            int n = 1;
            T WF1 = PsiNTerm(WF, Hamiltons, n);
            WFEnd = (T)(WFEnd + WF1);

            while (WF1.Norm() > 1E-30m)
            {
                n++;
                WF1 = PsiNTerm(WF1, Hamiltons, n);
                WFEnd = (T)(WFEnd + WF1);
            }
            return WFEnd;
        }

        protected abstract T PsiNTerm(T WF, List<IHamilton<T>> Hamiltons, int n);
    }
}