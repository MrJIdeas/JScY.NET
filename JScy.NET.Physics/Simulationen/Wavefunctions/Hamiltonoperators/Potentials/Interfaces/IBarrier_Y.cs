using System;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces
{
    public interface IBarrier_Y : IBarrier
    {
        public Tuple<int, int> limit_y { get; }
    }
}