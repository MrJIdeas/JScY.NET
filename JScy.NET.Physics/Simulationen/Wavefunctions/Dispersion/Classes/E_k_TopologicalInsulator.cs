using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JScy.NET.Physics.Simulationen.Wavefunctions.Dispersion.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.TopologicalInsulator.VarTypes;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Dispersion.Classes
{
    internal class E_k_TopologicalInsulator : E_k_Base
    {
        public readonly TPI_MaterialInfo Material;

        public E_k_TopologicalInsulator(TPI_MaterialInfo material, Eta_Base eta) : base(eta) => Material = material;

        public override double Calculate(double k)
        {
            throw new NotImplementedException();
        }
    }
}