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
    public class WF_2D : WF_1D, IWF_2D
    {
        public new int DimX { get; private set; }

        public int DimY { get; private set; }

        public new int Dimensions => 2;

        public WF_2D(WFInfo wfinfo, ECalculationMethod CalcMethod) : base(wfinfo, CalcMethod)
        {
            DimX = wfinfo.DimX;
            DimY = wfinfo.DimY;
        }

        public Complex this[int x, int y]
        {
            get => this[x + y * DimX];
            set => this[x + y * DimX] = value;
        }

        public Tuple<int, int> getCoordinates(int i)
        {
            int x = i % DimX;
            return new Tuple<int, int>(x, (i - x) / DimX);
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
                            if (coord.Item1 < DimX - 1)
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
                                    neu[i] = this[DimX - 1, coord.Item2];
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
                            if (coord.Item2 < DimY - 1)
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
                                    neu[i] = this[coord.Item1, DimY - 1];
                            }
                        }
                    });
                    return neu;
            }
        }

        public void SetField(int x, int y, Complex value) => this[x, y] = value;

        public double getNorm(int x, int y) => getNorm(x + y * DimX);

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
                Exits.Add(CreateCabExit(DimX - startx, starty));
            }
            if (ky != 0)
            {
                Exits.Add(CreateCabExit(startx, DimY - starty));
            }
            if (kx != 0 && ky != 0)
            {
                Exits.Add(CreateCabExit(DimX - startx, DimY - starty));
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
                    clone = (WF_2D)WFCreator.CreateDelta(DimX, DimY, x, y, Boundary, CalcMethod);
                    break;

                case EWaveType.Gauß:
                    double kx = WFInfo.GetAdditionalInfo<double>("kx");
                    double ky = WFInfo.GetAdditionalInfo<double>("ky");
                    double sigmax = WFInfo.GetAdditionalInfo<double>("sigmaX");
                    double sigmay = WFInfo.GetAdditionalInfo<double>("sigmaY");
                    clone = (WF_2D)WFCreator.CreateGaußWave(kx, ky, sigmax, sigmay, DimX, DimY, x, y, Boundary, CalcMethod);
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