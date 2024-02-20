using JScience.Mathe.ComplexNumbers.VarTypes;
using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF;
using System;

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
            IWF_1D erg = new WF_1D(DimX, boundary);
            for (int i = 0; i < DimX; i++)
                erg.SetField(i, DecComplex.Exp(DecComplex.ImaginaryOne * k * i));
            return NormWave(erg);
        }

        public static IWF_2D CreateFreeWave(decimal kx, decimal ky, int DimX, int DimY, ELatticeBoundary boundary)
        {
            IWF_2D erg = new WF_2D(DimX, DimY, boundary);
            for (int i = 0; i < DimX; i++)
                for (int j = 0; j < DimY; j++)
                    erg.SetField(i, j, DecComplex.Exp(DecComplex.ImaginaryOne * (kx * i + ky * j)));
            return NormWave(erg);
        }

        #endregion Free Electron

        #region Gauß

        public static IWF_1D CreateGaußWave(decimal k, decimal sigma, int DimX, int StartX, ELatticeBoundary boundary)
        {
            IWF_1D erg = new WF_1D(DimX, boundary);
            for (int i = 0; i < DimX; i++)
                erg.SetField(i, DecComplex.Exp(new DecComplex((decimal)-Math.Pow(i - StartX, 2) / sigma, 0) - DecComplex.ImaginaryOne * k * i));
            return NormWave(erg);
        }

        public static IWF_2D CreateGaußWave(decimal kx, decimal ky, decimal sigmaX, decimal sigmaY, int DimX, int DimY, int StartX, int StartY, ELatticeBoundary boundary)
        {
            IWF_2D erg = new WF_2D(DimX, DimY, boundary);
            for (int i = 0; i < DimX; i++)
                for (int j = 0; j < DimY; j++)
                    erg.SetField(i, j, DecComplex.Exp(-decimal.Multiply(i - StartX, i - StartX) / sigmaX - decimal.Multiply(j - StartY, j - StartY) / sigmaY - DecComplex.ImaginaryOne * (kx * i + ky * j)));
            return NormWave(erg);
        }

        #endregion Gauß

        #region Delta

        public static IWF_1D CreateDelta(int DimX, int StartX, ELatticeBoundary boundary)
        {
            IWF_1D erg = new WF_1D(DimX, boundary);
            erg.SetField(StartX, DecComplex.One);
            return NormWave(erg);
        }

        public static IWF_2D CreateDelta(int DimX, int DimY, int StartX, int StartY, ELatticeBoundary boundary)
        {
            IWF_2D erg = new WF_2D(DimX, DimY, boundary);
            erg.SetField(StartX, StartY, DecComplex.One);
            return NormWave(erg);
        }

        #endregion Delta
    }
}