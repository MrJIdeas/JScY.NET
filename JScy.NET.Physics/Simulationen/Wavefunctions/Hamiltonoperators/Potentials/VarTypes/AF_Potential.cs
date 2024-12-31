using System;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class AF_Potential<T> : BlockPotential<T>, IAFBarrier where T : IWF_2D
    {
        public AF_Potential(string name, int xSTART, int xEND, double Vmax, int Blocksize) : base(name, xSTART, xEND, Vmax)
        {
            this.Blocksize = Blocksize;
        }

        public AF_Potential(string name, int xSTART, int ySTART, int xEND, int yEND, double Vmax, int blocksize) : base(name, xSTART, ySTART, xEND, yEND, Vmax)
        {
            Blocksize = blocksize;
        }

        public int Blocksize { get; private set; }

        public override T HPsi(T psi)
        {
            T psiV = (T)Activator.CreateInstance(psi.GetType(), psi.WFInfo, psi.CalcMethod);
            for (int i = xStart; i < xEnd; i++)
                for (int j = yStart; j < yEnd; j++)
                    if ((i - xStart) % Blocksize % 2 == (j - yStart) % Blocksize % 2)
                        psiV.SetField(i, j, psi[i, j]);
                    else
                        psiV.SetField(i, j, -psi[i, j]);
            return (T)(psiV * Potential);
        }
    }
}