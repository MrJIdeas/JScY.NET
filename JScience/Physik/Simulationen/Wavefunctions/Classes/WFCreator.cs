using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF;
using System;
using System.IO;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Classes
{
    public static class WFCreator
    {
        private static IWF_1D NormWave(IWF_1D wave)
        {
            double norm = Math.Sqrt(wave.Norm());
            for (int i = 0; i < wave.DimX; i++)
                wave.SetField(i, wave[i] / norm);
            return wave;
        }

        private static IWF_2D NormWave(IWF_2D wave)
        {
            double norm = Math.Sqrt(wave.Norm());
            for (int i = 0; i < wave.DimX; i++)
                for (int j = 0; j < wave.DimY; j++)
                    wave.SetField(i, j, wave[i, j] / norm);
            return wave;
        }

        #region Free Electron

        public static IWF_1D CreateFreeWave(double k, int DimX, ELatticeBoundary boundary, bool UseGPU)
        {
            WFInfo wfinfo = new WFInfo(DimX, 1, 1, boundary);
            IWF_1D erg = new WF_1D(wfinfo, UseGPU);
            for (int i = 0; i < DimX; i++)
                erg.SetField(i, Complex.Exp(Complex.ImaginaryOne * k * i));
            return NormWave(erg);
        }

        public static IWF_2D CreateFreeWave(double kx, double ky, int DimX, int DimY, ELatticeBoundary boundary, bool UseGPU)
        {
            WFInfo wfinfo = new WFInfo(DimX, DimY, 1, boundary);
            WF_2D erg = new WF_2D(wfinfo, UseGPU);
            for (int i = 0; i < DimX; i++)
                for (int j = 0; j < DimY; j++)
                    erg.SetField(i, j, Complex.Exp(Complex.ImaginaryOne * (kx * i + ky * j)));
            return NormWave(erg);
        }

        #endregion Free Electron

        #region Gauß

        public static IWF_1D CreateGaußWave(double k, double sigma, int DimX, int StartX, ELatticeBoundary boundary, bool UseGPU)
        {
            WFInfo wfinfo = new WFInfo(DimX, 1, 1, boundary);
            IWF_1D erg = new WF_1D(wfinfo, UseGPU);
            for (int i = 0; i < DimX; i++)
                erg.SetField(i, Complex.Exp(new Complex(-Math.Pow(i - StartX, 2) / sigma, 0) - Complex.ImaginaryOne * k * i));
            return NormWave(erg);
        }

        public static IWF_2D CreateGaußWave(double kx, double ky, double sigmaX, double sigmaY, int DimX, int DimY, int StartX, int StartY, ELatticeBoundary boundary, bool UseGPU)
        {
            WFInfo wfinfo = new WFInfo(DimX, DimY, 1, boundary);
            WF_2D erg = new WF_2D(wfinfo, UseGPU);
            for (int i = 0; i < DimX; i++)
                for (int j = 0; j < DimY; j++)
                    erg.SetField(i, j, Complex.Exp(-((i - StartX) * (i - StartX) / sigmaX) - ((j - StartY) * (j - StartY) / sigmaY) - Complex.ImaginaryOne * (kx * i + ky * j)));
            return NormWave(erg);
        }

        #endregion Gauß

        #region Delta

        public static IWF_1D CreateDelta(int DimX, int StartX, ELatticeBoundary boundary, bool UseGPU)
        {
            WFInfo wfinfo = new WFInfo(DimX, 1, 1, boundary);
            IWF_1D erg = new WF_1D(wfinfo, UseGPU);
            erg.SetField(StartX, Complex.One);
            return NormWave(erg);
        }

        public static IWF_2D CreateDelta(int DimX, int DimY, int StartX, int StartY, ELatticeBoundary boundary, bool UseGPU)
        {
            WFInfo wfinfo = new WFInfo(DimX, DimY, 1, boundary);
            WF_2D erg = new WF_2D(wfinfo, UseGPU);
            erg.SetField(StartX, StartY, Complex.One);
            return NormWave(erg);
        }

        #endregion Delta

        #region From File

        public static IWF_1D FromFile1D(string FilePath, char Delimiter, ELatticeBoundary boundary, bool UseGPU)
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException("Invalid Path for Wavefunctíon File.");
            var lines = File.ReadAllLines(FilePath);
            WFInfo wfinfo = new WFInfo(lines.Length, 1, 1, boundary);
            IWF_1D erg = new WF_1D(wfinfo, UseGPU);
            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(Delimiter);
                if (parts.Length != 2)
                    throw new ArgumentException("Invalid Arguments for Wavefunction (2 Elements per Row Real+Imaginary).");
                erg.SetField(i, new Complex(double.Parse(parts[0]), double.Parse(parts[1])));
            }
            return erg;
        }

        public static IWF_2D FromFile2D(string FilePath, char Delimiter, ELatticeBoundary boundary, bool UseGPU)
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException("Invalid Path for Wavefunctíon File.");
            var lines = File.ReadAllLines(FilePath);
            WFInfo wfinfo = new WFInfo(lines.Length, lines[0].Length, 1, boundary);
            WF_2D erg = new WF_2D(wfinfo, UseGPU);
            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(Delimiter);
                if (parts.Length % 2 != 0)
                    throw new ArgumentException("Invalid Arguments for Wavefunction (2 Elements per Row Real+Imaginary).");
                for (int j = 0; j < parts.Length; j += 2)
                    erg.SetField(i, j, new Complex(double.Parse(parts[j]), double.Parse(parts[j + 1])));
            }
            return erg;
        }

        #endregion From File
    }
}