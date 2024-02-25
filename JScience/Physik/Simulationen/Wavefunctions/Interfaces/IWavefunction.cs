using System.Numerics;
using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Enums;
using JScience.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Threading.Tasks;
using Cloo;

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

        Image GetImage(int width, int height);

        bool UseGPU { get; }

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
                a[i].real *= b[i].real;
                a[i].imag *= b[i].imag;
            }
            kernel void MulSComplex(global Complex* a, Complex b)
            {
                int i = get_global_id(0);
                a[i].real *= b.real;
                a[i].imag *= b.imag;
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
        ";

        private static ComputeContext context { get; set; }
        private static ComputeCommandQueue queue { get; set; }

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

        public static void InitKernel()
        {
            context = new ComputeContext(ComputeDeviceTypes.All, new ComputeContextPropertyList(ComputePlatform.Platforms[0]), null, IntPtr.Zero);
            queue = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);
            program = new ComputeProgram(context, kernelSource);
            program.Build(null, null, null, IntPtr.Zero);

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
        }

        #endregion

        #region Operatoren WF-Complex

        public static IWavefunction operator +(Complex b, IWavefunction a)
        {
            if (a.UseGPU)
            {
                ComputeBuffer<Complex> aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                AddComplexkernel.SetMemoryArgument(0, aBuffer);
                AddComplexkernel.SetValueArgument(1, b);

                queue.Execute(AddComplexkernel, null, new long[] { a.field.Length }, null, null);
                Complex[] cf = a.field;
                queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                a.SetField(cf);
            }
            else
            {
                Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                        a.SetField(i, a[i] + b);
                });
            }
            return a;
        }

        public static IWavefunction operator -(IWavefunction a, Complex b)
        {
            if (a.UseGPU)
            {
                ComputeBuffer<Complex> aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                SubComplexkernel.SetMemoryArgument(0, aBuffer);
                SubComplexkernel.SetValueArgument(1, b);

                queue.Execute(SubComplexkernel, null, new long[] { a.field.Length }, null, null);
                Complex[] cf = a.field;
                queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                a.SetField(cf);
            }
            else
            {
                Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                        a.SetField(i, b - a[i]);
                });
            }
            return a;
        }

        public static IWavefunction operator *(Complex b, IWavefunction a)
        {
            if (a.UseGPU)
            {
                ComputeBuffer<Complex> aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                MulComplexkernel.SetMemoryArgument(0, aBuffer);
                MulComplexkernel.SetValueArgument(1, b);

                queue.Execute(MulComplexkernel, null, new long[] { a.field.Length }, null, null);
                Complex[] cf = a.field;
                queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                a.SetField(cf);
            }
            else
            {
                Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                        a.SetField(i, a[i] * b);
                });
            }
            return a;
        }

        public static IWavefunction operator +(IWavefunction a, Complex b) => b + a;

        public static IWavefunction operator -(Complex b, IWavefunction a) => -1 * a + b;

        public static IWavefunction operator *(IWavefunction a, Complex b) => b * a;

        public static IWavefunction operator /(IWavefunction a, Complex b)
        {
            Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    a.SetField(i, a[i] / b);
            });
            return a;
        }

        #endregion Operatoren WF-Complex

        #region Operatoren WF-double

        public static IWavefunction operator +(double b, IWavefunction a)
        {
            if (a.UseGPU)
            {
                ComputeBuffer<Complex> aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                AddDoublekernel.SetMemoryArgument(0, aBuffer);
                AddDoublekernel.SetValueArgument(1, b);

                queue.Execute(AddDoublekernel, null, new long[] { a.field.Length }, null, null);
                Complex[] cf = a.field;
                queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                a.SetField(cf);
            }
            else
            {
                Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                        a.SetField(i, a[i] + b);
                });
            }
            return a;
        }

        public static IWavefunction operator -(IWavefunction a, double b)
        {
            if (a.UseGPU)
            {
                ComputeBuffer<Complex> aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                SubDoublekernel.SetMemoryArgument(0, aBuffer);
                SubDoublekernel.SetValueArgument(1, b);

                queue.Execute(SubDoublekernel, null, new long[] { a.field.Length }, null, null);
                Complex[] cf = a.field;
                queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                a.SetField(cf);
            }
            else
            {
                Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                        a.SetField(i, a[i] - b);
                });
            }
            return a;
        }

        public static IWavefunction operator *(double b, IWavefunction a)
        {
            if (a.UseGPU)
            {
                ComputeBuffer<Complex> aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                MulDoublekernel.SetMemoryArgument(0, aBuffer);
                MulDoublekernel.SetValueArgument(1, b);

                queue.Execute(MulDoublekernel, null, new long[] { a.field.Length }, null, null);
                Complex[] cf = a.field;
                queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                a.SetField(cf);
            }
            else
            {
                Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                        a.SetField(i, a[i] * b);
                });
            }
            return a;
        }

        public static IWavefunction operator +(IWavefunction a, double b) => b + a;

        public static IWavefunction operator -(double b, IWavefunction a) => -1 * a + b;

        public static IWavefunction operator *(IWavefunction a, double b) => b * a;

        public static IWavefunction operator /(IWavefunction a, double b)
        {
            if (a.UseGPU)
            {
                ComputeBuffer<Complex> aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                DivDoublekernel.SetMemoryArgument(0, aBuffer);
                DivDoublekernel.SetValueArgument(1, b);

                queue.Execute(DivDoublekernel, null, new long[] { a.field.Length }, null, null);
                Complex[] cf = a.field;
                queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                a.SetField(cf);
            }
            else
            {
                Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                        a.SetField(i, a[i] / b);
                });
            }
            return a;
        }

        #endregion Operatoren WF-double

        #region Operatoren WF-WF

        public static IWavefunction operator +(IWavefunction a, IWavefunction b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            if (a.UseGPU && b.UseGPU)
            {
                ComputeBuffer<Complex> aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                ComputeBuffer<Complex> bBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, b.field);

                Addkernel.SetMemoryArgument(0, aBuffer);
                Addkernel.SetMemoryArgument(1, bBuffer);

                queue.Execute(Addkernel, null, new long[] { a.field.Length }, null, null);
                Complex[] cf = a.field;
                queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                a.SetField(cf);
            }
            else
            {
                Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                        a.SetField(i, a[i] + b[i]);
                });
            }
            return a;
        }

        public static IWavefunction operator -(IWavefunction a, IWavefunction b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            if (a.UseGPU && b.UseGPU)
            {
                ComputeBuffer<Complex> aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                ComputeBuffer<Complex> bBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, b.field);

                Subkernel.SetMemoryArgument(0, aBuffer);
                Subkernel.SetMemoryArgument(1, bBuffer);

                queue.Execute(Subkernel, null, new long[] { a.field.Length }, null, null);
                Complex[] cf = a.field;
                queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                a.SetField(cf);
            }
            else
            {
                Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                        a.SetField(i, a[i] - b[i]);
                });
            }
            return a;
        }

        public static IWavefunction operator *(IWavefunction a, IWavefunction b)
        {
            if (!(a.Dimensions == b.Dimensions))
                throw new Exception("Error with Dimensions.");
            if (a.UseGPU && b.UseGPU)
            {
                ComputeBuffer<Complex> aBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, a.field);
                ComputeBuffer<Complex> bBuffer = new ComputeBuffer<Complex>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, b.field);

                Mulkernel.SetMemoryArgument(0, aBuffer);
                Mulkernel.SetMemoryArgument(1, bBuffer);

                queue.Execute(Mulkernel, null, new long[] { a.field.Length }, null, null);
                Complex[] cf = a.field;
                queue.ReadFromBuffer(aBuffer, ref cf, true, null);
                a.SetField(cf);
            }
            else
            {
                Parallel.ForEach(a.rangePartitioner, (range, loopState) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                        a.SetField(i, a[i] * b[i]);
                });
            }
            return a;
        }

        #endregion Operatoren WF-WF
    }
}