using JScy.NET.Physics.Simulationen.Wavefunctions.Dispersion.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.TopologicalInsulator.VarTypes;
using System;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Dispersion.Classes
{
    public class E_k_TopologicalInsulator : E_k_Base
    {
        public readonly TPI_MaterialInfo Material;
        private readonly double vorfaktor_vorsign;
        private readonly double faktor_ende;
        private readonly double A2Quadrat;

        public E_k_TopologicalInsulator(TPI_MaterialInfo material, Eta_Base eta) : base(eta)
        {
            Material = material;
            vorfaktor_vorsign = Material.M * Material.D2 / Material.B2;
            faktor_ende = (Math.Pow(Material.B2, 2) - Math.Pow(Material.D2, 2)) / Math.Pow(Material.B2, 2);
            A2Quadrat = Math.Pow(Material.A2, 2);
        }

        public override double Calculate(double k) => vorfaktor_vorsign + Math.Sign(k) * Math.Sqrt(A2Quadrat * Math.Pow(Math.Sin(k), 2) * faktor_ende);
    }
}