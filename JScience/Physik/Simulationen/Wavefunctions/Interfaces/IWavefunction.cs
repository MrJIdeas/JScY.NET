using Cloo;
using JScience.Enums;
using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF;
using ManagedCuda;
using ManagedCuda.BasicTypes;
using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Threading.Tasks;

namespace JScience.Physik.Simulationen.Wavefunctions.Interfaces
{
    public interface IWavefunction
    {
        int Dimensions { get; }
        ELatticeBoundary Boundary { get; }

        WFInfo WFInfo { get; }

        OrderablePartitioner<Tuple<int, int>> rangePartitioner { get; }

        IWavefunction GetShift(EShift shift);

        void Clear();

        System.Drawing.Image GetImage(int width, int height);

        ECalculationMethod CalcMethod { get; }

        #region Feld

        Complex this[int x] { get; }

        Complex[] field { get; }

        void SetField(int x, Complex value);

        void SetField(Complex[] field);

        IWavefunction Conj();

        IWavefunction Clone();

        #endregion Feld

        #region Norm

        double Norm();

        double getNorm(int x);

        #endregion Norm

        #region Kernel CL

        #region Kernel String

        private static string kernelSource = @"
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
            if (context == null)
                context = new ComputeContext(ComputeDeviceTypes.All, new ComputeContextPropertyList(ComputePlatform.Platforms[0]), null, IntPtr.Zero);
            if (queue == null)
                queue = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);
            if (program == null)
            {
                program = new ComputeProgram(context, kernelSource);
                program.Build(null, null, null, IntPtr.Zero);
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

        #region Kernel CUDA

        #region Kernel String

        private static string Kernel_CUDA = @"
        typedef struct {
            double real;
            double imag;
        } Complex;

        __global__ void AddComplex(Complex* a, const Complex* b, int n)
        {
            int i = threadIdx.x + blockIdx.x * blockDim.x;
            if (i < n) {
                a[i].real += b[i].real;
                a[i].imag += b[i].imag;
            }
        }

        __global__ void AddSComplex(Complex* a, Complex b, int n)
        {
            int i = threadIdx.x + blockIdx.x * blockDim.x;
            if (i < n) {
                a[i].real += b.real;
                a[i].imag += b.imag;
            }
        }

        __global__ void AddDouble(Complex* a, double b, int n)
        {
            int i = threadIdx.x + blockIdx.x * blockDim.x;
            if (i < n) {
                a[i].real += b;
                a[i].imag += b;
            }
        }

        __global__ void MulComplex(Complex* a, const Complex* b, int n)
        {
            int i = threadIdx.x + blockIdx.x * blockDim.x;
            if (i < n) {
                Complex temp;
                temp.real = a[i].real * b[i].real - a[i].imag * b[i].imag;
                temp.imag = a[i].real * b[i].imag + a[i].imag * b[i].real;
                a[i] = temp;
            }
        }

        __global__ void MulSComplex(Complex* a, Complex b, int n)
        {
            int i = threadIdx.x + blockIdx.x * blockDim.x;
            if (i < n) {
                Complex temp;
                temp.real = a[i].real * b.real - a[i].imag * b.imag;
                temp.imag = a[i].real * b.imag + a[i].imag * b.real;
                a[i] = temp;
            }
        }

        __global__ void MulDouble(Complex* a, double b, int n)
        {
            int i = threadIdx.x + blockIdx.x * blockDim.x;
            if (i < n) {
                a[i].real *= b;
                a[i].imag *= b;
            }
        }

        __global__ void SubComplex(Complex* a, const Complex* b, int n)
        {
            int i = threadIdx.x + blockIdx.x * blockDim.x;
            if (i < n) {
                a[i].real -= b[i].real;
                a[i].imag -= b[i].imag;
            }
        }

        __global__ void SubSComplex(Complex* a, Complex b, int n)
        {
            int i = threadIdx.x + blockIdx.x * blockDim.x;
            if (i < n) {
                a[i].real -= b.real;
                a[i].imag -= b.imag;
            }
        }

        __global__ void SubDouble(Complex* a, double b, int n)
        {
            int i = threadIdx.x + blockIdx.x * blockDim.x;
            if (i < n) {
                a[i].real -= b;
                a[i].imag -= b;
            }
        }

        __global__ void DivDouble(Complex* a, double b, int n)
        {
            int i = threadIdx.x + blockIdx.x * blockDim.x;
            if (i < n) {
                a[i].real /= b;
                a[i].imag /= b;
            }
        }

        __global__ void DivComplex(Complex* a, Complex b, int n)
        {
            int i = threadIdx.x + blockIdx.x * blockDim.x;
            if (i < n) {
                double d = b.real * b.real + b.imag * b.imag;
                Complex temp;
                temp.real = (a[i].real * b.real + a[i].imag * b.imag) / d;
                temp.imag = (a[i].imag * b.real - a[i].real * b.imag) / d;
                a[i] = temp;
            }
        }

        __global__ void NormWF(double* result, const Complex* data, int n)
        {
            int i = threadIdx.x + blockIdx.x * blockDim.x;
            if (i < n) {
                result[i] = data[i].real * data[i].real + data[i].imag * data[i].imag;
            }
        }
";

        #endregion

        public static bool CUDA_Existing { get; private set; }
        protected static CudaContext context_CUDA { get; private set; }
        protected static CudaStream stream_CUDA { get; private set; }

        private static CudaKernel Addkernel_CUDA { get; set; }

        private static CudaKernel AddDoublekernel_CUDA { get; set; }
        private static CudaKernel AddComplexkernel_CUDA { get; set; }

        private static CudaKernel Subkernel_CUDA { get; set; }

        private static CudaKernel SubDoublekernel_CUDA { get; set; }
        private static CudaKernel SubComplexkernel_CUDA { get; set; }
        private static CudaKernel Mulkernel_CUDA { get; set; }
        private static CudaKernel MulDoublekernel_CUDA { get; set; }
        private static CudaKernel MulComplexkernel_CUDA { get; set; }
        private static CudaKernel DivDoublekernel_CUDA { get; set; }
        private static CudaKernel DivComplexkernel_CUDA { get; set; }
        protected static CudaKernel Normkernel_CUDA { get; private set; }

        private static CudaDeviceVariable<Complex> aBuffer_CUDA { get; set; }
        private static CudaDeviceVariable<Complex> bBuffer_CUDA { get; set; }
        private static CudaDeviceVariable<double> resultbuffer_CUDA { get; set; }

        public static void InitCUDA()
        {
            try
            {
                context_CUDA = new CudaContext(0, false);
                CUDA_Existing = true;
                stream_CUDA = new CudaStream();
                Addkernel_CUDA = context_CUDA.LoadKernelPTX(Kernel_CUDA, "AddComplex");
                AddDoublekernel_CUDA = context_CUDA.LoadKernelPTX(Kernel_CUDA, "AddDouble");
                Subkernel_CUDA = context_CUDA.LoadKernelPTX(Kernel_CUDA, "SubComplex");
                SubDoublekernel_CUDA = context_CUDA.LoadKernelPTX(Kernel_CUDA, "SubDouble");
                Mulkernel_CUDA = context_CUDA.LoadKernelPTX(Kernel_CUDA, "MulComplex");
                MulDoublekernel_CUDA = context_CUDA.LoadKernelPTX(Kernel_CUDA, "MulDouble");
                AddComplexkernel_CUDA = context_CUDA.LoadKernelPTX(Kernel_CUDA, "AddSComplex");
                SubComplexkernel_CUDA = context_CUDA.LoadKernelPTX(Kernel_CUDA, "SubSComplex");
                MulComplexkernel_CUDA = context_CUDA.LoadKernelPTX(Kernel_CUDA, "MulSComplex");
                DivDoublekernel_CUDA = context_CUDA.LoadKernelPTX(Kernel_CUDA, "DivDouble");
                DivComplexkernel_CUDA = context_CUDA.LoadKernelPTX(Kernel_CUDA, "DivComplex");
                Normkernel_CUDA = context_CUDA.LoadKernelPTX(Kernel_CUDA, "NormWF");
            }
            catch (DllNotFoundException)
            {
                CUDA_Existing = false;
            }
        }

        #endregion

        #region Operatoren WF-Complex

        public static IWavefunction operator +(Complex b, IWavefunction a)
        {
            switch (a.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        AddComplexkernel.SetMemoryArgument(0, aBuffer);
                        AddComplexkernel.SetValueArgument(1, b);

                        queue.Execute(AddComplexkernel, null, new long[] { a.field.Length }, null, null);
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
            switch (a.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        SubComplexkernel.SetMemoryArgument(0, aBuffer);
                        SubComplexkernel.SetValueArgument(1, b);

                        queue.Execute(SubComplexkernel, null, new long[] { a.field.Length }, null, null);
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
            switch (a.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        MulComplexkernel.SetMemoryArgument(0, aBuffer);
                        MulComplexkernel.SetValueArgument(1, b);

                        queue.Execute(MulComplexkernel, null, new long[] { a.field.Length }, null, null);
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
            switch (a.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        DivComplexkernel.SetMemoryArgument(0, aBuffer);
                        DivComplexkernel.SetValueArgument(1, b);

                        queue.Execute(DivComplexkernel, null, new long[] { a.field.Length }, null, null);
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
            switch (a.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        AddDoublekernel.SetMemoryArgument(0, aBuffer);
                        AddDoublekernel.SetValueArgument(1, b);

                        queue.Execute(AddDoublekernel, null, new long[] { a.field.Length }, null, null);
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
            switch (a.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        SubDoublekernel.SetMemoryArgument(0, aBuffer);
                        SubDoublekernel.SetValueArgument(1, b);

                        queue.Execute(SubDoublekernel, null, new long[] { a.field.Length }, null, null);
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
            switch (a.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        MulDoublekernel.SetMemoryArgument(0, aBuffer);
                        MulDoublekernel.SetValueArgument(1, b);

                        queue.Execute(MulDoublekernel, null, new long[] { a.field.Length }, null, null);
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
            switch (a.CalcMethod)
            {
                case ECalculationMethod.OpenCL:
                    {
                        if (aBuffer == null)
                            aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                        else
                            queue.WriteToBuffer(a.field, aBuffer, true, null);
                        DivDoublekernel.SetMemoryArgument(0, aBuffer);
                        DivDoublekernel.SetValueArgument(1, b);

                        queue.Execute(DivDoublekernel, null, new long[] { a.field.Length }, null, null);
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
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            switch (a.CalcMethod)
            {
                case ECalculationMethod.OpenCL when b.CalcMethod == ECalculationMethod.OpenCL:
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

                        queue.Execute(Addkernel, null, new long[] { a.field.Length }, null, null);
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
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            switch (a.CalcMethod)
            {
                case ECalculationMethod.OpenCL when b.CalcMethod == ECalculationMethod.OpenCL:
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

                        queue.Execute(Subkernel, null, new long[] { a.field.Length }, null, null);
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
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            switch (a.CalcMethod)
            {
                case ECalculationMethod.OpenCL when b.CalcMethod == ECalculationMethod.OpenCL:
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

                        queue.Execute(Mulkernel, null, new long[] { a.field.Length }, null, null);
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