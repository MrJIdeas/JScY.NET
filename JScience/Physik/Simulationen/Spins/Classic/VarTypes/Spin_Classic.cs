using JScience.Physik.AttributesCustom;
using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Classic.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JScience.Physik.Simulationen.Spins.Classic.VarTypes
{
    public abstract class Spin_Classic : ISpin_Classic, IEquatable<ISpin_Classic>
    {
        protected Spin_Classic(double PosX, double PosY, double PosZ, EParticleType pType)
        {
            Neighbors = new List<ISpin_Classic>();
            PositionXYZ = new Tuple<double, double, double>(PosX, PosY, PosZ);
            ParticleType = pType;
            Spin = (double)(ParticleType.GetType().GetTypeInfo().GetDeclaredField(ParticleType.ToString()).GetCustomAttribute(typeof(SpinAttribute)) as SpinAttribute).val;
        }

        public double Spin { get; protected set; }
        protected List<ISpin_Classic> Neighbors { get; private set; }

        public Tuple<double, double, double> PositionXYZ { get; private set; }
        public EParticleType ParticleType { get; private set; }

        public void AddNeighbor<T>(T neighbor) where T : ISpin_Classic
        {
            if (!Neighbors.Contains(neighbor))
                Neighbors.Add(neighbor);
        }

        public abstract void Flip();

        public abstract double getAbs();

        public abstract double getComponent(uint index);

        public void RemoveNeighbor<T>(T neighbor) where T : ISpin_Classic
        {
            if (Neighbors.Contains(neighbor))
                Neighbors.Remove(neighbor);
        }

        public abstract bool Equals(ISpin_Classic other);

        public List<ISpin_Classic> getNeighbors() => Neighbors;
    }
}