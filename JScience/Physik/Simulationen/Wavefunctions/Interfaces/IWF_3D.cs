using JScience.Mathe.ComplexNumbers.VarTypes;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWF_3D : IWF_2D
    {
        int DimZ { get; }
        DecComplex this[int x, int y, int z] { get; }

        void SetField(int x, int y, int z, DecComplex value);

        decimal getNorm(int x, int y, int z);
    }
}