﻿using System;
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

        public WF_1D(WFInfo wfinfo, ECalculationMethod Method)
        {
            WFInfo = wfinfo;
            field = new Complex[wfinfo.DimX * wfinfo.DimY * wfinfo.DimZ];
            Boundary = wfinfo.BoundaryInfo;
            rangePartitioner = Partitioner.Create(0, wfinfo.DimX * wfinfo.DimY * wfinfo.DimZ);
            CalcMethod = Method;
            result = new double[field.Length];
        }

        public OrderablePartitioner<Tuple<int, int>> rangePartitioner { get; private set; }
        public Complex[] field { get; private set; }

        private double[] result { get; set; }

        #region Interface

        public ELatticeBoundary Boundary { get; private set; }

        public Complex this[int i] { get => field[i]; protected set => field[i] = value; }
        public int DimX => field.Length;
        public int Dimensions => 1;

        public ECalculationMethod CalcMethod { get; private set; }

        public double Norm()
        {
            switch (CalcMethod)
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

                        IWavefunction.queue.Execute(IWavefunction.Normkernel, null, new long[] { field.Length }, null, null);
                        double[] buf = result;
                        IWavefunction.queue.ReadFromBuffer(IWavefunction.resultBuffer, ref buf, true, null);
                        result = buf;
                        return result.Sum();
                    }

                default:
                    return field.ToList().Sum(x => Math.Pow(x.Magnitude, 2));
            }
        }

        protected virtual IWavefunction getEmptyLikeThis() => (IWavefunction)Activator.CreateInstance(GetType(), WFInfo, CalcMethod);

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

        public IWavefunction GetShift(EShift shift)
        {
            WF_1D neu = new WF_1D(WFInfo, CalcMethod);
            switch (shift)
            {
                default:
                    return null;

                case EShift.Xm:
                    Parallel.For(0, neu.DimX - 1, (i) =>
                    {
                        neu.field[i] = field[i + 1];
                    });
                    if (Boundary == ELatticeBoundary.Periodic)
                        neu.field[neu.DimX - 1] = field[0];
                    return neu;

                case EShift.Xp:
                    Parallel.For(1, neu.DimX, (i) =>
                    {
                        neu.field[i] = field[i - 1];
                    });
                    if (Boundary == ELatticeBoundary.Periodic)
                        neu.field[0] = field[DimX - 1];
                    return neu;
            }
        }

        public void SetField(int x, Complex value) => field[x] = value;

        public void SetField(Complex value, params int[] x)
        {
            if (x != null && x.Length > 0)
                SetField(x[0], value);
        }

        public void SetField(Complex[] field) => this.field = field;

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
            List<CabExit> Exits = [CreateCabExit(startx), CreateCabExit(DimX - startx)];
            return Exits;
        }

        private CabExit CreateCabExit(int x)
        {
            IWavefunction clone;
            switch (WFInfo.waveType)
            {
                default:
                    throw new Exception("Not enough Data to Auto Set Cab Exits!");

                case EWaveType.Delta:
                    clone = (WF_1D)WFCreator.CreateDelta(DimX, x, Boundary, CalcMethod);
                    break;

                case EWaveType.Gauß:
                    double k = WFInfo.GetAdditionalInfo<double>("k");
                    double sigma = WFInfo.GetAdditionalInfo<double>("sigma");
                    clone = (WF_1D)WFCreator.CreateGaußWave(k, sigma, DimX, x, Boundary, CalcMethod);
                    break;
            }
            return clone != null ? new CabExit(x.ToString(), clone) : null;
        }

        #endregion Cab

        #endregion Interface
    }
}