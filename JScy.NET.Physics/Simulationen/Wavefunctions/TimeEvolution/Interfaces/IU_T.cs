using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using System.Collections.Generic;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.Interfaces
{
    public interface IU_T<T> where T : IWavefunction
    {
        double t_STEP { get; }

        T Do(T WF, List<IHamilton<T>> Hamiltons);
    }
}