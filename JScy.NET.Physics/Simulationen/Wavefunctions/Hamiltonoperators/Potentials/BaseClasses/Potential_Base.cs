using System;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses
{
    public abstract class Potential_Base<T> : Hamilton_Base<T>, IPotential<T> where T : IWavefunction
    {
        public string Name { get; private set; }

        public double Potential { get; private set; }

        public int xStart { get; private set; }

        public int xEnd { get; private set; }

        public int yStart { get; private set; }

        public int yEnd { get; private set; }

        public int zStart { get; private set; }

        public int zEnd { get; private set; }

        protected Potential_Base(string name, double Vmax, int xSTART, int xEND, int ySTART, int yEND, int zSTART, int zEND)
        {
            Name = name;
            Potential = Vmax;
            xStart = xSTART;
            xEnd = xEND;
            yStart = ySTART;
            yEnd = yEND;
            zStart = zSTART;
            zEnd = zEND;
        }

        protected IWavefunction getPsiV(T psi)
        {
            IWavefunction psiV = (IWavefunction)Activator.CreateInstance(psi.GetType(), psi.WFInfo, psi.CalcMethod);
            int dimYZ = psi.WFInfo.DimY * psi.WFInfo.DimZ;
            int idx, i, j, k;
            for (i = xStart; i < xEnd; i++)
                for (j = yStart; j < yEnd; j++)
                    for (k = zStart; k < zEnd; k++)
                    {
                        idx = dimYZ * k + psi.WFInfo.DimX * j + i;
                        psiV.field[idx] = psi.field[idx];
                    }
            return psiV;
        }

        public override T HPsi(T psi) => (T)(getPsiV(psi) * Potential);
    }
}