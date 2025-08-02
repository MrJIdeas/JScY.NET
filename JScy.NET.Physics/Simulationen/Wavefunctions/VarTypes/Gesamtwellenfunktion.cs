using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;
using System.Collections.Generic;
using System.Linq;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes
{
    public class Gesamtwellenfunktion : List<Orbital>
    {
        #region Entropie

        public double CalcEntropy() => this.Sum(x => x.CalcEntropy());

        #endregion Entropie
    }
}