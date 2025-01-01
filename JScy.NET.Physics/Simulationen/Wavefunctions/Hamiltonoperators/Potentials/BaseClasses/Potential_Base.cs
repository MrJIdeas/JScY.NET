using System;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses
{
    public abstract class Potential_Base(string name, double Vmax, int xSTART, int xEND, int ySTART, int yEND, int zSTART, int zEND) : Hamilton_Base, IPotential
    {
        public string Name { get; private set; } = name;

        public double Potential { get; private set; } = Vmax;

        public int xStart { get; private set; } = xSTART;

        public int xEnd { get; private set; } = xEND;

        public int yStart { get; private set; } = ySTART;

        public int yEnd { get; private set; } = yEND;

        public int zStart { get; private set; } = zSTART;

        public int zEnd { get; private set; } = zEND;

        protected IWavefunction getPsiV(IWavefunction psi)
        {
            IWavefunction psiV = (IWavefunction)Activator.CreateInstance(psi.GetType(), psi.WFInfo, psi.CalcMethod);
            int dimYZ = psi.WFInfo.DimInfo.DimY * psi.WFInfo.DimInfo.DimZ;
            int idx, i, j, k;
            for (i = xStart; i < xEnd; i++)
                for (j = yStart; j < yEnd; j++)
                    for (k = zStart; k < zEnd; k++)
                    {
                        idx = dimYZ * k + psi.WFInfo.DimInfo.DimX * j + i;
                        psiV.field[idx] = psi.field[idx];
                    }
            return psiV;
        }

        public override IWavefunction HPsi(IWavefunction psi) => (getPsiV(psi) * Potential);
    }
}