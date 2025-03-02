﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using JScy.NET.Enums;
using JScy.NET.Physics.Simulationen.Spins.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.AttributesCustom;
using JScy.NET.Physics.Simulationen.Wavefunctions.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF
{
    public class WF_2D(WFInfo wfinfo) : WF_1D(wfinfo), IWF_2D
    {
        public Complex this[int x, int y]
        {
            get => this[x + y * WFInfo.DimInfo.DimX];
            set => this[x + y * WFInfo.DimInfo.DimX] = value;
        }

        #region Koordinatenhandling

        public override int?[] getCoordinates(int i)
        {
            if (i < 0 || i >= field.Length) return null;
            Tuple<int, int> erg = getCoordinatesXY(i);
            return [erg.Item1, erg.Item2];
        }

        public Tuple<int, int> getCoordinatesXY(int i)
        {
            int x = i % WFInfo.DimInfo.DimX;
            int y = ((i - x) / WFInfo.DimInfo.DimX) % WFInfo.DimInfo.DimY;
            return new Tuple<int, int>(x, y);
        }

        #region X-Nachbarn ermitteln

        public int?[] getNeighborsX(int i, int positions = 1) => [getNeightborX(i, -positions), getNeightborX(i, positions)];

        public int? getNeightborX(int i, int direction)
        {
            if (direction == 0) return null;
            var coord = getCoordinates(i);
            int newX = (int)(coord[0] + direction);
            if (newX < 0 || newX >= WFInfo.DimInfo.DimX)
            {
                switch (WFInfo.BoundaryInfo)
                {
                    case ELatticeBoundary.Periodic:
                        newX = WFInfo.DimInfo.DimX - Math.Abs(newX);
                        break;

                    case ELatticeBoundary.Reflection:
                        return null;

                    default:
                        return null;
                }
            }
            var left = newX + coord[1] * WFInfo.DimInfo.DimX;
            return left < 0 || left >= field.Length ? null : left;
        }

        #endregion X-Nachbarn ermitteln

        #region Y-Nachbarn ermitteln

        public int?[] getNeighborsY(int i, int positions = 1) => [getNeightborY(i, -positions), getNeightborY(i, positions)];

        public int? getNeightborY(int i, int direction)
        {
            if (direction == 0) return null;
            var coord = getCoordinates(i);
            int newY = (int)(coord[1] + direction);
            if (newY < 0 || newY >= WFInfo.DimInfo.DimY)
            {
                switch (WFInfo.BoundaryInfo)
                {
                    case ELatticeBoundary.Periodic:
                        newY = WFInfo.DimInfo.DimY - Math.Abs(newY);
                        break;

                    case ELatticeBoundary.Reflection:
                        return null;

                    default:
                        return null;
                }
            }
            var left = newY * WFInfo.DimInfo.DimX + coord[0];
            return left < 0 || left >= field.Length ? null : left;
        }

        #endregion Y-Nachbarn ermitteln

        #endregion Koordinatenhandling

        #region Interface

        public new IWavefunction GetShift(EShift shift, int positions = 1)
        {
            if (positions < 0) throw new ArgumentException("Positions must be non-negative.");

            var fieldInfo = typeof(EShift).GetField(shift.ToString());
            var attribute = (MathSignAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(MathSignAttribute));

            // Überprüfen, ob das Attribut vorhanden ist und dann die Beschreibung anzeigen
            if (attribute == null || attribute.Sign == 0) return null;
            WF_2D neu = new(WFInfo);
            int dimX = WFInfo.DimInfo.DimX;
            int dimY = WFInfo.DimInfo.DimY;

            // Normalisierung der Positionen
            positions %= (shift == EShift.Xm || shift == EShift.Xp) ? dimX : dimY;

            switch (shift)
            {
                default:
                    return null;

                case EShift.Xm: // Verschiebung nach links
                case EShift.Xp: // Verschiebung nach rechts
                    if (WFInfo.CalcMethod is ECalculationMethod.CPU)
                    {
                        for (int i = 0; i < field.Length; i++)
                        {
                            int? neighbor = getNeightborX(i, attribute.Sign * positions);
                            if (neighbor != null)
                                neu[i] = field[(int)neighbor];
                        }
                    }
                    else if (WFInfo.CalcMethod is ECalculationMethod.CPU_Multihreading
                        or ECalculationMethod.OpenCL)
                    {
                        _ = Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                        {
                            for (int i = range.Item1; i < range.Item2; i++)
                            {
                                int? neighbor = getNeightborX(i, attribute.Sign * positions);
                                if (neighbor != null)
                                    neu[i] = field[(int)neighbor];
                            }
                        });
                    }
                    return neu;

                case EShift.Ym: // Verschiebung nach unten
                case EShift.Yp: // Verschiebung nach oben
                    if (WFInfo.CalcMethod is ECalculationMethod.CPU)
                    {
                        for (int i = 0; i < field.Length; i++)
                        {
                            int? neighbor = getNeightborY(i, attribute.Sign * positions);
                            if (neighbor != null)
                                neu[i] = field[(int)neighbor];
                        }
                    }
                    else if (WFInfo.CalcMethod is ECalculationMethod.CPU_Multihreading
                        or ECalculationMethod.OpenCL)
                    {
                        _ = Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                        {
                            for (int i = range.Item1; i < range.Item2; i++)
                            {
                                int? neighbor = getNeightborY(i, attribute.Sign * positions);
                                if (neighbor != null)
                                    neu[i] = field[(int)neighbor];
                            }
                        });
                    }
                    return neu;
            }
        }

        #region Felder setzen

        public void SetField(int x, int y, Complex value) => this[x, y] = value;

        public new void SetField(Complex value, params int[] x)
        {
            if (x != null && x.Length == 2)
                SetField(x[0], x[1], value);
            else
                throw new ApplicationException("Koordinatenarray fehlerhaft. Länge " + x?.Length);
        }

        #endregion Felder setzen

        public double getNorm(int x, int y) => getNorm(x + y * WFInfo.DimInfo.DimX);

        #region Cab

        public new List<CabExit> CreateCabExitAuto()
        {
            if (WFInfo.waveType == EWaveType.FreeWave) return null;
            int startx = WFInfo.GetAdditionalInfo<int>("startX");
            int starty = WFInfo.GetAdditionalInfo<int>("startY");
            if (startx <= 0 || starty <= 0)
                throw new Exception("Not enough Data to Auto Set Cab Exits!");
            List<CabExit> Exits = [];
            double kx = 0, ky = 0;
            switch (WFInfo.waveType)
            {
                default:
                    throw new Exception("Not enough Data to Auto Set Cab Exits!");

                case EWaveType.Delta:

                    break;

                case EWaveType.Gauß:
                    kx = WFInfo.GetAdditionalInfo<double>("kx");
                    ky = WFInfo.GetAdditionalInfo<double>("ky");
                    break;
            }
            Exits.Add(CreateCabExit(startx, starty));
            if (kx != 0)
            {
                Exits.Add(CreateCabExit(WFInfo.DimInfo.DimX - startx, starty));
            }
            if (ky != 0)
            {
                Exits.Add(CreateCabExit(startx, WFInfo.DimInfo.DimY - starty));
            }
            if (kx != 0 && ky != 0)
            {
                Exits.Add(CreateCabExit(WFInfo.DimInfo.DimX - startx, WFInfo.DimInfo.DimY - starty));
            }

            return Exits.DistinctBy(x => x.ExitName).ToList();
        }

        private CabExit CreateCabExit(int x, int y)
        {
            IWavefunction clone;
            switch (WFInfo.waveType)
            {
                default:
                    throw new Exception("Not enough Data to Auto Set Cab Exits!");

                case EWaveType.Delta:
                    clone = (WF_2D)WFCreator.CreateDelta(WFInfo.DimInfo.DimX, WFInfo.DimInfo.DimY, x, y, Boundary, WFInfo.CalcMethod);
                    break;

                case EWaveType.Gauß:
                    double kx = WFInfo.GetAdditionalInfo<double>("kx");
                    double ky = WFInfo.GetAdditionalInfo<double>("ky");
                    double sigmax = WFInfo.GetAdditionalInfo<double>("sigmaX");
                    double sigmay = WFInfo.GetAdditionalInfo<double>("sigmaY");
                    clone = (WF_2D)WFCreator.CreateGaußWave(kx, ky, sigmax, sigmay, WFInfo.DimInfo.DimX, WFInfo.DimInfo.DimY, x, y, Boundary, WFInfo.CalcMethod);
                    break;
            }
            if (clone != null)
            {
                var key = string.Format("x_{0}_y_{1}", x, y);
                return new CabExit(key, clone);
            }
            else
                return null;
        }

        #endregion Cab

        #endregion Interface
    }
}