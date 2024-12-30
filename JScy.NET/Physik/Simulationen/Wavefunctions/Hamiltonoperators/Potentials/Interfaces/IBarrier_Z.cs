namespace JScy.NET.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces
{
    public interface IBarrier_Z : IBarrier
    {
        public int zStart { get; }

        public int zEnd { get; }
    }
}