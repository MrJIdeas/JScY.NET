using JScience.Classes.Videogeneration;
using JScience.Enums;
using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Classes;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.VarTypes;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Classes;
using JScience.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF;

IWavefunction.InitOpenCL();

//WF_1D test = (WF_1D)WFCreator.CreateGaußWave(-5, 10, 100, 25, ELatticeBoundary.Reflection, ECalculationMethod.OpenCL);
//WF_1D test = (WF_1D)WFCreator.CreateFreeWave(1, 100, ELatticeBoundary.Periodic,  ECalculationMethod.OpenCL);
//WF_1D test = (WF_1D)WFCreator.CreateDelta(100, 50, ELatticeBoundary.Periodic,  ECalculationMethod.OpenCL);
WF_2D test = (WF_2D)WFCreator.CreateGaußWave(-1, -1, 25, 25, 100, 100, 25, 25, ELatticeBoundary.Reflection, ECalculationMethod.OpenCL);
//WF_2D test = (WF_2D)WFCreator.CreateFreeWave(1, 0, 100, 100, 50, 50, ELatticeBoundary.Reflection,  ECalculationMethod.OpenCL);
//WF_2D test = (WF_2D)WFCreator.CreateDelta(100, 100, 50, 50, ELatticeBoundary.Reflection,  ECalculationMethod.OpenCL);
Console.WriteLine("Norm: " + test.Norm());

List<IHamilton<WF_2D>> hamlist = new List<IHamilton<WF_2D>>();

TightBinding<WF_2D> ham = new TightBinding<WF_2D>(1);
BlockPotential<WF_2D> pot1 = new BlockPotential<WF_2D>("PotMitte", 40 * test.DimX / 100, 0, 60 * test.DimX / 100, test.DimY, 1);
BlockPotential<WF_2D> pot2 = new BlockPotential<WF_2D>("PotMitte", 0, 40 * test.DimY / 100, test.DimX, 60 * test.DimY / 100, 0.1);
AF_Potential<WF_2D> afpot1 = new AF_Potential<WF_2D>("PotMitte", 40 * test.DimX / 100, 0, 60 * test.DimX / 100, test.DimY, 1, 5);
AF_Potential<WF_2D> afpot2 = new AF_Potential<WF_2D>("PotMitte", 0, 40 * test.DimY / 100, test.DimX, 60 * test.DimY / 100, 1, 5);
ImaginaryPotential<WF_2D> imagpotl = new ImaginaryPotential<WF_2D>("ImagPotLinks", 0, 3 * test.DimX / 100, 0, test.DimY, 10);
ImaginaryPotential<WF_2D> imagpotr = new ImaginaryPotential<WF_2D>("ImagPotRechts", 97 * test.DimX / 100, test.DimX, 0, test.DimY, 10);
ImaginaryPotential<WF_2D> imagpoto = new ImaginaryPotential<WF_2D>("ImagPotLinks", 3 * test.DimX / 100, 97 * test.DimX / 100, 97 * test.DimY / 100, test.DimY, 10);
ImaginaryPotential<WF_2D> imagpotu = new ImaginaryPotential<WF_2D>("ImagPotRechts", 3 * test.DimX / 100, 97 * test.DimX / 100, 0, 3 * test.DimY / 100, 10);
hamlist.Add(ham);
hamlist.Add(imagpotl);
hamlist.Add(imagpotr);
hamlist.Add(imagpoto);
hamlist.Add(imagpotu);
hamlist.Add(afpot1);
hamlist.Add(afpot2);
//hamlist.Add(pot1);
//hamlist.Add(pot2);

//U_T<WF_1D> ze = new U_T<WF_1D>(0.5);
U_T<WF_2D> ze = new U_T<WF_2D>(0.5);

FFMpeg_ImageToVideo recorder = new FFMpeg_ImageToVideo(@"C:\ffmpeg\bin\", @"C:\ffmpeg\out.mp4", 800, 600, 50);

var erg = test.GetImage(800, 600);
recorder.AddNextImage(erg);

for (int i = 0; i < 200; i++)
{
    DateTime start = DateTime.Now;
    test = ze.Do(test, hamlist);
    Console.WriteLine("Dauer Sek: " + (DateTime.Now - start).TotalSeconds);
    Console.WriteLine("Norm: " + test.Norm());
    if (i % 2 == 0)
    {
        erg = test.GetImage(800, 600);
        recorder.AddNextImage(erg);
    }
}
recorder.Dispose();