using System.Collections.Generic;
using System.Linq;
using JScy.NET.Physics.Simulationen.Wavefunctions.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.VarTypes
{
    public class TightBinding(double t_hop) : Hamilton_Base
    {
        private readonly List<EShift> Shifts = new List<EShift>();

        public double t_Hopping { get; } = t_hop;

        public override IWavefunction HPsi(IWavefunction psi)
        {
            Shifts.Clear();
            Shifts.Add(EShift.Xm);
            Shifts.Add(EShift.Xp);
            if (psi.Dimensions > 1)
            {
                Shifts.Add(EShift.Ym);
                Shifts.Add(EShift.Yp);
            }
            if (psi.Dimensions > 2)
            {
                Shifts.Add(EShift.Zm);
                Shifts.Add(EShift.Zp);
            }
            IWavefunction erg = psi.GetShift(Shifts.First());
            for (int i = 1; i < Shifts.Count; i++)
            {
                var erg2 = psi.GetShift(Shifts[i]);
                if (erg2 == null)
                    continue;
                erg += erg2;
            }
            return (-t_Hopping * erg);
        }
    }
}