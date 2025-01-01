﻿using JScy.NET.Classes.Videogeneration;
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

//WF_1D test = (WF_1D)WFCreator.CreateGaußWave(-5, 10, 100, 25, ELatticeBoundary.Periodic, ECalculationMethod.CPU);
//WF_1D test = (WF_1D)WFCreator.CreateFreeWave(1, 100, ELatticeBoundary.Periodic,  ECalculationMethod.OpenCL);
//WF_1D test = (WF_1D)WFCreator.CreateDelta(100, 50, ELatticeBoundary.Periodic,  ECalculationMethod.OpenCL);
//WF_2D test = (WF_2D)WFCreator.CreateGaußWave(5, 5, 100, 100, 100, 50, 25, 25, ELatticeBoundary.Periodic, ECalculationMethod.OpenCL);
WF_2D test = (WF_2D)WFCreator.CreateFreeWave(1, 0, 100, 100, ELatticeBoundary.Reflection, ECalculationMethod.OpenCL);
//WF_2D test = (WF_2D)WFCreator.CreateDelta(100, 100, 50, 50, ELatticeBoundary.Reflection, ECalculationMethod.OpenCL);
Console.WriteLine("Norm: " + test.Norm());

List<IHamilton> hamlist = [];

TightBinding ham = new(1);
BlockPotential pot1 = new("PotMitte", 40 * test.WFInfo.DimInfo.DimX / 100, 0, 60 * test.WFInfo.DimInfo.DimX / 100, test.WFInfo.DimInfo.DimY, 1);
BlockPotential pot2 = new("PotMitte", 0, 40 * test.WFInfo.DimInfo.DimY / 100, test.WFInfo.DimInfo.DimX, 60 * test.WFInfo.DimInfo.DimY / 100, 0.1);
AF_Potential afpot1 = new("PotMitte", 40 * test.WFInfo.DimInfo.DimX / 100, 0, 60 * test.WFInfo.DimInfo.DimX / 100, test.WFInfo.DimInfo.DimY, 1, 5);
AF_Potential afpot2 = new("PotMitte", 0, 40 * test.WFInfo.DimInfo.DimY / 100, test.WFInfo.DimInfo.DimX, 60 * test.WFInfo.DimInfo.DimY / 100, 1, 5);
ImaginaryPotential imagpotl = new("ImagPotLinks", 0, 3 * test.WFInfo.DimInfo.DimX / 100, 0, test.WFInfo.DimInfo.DimY, 2);
ImaginaryPotential imagpotr = new("ImagPotRechts", 97 * test.WFInfo.DimInfo.DimX / 100, test.WFInfo.DimInfo.DimX, 0, test.WFInfo.DimInfo.DimY, 2);
ImaginaryPotential imagpoto = new("ImagPotLinks", 3 * test.WFInfo.DimInfo.DimX / 100, 97 * test.WFInfo.DimInfo.DimX / 100, 97 * test.WFInfo.DimInfo.DimY / 100, test.WFInfo.DimInfo.DimY, 2);
ImaginaryPotential imagpotu = new("ImagPotRechts", 3 * test.WFInfo.DimInfo.DimX / 100, 97 * test.WFInfo.DimInfo.DimX / 100, 0, 3 * test.WFInfo.DimInfo.DimY / 100, 2);
hamlist.Add(ham);
//hamlist.Add(imagpotl);
//hamlist.Add(imagpotr);
//hamlist.Add(imagpoto);
//hamlist.Add(imagpotu);
//hamlist.Add(afpot1);
//hamlist.Add(afpot2);
//hamlist.Add(pot1);
//hamlist.Add(pot2);

U_T ze = new(0.5);
Orbital orb = new(test, 0.5f, EOrbitalLabel.Alpha);
FFMpegCore_ImageToVideo recorder = new(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "out.mp4", 800, 600, 50);

var erg = orb.Plotter.GetImage(800, 600);
recorder.AddNextImage(erg);
orb.CreateCabExitAuto();
var cabimg = orb.Plotter.GetCabExitImage(800, 600);
cabimg?.Save(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "CAB_Exits.png");
CabLogger logger = new();
for (int i = 0; i < 250; i++)
{
    DateTime start = DateTime.Now;
    orb = ze.Do(orb, hamlist);
    logger.AddCab(i * ze.t_STEP, orb);
    Console.WriteLine("Dauer Sek: " + (DateTime.Now - start).TotalSeconds);
    Console.WriteLine("Norm: " + test.Norm());
    if (i % 2 == 0)
    {
        erg = orb.Plotter.GetImage(800, 600);
        recorder.AddNextImage(erg);
    }
}
var img = logger.GetImage(800, 600);
if (img != null)
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