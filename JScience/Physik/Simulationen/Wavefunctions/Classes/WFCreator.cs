using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using JScience.Physik.Simulationen.Wavefunctions.VarTypes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace JScience.Physik.Simulationen.Wavefunctions.Classes
{
    public static class WFCreator
    {
        private static IWF_1D NormWave(IWF_1D wave)
        {
            double norm = wave.Norm();
            for (int i = 0; i < wave.DimX; i++)
                wave.SetField(i, wave[i] / norm);
            return wave;
        }

        #region Free Electron

        public static IWF_1D CreateFreeWave(double k, int DimX)
        {
            IWF_1D erg = new WF_1D(DimX);
            for (int i = 0; i < DimX; i++)
                erg.SetField(i, Complex.Exp(-Complex.ImaginaryOne * k * i));
            return NormWave(erg);
        }

        #endregion Free Electron

        #region Gauß

        public static IWF_1D CreateGaußWave(double k, double sigma, int DimX, int StartX)
        {
            IWF_1D erg = new WF_1D(DimX);
            for (int i = 0; i < DimX; i++)
                erg.SetField(i, Complex.Exp(-Math.Pow(i - StartX, 2) / sigma - Complex.ImaginaryOne * k * i));
            return NormWave(erg);
        }

        #endregion Gauß

        #region Delta

        public static IWF_1D CreateDelta(int DimX, int StartX)
        {
            IWF_1D erg = new WF_1D(DimX);
            erg.SetField(StartX, Complex.One);
            return NormWave(erg);
        }

        #endregion Delta
    }
}