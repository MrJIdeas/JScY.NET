using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Classic.Simulations.Lattice;
using JScience.Physik.Simulationen.Spins.Enums;

ANNNI_Classic_1D_Lattice test = new ANNNI_Classic_1D_Lattice(1, 0, 0, 1, 10, 100000, EParticleType.ZBoson, ELatticeBoundary.Periodic, 1);
Console.WriteLine(test.H());
test.Start();
Console.WriteLine(test.H());