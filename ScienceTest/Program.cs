using JScy.NET.Classes.Videogeneration;
using JScy.NET.Enums;
using JScy.NET.Physics.Simulationen.Spins.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF;

IWavefunction.InitOpenCL();

//WF_1D test = (WF_1D)WFCreator.CreateGaußWave(5, 10, 200, 50, ELatticeBoundary.Periodic, ECalculationMethod.CPU);
//WF_1D test = (WF_1D)WFCreator.CreateFreeWave(1, 100, ELatticeBoundary.Periodic,  ECalculationMethod.OpenCL);
//WF_1D test = (WF_1D)WFCreator.CreateDelta(100, 50, ELatticeBoundary.Periodic,  ECalculationMethod.OpenCL);
WF_2D test = (WF_2D)WFCreator.CreateGaußWave(5, 0, 100, 100, 100, 50, 25, 25, ELatticeBoundary.Periodic, ECalculationMethod.CPU);
//WF_2D test = (WF_2D)WFCreator.CreateFreeWave(1, 0, 100, 100, ELatticeBoundary.Periodic, ECalculationMethod.CPU);
//WF_2D test = (WF_2D)WFCreator.CreateDelta(100, 100, 50, 50, ELatticeBoundary.Reflection, ECalculationMethod.OpenCL);
Console.WriteLine("Norm: " + test.Norm());

List<IHamilton> hamlist = [];

TightBinding ham = new(1);
//BlockPotential pot1 = new("PotMitte", 40 * test.WFInfo.DimInfo.DimX / 100, 0, 60 * test.WFInfo.DimInfo.DimX / 100, test.WFInfo.DimInfo.DimY, 5);
//BlockPotential pot2 = new("PotMitte", 0, 40 * test.WFInfo.DimInfo.DimY / 100, test.WFInfo.DimInfo.DimX, 60 * test.WFInfo.DimInfo.DimY / 100, 0.1);
//AF_Potential afpot1 = new("PotMitte", 40 * test.WFInfo.DimInfo.DimX / 100, 0, 60 * test.WFInfo.DimInfo.DimX / 100, test.WFInfo.DimInfo.DimY, 1, 5);
//AF_Potential afpot2 = new("PotMitte", 0, 40 * test.WFInfo.DimInfo.DimY / 100, test.WFInfo.DimInfo.DimX, 60 * test.WFInfo.DimInfo.DimY / 100, 1, 5);
ImaginaryBlockPotential imagpotl = new("ImagPotLinks", 0, 5 * test.WFInfo.DimInfo.DimX / 100, 0, test.WFInfo.DimInfo.DimY, 2);
ImaginaryBlockPotential imagpotr = new("ImagPotRechts", 95 * test.WFInfo.DimInfo.DimX / 100, test.WFInfo.DimInfo.DimX, 0, test.WFInfo.DimInfo.DimY, 2);
ImaginaryBlockPotential imagpoto = new("ImagPotLinks", 5 * test.WFInfo.DimInfo.DimX / 100, 95 * test.WFInfo.DimInfo.DimX / 100, 95 * test.WFInfo.DimInfo.DimY / 100, test.WFInfo.DimInfo.DimY, 2);
ImaginaryBlockPotential imagpotu = new("ImagPotRechts", 5 * test.WFInfo.DimInfo.DimX / 100, 95 * test.WFInfo.DimInfo.DimX / 100, 0, 5 * test.WFInfo.DimInfo.DimY / 100, 2);
imagpotl.getPsiV(test.WFInfo);
imagpotr.getPsiV(test.WFInfo);
imagpoto.getPsiV(test.WFInfo);
imagpotu.getPsiV(test.WFInfo);

PotentialCollection Potcollection = new("Potcollection", test.WFInfo);

hamlist.Add(ham);
Potcollection.MigratePotential(imagpotl);
Potcollection.MigratePotential(imagpotr);
Potcollection.MigratePotential(imagpoto);
Potcollection.MigratePotential(imagpotu);
//Potcollection.MigratePotential(afpot1);
//Potcollection.MigratePotential(afpot2);
//Potcollection.MigratePotential(pot1);
//Potcollection.MigratePotential(pot2);
hamlist.Add(Potcollection);

U_T ze = new(0.5);
Orbital orb = new(test, 0.5f, EOrbitalLabel.Alpha);
FFMpegCore_ImageToVideo recorder = new(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "out.mp4", 800, 600, 50);

var erg = orb.Plotter.GetImage(800, 600);
recorder.AddNextImage(erg);
orb.CreateCabExitAuto();
var cabimg = orb.Plotter.GetCabExitImage(800, 600);
cabimg?.Save(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "CAB_Exits.png");
CabLogger logger = new();
Normlogger normlogger = new();
for (int i = 0; i < 50; i++)
{
    var step = i * ze.t_STEP;
    DateTime start = DateTime.Now;
    orb = ze.Do(ref orb, hamlist);
    //logger.Add(step, orb);
    Console.WriteLine("Dauer Sek: " + (DateTime.Now - start).TotalSeconds + ";Norm: " + orb.WF.Norm());
    normlogger.Add(step, orb);
    if (i % 2 == 0)
    {
        erg = orb.Plotter.GetImage(800, 600);
        recorder.AddNextImage(erg);
    }
}
var img_pot = Potcollection.GetImage(800, 600);
if (img_pot != null)
    img_pot.Save(Environment.CurrentDirectory + Path.DirectorySeparatorChar + Potcollection.Name + ".png");
//var img = logger.GetImage(800, 600);
//if (img != null)
//    for (int i1 = 0; i1 < img.Count; i1++)
//    {
//        System.Drawing.Image? im = img[i1];
//        im.Save(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "CAB_" + i1 + ".png");
//    }
var img = normlogger.GetImage(800, 600);
if (img != null)
    for (int i1 = 0; i1 < img.Count; i1++)
    {
        System.Drawing.Image? im = img[i1];
        im.Save(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Norm_" + i1 + ".png");
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