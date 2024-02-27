using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.VarTypes
{
    public class TightBindung3D<T> : ITightBinding<T> where T : IWF_3D
    {
        public TightBindung3D(double t_hop) : base(t_hop)
        {
        }

        public override T HPsi(T psi) => (T)(-t_Hopping * (T)(psi.GetShift(EShift.Xm) + psi.GetShift(EShift.Xp)
                                                            + psi.GetShift(EShift.Ym) + psi.GetShift(EShift.Yp)
                                                            + psi.GetShift(EShift.Zm) + psi.GetShift(EShift.Zp)));
    }
}