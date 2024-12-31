using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using JScy.NET.Physics.AttributesCustom;
using JScy.NET.Physics.Enums;
using JScy.NET.Physics.Simulationen.Spins.Classic.Interfaces;
using JScy.NET.Physics.Simulationen.Spins.Enums;

namespace JScy.NET.Physics.Simulationen.Spins.Classic.VarTypes
{
    public abstract class Spin_Classic : ISpin_Classic, IEquatable<ISpin_Classic>
    {
        protected Spin_Classic(float PosX, float PosY, float PosZ, EParticleType pType)
        {
            Neighbors = new List<ISpin_Classic>();
            PositionXYZ = new Vector3(PosX, PosY, PosZ);
            ParticleType = pType;
            Spin = (double)(ParticleType.GetType().GetTypeInfo().GetDeclaredField(ParticleType.ToString()).GetCustomAttribute(typeof(SpinAttribute)) as SpinAttribute).val;
        }

        public ESpinType spinType { get; protected set; }

        public double Spin { get; protected set; }
        protected List<ISpin_Classic> Neighbors { get; private set; }

        public Vector3 PositionXYZ { get; private set; }
        public EParticleType ParticleType { get; private set; }

        public void AddNeighbor<T>(T neighbor) where T : ISpin_Classic
        {
            if (neighbor.spinType == spinType
                && !neighbor.Equals(this)
                && !Neighbors.Contains(neighbor))
                Neighbors.Add(neighbor);
        }

        public abstract void Flip();

        public double getAbs() => Math.Abs(Spin);

        public abstract double getComponent(uint index);

        public void RemoveNeighbor<T>(T neighbor) where T : ISpin_Classic
        {
            if (Neighbors.Contains(neighbor))
                Neighbors.Remove(neighbor);
        }

        public bool Equals(ISpin_Classic other) => spinType == other.spinType && PositionXYZ.Equals(other.PositionXYZ);

        public List<ISpin_Classic> getNeighbors() => Neighbors;
    }
}