using System;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces
{
    public interface IBarrier_Z : IBarrier
    {
        public Tuple<int, int> limit_z { get; }
    }
}