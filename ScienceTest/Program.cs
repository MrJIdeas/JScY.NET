﻿using Cloo;
using JScience.Classes.Videogeneration;
using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Classes;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.VarTypes;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Classes;
using JScience.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF;

IWavefunction.InitKernel();

//WF_1D test = (WF_1D)WFCreator.CreateGaußWave(-5, 10, 100, 25, ELatticeBoundary.Reflection);
//WF_1D test = (WF_1D)WFCreator.CreateFreeWave(1, 100, ELatticeBoundary.Periodic);
//WF_1D test = (WF_1D)WFCreator.CreateDelta(100, 50, ELatticeBoundary.Periodic);
WF_2D test = (WF_2D)WFCreator.CreateGaußWave(1, 0, 25, 25, 100, 100, 25, 25, ELatticeBoundary.Reflection, true);
//WF_2D test = (WF_2D)WFCreator.CreateFreeWave(1, 0, 100, 100, 50, 50, ELatticeBoundary.Reflection);
//WF_2D test = (WF_2D)WFCreator.CreateDelta(100, 100, 50, 50, ELatticeBoundary.Reflection);
Console.WriteLine("Norm: " + test.Norm());

//List<IHamilton<WF_1D>> hamlist = new List<IHamilton<WF_1D>>();

//TightBindung1D<WF_1D> ham = new TightBindung1D<WF_1D>(1);
//Potential1D<WF_1D> pot1 = new Potential1D<WF_1D>("PotMitte", 40 * test.DimX / 100, 60 * test.DimX / 100, 1);
//AF_Potential1D<WF_1D> afpot1 = new AF_Potential1D<WF_1D>("PotMitte", 40 * test.DimX / 100, 60 * test.DimX / 100, 1, 5);
//ImaginaryPotential1D<WF_1D> imagpotl = new ImaginaryPotential1D<WF_1D>("ImagPotLinks", 0, test.DimX / 100, 100);
//ImaginaryPotential1D<WF_1D> imagpotr = new ImaginaryPotential1D<WF_1D>("ImagPotRechts", 90 * test.DimX / 100, test.DimX, 100);

List<IHamilton<WF_2D>> hamlist = new List<IHamilton<WF_2D>>();

TightBindung2D<WF_2D> ham = new TightBindung2D<WF_2D>(1);
Potential2D<WF_2D> pot1 = new Potential2D<WF_2D>("PotMitte", 40 * test.DimX / 100, 0, 60 * test.DimX / 100, test.DimY, 10);
Potential2D<WF_2D> pot2 = new Potential2D<WF_2D>("PotMitte", 0, 40 * test.DimY / 100, test.DimX, 60 * test.DimY / 100, 10);
AF_Potential2D<WF_2D> afpot1 = new AF_Potential2D<WF_2D>("PotMitte", 40 * test.DimX / 100, 0, 60 * test.DimX / 100, test.DimY, 1, 5);
AF_Potential2D<WF_2D> afpot2 = new AF_Potential2D<WF_2D>("PotMitte", 0, 40 * test.DimY / 100, test.DimX, 60 * test.DimY / 100, 1, 5);
ImaginaryPotential2D<WF_2D> imagpotl = new ImaginaryPotential2D<WF_2D>("ImagPotLinks", 0, 5 * test.DimX / 100, 0, test.DimY, 10);
ImaginaryPotential2D<WF_2D> imagpotr = new ImaginaryPotential2D<WF_2D>("ImagPotRechts", 95 * test.DimX / 100, test.DimX, 0, test.DimY, 10);
ImaginaryPotential2D<WF_2D> imagpoto = new ImaginaryPotential2D<WF_2D>("ImagPotLinks", 5 * test.DimX / 100, 95 * test.DimX / 100, 95 * test.DimY / 100, test.DimY, 10);
ImaginaryPotential2D<WF_2D> imagpotu = new ImaginaryPotential2D<WF_2D>("ImagPotRechts", 5 * test.DimX / 100, 95 * test.DimX / 100, 0, 5 * test.DimY / 100, 10);
hamlist.Add(ham);
hamlist.Add(imagpotl);
hamlist.Add(imagpotr);
hamlist.Add(imagpoto);
hamlist.Add(imagpotu);
//hamlist.Add(afpot1);
//hamlist.Add(afpot2);
hamlist.Add(pot1);
hamlist.Add(pot2);

//U_T<WF_1D> ze = new U_T<WF_1D>(0.5);
U_T<WF_2D> ze = new U_T<WF_2D>(0.5);

FFMpeg_ImageToVideo recorder = new FFMpeg_ImageToVideo(@"C:\ffmpeg\bin\", @"C:\ffmpeg\out.mp4", 800, 600, 30);

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