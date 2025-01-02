using System;
using System.Numerics;
using System.Threading.Tasks;
using JScy.NET.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses
{
    public abstract class Potential_Base : Hamilton_Base, IPotential
    {
        public string Name { get; private set; }

        public double Potential { get; private set; }

        public Tuple<int, int> limit_x { get; private set; }
        public Tuple<int, int> limit_y { get; private set; }
        public Tuple<int, int> limit_z { get; private set; }

        public Potential_Base(string name, double Vmax, int xSTART, int xEND, int ySTART, int yEND, int zSTART, int zEND)
        {
            Name = name;
            Potential = Vmax;
            limit_x = new Tuple<int, int>(xSTART, xEND);
            limit_y = new Tuple<int, int>(ySTART, yEND);
            limit_z = new Tuple<int, int>(zSTART, zEND);
        }

        protected IWavefunction getPsiV(ref IWavefunction psi)
        {
            IWavefunction psiV = (IWavefunction)Activator.CreateInstance(psi.GetType(), psi.WFInfo);
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

        public override IWavefunction HPsi(ref IWavefunction psi) => (getPsiV(ref psi) * Potential);
    }
}