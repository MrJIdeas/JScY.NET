using System.Collections.Generic;

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
    }
}