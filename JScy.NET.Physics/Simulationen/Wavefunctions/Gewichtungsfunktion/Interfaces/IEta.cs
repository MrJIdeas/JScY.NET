using JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.VarTypes;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.Interfaces
{
    public interface IEta
    {
        Eta_K GetEta(int idx);

        Eta_K[] GetEta();

        double GetNorm();
    }
}