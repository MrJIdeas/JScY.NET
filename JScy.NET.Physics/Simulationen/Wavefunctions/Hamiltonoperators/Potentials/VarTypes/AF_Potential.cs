using System;
using HarfBuzzSharp;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using ScottPlot;

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

        public override IWavefunction HPsi(IWavefunction psi)
        {
            IWavefunction psiV = (IWavefunction)Activator.CreateInstance(psi.GetType(), psi.WFInfo, psi.CalcMethod);
            var dims = psi.GetDimensionLength();
            var dimX = dims["x"];
            var dimY = dims.ContainsKey("y") ? dims["y"] : 1;

            for (int i = xStart; i < xEnd; i++)
                for (int j = yStart; j < yEnd; j++)
                    for (int k = zStart; k < zEnd; k++)
                    {
                        var idx = i + j * dimX + k * dimY * dimX;
                        if ((i - xStart) % Blocksize % 2 == (j - yStart) % Blocksize % 2)
                            psiV.SetField(psi.field[idx], i, j, k);
                        else
                            psiV.SetField(-psi.field[idx], i, j, k);
                    }
            return psiV * Potential;
        }
    }
}