using JScy.NET.Classes.Videogeneration;
using JScy.NET.Enums;
using JScy.NET.Physics.Simulationen.Spins.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF;

IWavefunction.InitOpenCL();

//WF_1D test = (WF_1D)WFCreator.CreateGaußWave(-5, 10, 100, 25, ELatticeBoundary.Reflection, ECalculationMethod.OpenCL);
//WF_1D test = (WF_1D)WFCreator.CreateFreeWave(1, 100, ELatticeBoundary.Periodic,  ECalculationMethod.OpenCL);
//WF_1D test = (WF_1D)WFCreator.CreateDelta(100, 50, ELatticeBoundary.Periodic,  ECalculationMethod.OpenCL);
WF_2D test = (WF_2D)WFCreator.CreateGaußWave(5, 0, 100, 100, 100, 50, 25, 25, ELatticeBoundary.Reflection, ECalculationMethod.OpenCL);
//WF_2D test = (WF_2D)WFCreator.CreateFreeWave(1, 0, 100, 100, 50, 50, ELatticeBoundary.Reflection,  ECalculationMethod.OpenCL);
//WF_2D test = (WF_2D)WFCreator.CreateDelta(100, 100, 50, 50, ELatticeBoundary.Reflection,  ECalculationMethod.OpenCL);
Console.WriteLine("Norm: " + test.Norm());

List<IHamilton<WF_2D>> hamlist = new List<IHamilton<WF_2D>>();

TightBinding<WF_2D> ham = new TightBinding<WF_2D>(1);
BlockPotential<WF_2D> pot1 = new BlockPotential<WF_2D>("PotMitte", 40 * test.DimX / 100, 0, 60 * test.DimX / 100, test.DimY, 1);
BlockPotential<WF_2D> pot2 = new BlockPotential<WF_2D>("PotMitte", 0, 40 * test.DimY / 100, test.DimX, 60 * test.DimY / 100, 0.1);
AF_Potential<WF_2D> afpot1 = new AF_Potential<WF_2D>("PotMitte", 40 * test.DimX / 100, 0, 60 * test.DimX / 100, test.DimY, 1, 5);
AF_Potential<WF_2D> afpot2 = new AF_Potential<WF_2D>("PotMitte", 0, 40 * test.DimY / 100, test.DimX, 60 * test.DimY / 100, 1, 5);
ImaginaryPotential<WF_2D> imagpotl = new ImaginaryPotential<WF_2D>("ImagPotLinks", 0, 3 * test.DimX / 100, 0, test.DimY, 2);
ImaginaryPotential<WF_2D> imagpotr = new ImaginaryPotential<WF_2D>("ImagPotRechts", 97 * test.DimX / 100, test.DimX, 0, test.DimY, 2);
ImaginaryPotential<WF_2D> imagpoto = new ImaginaryPotential<WF_2D>("ImagPotLinks", 3 * test.DimX / 100, 97 * test.DimX / 100, 97 * test.DimY / 100, test.DimY, 2);
ImaginaryPotential<WF_2D> imagpotu = new ImaginaryPotential<WF_2D>("ImagPotRechts", 3 * test.DimX / 100, 97 * test.DimX / 100, 0, 3 * test.DimY / 100, 2);
hamlist.Add(ham);
hamlist.Add(imagpotl);
hamlist.Add(imagpotr);
hamlist.Add(imagpoto);
hamlist.Add(imagpotu);
//hamlist.Add(afpot1);
//hamlist.Add(afpot2);
//hamlist.Add(pot1);
//hamlist.Add(pot2);

//U_T<WF_1D> ze = new U_T<WF_1D>(0.5);
U_T<WF_2D> ze = new U_T<WF_2D>(0.5);

FFMpeg_ImageToVideo recorder = new(@"C:\ffmpeg\bin\", Environment.CurrentDirectory + Path.DirectorySeparatorChar + "out.mp4", 800, 600, 50);

var erg = test.GetImage(800, 600);
recorder.AddNextImage(erg);
test.AddCabExitAuto();
var cabimg = test.GetCabExitImage(800, 600);
cabimg.Save(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "CAB_Exits.png");
CabLogger logger = new();
for (int i = 0; i < 200; i++)
{
    DateTime start = DateTime.Now;
    test = ze.Do(test, hamlist);
    logger.AddCab(i * ze.t_STEP, test);
    Console.WriteLine("Dauer Sek: " + (DateTime.Now - start).TotalSeconds);
    Console.WriteLine("Norm: " + test.Norm());
    if (i % 2 == 0)
    {
        erg = test.GetImage(800, 600);
        recorder.AddNextImage(erg);
    }
}
var img = logger.GetImage(800, 600);
for (int i1 = 0; i1 < img.Count; i1++)
{
    System.Drawing.Image? im = img[i1];
    im.Save(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "CAB_" + i1 + ".png");
}
//Sab sablogger = new Sab();
//sablogger.CalcSab(logger, -5, 5);
//img = sablogger.GetImage(800, 600);
//for (int i1 = 0; i1 < img.Count; i1++)
//{
//    System.Drawing.Image? im = img[i1];
//    im.Save(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "SAB_" + i1 + ".png");
//}
recorder.Dispose();