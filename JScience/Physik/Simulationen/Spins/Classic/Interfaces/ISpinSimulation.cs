using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Enums;

namespace JScience.Physik.Simulationen.Spins.Classic.Interfaces
{
    public interface ISpinSimulation
    {
        double B { get; }
        string Bezeichnung { get; }
        ELatticeBoundary Boundary { get; }
        uint DimLength { get; }
        double J { get; }
        EMagnetiseType MagType { get; }
        ulong n { get; }
        EParticleType ParticleType { get; }
        double T { get; }
    }
}