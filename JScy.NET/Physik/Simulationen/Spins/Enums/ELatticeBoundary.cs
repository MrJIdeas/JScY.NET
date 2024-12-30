namespace JScience.Physik.Simulationen.Spins.Enums
{
    /// <summary>
    /// Enum für Gitterbegrenzungen.
    /// </summary>
    public enum ELatticeBoundary
    {
        /// <summary>
        /// Keine Angabe.
        /// </summary>
        None = 0,

        /// <summary>
        /// Feste Ränder.
        /// </summary>
        Reflection = 1,

        /// <summary>
        /// Periodische Fortsetzung.
        /// </summary>
        Periodic = 2
    }
}