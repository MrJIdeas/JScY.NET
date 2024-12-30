using System.Numerics;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.VarTypes
{
    public struct Eta_K
    {
        public double k { get; set; }
        public Complex Eta { get; set; }

        public Eta_K Conjugate() => new Eta_K() { k = k, Eta = Complex.Conjugate(Eta) };

        public double Norm() => (Eta * Complex.Conjugate(Eta)).Real;

        public static Eta_K operator *(Eta_K a, double b)
        {
            a.Eta *= b;
            return a;
        }

        public static Eta_K operator /(Eta_K a, double b)
        {
            a.Eta /= b;
            return a;
        }
    }
}