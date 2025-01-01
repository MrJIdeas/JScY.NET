namespace JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces
{
    public interface IWF_1D : IWavefunction
    {
        int?[] getNeighborsX(int i, int positions = 1);

        int? getNeightborX(int i, int direction);
    }
}