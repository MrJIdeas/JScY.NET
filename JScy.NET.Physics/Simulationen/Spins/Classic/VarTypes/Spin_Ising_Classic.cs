using System;
using JScy.NET.Physics.Enums;
using JScy.NET.Physics.Simulationen.Spins.Classic.Interfaces;
using JScy.NET.Physics.Simulationen.Spins.Enums;

namespace JScy.NET.Physics.Simulationen.Spins.Classic.VarTypes
{
    public class Spin_Ising_Classic : Spin_Classic, IEquatable<ISpin_Classic>
    {
        public Spin_Ising_Classic(EParticleType eParticleType, uint x, uint y, uint z) : base(x, y, z, eParticleType)
        {
            spinType = ESpinType.Ising;
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