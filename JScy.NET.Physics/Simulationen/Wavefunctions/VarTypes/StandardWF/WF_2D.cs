using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
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

        public Tuple<int, int> getCoordinates(int i)
        {
            int x = i % WFInfo.DimInfo.DimX;
            return new Tuple<int, int>(x, (i - x) / WFInfo.DimInfo.DimX);
        }

        #region Interface

        public new IWavefunction GetShift(EShift shift, int positions = 1)
        {
            if (positions < 0) throw new ArgumentException("Positions must be non-negative.");

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
                    _ = Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            var coord = getCoordinates(i);
                            int sourceX = (coord.Item1 + positions) % dimX;
                            if (sourceX <= dimX - positions)
                                neu[i] = this[sourceX, coord.Item2];
                        }
                    });
                    return neu;

                case EShift.Xp: // Verschiebung nach rechts
                    _ = Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            var coord = getCoordinates(i);
                            int sourceX = (coord.Item1 - positions + dimX) % dimX;
                            if (sourceX > positions)
                                neu[i] = this[sourceX, coord.Item2];
                        }
                    });
                    return neu;

                case EShift.Ym: // Verschiebung nach unten
                    _ = Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            var coord = getCoordinates(i);
                            int sourceY = (coord.Item2 + positions) % dimY;
                            if (sourceY <= dimY - positions)
                                neu[i] = this[coord.Item1, sourceY];
                        }
                    });
                    return neu;

                case EShift.Yp: // Verschiebung nach oben
                    _ = Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            var coord = getCoordinates(i);
                            int sourceY = (coord.Item2 - positions + dimY) % dimY;
                            if (sourceY > positions)
                                neu[i] = this[coord.Item1, sourceY];
                        }
                    });
                    return neu;
            }
        }

        public void SetField(int x, int y, Complex value) => this[x, y] = value;

        public new void SetField(Complex value, params int[] x)
        {
            if (x != null && x.Length > 1)
                SetField(x[0], x[1], value);
        }

        public double getNorm(int x, int y) => getNorm(x + y * WFInfo.DimInfo.DimX);

        #region Cab

        public new List<CabExit> CreateCabExitAuto()
        {
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

            return Exits;
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