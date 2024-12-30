using JScy.NET.Physik.Enums;

namespace JScy.NET.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface ISpinWaveFunction : IWavefunction
    {
        EParticleType ParticleType { get; }

        double Spin { get; }
    }
}