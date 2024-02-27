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

        public override T HPsi(T psi) => (T)(-t_Hopping * (psi.GetShift(EShift.Xm) + psi.GetShift(EShift.Xp)
                                                            + psi.GetShift(EShift.Ym) + psi.GetShift(EShift.Yp)));
    }
}