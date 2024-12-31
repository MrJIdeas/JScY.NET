using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes
{
    public class CabExit(string exitName, IWavefunction wavefunction)
    {
        public readonly string ExitName = exitName;

        public readonly IWavefunction wavefunction = wavefunction;
    }
}