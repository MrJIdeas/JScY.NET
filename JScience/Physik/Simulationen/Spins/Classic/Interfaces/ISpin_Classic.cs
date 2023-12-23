using JScience.Physik.Enums;
using System;
using System.Collections.Generic;

namespace JScience.Physik.Simulationen.Spins.Classic.Interfaces
{
    public interface ISpin_Classic
    {
        EParticleType ParticleType { get; }
        Tuple<double, double, double> PositionXYZ { get; }

        double getAbs();

        double getComponent(uint index);

        void AddNeighbor<T>(T neighbor) where T : ISpin_Classic;

        void RemoveNeighbor<T>(T neighbor) where T : ISpin_Classic;

        List<ISpin_Classic> getNeighbors();

        void Flip();
    }
}