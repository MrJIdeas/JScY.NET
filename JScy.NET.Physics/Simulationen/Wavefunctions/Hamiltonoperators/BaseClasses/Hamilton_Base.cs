using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.BaseClasses
{
    public abstract class Hamilton_Base<T> : IHamilton<T> where T : IWavefunction
    {
        public abstract T HPsi(T psi);

        public double E(T psi)
        {
            double erg = 0;
            IWavefunction erg2 = psi.Conj() * HPsi(psi);
            for (int i = 0; i < erg2.WFInfo.DimX * erg2.WFInfo.DimY * erg2.WFInfo.DimZ; i++)
                erg += erg2[i].Real;
            return erg;
        }
    }
}