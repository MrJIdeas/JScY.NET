using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HarfBuzzSharp;
using JScy.NET.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using ScottPlot;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class AF_Potential : BlockPotential, IAFBarrier
    {
        public AF_Potential(string name, int xSTART, int xEND, double Vmax, int Blocksize) : base(name, xSTART, xEND, Vmax)
        {
            this.Blocksize = Blocksize;
        }

        public AF_Potential(string name, int xSTART, int ySTART, int xEND, int yEND, double Vmax, int blocksize) : base(name, xSTART, ySTART, xEND, yEND, Vmax)
        {
            Blocksize = blocksize;
        }

        public AF_Potential(string name, int xSTART, int ySTART, int zStart, int xEND, int yEND, int zEND, double Vmax, int blocksize) : base(name, xSTART, zStart, ySTART, xEND, yEND, zEND, Vmax)
        {
            Blocksize = blocksize;
        }

        public int Blocksize { get; private set; }
        private Dictionary<int, int> fieldpos = [];

        public override IWavefunction HPsi(ref IWavefunction psi)
        {
            IWavefunction psiV = (IWavefunction)Activator.CreateInstance(psi.GetType(), psi.WFInfo);

            int dimYZ = psi.WFInfo.DimInfo.DimY * psi.WFInfo.DimInfo.DimZ;

            int idx, sign, i, j, k;
            for (i = xStart; i < xEnd; i++)
                for (j = yStart; j < yEnd; j++)
                    for (k = zStart; k < zEnd; k++)
                    {
                        idx = dimYZ * k + psi.WFInfo.DimInfo.DimX * j + i;
                        sign = (i - xStart) % Blocksize % 2 == (j - yStart) % Blocksize % 2 ? 1 : -1;
                        fieldpos.Add(idx, sign);
                    }
            switch (psi.WFInfo.CalcMethod)
            {
                default:
                    return null;

                case ECalculationMethod.CPU:
                    foreach (var number in fieldpos)
                        psiV.field[number.Key] = number.Value * psi.field[number.Key];
                    break;

                case ECalculationMethod.CPU_Multihreading:
                case ECalculationMethod.OpenCL:
                    var psi2 = psi;
                    Parallel.ForEach(fieldpos, number =>
                    {
                        psiV.field[number.Key] = number.Value * psi2.field[number.Key];
                    });
                    break;
            }
            fieldpos.Clear();
            return psiV * Potential;
        }
    }
}