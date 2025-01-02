using Cloo;
using JScy.NET.Enums;
using JScy.NET.Physics.Simulationen.Spins.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces
{
    public interface IWavefunction
    {
        ELatticeBoundary Boundary { get; }

        WFInfo WFInfo { get; }

        OrderablePartitioner<Tuple<int, int>> rangePartitioner { get; }

        IWavefunction GetShift(EShift shift, int positions = 1);

        void Clear();

        #region Feld

        Complex this[int x] { get; }

        Complex[] field { get; }

        void SetField(int x, Complex value);

        void SetField(Complex[] field);

        IWavefunction Conj();

        IWavefunction Clone();

        void SetField(Complex value, params int[] dims);

        int?[] getCoordinates(int i);

        #endregion Feld

        #region Norm

        double Norm();

        double getNorm(int x);

        #endregion Norm

        #region C alpha beta

        List<CabExit> CreateCabExitAuto();

        #endregion

        #region Kernel CL

        #region Kernel String

        private static readonly string kernelSource = @"
            typedef struct {
                double real;
                double imag;
            } Complex;

            kernel void AddComplex(global Complex* a, global const Complex* b)
            {
                int i = get_global_id(0);
                a[i].real += b[i].real;
                a[i].imag += b[i].imag;
            }

            kernel void AddSComplex(global Complex* a, Complex b)
            {
                int i = get_global_id(0);
                a[i].real += b.real;
                a[i].imag += b.imag;
            }

            kernel void AddDouble(global Complex* a, double b)
            {
                int i = get_global_id(0);
                a[i].real += b;
                a[i].imag += b;
            }

            kernel void MulComplex(global Complex* a, global const Complex* b)
            {
                int i = get_global_id(0);
                Complex temp;
                temp.real = a[i].real * b[i].real-a[i].imag * b[i].imag;
                temp.imag = a[i].real * b[i].imag+a[i].imag * b[i].real;
                a[i]=temp;
            }

            kernel void MulSComplex(global Complex* a, Complex b)
            {
                int i = get_global_id(0);
                Complex temp;
                temp.real = a[i].real * b.real-a[i].imag * b.imag;
                temp.imag =a[i].real * b.imag+a[i].imag * b.real;
                a[i]=temp;
            }

            kernel void MulDouble(global Complex* a, double b)
            {
                int i = get_global_id(0);
                a[i].real *= b;
                a[i].imag *= b;
            }

            kernel void SubComplex(global Complex* a, global const Complex* b)
            {
                int i = get_global_id(0);
                a[i].real -= b[i].real;
                a[i].imag -= b[i].imag;
            }

            kernel void SubSComplex(global Complex* a, Complex b)
            {
                int i = get_global_id(0);
                a[i].real -= b.real;
                a[i].imag -= b.imag;
            }

            kernel void SubDouble(global Complex* a, double b)
            {
                int i = get_global_id(0);
                a[i].real -= b;
                a[i].imag -= b;
            }

            kernel void DivDouble(global Complex* a, double b)
            {
                int i = get_global_id(0);
                a[i].real /= b;
                a[i].imag /= b;
            }

            kernel void DivComplex(global Complex* a, Complex b)
            {
                int i = get_global_id(0);
                double d=b.real*b.real+b.imag*b.imag;
                Complex temp;
                temp.real=(a[i].real*b.real+a[i].imag*b.imag)/d;
                temp.imag=(a[i].imag*b.real-a[i].real*b.imag)/d;
                a[i]=temp;
            }

            kernel void NormWF( global double* result,global const Complex* data)
            {
                int i = get_global_id(0);
                result[i] = data[i].real * data[i].real + data[i].imag * data[i].imag;
            }
        ";

        #endregion

        protected static ComputeContext context { get; private set; }
        protected static ComputeCommandQueue queue { get; private set; }

        private static ComputeProgram program { get; set; }

        private static ComputeKernel Addkernel { get; set; }

        private static ComputeKernel AddDoublekernel { get; set; }
        private static ComputeKernel AddComplexkernel { get; set; }

        private static ComputeKernel Subkernel { get; set; }

        private static ComputeKernel SubDoublekernel { get; set; }
        private static ComputeKernel SubComplexkernel { get; set; }
        private static ComputeKernel Mulkernel { get; set; }
        private static ComputeKernel MulDoublekernel { get; set; }
        private static ComputeKernel MulComplexkernel { get; set; }
        private static ComputeKernel DivDoublekernel { get; set; }
        private static ComputeKernel DivComplexkernel { get; set; }
        protected static ComputeKernel Normkernel { get; private set; }

        protected static ComputeBuffer<Complex> aBuffer { get; set; }
        protected static ComputeBuffer<Complex> bBuffer { get; set; }

        protected static ComputeBuffer<double> resultBuffer { get; set; }

        public static void InitOpenCL()
        {
            context ??= new ComputeContext(ComputeDeviceTypes.All, new ComputeContextPropertyList(ComputePlatform.Platforms[0]), null, nint.Zero);
            queue ??= new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);
            if (program == null)
            {
                program = new ComputeProgram(context, kernelSource);
                program.Build(null, null, null, nint.Zero);
            }
            Addkernel = program.CreateKernel("AddComplex");
            AddDoublekernel = program.CreateKernel("AddDouble");
            Subkernel = program.CreateKernel("SubComplex");
            SubDoublekernel = program.CreateKernel("SubDouble");
            Mulkernel = program.CreateKernel("MulComplex");
            MulDoublekernel = program.CreateKernel("MulDouble");
            AddComplexkernel = program.CreateKernel("AddSComplex");
            SubComplexkernel = program.CreateKernel("SubSComplex");
            MulComplexkernel = program.CreateKernel("MulSComplex");
            DivDoublekernel = program.CreateKernel("DivDouble");
            DivComplexkernel = program.CreateKernel("DivComplex");
            Normkernel = program.CreateKernel("NormWF");
        }

        #endregion

        #region Operatoren WF-Complex

        public static IWavefunction operator +(Complex b, IWavefunction a)
        {
            switch (a.WFInfo.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        AddComplexkernel.SetMemoryArgument(0, aBuffer);
                        AddComplexkernel.SetValueArgument(1, b);

                        queue.Execute(AddComplexkernel, null, [a.field.Length], null, null);
                        Complex[] cf = a.field;
                        queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                        a.SetField(cf);
                        break;
                    }

                case ECalculationMethod.CPU_Multihreading:
                    {
                        Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                        {
                            for (int i = range.Item1; i < range.Item2; i++)
                                a.SetField(i, a[i] + b);
                        });
                        break;
                    }

                default:
                    {
                        for (int i = 0; i < a.field.Length; i++)
                            a.SetField(i, a[i] + b);
                        break;
                    }
            }
            return a;
        }

        public static IWavefunction operator -(IWavefunction a, Complex b)
        {
            switch (a.WFInfo.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        SubComplexkernel.SetMemoryArgument(0, aBuffer);
                        SubComplexkernel.SetValueArgument(1, b);

                        queue.Execute(SubComplexkernel, null, [a.field.Length], null, null);
                        Complex[] cf = a.field;
                        queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                        a.SetField(cf);
                        break;
                    }

                case ECalculationMethod.CPU_Multihreading:
                    {
                        Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                        {
                            for (int i = range.Item1; i < range.Item2; i++)
                                a.SetField(i, b - a[i]);
                        });
                        break;
                    }

                default:
                    {
                        for (int i = 0; i < a.field.Length; i++)
                            a.SetField(i, b - a[i]);
                        break;
                    }
            }
            return a;
        }

        public static IWavefunction operator *(Complex b, IWavefunction a)
        {
            switch (a.WFInfo.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        MulComplexkernel.SetMemoryArgument(0, aBuffer);
                        MulComplexkernel.SetValueArgument(1, b);

                        queue.Execute(MulComplexkernel, null, [a.field.Length], null, null);
                        Complex[] cf = a.field;
                        queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                        a.SetField(cf);
                        break;
                    }

                case ECalculationMethod.CPU_Multihreading:
                    {
                        Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                        {
                            for (int i = range.Item1; i < range.Item2; i++)
                                a.SetField(i, a[i] * b);
                        });
                        break;
                    }

                default:
                    {
                        for (int i = 0; i < a.field.Length; i++)
                            a.SetField(i, a[i] * b);
                        break;
                    }
            }
            return a;
        }

        public static IWavefunction operator +(IWavefunction a, Complex b) => b + a;

        public static IWavefunction operator -(Complex b, IWavefunction a) => -1 * a + b;

        public static IWavefunction operator *(IWavefunction a, Complex b) => b * a;

        public static IWavefunction operator /(IWavefunction a, Complex b)
        {
            switch (a.WFInfo.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        DivComplexkernel.SetMemoryArgument(0, aBuffer);
                        DivComplexkernel.SetValueArgument(1, b);

                        queue.Execute(DivComplexkernel, null, [a.field.Length], null, null);
                        Complex[] cf = a.field;
                        queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                        a.SetField(cf);
                        break;
                    }

                case ECalculationMethod.CPU_Multihreading:
                    {
                        Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                        {
                            for (int i = range.Item1; i < range.Item2; i++)
                                a.SetField(i, a[i] / b);
                        });
                        break;
                    }

                default:
                    {
                        for (int i = 0; i < a.field.Length; i++)
                            a.SetField(i, a[i] / b);
                        break;
                    }
            }
            return a;
        }

        #endregion Operatoren WF-Complex

        #region Operatoren WF-double

        public static IWavefunction operator +(double b, IWavefunction a)
        {
            switch (a.WFInfo.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        AddDoublekernel.SetMemoryArgument(0, aBuffer);
                        AddDoublekernel.SetValueArgument(1, b);

                        queue.Execute(AddDoublekernel, null, [a.field.Length], null, null);
                        Complex[] cf = a.field;
                        queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                        a.SetField(cf);
                        break;
                    }

                case ECalculationMethod.CPU_Multihreading:
                    {
                        Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                        {
                            for (int i = range.Item1; i < range.Item2; i++)
                                a.SetField(i, a[i] + b);
                        });
                        break;
                    }
                default:
                    {
                        for (int i = 0; i < a.field.Length; i++)
                            a.SetField(i, a[i] + b);
                        break;
                    }
            }
            return a;
        }

        public static IWavefunction operator -(IWavefunction a, double b)
        {
            switch (a.WFInfo.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        SubDoublekernel.SetMemoryArgument(0, aBuffer);
                        SubDoublekernel.SetValueArgument(1, b);

                        queue.Execute(SubDoublekernel, null, [a.field.Length], null, null);
                        Complex[] cf = a.field;
                        queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                        a.SetField(cf);
                        break;
                    }

                case ECalculationMethod.CPU_Multihreading:
                    {
                        Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                        {
                            for (int i = range.Item1; i < range.Item2; i++)
                                a.SetField(i, a[i] - b);
                        });
                        break;
                    }

                default:
                    {
                        for (int i = 0; i < a.field.Length; i++)
                            a.SetField(i, a[i] - b);
                        break;
                    }
            }
            return a;
        }

        public static IWavefunction operator *(double b, IWavefunction a)
        {
            switch (a.WFInfo.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        MulDoublekernel.SetMemoryArgument(0, aBuffer);
                        MulDoublekernel.SetValueArgument(1, b);

                        queue.Execute(MulDoublekernel, null, [a.field.Length], null, null);
                        Complex[] cf = a.field;
                        queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                        a.SetField(cf);
                        break;
                    }

                case ECalculationMethod.CPU_Multihreading:
                    {
                        Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                        {
                            for (int i = range.Item1; i < range.Item2; i++)
                                a.SetField(i, a[i] * b);
                        });
                        break;
                    }

                default:
                    {
                        for (int i = 0; i < a.field.Length; i++)
                            a.SetField(i, a[i] * b);
                        break;
                    }
            }
            return a;
        }

        public static IWavefunction operator +(IWavefunction a, double b) => b + a;

        public static IWavefunction operator -(double b, IWavefunction a) => -1 * a + b;

        public static IWavefunction operator *(IWavefunction a, double b) => b * a;

        public static IWavefunction operator /(IWavefunction a, double b)
        {
            switch (a.WFInfo.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        DivDoublekernel.SetMemoryArgument(0, aBuffer);
                        DivDoublekernel.SetValueArgument(1, b);

                        queue.Execute(DivDoublekernel, null, [a.field.Length], null, null);
                        Complex[] cf = a.field;
                        queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                        a.SetField(cf);
                        break;
                    }

                case ECalculationMethod.CPU_Multihreading:
                    {
                        Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                        {
                            for (int i = range.Item1; i < range.Item2; i++)
                                a.SetField(i, a[i] / b);
                        });
                        break;
                    }

                default:
                    {
                        for (int i = 0; i < a.field.Length; i++)
                            a.SetField(i, a[i] / b);
                        break;
                    }
            }
            return a;
        }

        #endregion Operatoren WF-double

        #region Operatoren WF-WF

        public static IWavefunction operator +(IWavefunction a, IWavefunction b)
        {
            if (!(a.WFInfo.DimInfo.Dimensions == b.WFInfo.DimInfo.Dimensions))
                throw new Exception("Error with Dimensions.");
            switch (a.WFInfo.CalcMethod)
            {
                case ECalculationMethod.OpenCL when b.WFInfo.CalcMethod == ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        if (bBuffer == null)
                            bBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, b.field);
                        else
                            queue.WriteToBuffer(b.field, bBuffer, true, null);

                        Addkernel.SetMemoryArgument(0, aBuffer);
                        Addkernel.SetMemoryArgument(1, bBuffer);

                        queue.Execute(Addkernel, null, [a.field.Length], null, null);
                        Complex[] cf = a.field;
                        queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                        a.SetField(cf);
                        break;
                    }

                case ECalculationMethod.CPU_Multihreading:
                    {
                        Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                        {
                            for (int i = range.Item1; i < range.Item2; i++)
                                a.SetField(i, a[i] + b[i]);
                        });
                        break;
                    }

                default:
                    {
                        for (int i = 0; i < a.field.Length; i++)
                            a.SetField(i, a[i] + b[i]);
                        break;
                    }
            }
            return a;
        }

        public static IWavefunction operator -(IWavefunction a, IWavefunction b)
        {
            if (!(a.WFInfo.DimInfo.Dimensions == b.WFInfo.DimInfo.Dimensions))
                throw new Exception("Error with Dimensions.");
            switch (a.WFInfo.CalcMethod)
            {
                case ECalculationMethod.OpenCL when b.WFInfo.CalcMethod == ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        if (bBuffer == null)
                            bBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, b.field);
                        else
                            queue.WriteToBuffer(b.field, bBuffer, true, null);
                        Subkernel.SetMemoryArgument(0, aBuffer);
                        Subkernel.SetMemoryArgument(1, bBuffer);

                        queue.Execute(Subkernel, null, [a.field.Length], null, null);
                        Complex[] cf = a.field;
                        queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                        a.SetField(cf);
                        break;
                    }

                default:
                    {
                        Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                        {
                            for (int i = range.Item1; i < range.Item2; i++)
                                a.SetField(i, a[i] - b[i]);
                        });
                        break;
                    }
            }
            return a;
        }

        public static IWavefunction operator *(IWavefunction a, IWavefunction b)
        {
            if (!(a.WFInfo.DimInfo.Dimensions == b.WFInfo.DimInfo.Dimensions))
                throw new Exception("Error with Dimensions.");
            switch (a.WFInfo.CalcMethod)
            {
                case ECalculationMethod.OpenCL when b.WFInfo.CalcMethod == ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        if (bBuffer == null)
                            bBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, b.field);
                        else
                            queue.WriteToBuffer(b.field, bBuffer, true, null);
                        Mulkernel.SetMemoryArgument(0, aBuffer);
                        Mulkernel.SetMemoryArgument(1, bBuffer);

                        queue.Execute(Mulkernel, null, [a.field.Length], null, null);
                        Complex[] cf = a.field;
                        queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                        a.SetField(cf);
                        break;
                    }

                case ECalculationMethod.CPU_Multihreading:
                    {
                        Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                        {
                            for (int i = range.Item1; i < range.Item2; i++)
                                a.SetField(i, a[i] * b[i]);
                        });
                        break;
                    }

                default:
                    {
                        for (int i = 0; i < a.field.Length; i++)
                            a.SetField(i, a[i] * b[i]);
                        break;
                    }
            }
            return a;
        }

        #endregion Operatoren WF-WF
    }
}