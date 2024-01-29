using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Enums;
using ScottPlot;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWavefunction
    {
        int Dimensions { get; }
        ELatticeBoundary Boundary { get; }

        double Norm();

        IWavefunction Conj();

        IWavefunction GetShift(EShift shift);

        void Clear();

        IWavefunction Clone();

        Image GetImage(int width, int height);
    }
}