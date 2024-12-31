using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JScy.NET.Physics.Simulationen.Wavefunctions.Dispersion.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Dispersion.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Gruppengeschwindigkeit.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.TopologicalInsulator.VarTypes;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Gruppengeschwindigkeit.Classes
{
    public class GroupV_E_TPI : GroupV_E_Base
    {
        private readonly double vorfaktor_nachsign;
        private readonly double D2M;
        private readonly double B2MinD2Quadrat;
        public readonly TPI_MaterialInfo Material;

        public GroupV_E_TPI(E_k_TopologicalInsulator e) : base(e)
        {
            Material = e.Material;
            vorfaktor_nachsign = e.Material.A2 * Math.Sqrt(1 - Math.Pow(e.Material.D2, 2) / Math.Pow(e.Material.B2, 2));
            D2M = e.Material.D2 * e.Material.M;
            B2MinD2Quadrat = Math.Pow(e.Material.B2, 2) - Math.Pow(e.Material.D2, 2);
        }

        public override double Calculate(double E)
        {
            double key = this.Where(x => x.Value.Equals(E)).Select(x => x.Key).FirstOrDefault();
            return Math.Sin(key) * vorfaktor_nachsign * Math.Sqrt(1 - Math.Pow(Material.B2 * E - D2M, 2) / B2MinD2Quadrat);
        }
    }
}