using System.Numerics;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes
{
    public struct SabEntry
    {
        public string ExitName { get; set; }
        public double v { get; set; }
        public Complex sab { get; set; }

        public readonly double GetSab2() => (Complex.Conjugate(sab) * sab).Real;
    }
}