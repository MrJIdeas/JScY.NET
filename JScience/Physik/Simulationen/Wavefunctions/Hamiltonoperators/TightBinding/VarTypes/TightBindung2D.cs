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

        public override T HPsi(T psi) => (T)(-t_Hopping * (T)((T)psi.GetShift(EShift.Xm) + (T)psi.GetShift(EShift.Xp))
                                                            + (T)psi.GetShift(EShift.Ym) + (T)psi.GetShift(EShift.Yp));
    }
}