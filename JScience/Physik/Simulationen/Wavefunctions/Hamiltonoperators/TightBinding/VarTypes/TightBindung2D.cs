using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.VarTypes
{
    public class TightBindung2D<T> : ITightBinding<T> where T : IWF_2D
    {
        public TightBindung2D(double t_hop) : base(t_hop)
        {
        }

        public override double E(T psi)
        {
            double erg = 0;
            T erg2 = (T)((T)psi.Conj() * HPsi(psi));
            for (int i = 0; i < erg2.DimX; i++)
                for (int j = 0; j < erg2.DimY; j++)
                    erg += erg2[i, j].Magnitude;
            return erg;
        }

        public override T HPsi(T psi) => (T)(-t_Hopping * (T)((T)psi.GetShift(EShift.Xm) + (T)psi.GetShift(EShift.Xp))
                                                            + (T)psi.GetShift(EShift.Ym) + (T)psi.GetShift(EShift.Yp));
    }
}