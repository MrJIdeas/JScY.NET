using System;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces
{
    public interface IBarrier_X : IBarrier
    {
        public Tuple<int, int> limit_x { get; }
    }
}