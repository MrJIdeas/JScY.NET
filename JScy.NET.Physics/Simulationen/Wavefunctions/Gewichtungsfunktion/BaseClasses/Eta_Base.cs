using System;
using System.Linq;
using JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.VarTypes;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Gewichtungsfunktion.BaseClasses
{
    public abstract class Eta_Base : IEta
    {
        protected Eta_K[] _field { get; set; }
        protected Eta_K[] _raw { get; set; }

        public Eta_K GetEta(int idx) => idx >= 0 && idx < _field.Length ? _field[idx] : throw new IndexOutOfRangeException();

        public Eta_K[] GetEta() => _field;

        public double GetNorm() => (from a in _raw
                                    select a.Norm()).Sum();
    }
}