using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.VarTypes;
using System;
using System.Numerics;

namespace JScience.Physik.Simulationen.Wavefunctions.Classes
{
    public static class WFCreator
    {
        private static IWF_1D NormWave(IWF_1D wave)
        {
            double norm = wave.Norm();
            for (int i = 0; i < wave.DimX; i++)
                wave.SetField(i, wave[i] / Math.Sqrt(norm));
            return wave;
        }

        private static IWF_2D NormWave(IWF_2D wave)
        {
            double norm = wave.Norm();
            for (int i = 0; i < wave.DimX; i++)
                for (int j = 0; j < wave.DimY; j++)
                    wave.SetField(i, j, wave[i, j] / Math.Sqrt(norm));
            return wave;
        }

        #region Free Electron

        public static IWF_1D CreateFreeWave(double k, int DimX, ELatticeBoundary boundary)
        {
            IWF_1D erg = new WF_1D(DimX, boundary);
            for (int i = 0; i < DimX; i++)
                erg.SetField(i, Complex.Exp(-Complex.ImaginaryOne * k * i));
            return NormWave(erg);
        }

        public static IWF_2D CreateFreeWave(double kx, double ky, int DimX, int DimY, ELatticeBoundary boundary)
        {
            IWF_2D erg = new WF_2D(DimX, DimY, boundary);
            for (int i = 0; i < DimX; i++)
                for (int j = 0; j < DimY; j++)
                    erg.SetField(i, j, Complex.Exp(-Complex.ImaginaryOne * (kx * i + ky * j)));
            return NormWave(erg);
        }

        #endregion Free Electron

        #region Gauß

        public static IWF_1D CreateGaußWave(double k, double sigma, int DimX, int StartX, ELatticeBoundary boundary)
        {
            IWF_1D erg = new WF_1D(DimX, boundary);
            for (int i = 0; i < DimX; i++)
                erg.SetField(i, Complex.Exp(-Math.Pow(i - StartX, 2) / sigma - Complex.ImaginaryOne * k * i));
            return NormWave(erg);
        }

        public static IWF_2D CreateGaußWave(double kx, double ky, double sigmaX, double sigmaY, int DimX, int DimY, int StartX, int StartY, ELatticeBoundary boundary)
        {
            IWF_2D erg = new WF_2D(DimX, DimY, boundary);
            for (int i = 0; i < DimX; i++)
                for (int j = 0; j < DimY; j++)
                    erg.SetField(i, j, Complex.Exp(-Math.Pow(i - StartX, 2) / sigmaX - Math.Pow(j - StartY, 2) / sigmaY - Complex.ImaginaryOne * (kx * i + ky * j)));
            return NormWave(erg);
        }

        #endregion Gauß

        #region Delta

        public static IWF_1D CreateDelta(int DimX, int StartX, ELatticeBoundary boundary)
        {
            IWF_1D erg = new WF_1D(DimX, boundary);
            erg.SetField(StartX, Complex.One);
            return NormWave(erg);
        }

        public static IWF_2D CreateDelta(int DimX, int DimY, int StartX, int StartY, ELatticeBoundary boundary)
        {
            IWF_2D erg = new WF_2D(DimX, DimY, boundary);
            erg.SetField(StartX, StartY, Complex.One);
            return NormWave(erg);
        }

        #endregion Delta
    }
}