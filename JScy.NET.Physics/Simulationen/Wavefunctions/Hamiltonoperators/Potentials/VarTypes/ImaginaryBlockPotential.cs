using JScy.NET.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF;
using System;
using System.Threading.Tasks;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class ImaginaryBlockPotential : ImaginaryPotential_Base, IBarrier_X
    {
        public ImaginaryBlockPotential(string name, int xSTART, int xEND, double damping) : base(name, -damping, xSTART, xEND, 0, 1, 0, 1)
        {
        }

        public ImaginaryBlockPotential(string name, int xSTART, int xEND, int ySTART, int yEND, double damping) : base(name, -damping, xSTART, xEND, ySTART, yEND, 0, 1)
        {
        }

        public ImaginaryBlockPotential(string name, int xSTART, int ySTART, int xEND, int yEND, int zSTART, int zEND, double Vmax) : base(name, Vmax, xSTART, xEND, ySTART, yEND, zSTART, zEND)
        {
        }

        public override IWavefunction getPsiV(WFInfo wfinfo)
        {
            if (psiV == null)
            {
                int dimYZ = wfinfo.DimInfo.DimY * wfinfo.DimInfo.DimZ;
                psiV = (IWavefunction)Activator.CreateInstance(wfinfo.Type, wfinfo);

                switch (wfinfo.CalcMethod)
                {
                    case ECalculationMethod.CPU:
                        int idx, i, j, k;
                        for (i = limit_x.Item1; i < limit_x.Item2; i++)
                            for (j = limit_y.Item1; j < limit_y.Item2; j++)
                                for (k = limit_z.Item1; k < limit_z.Item2; k++)
                                {
                                    idx = dimYZ * k + wfinfo.DimInfo.DimX * j + i;
                                    psiV.field[idx] = ImagPotential;
                                }

                        break;

                    case ECalculationMethod.CPU_Multihreading:
                    case ECalculationMethod.OpenCL:
                        Parallel.For(0, psiV.field.Length, i =>
                        {
                            int x = i % wfinfo.DimInfo.DimX;
                            int y = (i / wfinfo.DimInfo.DimX) % wfinfo.DimInfo.DimY;
                            int z = i / (wfinfo.DimInfo.DimX * wfinfo.DimInfo.DimY);

                            if (x >= limit_x.Item1 && x < limit_x.Item2 &&
                                y >= limit_y.Item1 && y < limit_y.Item2 &&
                                z >= limit_z.Item1 && z < limit_z.Item2)
                            {
                                psiV.field[i] = ImagPotential;
                            }
                        });
                        break;
                }
            }
            return psiV;
        }
    }
}