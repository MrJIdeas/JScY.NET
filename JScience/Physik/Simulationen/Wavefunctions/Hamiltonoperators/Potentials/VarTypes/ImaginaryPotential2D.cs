using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class ImaginaryPotential2D<T> : IImagenaryPotential<T>, IBarrier_X, IBarrier_Y where T : IWF_2D
    {
        public int xStart { get; private set; }

        public int xEnd { get; private set; }

        public int yStart { get; private set; }

        public int yEnd { get; private set; }

        public ImaginaryPotential2D(string name, int xSTART, int xEND, int ySTART, int yEND, double damping) : base(name, damping)
        {
            xStart = xSTART;
            xEnd = xEND;
            yStart = ySTART;
            yEnd = yEND;
        }

        public override double E(T psi)
        {
            double erg = 0;
            T erg2 = (T)((T)psi.Conj() * HPsi(psi));
            for (int i = 0; i < erg2.DimX; i++)
                for (int j = 0; j < erg2.DimY; j++)
                    erg += (Complex.Conjugate(erg2[i, j]) * erg2[i, j]).Real;
            return erg;
        }

        public override T HPsi(T psi)
        {
            T psiV = (T)Activator.CreateInstance(psi.GetType(), psi.DimX);
            for (int i = xStart; i < xEnd; i++)
                for (int j = yStart; j < yEnd; j++)
                    psiV.SetField(i, j, Damping * psi[i, j]);
            return psiV;
        }
    }
}