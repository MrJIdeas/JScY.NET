using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.BaseClasses
{
    public abstract class Eta_Base : Dictionary<double, Complex>, IEta
    {
        public virtual Complex GetEta(double k)
        {
            if (ContainsKey(k))
            {
                return this[k];
            }
            else
            {
                var value = Calculate(k);
                Add(k, value);
                return value;
            }
        }

        public abstract Complex Calculate(double k);

        public double GetNorm() => (from a in this
                                    select (a.Value * Complex.Conjugate(a.Value)).Real).Sum();
    }
}