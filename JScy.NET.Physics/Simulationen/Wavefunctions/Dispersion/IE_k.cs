namespace JScy.NET.Physics.Simulationen.Wavefunctions.Dispersion
{
    public interface IE_k
    {
        double Calculate(double k);

        double GetE(double k);
    }
}