using JScy.NET.Mathe.Vectors.Enums;

namespace JScy.NET.Mathe.Vectors.Interfaces
{
    /// <summary>
    /// Interface für einen Vektor.
    /// </summary>
    public interface IVector
    {
        /// <summary>
        /// Vektorbezeichnung.
        /// </summary>
        string Bezeichnung { get; }

        /// <summary>
        /// Anzahl Dimensionen.
        /// </summary>
        int Dimensions { get; }

        /// <summary>
        /// Betragsquadrat.
        /// </summary>
        double Abs2 { get; }

        /// <summary>
        /// Betrag.
        /// </summary>
        double Norm { get; }

        /// <summary>
        /// Vektortyp.
        /// </summary>
        EVecType VectorType { get; }
    }
}