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

            switch (psi.WFInfo.CalcMethod)
            {
                case ECalculationMethod.CPU:
                    int idx, i, j, k;
                    for (i = limit_x.Item1; i < limit_x.Item2; i++)
                        for (j = limit_y.Item1; j < limit_y.Item2; j++)
                            for (k = limit_z.Item1; k < limit_z.Item2; k++)
                            {
                                idx = dimYZ * k + psi.WFInfo.DimInfo.DimX * j + i;
                                psiV.field[idx] = psi.field[idx];
                            }

                    break;

                case ECalculationMethod.CPU_Multihreading:
                case ECalculationMethod.OpenCL:
                    var psi2 = psi;
                    Parallel.For(limit_x.Item1, limit_x.Item2, i =>
                    {
                        int x = i % psi2.WFInfo.DimInfo.DimX;
                        int y = (i / psi2.WFInfo.DimInfo.DimX) % psi2.WFInfo.DimInfo.DimY;
                        int z = i / (psi2.WFInfo.DimInfo.DimX * psi2.WFInfo.DimInfo.DimY);

                        if (x >= limit_x.Item1 && x < limit_x.Item2 &&
                            y >= limit_y.Item1 && y < limit_y.Item2 &&
                            z >= limit_z.Item1 && z < limit_z.Item2)
                        {
                            psiV.field[i] = psi2.field[i];
                        }
                    });
                    break;
            }
            return psiV;
        }

        public override IWavefunction HPsi(ref IWavefunction psi) => (getPsiV(ref psi) * Potential);
    }
}