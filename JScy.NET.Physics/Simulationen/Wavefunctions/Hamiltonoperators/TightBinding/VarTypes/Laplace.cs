using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.VarTypes
{
    public class Laplace(double coupling) : TightBinding(coupling)
    {
        public override IWavefunction HPsi(ref IWavefunction psi) => (-t_Hopping * 2 * psi.WFInfo.DimInfo.Dimensions) * psi - base.HPsi(ref psi);
    }
}