using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using JScy.NET.Enums;
using JScy.NET.Physics.Simulationen.Spins.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF
{
    public class WF_2D(WFInfo wfinfo, ECalculationMethod CalcMethod) : WF_1D(wfinfo, CalcMethod), IWF_2D
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

        public new IWavefunction GetShift(EShift shift)
        {
            WF_2D neu = new WF_2D(WFInfo, CalcMethod);
            switch (shift)
            {
                default:
                    return null;

                case EShift.Xm:
                    Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            var coord = getCoordinates(i);
                            if (coord.Item1 < WFInfo.DimInfo.DimX - 1)
                                neu[i] = this[coord.Item1 + 1, coord.Item2];
                            else
                            {
                                if (Boundary == ELatticeBoundary.Periodic)
                                    neu[i] = this[0, coord.Item2];
                            }
                        }
                    });
                    return neu;

                case EShift.Xp:
                    Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            var coord = getCoordinates(i);
                            if (coord.Item1 > 0)
                                neu[i] = this[coord.Item1 - 1, coord.Item2];
                            else
                            {
                                if (Boundary == ELatticeBoundary.Periodic)
                                    neu[i] = this[WFInfo.DimInfo.DimX - 1, coord.Item2];
                            }
                        }
                    });
                    return neu;

                case EShift.Ym:
                    Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            var coord = getCoordinates(i);
                            if (coord.Item2 < WFInfo.DimInfo.DimY - 1)
                                neu[i] = this[coord.Item1, coord.Item2 + 1];
                            else
                            {
                                if (Boundary == ELatticeBoundary.Periodic)
                                    neu[i] = this[coord.Item1, 0];
                            }
                        }
                    });
                    return neu;

                case EShift.Yp:
                    Parallel.ForEach(neu.rangePartitioner, (range, loopState) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            var coord = getCoordinates(i);
                            if (coord.Item2 > 0)
                                neu[i] = this[coord.Item1, coord.Item2 - 1];
                            else
                            {
                                if (Boundary == ELatticeBoundary.Periodic)
                                    neu[i] = this[coord.Item1, WFInfo.DimInfo.DimY - 1];
                            }
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
            List<CabExit> Exits = new();
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
                    clone = (WF_2D)WFCreator.CreateDelta(WFInfo.DimInfo.DimX, WFInfo.DimInfo.DimY, x, y, Boundary, CalcMethod);
                    break;

                case EWaveType.Gauß:
                    double kx = WFInfo.GetAdditionalInfo<double>("kx");
                    double ky = WFInfo.GetAdditionalInfo<double>("ky");
                    double sigmax = WFInfo.GetAdditionalInfo<double>("sigmaX");
                    double sigmay = WFInfo.GetAdditionalInfo<double>("sigmaY");
                    clone = (WF_2D)WFCreator.CreateGaußWave(kx, ky, sigmax, sigmay, WFInfo.DimInfo.DimX, WFInfo.DimInfo.DimY, x, y, Boundary, CalcMethod);
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