using JScience.Physik.Enums;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface ISpinWaveFunction : IWavefunction
    {
        EParticleType ParticleType { get; }

        decimal Spin { get; }
    }
}