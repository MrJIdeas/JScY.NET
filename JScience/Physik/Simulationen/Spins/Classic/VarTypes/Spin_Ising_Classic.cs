using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Classic.Interfaces;
using System;

namespace JScience.Physik.Simulationen.Spins.Classic.VarTypes
{
    public class Spin_Ising_Classic : Spin_Classic, IEquatable<ISpin_Classic>
    {
        public Spin_Ising_Classic(EParticleType eParticleType, uint x, uint y, uint z) : base(x, y, z, eParticleType)
        {
        }

        public override double getComponent(uint index)
        {
            if (index == 0)
                return Spin;
            else
                throw new Exception("No Valid Component Index");
        }

        public override void Flip() => Spin *= -1;
    }
}