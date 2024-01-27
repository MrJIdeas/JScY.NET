using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Interfaces;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Classes
{
    internal class U_T_1D<T> : IU_T<T> where T : IWF_1D
    {
        public double t_STEP { get; private set; }

        public T Do(T WF, List<IHamilton<T>> Hamiltons)
        {
            T WFp1 = (T)Activator.CreateInstance(typeof(T), true, WF.DimX);
            foreach (var ham in Hamiltons)
            {
                var hpsi = ham.HPsi(WF);
                if (hpsi.Norm() < double.Epsilon)
                    Hamiltons.Remove(ham);
                else
                    WFp1 = (T)(hpsi + WFp1);
            }
            T erg = (T)(-Complex.ImaginaryOne * t_STEP * (WF + WFp1));
            if (WFp1.Norm() < double.Epsilon)
                return erg;
            else
                return Do(erg, Hamiltons);
        }
    }
}