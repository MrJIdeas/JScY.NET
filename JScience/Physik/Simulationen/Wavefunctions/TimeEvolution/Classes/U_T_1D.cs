using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Interfaces;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Classes
{
    public class U_T_1D<T> : IU_T<T> where T : IWF_1D
    {
        public U_T_1D(double t_step)
        {
            t_STEP = t_step;
        }

        public double t_STEP { get; private set; }

        public T Do(T WF, List<IHamilton<T>> Hamiltons)
        {
            T WFEnd = (T)((T)WF.Clone() + WF);
            T WF1 = PsiNTerm(WF, Hamiltons, 1);
            WFEnd = (T)(WFEnd + WF1);
            int n = 2;
            while (WF1.Norm() > double.Epsilon)
            {
                T WF2 = PsiNTerm(WF1, Hamiltons, n);
                WFEnd = (T)(WF + WF2);
                WF1 = (T)(WF2.Clone());
                n++;
            }
            return WFEnd;
        }

        private T PsiNTerm(T WF, List<IHamilton<T>> Hamiltons, int n)
        {
            T WF1 = (T)Activator.CreateInstance(WF.GetType(), WF.DimX);
            foreach (var ham in Hamiltons)
                WF1 = (T)(WF1 + ham.HPsi(WF));
            WF1 = (T)(-Complex.ImaginaryOne * t_STEP / n * WF1);
            return WF1;
        }
    }
}