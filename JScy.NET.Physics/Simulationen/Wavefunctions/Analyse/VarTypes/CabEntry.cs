using System.Numerics;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes
{
    public struct CabEntry
    {
        public string ExitName { get; set; }
        public double t { get; set; }
        public Complex cab { get; set; }

        public double GetCab2() => (Complex.Conjugate(cab) * cab).Real;
    }
}