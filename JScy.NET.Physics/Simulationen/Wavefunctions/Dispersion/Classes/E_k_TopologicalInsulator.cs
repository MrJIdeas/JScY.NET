using System;
using JScy.NET.Physics.Simulationen.Wavefunctions.Dispersion.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.TopologicalInsulator.VarTypes;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Dispersion.Classes
{
    internal class E_k_TopologicalInsulator : E_k_Base
    {
        public readonly TPI_MaterialInfo Material;

        public override double Calculate(double k)
        {
            throw new NotImplementedException();
        }
    }
}