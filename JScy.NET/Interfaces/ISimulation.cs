using System;

namespace JScy.NET.Interfaces
{
    /// <summary>
    /// Interface für Simulationen.
    /// </summary>
    public interface ISimulation
    {
        /// <summary>
        /// Simulationsbezeichnung.
        /// </summary>
        string Bezeichnung { get; }

        /// <summary>
        /// Zeitstempel Beginn.
        /// </summary>
        DateTime StartDate { get; }

        /// <summary>
        /// Zeitstempel Ende.
        /// </summary>
        DateTime EndDate { get; }

        /// <summary>
        /// Simulation beendet.
        /// </summary>
        bool Finished { get; }

        /// <summary>
        /// Methode zum Starten der Simulation.
        /// </summary>
        void Start();

        /// <summary>
        /// Methode zum Beendender Simulation.
        /// </summary>
        void Stop();

        /// <summary>
        /// Methode zum Zurücksetzen der Simulation.
        /// </summary>
        void Reset();
    }
}