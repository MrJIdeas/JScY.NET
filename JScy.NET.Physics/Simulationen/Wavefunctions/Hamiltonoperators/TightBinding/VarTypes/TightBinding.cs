using JScy.NET.Physics.Simulationen.Wavefunctions.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF;
using System.Collections.Generic;
using System.Linq;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.VarTypes
{
    public class TightBinding<T> : Hamilton_Base<T> where T : IWavefunction
    {
        private List<EShift> Shifts { get; set; }

        public TightBinding(double t_hop)
        {
            t_Hopping = t_hop;
            Shifts = new List<EShift>();
            if (typeof(T).Equals(typeof(WF_1D)))
            {
                Shifts.Add(EShift.Xm);
                Shifts.Add(EShift.Xp);
            }
            else if (typeof(T).Equals(typeof(WF_2D)))
            {
                Shifts.Add(EShift.Xm);
                Shifts.Add(EShift.Xp);
                Shifts.Add(EShift.Ym);
                Shifts.Add(EShift.Yp);
            }
            else
            {
                Shifts.Add(EShift.Xm);
                Shifts.Add(EShift.Xp);
                Shifts.Add(EShift.Ym);
                Shifts.Add(EShift.Yp);
                Shifts.Add(EShift.Zm);
                Shifts.Add(EShift.Zp);
            }
        }

        public double t_Hopping { get; }

        public override T HPsi(T psi)
        {
            IWavefunction erg = psi.GetShift(Shifts.First());
            for (int i = 1; i < Shifts.Count; i++)
                erg += psi.GetShift(Shifts[i]);
            return (T)(-t_Hopping * erg);
        }
    }
}