using JScy.NET.Physics.Simulationen.Wavefunctions.AttributesCustom;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Enums
{
    /// <summary>
    /// Enum für Bewegungsrichtung.
    /// </summary>
    public enum EShift
    {
        /// <summary>
        /// kein Shift.
        /// </summary>
        [MathSign(0)]
        None = 0,

        /// <summary>
        /// Verschiebung nach Links.
        /// </summary>
        [MathSign(-1)]
        Xm = 1,

        /// <summary>
        /// Verschiebung nach rechts.
        /// </summary>
        [MathSign(1)]
        Xp = 2,

        /// <summary>
        /// Verschiebung nach unten.
        /// </summary>
        [MathSign(-1)]
        Ym = 3,

        /// <summary>
        /// Verschiebung nach oben.
        /// </summary>
        [MathSign(1)]
        Yp = 4,

        /// <summary>
        /// Verschiebung nach hinten.
        /// </summary>
        [MathSign(-1)]
        Zm = 5,

        /// <summary>
        /// Verschiebung nach vorne.
        /// </summary>
        [MathSign(1)]
        Zp = 6
    }
}