using JScy.NET.Physics.Enums;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces
{
    public interface ISpinWaveFunction : IWavefunction
    {
        EParticleType ParticleType { get; }

        double Spin { get; }
    }
}