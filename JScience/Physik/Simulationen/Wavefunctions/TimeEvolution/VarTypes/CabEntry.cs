using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.VarTypes
{
    public struct CabEntry
    {
        public string ExitName { get; set; }
        public double t { get; set; }
        public Complex cab { get; set; }
    }
}