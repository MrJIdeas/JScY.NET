using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Cloo;
using JScy.NET.Enums;
using JScy.NET.Physics.Simulationen.Spins.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF
{
    public class WF_1D : IWF_1D
    {
        public WFInfo WFInfo { get; private set; }

        public WF_1D(WFInfo wfinfo)
        {
            WFInfo = wfinfo;
            field = new Complex[wfinfo.DimInfo.DimX * wfinfo.DimInfo.DimY * wfinfo.DimInfo.DimZ];
            Boundary = wfinfo.BoundaryInfo;
            rangePartitioner = Partitioner.Create(0, wfinfo.DimInfo.DimX * wfinfo.DimInfo.DimY * wfinfo.DimInfo.DimZ);
            result = new double[field.Length];
        }

        public OrderablePartitioner<Tuple<int, int>> rangePartitioner { get; private set; }
        public Complex[] field { get; private set; }

        private double[] result { get; set; }

        #region Interface

        #region X-Nachbarn ermitteln

        public int?[] getNeighborsX(int i, int positions = 1) => [getNeightborX(i, -positions), getNeightborX(i, positions)];

        public int? getNeightborX(int i, int direction)
        {
            if (direction == 0) return null;
            int newX = i + direction;
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
            return newX < 0 || newX >= field.Length ? null : newX;
        }

        #endregion X-Nachbarn ermitteln

        public ELatticeBoundary Boundary { get; private set; }

        public Complex this[int i] { get => field[i]; protected set => field[i] = value; }

        public double Norm()
        {
            switch (WFInfo.CalcMethod)
            {
                case ECalculationMethod.CPU_Multihreading:
                    return field.ToList().AsParallel().Sum(x => Math.Pow(x.Magnitude, 2));

                case ECalculationMethod.OpenCL:
                    {
                        if (IWavefunction.bBuffer == null)
                            IWavefunction.bBuffer = new ComputeBuffer<Complex>(IWavefunction.context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, field);
                        else
                            IWavefunction.queue.WriteToBuffer(field, IWavefunction.bBuffer, true, null);
                        if (IWavefunction.resultBuffer == null)
                            IWavefunction.resultBuffer = new ComputeBuffer<double>(IWavefunction.context, ComputeMemoryFlags.WriteOnly, result.Length);
                        else
                            IWavefunction.queue.WriteToBuffer(result, IWavefunction.resultBuffer, true, null);

                        IWavefunction.Normkernel.SetMemoryArgument(0, IWavefunction.resultBuffer);
                        IWavefunction.Normkernel.SetMemoryArgument(1, IWavefunction.bBuffer);

                        IWavefunction.queue.Execute(IWavefunction.Normkernel, null, [field.Length], null, null);
                        double[] buf = result;
                        IWavefunction.queue.ReadFromBuffer(IWavefunction.resultBuffer, ref buf, true, null);
                        result = buf;
                        return result.Sum();
                    }

                default:
                    return field.ToList().Sum(x => Math.Pow(x.Magnitude, 2));
            }
        }

        protected virtual IWavefunction getEmptyLikeThis() => (IWavefunction)Activator.CreateInstance(GetType(), WFInfo);

        public IWavefunction Conj()
        {
            IWavefunction conj = getEmptyLikeThis();
            Parallel.ForEach(conj.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    conj.SetField(i, Complex.Conjugate(field[i]));
            });
            return conj;
        }

        #region Shifting

        public IWavefunction GetShift(EShift shift, int positions = 1)
        {
            if (positions < 0) throw new ArgumentException("Positions must be non-negative.");

            WF_1D neu = new(WFInfo);
            int dimX = WFInfo.DimInfo.DimX;

            // Normalisierung der Positionen, falls die Verschiebung größer als die Dimension ist
            positions %= dimX;

            switch (shift)
            {
                default:
                    return null;

                case EShift.Xm: // Verschiebung nach links
                    if (WFInfo.CalcMethod == ECalculationMethod.CPU)
                    {
                        for (int i = 0; i < dimX; i++)
                        {
                            int? sourceIndex = getNeightborX(i, positions); // Berechnung des neuen Indexes
                            if (sourceIndex != null)
                                neu.field[i] = field[(int)sourceIndex];
                        }
                    }
                    else if (WFInfo.CalcMethod is ECalculationMethod.CPU_Multihreading
                        or ECalculationMethod.OpenCL)
                    {
                        Parallel.For(0, dimX, i =>
                        {
                            int? sourceIndex = getNeightborX(i, positions); // Berechnung des neuen Indexes
                            if (sourceIndex != null)
                                neu.field[i] = field[(int)sourceIndex];
                        });
                    }
                    return neu;

                case EShift.Xp: // Verschiebung nach rechts
                    if (WFInfo.CalcMethod == ECalculationMethod.CPU)
                    {
                        for (int i = 0; i < dimX; i++)
                        {
                            int? sourceIndex = getNeightborX(i, -positions); // Berechnung des neuen Indexes
                            if (sourceIndex != null)
                                neu.field[i] = field[(int)sourceIndex];
                        }
                    }
                    else if (WFInfo.CalcMethod is ECalculationMethod.CPU_Multihreading
                        or ECalculationMethod.OpenCL)
                    {
                        Parallel.For(0, dimX, i =>
                        {
                            int? sourceIndex = getNeightborX(i, -positions); // Berechnung des neuen Indexes
                            if (sourceIndex != null)
                                neu.field[i] = field[(int)sourceIndex];
                        });
                    }
                    return neu;
            }
        }

        #endregion Shifting

        #region Feldwerte setzen

        public void SetField(int x, Complex value) => field[x] = value;

        public void SetField(Complex value, params int[] x)
        {
            if (x != null && x.Length > 0)
                SetField(x[0], value);
        }

        public void SetField(Complex[] field) => this.field = field;

        #endregion Feldwerte setzen

        public void Clear()
        {
            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    field[i] = Complex.Zero;
            });
        }

        public IWavefunction Clone()
        {
            IWavefunction conj = getEmptyLikeThis();
            Parallel.ForEach(conj.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    conj.SetField(i, field[i]);
            });
            return conj;
        }

        public double getNorm(int x) => field[x].Magnitude;

        #region Cab

        public List<CabExit> CreateCabExitAuto()
        {
            int startx = WFInfo.GetAdditionalInfo<int>("startX");
            if (startx <= 0)
                throw new Exception("Not enough Data to Auto Set Cab Exits!");
            List<CabExit> Exits = [CreateCabExit(startx), CreateCabExit(WFInfo.DimInfo.DimX - startx)];
            return Exits.DistinctBy(x => x.ExitName).ToList();
        }

        private CabExit CreateCabExit(int x)
        {
            IWavefunction clone;
            switch (WFInfo.waveType)
            {
                default:
                    throw new Exception("Not enough Data to Auto Set Cab Exits!");

                case EWaveType.Delta:
                    clone = (WF_1D)WFCreator.CreateDelta(WFInfo.DimInfo.DimX, x, Boundary, WFInfo.CalcMethod);
                    break;

                case EWaveType.Gauß:
                    double k = WFInfo.GetAdditionalInfo<double>("k");
                    double sigma = WFInfo.GetAdditionalInfo<double>("sigma");
                    clone = (WF_1D)WFCreator.CreateGaußWave(k, sigma, WFInfo.DimInfo.DimX, x, Boundary, WFInfo.CalcMethod);
                    break;
            }
            return clone != null ? new CabExit(x.ToString(), clone) : null;
        }

        #endregion Cab

        #endregion Interface
    }
}