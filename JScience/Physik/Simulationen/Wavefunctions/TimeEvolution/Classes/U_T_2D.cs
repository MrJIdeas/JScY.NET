using JScience.Mathe.ComplexNumbers.VarTypes;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Classes
{
    public class U_T_2D<T> : IU_T<T> where T : IWF_2D
    {
        public U_T_2D(decimal t_step)
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

        private T PsiNTerm(T WF, List<IHamilton<T>> Hamiltons, int n)
        {
            T WF1 = (T)Activator.CreateInstance(WF.GetType(), WF.DimX, WF.DimY, WF.Boundary);
            List<IHamilton<T>> hamtodelete = new List<IHamilton<T>>();
            foreach (var (ham, hampsi) in from ham in Hamiltons
                                          let hampsi = ham.HPsi(WF)
                                          select (ham, hampsi))
            {
                if (hampsi.Norm() > 1E-30m || ham is IPotential<T>)
                    WF1 = (T)(WF1 + hampsi);
                else
                    hamtodelete.Add(ham);
            }

            foreach (var ham in from ham in hamtodelete
                                where Hamiltons.Contains(ham)
                                select ham)
            {
                Hamiltons.Remove(ham);
            }

            WF1 = (T)(-1 * DecComplex.ImaginaryOne * t_STEP / n * WF1);
            return WF1;
        }
    }
}