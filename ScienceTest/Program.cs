using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Classic.Simulations;
using JScience.Physik.Simulationen.Spins.Enums;

ANNNI_Classic_1D test = new ANNNI_Classic_1D(-1, 0.3, 0, 1, 1000, 100000, EParticleType.ZBoson, ELatticeBoundary.Periodic, 1);
Console.WriteLine(test.H());
test.Start();
Console.WriteLine(test.H());