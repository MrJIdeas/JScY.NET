﻿using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.BaseClasses
{
    public abstract class Hamilton_Base : IHamilton
    {
        public abstract IWavefunction HPsi(ref IWavefunction psi);

        public double E(ref IWavefunction psi)
        {
            double erg = 0;
            IWavefunction erg2 = psi.Conj() * HPsi(ref psi);
            for (int i = 0; i < erg2.WFInfo.DimInfo.DimX * erg2.WFInfo.DimInfo.DimY * erg2.WFInfo.DimInfo.DimZ; i++)
                erg += erg2[i].Real;
            return erg;
        }
    }
}