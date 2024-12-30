using JScy.NET.Physik.AttributesCustom;

namespace JScy.NET.Physik.Enums
{
    /// <summary>
    /// Partikeltyp.
    /// </summary>
    public enum EParticleType
    {
        None = 0,

        [Charge(0)]
        [Spin(1)]
        [ParticleFamily(EParticleFamily.Boson)]
        Photon = 1,

        [Spin(1)]
        [Charge(1)]
        [ParticleFamily(EParticleFamily.Boson)]
        WBosonPlus = 2,

        [Spin(1)]
        [Charge(-1)]
        [ParticleFamily(EParticleFamily.Boson)]
        WBosonMinus = 2,

        [Charge(0)]
        [Spin(1)]
        [ParticleFamily(EParticleFamily.Boson)]
        ZBoson = 3,

        [Charge(0)]
        [Spin(1)]
        [ParticleFamily(EParticleFamily.Boson)]
        Gluon = 4,

        [Charge(0)]
        [Spin(0)]
        [ParticleFamily(EParticleFamily.Boson)]
        HiggsBoson = 5,

        [Spin(0.5f)]
        [ParticleFamily(EParticleFamily.Fermion)]
        Fermion = 6,

        [Spin(0.5f)]
        [Charge(-1)]
        [ParticleFamily(EParticleFamily.Lepton)]
        Electron = 7,

        [Spin(0.5f)]
        [Charge(-1)]
        [ParticleFamily(EParticleFamily.Lepton)]
        Muon = 8,

        [Spin(0.5f)]
        [Charge(-1)]
        [ParticleFamily(EParticleFamily.Lepton)]
        Tauon = 9,

        [Charge(0)]
        [Spin(0.5f)]
        [ParticleFamily(EParticleFamily.Lepton)]
        ElectronNeutrino = 10,

        [Charge(0)]
        [Spin(0.5f)]
        [ParticleFamily(EParticleFamily.Lepton)]
        MuonNeutrino = 11,

        [Charge(0)]
        [Spin(0.5f)]
        [ParticleFamily(EParticleFamily.Lepton)]
        TauonNeutrino = 12,
    }
}