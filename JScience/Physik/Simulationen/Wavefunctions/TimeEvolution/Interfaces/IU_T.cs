using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System.Collections.Generic;

namespace JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Interfaces
{
    public interface IU_T<T> where T : IWavefunction
    {
        decimal t_STEP { get; }

        T Do(T WF, List<IHamilton<T>> Hamiltons);
    }
}