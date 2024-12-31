using System.Collections.Generic;
using System.Numerics;
using JScy.NET.Physics.Enums;
using JScy.NET.Physics.Simulationen.Spins.Enums;

namespace JScy.NET.Physics.Simulationen.Spins.Classic.Interfaces
{
    /// <summary>
    /// Interface für Klassischen Spin.
    /// </summary>
    public interface ISpin_Classic
    {
        /// <summary>
        /// Art des Spins.
        /// </summary>
        ESpinType spinType { get; }

        /// <summary>
        /// Partikeltyp.
        /// </summary>
        EParticleType ParticleType { get; }

        /// <summary>
        /// Position im Raum.
        /// </summary>
        Vector3 PositionXYZ { get; }

        /// <summary>
        /// Methode zum Abrufen des Betrages des Spinvektors.
        /// </summary>
        /// <returns>Betrag.</returns>
        double getAbs();

        /// <summary>
        /// Methode zur Ausgabe der Komponente.
        /// </summary>
        /// <param name="index">Komponente-</param>
        /// <returns>Wert.</returns>
        double getComponent(uint index);

        /// <summary>
        /// Füge dem Spin einen Nachbarn hinzu.
        /// </summary>
        /// <typeparam name="T">Spintyp.</typeparam>
        /// <param name="neighbor">Nachbar.</param>
        void AddNeighbor<T>(T neighbor) where T : ISpin_Classic;

        /// <summary>
        /// Entferne den Nachbarspin.
        /// </summary>
        /// <typeparam name="T">Spintyp.</typeparam>
        /// <param name="neighbor">Nachbar.</param>
        void RemoveNeighbor<T>(T neighbor) where T : ISpin_Classic;

        /// <summary>
        /// Gibt alle Nachbarn des Spins aus.
        /// </summary>
        /// <returns>Liste mit Nachbarn.</returns>
        List<ISpin_Classic> getNeighbors();

        /// <summary>
        /// Wechsel die Spinrichtung um 180°.
        /// </summary>
        void Flip();
    }
}