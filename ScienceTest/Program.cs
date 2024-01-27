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

WF_1D test = (WF_1D)WFCreator.CreateGaußWave(0.5, 50, 500, 10);
//WF_1D test = (WF_1D)WFCreator.CreateDelta(100, 5);
//for (int i = 0; i < test.DimX; i++)
//    Console.WriteLine(test.getNorm(i));
Console.WriteLine("Norm: " + test.Norm());

List<IHamilton<WF_1D>> hamlist = new List<IHamilton<WF_1D>>();

TightBindung1D<WF_1D> ham = new TightBindung1D<WF_1D>(1);

hamlist.Add(ham);

U_T_1D<WF_1D> ze = new U_T_1D<WF_1D>(0.01);
test = ze.Do(test, hamlist);
//for (int i = 0; i < test.DimX; i++)
//    Console.WriteLine(test.getNorm(i));
Console.WriteLine("Norm: " + test.Norm());

List<double> x = new List<double>();
for (int i = 0; i < test.DimX; i++)
    x.Add(i);
List<double> y = new List<double>();
for (int i = 0; i < test.DimX; i++)
    y.Add(test.getNorm(i));
ScottPlot.Plot myPlot = new();

myPlot.Add.Scatter(x.ToArray(), y.ToArray());
myPlot.SavePng("quickstart.png", 800, 600);