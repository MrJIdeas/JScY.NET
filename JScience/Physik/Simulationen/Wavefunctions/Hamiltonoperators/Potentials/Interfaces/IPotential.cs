using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces
{
    public interface IPotential<T> : IHamilton<T> where T : IWavefunction
    {
    }
}