using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Classes;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.VarTypes;
using JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Classes;
using JScience.Physik.Simulationen.Wavefunctions.VarTypes;

//Ising_Classic_1D_Lattice test = new Ising_Classic_1D_Lattice(-1, 0, 0, 6, 10000, EParticleType.ZBoson, ELatticeBoundary.Periodic, 1);
//Console.WriteLine(test.H());
//test.Start();
//Console.WriteLine(test.H());
//Console.WriteLine(test.n);

//WF_1D test = (WF_1D)WFCreator.CreateGaußWave(1, 25, 500, 250, ELatticeBoundary.Periodic);
//WF_2D test = (WF_2D)WFCreator.CreateGaußWave(1, 0, 25, 25, 100, 100, 50, 50, ELatticeBoundary.Periodic);
//WF_1D test = (WF_1D)WFCreator.CreateFreeWave(1, 100, ELatticeBoundary.Periodic);
//WF_2D test = (WF_2D)WFCreator.CreateFreeWave(1, 0, 100, 100, 50, 50, ELatticeBoundary.Reflection);
WF_1D test = (WF_1D)WFCreator.CreateDelta(100, 50, ELatticeBoundary.Periodic);
//WF_2D test = (WF_2D)WFCreator.CreateDelta(100, 100, 25, 25, ELatticeBoundary.Periodic);
Console.WriteLine("Norm: " + test.Norm());

List<IHamilton<WF_1D>> hamlist = new List<IHamilton<WF_1D>>();

TightBindung1D<WF_1D> ham = new TightBindung1D<WF_1D>(1);

//List<IHamilton<WF_2D>> hamlist = new List<IHamilton<WF_2D>>();

//TightBindung2D<WF_2D> ham = new TightBindung2D<WF_2D>(1);

hamlist.Add(ham);

U_T_1D<WF_1D> ze = new U_T_1D<WF_1D>(0.5m);
//U_T_2D<WF_2D> ze = new U_T_2D<WF_2D>(0.5);

var erg = test.GetImage(800, 600);
erg.SavePng("test_START.png");
for (int i = 0; i < 5000; i++)
{
    DateTime start = DateTime.Now;
    test = ze.Do(test, hamlist);
    Console.WriteLine("Dauer Sek: " + (DateTime.Now - start).TotalSeconds);
    Console.WriteLine("Norm: " + test.Norm());
    if (i % 10 == 0)
    {
        erg = test.GetImage(800, 600);
        erg.SavePng("test" + i + ".png");
    }
}