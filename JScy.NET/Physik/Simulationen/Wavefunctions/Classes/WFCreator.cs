using JScy.NET.Enums;
using JScy.NET.Physik.Simulationen.Spins.Enums;
using JScy.NET.Physik.Simulationen.Wavefunctions.Enums;
using JScy.NET.Physik.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF;
using ScottPlot.Plottables;
using System;
using System.IO;
using System.Numerics;

namespace JScy.NET.Physik.Simulationen.Wavefunctions.Classes
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

        /// <summary>
        /// Erstellt eine 1D Freie Welle
        /// </summary>
        /// <param name="k">Wellenzahl.</param>
        /// <param name="DimX">Anzahl Gitterplätze x</param>
        /// <param name="boundary">Randbedingungen.</param>
        /// <param name="CalcMethod">Kalkulationsmethode.</param>
        /// <returns>Wellenfunktion.</returns>
        public static IWF_1D CreateFreeWave(double k, int DimX, ELatticeBoundary boundary, ECalculationMethod CalcMethod)
        {
            WFInfo wfinfo = new WFInfo(DimX, 1, 1, boundary, EWaveType.FreeWave);
            wfinfo.AddAdditionalInfo("k", k);
            IWF_1D erg = new WF_1D(wfinfo, CalcMethod);
            for (int i = 0; i < DimX; i++)
                erg.SetField(i, Complex.Exp(Complex.ImaginaryOne * k * i));
            return NormWave(erg);
        }

        /// <summary>
        /// Erstellt eine 2D Freie Welle
        /// </summary>
        /// <param name="kx">Wellenzahl x.</param>
        /// <param name="ky">Wellenzahl y.</param>
        /// <param name="DimX">Anzahl Gitterplätze x</param>
        /// <param name="DimY">Anzahl Gitterplätze y</param>
        /// <param name="boundary">Randbedingungen.</param>
        /// <param name="CalcMethod">Kalkulationsmethode.</param>
        /// <returns>Wellenfunktion.</returns>
        public static IWF_2D CreateFreeWave(double kx, double ky, int DimX, int DimY, ELatticeBoundary boundary, ECalculationMethod CalcMethod)
        {
            WFInfo wfinfo = new WFInfo(DimX, DimY, 1, boundary, EWaveType.FreeWave);
            wfinfo.AddAdditionalInfo("kx", kx);
            wfinfo.AddAdditionalInfo("ky", ky);
            WF_2D erg = new WF_2D(wfinfo, CalcMethod);
            for (int i = 0; i < DimX; i++)
                for (int j = 0; j < DimY; j++)
                    erg.SetField(i, j, Complex.Exp(Complex.ImaginaryOne * (kx * i + ky * j)));
            return NormWave(erg);
        }

        #endregion Free Electron

        #region Gauß

        /// <summary>
        /// Erstellt eine 1D Gaußsche Welle.
        /// </summary>
        /// <param name="k">Wellenzahl x.</param>
        /// <param name="DimX">Anzahl Gitterplätze x</param>
        /// <param name="StartX">Startwert x</param>
        /// <param name="sigma">Breite Gaußkurve x.</param>
        /// <param name="boundary">Randbedingungen.</param>
        /// <param name="CalcMethod">Kalkulationsmethode.</param>
        /// <returns>Wellenfunktion.</returns>
        public static IWF_1D CreateGaußWave(double k, double sigma, int DimX, int StartX, ELatticeBoundary boundary, ECalculationMethod CalcMethod)
        {
            WFInfo wfinfo = new WFInfo(DimX, 1, 1, boundary, EWaveType.Gauß);
            wfinfo.AddAdditionalInfo("k", k);
            wfinfo.AddAdditionalInfo("sigma", sigma);
            wfinfo.AddAdditionalInfo("startX", StartX);
            IWF_1D erg = new WF_1D(wfinfo, CalcMethod);
            for (int i = 0; i < DimX; i++)
                erg.SetField(i, Complex.Exp(new Complex(-Math.Pow(i - StartX, 2) / sigma, 0) - Complex.ImaginaryOne * k * i));
            return NormWave(erg);
        }

        /// <summary>
        /// Erstellt eine 2D Gaußsche Welle
        /// </summary>
        /// <param name="kx">Wellenzahl x.</param>
        /// <param name="ky">Wellenzahl y.</param>
        /// <param name="DimX">Anzahl Gitterplätze x</param>
        /// <param name="DimY">Anzahl Gitterplätze y</param>
        /// <param name="sigmaX">Breite Gaußkurve x.</param>
        /// <param name="sigmaY">Breite Gaußkurve y.</param>
        /// <param name="StartX">Startwert x</param>
        /// <param name="StartY">Startwert y</param>
        /// <param name="boundary">Randbedingungen.</param>
        /// <param name="CalcMethod">Kalkulationsmethode.</param>
        /// <returns>Wellenfunktion.</returns>
        public static IWF_2D CreateGaußWave(double kx, double ky, double sigmaX, double sigmaY, int DimX, int DimY, int StartX, int StartY, ELatticeBoundary boundary, ECalculationMethod CalcMethod)
        {
            WFInfo wfinfo = new WFInfo(DimX, DimY, 1, boundary, EWaveType.Gauß);
            wfinfo.AddAdditionalInfo("kx", kx);
            wfinfo.AddAdditionalInfo("ky", ky);
            wfinfo.AddAdditionalInfo("sigmaX", sigmaX);
            wfinfo.AddAdditionalInfo("sigmaY", sigmaY);
            wfinfo.AddAdditionalInfo("startX", StartX);
            wfinfo.AddAdditionalInfo("startY", StartY);
            WF_2D erg = new WF_2D(wfinfo, CalcMethod);
            for (int i = 0; i < DimX; i++)
                for (int j = 0; j < DimY; j++)
                    erg.SetField(i, j, Complex.Exp(-((i - StartX) * (i - StartX) / sigmaX) - (j - StartY) * (j - StartY) / sigmaY - Complex.ImaginaryOne * (kx * i + ky * j)));
            return NormWave(erg);
        }

        #endregion Gauß

        #region Delta

        /// <summary>
        /// Erstellt eine 1D Delta Peak.
        /// </summary>
        /// <param name="DimX">Anzahl Gitterplätze x</param>
        /// <param name="StartX">Startwert x</param>
        /// <param name="boundary">Randbedingungen.</param>
        /// <param name="CalcMethod">Kalkulationsmethode.</param>
        /// <returns>Wellenfunktion.</returns>
        public static IWF_1D CreateDelta(int DimX, int StartX, ELatticeBoundary boundary, ECalculationMethod CalcMethod)
        {
            WFInfo wfinfo = new WFInfo(DimX, 1, 1, boundary, EWaveType.Delta);
            wfinfo.AddAdditionalInfo("startX", StartX);
            IWF_1D erg = new WF_1D(wfinfo, CalcMethod);
            erg.SetField(StartX, Complex.One);
            return NormWave(erg);
        }

        /// <summary>
        /// Erstellt einen 2D Delta-Peak.
        /// </summary>
        /// <param name="DimX">Anzahl Gitterplätze x</param>
        /// <param name="DimY">Anzahl Gitterplätze y</param>
        /// <param name="StartX">Startwert x</param>
        /// <param name="StartY">Startwert y</param>
        /// <param name="boundary">Randbedingungen.</param>
        /// <param name="CalcMethod">Kalkulationsmethode.</param>
        /// <returns>Wellenfunktion.</returns>
        public static IWF_2D CreateDelta(int DimX, int DimY, int StartX, int StartY, ELatticeBoundary boundary, ECalculationMethod CalcMethod)
        {
            WFInfo wfinfo = new WFInfo(DimX, DimY, 1, boundary, EWaveType.Delta);
            wfinfo.AddAdditionalInfo("startX", StartX);
            wfinfo.AddAdditionalInfo("startY", StartY);
            WF_2D erg = new WF_2D(wfinfo, CalcMethod);
            erg.SetField(StartX, StartY, Complex.One);
            return NormWave(erg);
        }

        #endregion Delta

        #region From File

        /// <summary>
        /// Erstellt eine Wellenfunktion anhand einer Datei.
        /// </summary>
        /// <param name="FilePath">Dateipfad.</param>
        /// <param name="Delimiter">Trennzeichen für Real-/Imaginärteil.</param>
        /// <param name="boundary">Randbedingungen.</param>
        /// <param name="CalcMethod">Kalulationsmethode.</param>
        /// <returns>Wellenfunktion.</returns>
        /// <exception cref="FileNotFoundException">Wenn Datei nicht gefunden wird.</exception>
        /// <exception cref="ArgumentException">Falsches Trennzeichen?</exception>
        public static IWF_1D FromFile1D(string FilePath, char Delimiter, ELatticeBoundary boundary, ECalculationMethod CalcMethod)
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException("Invalid Path for Wavefunctíon File.");
            var lines = File.ReadAllLines(FilePath);
            WFInfo wfinfo = new WFInfo(lines.Length, 1, 1, boundary, EWaveType.Custom);
            IWF_1D erg = new WF_1D(wfinfo, CalcMethod);
            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(Delimiter);
                if (parts.Length != 2)
                    throw new ArgumentException("Invalid Arguments for Wavefunction (2 Elements per Row Real+Imaginary).");
                erg.SetField(i, new Complex(double.Parse(parts[0]), double.Parse(parts[1])));
            }
            return erg;
        }

        /// <summary>
        /// Erstellt eine Wellenfunktion anhand einer Datei.
        /// </summary>
        /// <param name="FilePath">Dateipfad.</param>
        /// <param name="Delimiter">Trennzeichen für Real-/Imaginärteil.</param>
        /// <param name="boundary">Randbedingungen.</param>
        /// <param name="CalcMethod">Kalulationsmethode.</param>
        /// <returns>Wellenfunktion.</returns>
        /// <exception cref="FileNotFoundException">Wenn Datei nicht gefunden wird.</exception>
        /// <exception cref="ArgumentException">Falsches Trennzeichen?</exception>
        public static IWF_2D FromFile2D(string FilePath, char Delimiter, ELatticeBoundary boundary, ECalculationMethod CalcMethod)
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException("Invalid Path for Wavefunctíon File.");
            var lines = File.ReadAllLines(FilePath);
            WFInfo wfinfo = new WFInfo(lines.Length, lines[0].Length, 1, boundary, EWaveType.Custom);
            WF_2D erg = new WF_2D(wfinfo, CalcMethod);
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