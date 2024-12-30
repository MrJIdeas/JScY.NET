using System;
using System.Collections.Generic;
using JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.VarTypes;

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
            double lauf = k_min;
            List<Eta_K> laufliste = [];
            while (lauf < k_max)
            {
                laufliste.Add(new Eta_K() { k = lauf, Eta = Math.Exp(-Math.Pow(lauf, 2) / Math.Pow(deltak, 2)) });
                lauf += deltak;
            }
            _raw = laufliste.ToArray();
            double norm = GetNorm();
            _field = new Eta_K[_raw.Length];
            for (int i = 0; i < _field.Length; i++)
            {
                _field[i] = _raw[i] / norm;
            }
        }
    }
}