using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;
using FFMediaToolkit;
using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Classes;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.Hamiltonoperators.TightBinding.VarTypes;
using JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Classes;
using JScience.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF;
using SkiaSharp;
using System.Drawing;
using System.Drawing.Imaging;

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

for (int i = 0; i < 100; i++)
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