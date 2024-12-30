using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.VarTypes
{
    public struct SabEntry
    {
        public string ExitName { get; set; }
        public double v { get; set; }
        public Complex sab { get; set; }

        public double GetSab2() => (Complex.Conjugate(sab) * sab).Real;
    }
}