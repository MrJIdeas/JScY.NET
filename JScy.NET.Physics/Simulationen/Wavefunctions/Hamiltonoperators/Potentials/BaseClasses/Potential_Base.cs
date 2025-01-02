﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JScy.NET.Enums;
using JScy.NET.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.BaseClasses;
using JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using ScottPlot;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.BaseClasses
{
    public abstract class Potential_Base(string name, double Vmax, int xSTART, int xEND, int ySTART, int yEND, int zSTART, int zEND) : Hamilton_Base, IPotential, IPlotter
    {
        public string Name { get; private set; } = name;

        public double Potential { get; private set; } = Vmax;

        public Tuple<int, int> limit_x { get; private set; } = new Tuple<int, int>(xSTART, xEND);
        public Tuple<int, int> limit_y { get; private set; } = new Tuple<int, int>(ySTART, yEND);
        public Tuple<int, int> limit_z { get; private set; } = new Tuple<int, int>(zSTART, zEND);
        protected IWavefunction psiV { get; set; }

        private readonly Plot myPlot_wf = new();

        protected virtual IWavefunction getPsiV(ref IWavefunction psi)
        {
            if (psiV == null)
            {
                int dimYZ = psi.WFInfo.DimInfo.DimY * psi.WFInfo.DimInfo.DimZ;
                psiV = (IWavefunction)Activator.CreateInstance(psi.GetType(), psi.WFInfo);

                switch (psi.WFInfo.CalcMethod)
                {
                    case ECalculationMethod.CPU:
                        int idx, i, j, k;
                        for (i = limit_x.Item1; i < limit_x.Item2; i++)
                            for (j = limit_y.Item1; j < limit_y.Item2; j++)
                                for (k = limit_z.Item1; k < limit_z.Item2; k++)
                                {
                                    idx = dimYZ * k + psi.WFInfo.DimInfo.DimX * j + i;
                                    psiV.field[idx] = Potential;
                                }

                        break;

                    case ECalculationMethod.CPU_Multihreading:
                    case ECalculationMethod.OpenCL:
                        var psi2 = psi;
                        Parallel.For(limit_x.Item1, limit_x.Item2, i =>
                        {
                            int x = i % psi2.WFInfo.DimInfo.DimX;
                            int y = (i / psi2.WFInfo.DimInfo.DimX) % psi2.WFInfo.DimInfo.DimY;
                            int z = i / (psi2.WFInfo.DimInfo.DimX * psi2.WFInfo.DimInfo.DimY);

                            if (x >= limit_x.Item1 && x < limit_x.Item2 &&
                                y >= limit_y.Item1 && y < limit_y.Item2 &&
                                z >= limit_z.Item1 && z < limit_z.Item2)
                            {
                                psiV.field[i] = Potential;
                            }
                        });
                        break;
                }
            }
            return psiV;
        }

        public override IWavefunction HPsi(ref IWavefunction psi) => psi * getPsiV(ref psi);

        public System.Drawing.Image GetImage(int width, int height)
        {
            if (psiV == null) return null;
            myPlot_wf.Clear();
            if (psiV.WFInfo.DimInfo.Dimensions == 1 && psiV is IWF_1D d)
            {
                var DimX = d.WFInfo.DimInfo.DimX;
                List<double> x = [];
                for (int i = 0; i < DimX; i++)
                    x.Add(i);
                List<double> y = [];
                for (int i = 0; i < DimX; i++)
                    y.Add(psiV.field[i].Magnitude);
                myPlot_wf.Add.Bars(x, y);
            }
            else if (psiV.WFInfo.DimInfo.Dimensions == 2 && psiV is IWF_2D d1)
            {
                double[,] data = new double[d1.WFInfo.DimInfo.DimX, d1.WFInfo.DimInfo.DimY];
                for (int i = 0; i < d1.WFInfo.DimInfo.DimX; i++)
                    for (int j = 0; j < d1.WFInfo.DimInfo.DimY; j++)
                        data[i, j] = d1[i, j].Magnitude;
                var hm1 = myPlot_wf.Add.Heatmap(data);
                hm1.Colormap = new ScottPlot.Colormaps.Turbo();
                myPlot_wf.Add.ColorBar(hm1);
            }
            else
                return null;
            var img = System.Drawing.Image.FromStream(new MemoryStream(myPlot_wf.GetImage(width, height).GetImageBytes()));
            return img;
        }
    }
}