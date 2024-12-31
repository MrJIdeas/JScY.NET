using System.Numerics;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.Interfaces
{
    public interface IEta
    {
        double GetNorm();

        Complex Calculate(double k);

        Complex GetEta(double k);
    }
}