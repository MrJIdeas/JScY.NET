using System.Collections.Generic;
using JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.BaseClasses;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Dispersion.BaseClasses
{
    public abstract class E_k_Base : Dictionary<double, double>, IE_k
    {
        public abstract double Calculate(double k);

        public virtual double GetE(double k)
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

        protected E_k_Base(Eta_Base eta)
        {
            foreach (var key in eta.Keys)
            {
                Add(key, Calculate(key));
            }
        }
    }
}