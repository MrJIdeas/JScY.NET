using JScience.Physik.Enums;
using System;

namespace JScience.Physik.Simulationen.Spins.Classic.Interfaces
{
    public interface ISpin_Classic
    {
        EParticleType ParticleType { get; }
        Tuple<uint, uint, uint> PositionXYZ { get; }

        double getAbs();

        double getComponent(uint index);

        void AddNeighbor<T>(T neighbor) where T : ISpin_Classic;

        void RemoveNeighbor<T>(T neighbor) where T : ISpin_Classic;

        void Flip();
    }
}