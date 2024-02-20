using FFMediaToolkit;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;
using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Classes;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.VarTypes;
using JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Classes;
using JScience.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF;
using System.Drawing;
using System.Drawing.Imaging;

//Ising_Classic_1D_Lattice test = new Ising_Classic_1D_Lattice(-1, 0, 0, 6, 10000, EParticleType.ZBoson, ELatticeBoundary.Periodic, 1);
//Console.WriteLine(test.H());
//test.Start();
//Console.WriteLine(test.H());
//Console.WriteLine(test.n);

//WF_1D test = (WF_1D)WFCreator.CreateGaußWave(-5, 10, 100, 25, ELatticeBoundary.Reflection);
//WF_1D test = (WF_1D)WFCreator.CreateFreeWave(1, 100, ELatticeBoundary.Periodic);
//WF_1D test = (WF_1D)WFCreator.CreateDelta(100, 50, ELatticeBoundary.Periodic);
WF_2D test = (WF_2D)WFCreator.CreateGaußWave(-1, 0, 25, 25, 100, 100, 25, 25, ELatticeBoundary.Reflection);
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
Potential2D<WF_2D> pot1 = new Potential2D<WF_2D>("PotMitte", 40 * test.DimX / 100, 60 * test.DimX / 100, 0, test.DimY, 10);
Potential2D<WF_2D> pot2 = new Potential2D<WF_2D>("PotMitte", 0, test.DimX, 40 * test.DimY / 100, 60 * test.DimY / 100, 10);
AF_Potential2D<WF_2D> afpot1 = new AF_Potential2D<WF_2D>("PotMitte", 40 * test.DimX / 100, 0, 60 * test.DimX / 100, test.DimY, 1, 5);
AF_Potential2D<WF_2D> afpot2 = new AF_Potential2D<WF_2D>("PotMitte", 0, 40 * test.DimY / 100, test.DimX, 60 * test.DimY / 100, 1, 5);
ImaginaryPotential2D<WF_2D> imagpotl = new ImaginaryPotential2D<WF_2D>("ImagPotLinks", 0, 5 * test.DimX / 100, 0, test.DimY, 10);
ImaginaryPotential2D<WF_2D> imagpotr = new ImaginaryPotential2D<WF_2D>("ImagPotRechts", 95 * test.DimX / 100, test.DimX, 0, test.DimY, 10);
ImaginaryPotential2D<WF_2D> imagpoto = new ImaginaryPotential2D<WF_2D>("ImagPotLinks", 0, test.DimX, 95 * test.DimY / 100, test.DimY, 10);
ImaginaryPotential2D<WF_2D> imagpotu = new ImaginaryPotential2D<WF_2D>("ImagPotRechts", 0, test.DimX, 0, 5 * test.DimY / 100, 10);
hamlist.Add(ham);
hamlist.Add(imagpotl);
hamlist.Add(imagpotr);
hamlist.Add(imagpoto);
hamlist.Add(imagpotu);
hamlist.Add(afpot1);
hamlist.Add(afpot2);
//hamlist.Add(pot1);
//hamlist.Add(pot2);

//U_T_1D<WF_1D> ze = new U_T_1D<WF_1D>(0.5m);
U_T_2D<WF_2D> ze = new U_T_2D<WF_2D>(0.5m);

FFmpegLoader.FFmpegPath = @"C:\ffmpeg\bin\";

var settings = new VideoEncoderSettings(width: 960, height: 544, framerate: 30, codec: VideoCodec.H264);
settings.EncoderPreset = EncoderPreset.Fast;
settings.Framerate = 1;
settings.CRF = 17;
var file = MediaBuilder.CreateContainer(@"C:\ffmpeg\out.mp4").WithVideo(settings).Create();

var erg = test.GetImage(800, 600);
//erg.SavePng("test_START.png");
Bitmap pic = (Bitmap)Image.FromStream(new MemoryStream(erg.GetImageBytes()));
var rect = new Rectangle(Point.Empty, pic.Size);
var bitLock = pic.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
var bitmapData = ImageData.FromPointer(bitLock.Scan0, ImagePixelFormat.Bgr24, pic.Size);
file.Video.AddFrame(bitmapData); // Encode the frame
pic.UnlockBits(bitLock);

for (int i = 0; i < 200; i++)
{
    DateTime start = DateTime.Now;
    test = ze.Do(test, hamlist);
    Console.WriteLine("Dauer Sek: " + (DateTime.Now - start).TotalSeconds);
    Console.WriteLine("Norm: " + test.Norm());
    if (i % 2 == 0)
    {
        erg = test.GetImage(800, 600);
        //erg.SavePng("test" + i + ".png");
        pic = (Bitmap)Image.FromStream(new MemoryStream(erg.GetImageBytes()));
        rect = new Rectangle(Point.Empty, pic.Size);
        bitLock = pic.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        bitmapData = ImageData.FromPointer(bitLock.Scan0, ImagePixelFormat.Bgr24, pic.Size);
        file.Video.AddFrame(bitmapData); // Encode the frame
        pic.UnlockBits(bitLock);
    }
}
file.Dispose();