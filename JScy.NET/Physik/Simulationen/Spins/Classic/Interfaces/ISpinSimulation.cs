using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Enums;

namespace JScience.Physik.Simulationen.Spins.Classic.Interfaces
{
    /// <summary>
    /// Interface für Spinsimulation
    /// </summary>
    public interface ISpinSimulation
    {
        /// <summary>
        /// Magnetische Flussdichte.
        /// </summary>
        double B { get; }

        /// <summary>
        /// Bezeichnung der Simulation.
        /// </summary>
        string Bezeichnung { get; }

        /// <summary>
        /// Begrenzung.
        /// </summary>
        ELatticeBoundary Boundary { get; }

        /// <summary>
        /// Dimensionen.
        /// </summary>
        uint DimLength { get; }

        /// <summary>
        /// Kopplungskonstante der Spins.
        /// </summary>
        double J { get; }

        /// <summary>
        /// Magnetisierungstyp.
        /// </summary>
        EMagnetiseType MagType { get; }

        /// <summary>
        /// Anzahl Spins.
        /// </summary>
        ulong n { get; }

        /// <summary>
        /// Partikeltypen.
        /// </summary>
        EParticleType ParticleType { get; }

        /// <summary>
        /// Temperatur
        /// </summary>
        double T { get; }
    }
}