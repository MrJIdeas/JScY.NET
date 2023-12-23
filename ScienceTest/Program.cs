using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Classic.Simulations.Lattice;
using JScience.Physik.Simulationen.Spins.Enums;

Ising_Classic_1D_Lattice test = new Ising_Classic_1D_Lattice(-1, 0, 0, 6, 10000, EParticleType.ZBoson, ELatticeBoundary.Periodic, 1);
Console.WriteLine(test.H());
test.Start();
Console.WriteLine(test.H());
Console.WriteLine(test.n);