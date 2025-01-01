using System;
using System.Numerics;
using JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.BaseClasses;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.Classes
{
    public class Eta_K_Gauß : Eta_Base
    {
        public readonly double k_min;

        public readonly double k_max;
        public readonly double deltak;

        public Eta_K_Gauß(double k_min, double k_max, double deltak)
        {
            this.k_min = k_min;
            this.k_max = k_max;
            this.deltak = deltak;
            for (double lauf = k_min; lauf < k_max; lauf += deltak)
                Add(lauf, Calculate(lauf));
        }

        public override Complex Calculate(double k) => Math.Exp(-Math.Pow(k, 2) / Math.Pow(deltak, 2));

        public override Complex GetEta(double k) => k < k_min || k > k_max
                ? throw new ArgumentOutOfRangeException("k liegt außerhalb der Limit von k_min und k_max.")
                : base.GetEta(k);
    }
}