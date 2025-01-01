using System.Collections.Generic;
using System.IO;
using System.Linq;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;
using ScottPlot;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Classes
{
    public class WFPlotter(Orbital orbital)
    {
        private readonly Orbital orbital = orbital;

        public System.Drawing.Image GetImage(int width, int height)
        {
            Plot myPlot_wf = new();
            if (orbital.WF.WFInfo.DimInfo.Dimensions == 1 && orbital.WF is IWF_1D d)
            {
                var DimX = d.WFInfo.DimInfo.DimX;
                List<double> x = [];
                for (int i = 0; i < DimX; i++)
                    x.Add(i);
                List<double> y = [];
                for (int i = 0; i < DimX; i++)
                    y.Add((double)orbital.WF.getNorm(i));
                myPlot_wf.Add.Bars(x, y);
            }
            else if (orbital.WF.WFInfo.DimInfo.Dimensions == 2 && orbital.WF is IWF_2D d1)
            {
                var formatted = d1;
                double[,] data = new double[formatted.WFInfo.DimInfo.DimX, formatted.WFInfo.DimInfo.DimY];
                for (int i = 0; i < formatted.WFInfo.DimInfo.DimX; i++)
                    for (int j = 0; j < formatted.WFInfo.DimInfo.DimY; j++)
                        data[i, j] = formatted.getNorm(i, j);
                var hm1 = myPlot_wf.Add.Heatmap(data);
                hm1.Colormap = new ScottPlot.Colormaps.Turbo();
                myPlot_wf.Add.ColorBar(hm1);
            }
            else
                return null;
            var img = System.Drawing.Image.FromStream(new MemoryStream(myPlot_wf.GetImage(width, height).GetImageBytes()));
            return img;
        }

        public System.Drawing.Image GetCabExitImage(int width, int height)
        {
            Plot myPlot_cab = new();
            if (orbital.WF.WFInfo.DimInfo.Dimensions == 1 && orbital.WF is IWF_1D d)
            {
                var DimX = d.WFInfo.DimInfo.DimX;
                IWavefunction super = orbital.CabExits.FirstOrDefault()?.wavefunction;
                if (super == null)
                    return null;
                for (int i = 1; i < orbital.CabExits.Count; i++)
                    super += orbital.CabExits.ElementAt(i).wavefunction;
                List<double> x = [];
                for (int i = 0; i < DimX; i++)
                    x.Add(i);
                List<double> y = [];
                for (int i = 0; i < DimX; i++)
                    y.Add((double)super.getNorm(i));
                myPlot_cab.Add.Bars(x, y);
            }
            else if (orbital.WF.WFInfo.DimInfo.Dimensions == 2 && orbital.WF is IWF_2D d1)
            {
                var formatted = d1;
                IWavefunction super = orbital.CabExits.FirstOrDefault()?.wavefunction;
                if (super == null)
                    return null;
                for (int i = 1; i < orbital.CabExits.Count; i++)
                    super += (orbital.CabExits.ElementAt(i).wavefunction);
                double[,] data = new double[formatted.WFInfo.DimInfo.DimX, formatted.WFInfo.DimInfo.DimY];
                for (int i = 0; i < formatted.WFInfo.DimInfo.DimX; i++)
                    for (int j = 0; j < formatted.WFInfo.DimInfo.DimY; j++)
                        data[i, j] = ((IWF_2D)super).getNorm(i, j);
                var hm1 = myPlot_cab.Add.Heatmap(data);
                hm1.Colormap = new ScottPlot.Colormaps.Turbo();
                myPlot_cab.Add.ColorBar(hm1);
            }
            else
                return null;
            var img = System.Drawing.Image.FromStream(new MemoryStream(myPlot_cab.GetImage(width, height).GetImageBytes()));
            return img;
        }
    }
}