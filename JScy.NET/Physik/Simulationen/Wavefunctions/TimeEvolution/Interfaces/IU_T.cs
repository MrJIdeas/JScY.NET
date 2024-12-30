using JScy.NET.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScy.NET.Physik.Simulationen.Wavefunctions.Interfaces;
using System.Collections.Generic;

namespace JScy.NET.Physik.Simulationen.Wavefunctions.TimeEvolution.Interfaces
{
    public interface IU_T<T> where T : IWavefunction
    {
        double t_STEP { get; }

        T Do(T WF, List<IHamilton<T>> Hamiltons);
    }
}