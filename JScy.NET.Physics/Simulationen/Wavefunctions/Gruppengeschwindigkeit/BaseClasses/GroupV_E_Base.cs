using JScy.NET.Physics.Simulationen.Wavefunctions.Dispersion.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Gruppengeschwindigkeit.Interfaces;
using System.Collections.Generic;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Gruppengeschwindigkeit.BaseClasses
{
    public abstract class GroupV_E_Base : Dictionary<double, double>, IGroupV_E
    {
        protected GroupV_E_Base(E_k_Base e)
        {
            foreach (var key in e.Values)
            {
                Add(key, Calculate(key));
            }
        }

        public virtual double GetV(double E)
        {
            if (ContainsKey(E))
            {
                return this[E];
            }
            else
            {
                var value = Calculate(E);
                Add(E, value);
                return value;
            }
        }

        public abstract double Calculate(double E);
    }
}