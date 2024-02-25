using JScience.Mathe.ComplexNumbers.VarTypes;
using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF;
using System;
using System.IO;

namespace JScience.Physik.Simulationen.Wavefunctions.Classes
{
    public static class WFCreator
    {
        private static IWF_1D NormWave(IWF_1D wave)
        {
            decimal norm = (decimal)Math.Sqrt((double)wave.Norm());
            for (int i = 0; i < wave.DimX; i++)
                wave.SetField(i, wave[i] / norm);
            return wave;
        }

        private static IWF_2D NormWave(IWF_2D wave)
        {
            decimal norm = (decimal)Math.Sqrt((double)wave.Norm());
            for (int i = 0; i < wave.DimX; i++)
                for (int j = 0; j < wave.DimY; j++)
                    wave.SetField(i, j, wave[i, j] / norm);
            return wave;
        }

        #region Free Electron

        public static IWF_1D CreateFreeWave(decimal k, int DimX, ELatticeBoundary boundary)
        {
            WFInfo wfinfo = new WFInfo(DimX, 1, 1, boundary);
            IWF_1D erg = new WF_1D(wfinfo);
            for (int i = 0; i < DimX; i++)
                erg.SetField(i, DecComplex.Exp(DecComplex.ImaginaryOne * k * i));
            return NormWave(erg);
        }

        public static IWF_2D CreateFreeWave(decimal kx, decimal ky, int DimX, int DimY, ELatticeBoundary boundary)
        {
            WFInfo wfinfo = new WFInfo(DimX, DimY, 1, boundary);
            WF_2D erg = new WF_2D(wfinfo);
            for (int i = 0; i < DimX; i++)
                for (int j = 0; j < DimY; j++)
                    erg.SetField(i, j, DecComplex.Exp(DecComplex.ImaginaryOne * (kx * i + ky * j)));
            return NormWave(erg);
        }

        #endregion Free Electron

        #region Gauß

        public static IWF_1D CreateGaußWave(decimal k, decimal sigma, int DimX, int StartX, ELatticeBoundary boundary)
        {
            WFInfo wfinfo = new WFInfo(DimX, 1, 1, boundary);
            IWF_1D erg = new WF_1D(wfinfo);
            for (int i = 0; i < DimX; i++)
                erg.SetField(i, DecComplex.Exp(new DecComplex((decimal)-Math.Pow(i - StartX, 2) / sigma, 0) - DecComplex.ImaginaryOne * k * i));
            return NormWave(erg);
        }

        public static IWF_2D CreateGaußWave(decimal kx, decimal ky, decimal sigmaX, decimal sigmaY, int DimX, int DimY, int StartX, int StartY, ELatticeBoundary boundary)
        {
            WFInfo wfinfo = new WFInfo(DimX, DimY, 1, boundary);
            WF_2D erg = new WF_2D(wfinfo);
            for (int i = 0; i < DimX; i++)
                for (int j = 0; j < DimY; j++)
                    erg.SetField(i, j, DecComplex.Exp(-decimal.Multiply(i - StartX, i - StartX) / sigmaX - decimal.Multiply(j - StartY, j - StartY) / sigmaY - DecComplex.ImaginaryOne * (kx * i + ky * j)));
            return NormWave(erg);
        }

        #endregion Gauß

        #region Delta

        public static IWF_1D CreateDelta(int DimX, int StartX, ELatticeBoundary boundary)
        {
            WFInfo wfinfo = new WFInfo(DimX, 1, 1, boundary);
            IWF_1D erg = new WF_1D(wfinfo);
            erg.SetField(StartX, DecComplex.One);
            return NormWave(erg);
        }

        public static IWF_2D CreateDelta(int DimX, int DimY, int StartX, int StartY, ELatticeBoundary boundary)
        {
            WFInfo wfinfo = new WFInfo(DimX, DimY, 1, boundary);
            WF_2D erg = new WF_2D(wfinfo);
            erg.SetField(StartX, StartY, DecComplex.One);
            return NormWave(erg);
        }

        #endregion Delta

        #region From File

        public static IWF_1D FromFile1D(string FilePath, char Delimiter, ELatticeBoundary boundary)
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException("Invalid Path for Wavefunctíon File.");
            var lines = File.ReadAllLines(FilePath);
            WFInfo wfinfo = new WFInfo(lines.Length, 1, 1, boundary);
            IWF_1D erg = new WF_1D(wfinfo);
            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(Delimiter);
                if (parts.Length != 2)
                    throw new ArgumentException("Invalid Arguments for Wavefunction (2 Elements per Row Real+Imaginary).");
                erg.SetField(i, new DecComplex(Decimal.Parse(parts[0]), Decimal.Parse(parts[1])));
            }
            return erg;
        }

        public static IWF_2D FromFile2D(string FilePath, char Delimiter, ELatticeBoundary boundary)
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException("Invalid Path for Wavefunctíon File.");
            var lines = File.ReadAllLines(FilePath);
            WFInfo wfinfo = new WFInfo(lines.Length, lines[0].Length, 1, boundary);
            WF_2D erg = new WF_2D(wfinfo);
            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(Delimiter);
                if (parts.Length % 2 != 0)
                    throw new ArgumentException("Invalid Arguments for Wavefunction (2 Elements per Row Real+Imaginary).");
                for (int j = 0; j < parts.Length; j += 2)
                    erg.SetField(i, j, new DecComplex(Decimal.Parse(parts[j]), Decimal.Parse(parts[j + 1])));
            }
            return erg;
        }

        #endregion From File
    }
}