using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.VarTypes
{
    public class TightBindung3D<T> : ITightBinding<T> where T : IWF_3D
    {
        public TightBindung3D(decimal t_hop) : base(t_hop)
        {
        }

        public override decimal E(T psi)
        {
            decimal erg = 0;
            T erg2 = (T)((T)psi.Conj() * HPsi(psi));
            for (int i = 0; i < erg2.DimX; i++)
                for (int j = 0; j < erg2.DimY; j++)
                    for (int k = 0; k < erg2.DimZ; k++)
                        erg += (erg2[i, j, k].Conj() * erg2[i, j, k]).Real;
            return erg;
        }

        public override T HPsi(T psi) => (T)(-t_Hopping * (T)((T)psi.GetShift(EShift.Xm) + (T)psi.GetShift(EShift.Xp))
                                                            + (T)psi.GetShift(EShift.Ym) + (T)psi.GetShift(EShift.Yp)
                                                            + (T)psi.GetShift(EShift.Zm) + (T)psi.GetShift(EShift.Zp));
    }
}