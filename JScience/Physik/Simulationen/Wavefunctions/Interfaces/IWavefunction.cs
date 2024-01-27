using JScience.Physik.Simulationen.Wavefunctions.Enums;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWavefunction
    {
        int Dimensions { get; }

        double Norm();

        IWavefunction Conj();

        IWavefunction GetShift(EShift shift);

        void Clear();
    }
}