using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Enums;
using ScottPlot;
using System.Collections.Concurrent;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWavefunction
    {
        int Dimensions { get; }
        ELatticeBoundary Boundary { get; }

        OrderablePartitioner<Tuple<int, int>> rangePartitioner { get; }

        double Norm();

        IWavefunction Conj();

        IWavefunction GetShift(EShift shift);

        void Clear();

        IWavefunction Clone();

        Image GetImage(int width, int height);
    }
}