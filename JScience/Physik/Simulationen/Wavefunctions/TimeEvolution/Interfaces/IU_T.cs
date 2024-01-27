using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Interfaces
{
    public interface IU_T<T> where T : IWavefunction
    {
        double t_STEP { get; }

        T Do(T WF, List<IHamilton<T>> Hamiltons);
    }
}