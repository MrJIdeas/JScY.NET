﻿using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Classic.Interfaces;
using System;
using System.Collections.Generic;

namespace JScience.Physik.Simulationen.Spins.Classic.VarTypes
{
    public class Spin_ANNNI_Classic : Spin_Ising_Classic
    {
        public Spin_ANNNI_Classic(EParticleType eParticleType, uint x, uint y, uint z) : base(eParticleType, x, y, z)
        {
            AfterNeighbors = new List<Spin_Ising_Classic>();
        }

        private List<Spin_Ising_Classic> AfterNeighbors { get; set; }

        public List<Spin_Ising_Classic> getAfterNeighbors() => AfterNeighbors;

        public void AddAfterNeighbor<T>(T neighbor) where T : ISpin_Classic
        {
            if (!(neighbor is Spin_Ising_Classic))
                throw new Exception("Spin is not classic Ising!");
            if (neighbor.Equals(this))
                return;
            if (!AfterNeighbors.Contains(neighbor as Spin_Ising_Classic))
                AfterNeighbors.Add(neighbor as Spin_Ising_Classic);
        }

        public void RemoveAfterNeighbor<T>(T neighbor) where T : ISpin_Classic
        {
            if (neighbor is Spin_Ising_Classic && AfterNeighbors.Contains(neighbor as Spin_Ising_Classic))
            {
                AfterNeighbors.Remove(neighbor as Spin_Ising_Classic);
            }
            else
                throw new Exception("Spin is not classic Ising!");
        }
    }
}