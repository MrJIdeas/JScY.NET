using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class AF_Potential1D<T> : Potential1D<T>, IAFBarrier where T : IWF_1D
    {
        public AF_Potential1D(string name, int xSTART, int xEND, double Vmax, int Blocksize) : base(name, xSTART, xEND, Vmax)
        {
            this.Blocksize = Blocksize;
        }

        public int Blocksize { get; private set; }

        public override T HPsi(T psi)
        {
            T psiV = (T)Activator.CreateInstance(psi.GetType(), psi.WFInfo, psi.UseGPU);
            for (int i = xStart; i < xEnd; i++)
            {
                if ((i - xStart) % Blocksize % 2 == 0)
                    psiV.SetField(i, psi[i]);
                else
                    psiV.SetField(i, -psi[i]);
            }
            return (T)(psiV * Potential);
        }
    }
}