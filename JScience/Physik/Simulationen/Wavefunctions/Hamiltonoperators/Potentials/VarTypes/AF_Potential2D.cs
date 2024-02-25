using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class AF_Potential2D<T> : Potential2D<T>, IAFBarrier where T : IWF_2D
    {
        public AF_Potential2D(string name, int xSTART, int ySTART, int xEND, int yEND, double Vmax, int blocksize) : base(name, xSTART, ySTART, xEND, yEND, Vmax)
        {
            Blocksize = blocksize;
        }

        public int Blocksize { get; private set; }

        public override T HPsi(T psi)
        {
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