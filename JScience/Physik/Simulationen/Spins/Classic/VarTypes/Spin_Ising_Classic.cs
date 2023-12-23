using JScience.Physik.AttributesCustom;
using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Classic.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JScience.Physik.Simulationen.Spins.Classic.VarTypes
{
    public class Spin_Ising_Classic : ISpin_Classic, IEquatable<ISpin_Classic>
    {
        public Spin_Ising_Classic(EParticleType eParticleType, uint x, uint y, uint z)
        {
            PositionXYZ = new Tuple<uint, uint, uint>(x, y, z);
            ParticleType = eParticleType;
            Neighbors = new List<Spin_Ising_Classic>();
            AktValue = (short)(ParticleType.GetType().GetTypeInfo().GetDeclaredField(ParticleType.ToString()).GetCustomAttribute(typeof(SpinAttribute)) as SpinAttribute).val;
        }

        private List<Spin_Ising_Classic> Neighbors { get; set; }
        private short AktValue { get; set; }
        public Tuple<uint, uint, uint> PositionXYZ { get; private set; }
        public EParticleType ParticleType { get; private set; }

        public double getAbs()
        {
            return Math.Abs(AktValue);
        }

        public double getComponent(uint index)
        {
            if (index == 0)
                return AktValue;
            else
                throw new Exception("No Valid Component Index");
        }

        public List<Spin_Ising_Classic> getNeighbors() => Neighbors;

        public void AddNeighbor<T>(T neighbor) where T : ISpin_Classic
        {
            if (!(neighbor is Spin_Ising_Classic))
                throw new Exception("Spin is not classic Ising!");
            if (neighbor.Equals(this))
                return;
            if (!Neighbors.Contains(neighbor as Spin_Ising_Classic))
                Neighbors.Add(neighbor as Spin_Ising_Classic);
        }

        public void RemoveNeighbor<T>(T neighbor) where T : ISpin_Classic
        {
            if (neighbor is Spin_Ising_Classic && Neighbors.Contains(neighbor as Spin_Ising_Classic))
            {
                Neighbors.Remove(neighbor as Spin_Ising_Classic);
            }
            else
                throw new Exception("Spin is not classic Ising!");
        }

        public void Flip() => AktValue *= -1;

        public bool Equals(ISpin_Classic other) => PositionXYZ.Item1 == other.PositionXYZ.Item1 && PositionXYZ.Item2 == other.PositionXYZ.Item2 && PositionXYZ.Item3 == other.PositionXYZ.Item3;
    }
}