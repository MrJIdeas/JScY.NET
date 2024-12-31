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

        private readonly Plot myPlot = new Plot();

        public System.Drawing.Image GetImage(int width, int height)
        {
            myPlot.Clear();
            if (orbital.WF.Dimensions == 1 && orbital.WF is IWF_1D)
            {
                var DimX = ((IWF_1D)orbital.WF).DimX;
                List<double> x = new List<double>();
                for (int i = 0; i < DimX; i++)
                    x.Add(i);
                List<double> y = new List<double>();
                for (int i = 0; i < DimX; i++)
                    y.Add((double)orbital.WF.getNorm(i));
                myPlot.Add.Bars(x, y);
            }
            else if (orbital.WF.Dimensions == 2 && orbital.WF is IWF_2D)
            {
                var formatted = (IWF_2D)orbital.WF;
                double[,] data = new double[formatted.DimX, formatted.DimY];
                for (int i = 0; i < formatted.DimX; i++)
                    for (int j = 0; j < formatted.DimY; j++)
                        data[i, j] = formatted.getNorm(i, j);
                var hm1 = myPlot.Add.Heatmap(data);
                hm1.Colormap = new ScottPlot.Colormaps.Turbo();
                myPlot.Add.ColorBar(hm1);
            }
            else
                return null;
            var img = System.Drawing.Image.FromStream(new MemoryStream(myPlot.GetImage(width, height).GetImageBytes()));
            return img;
        }

        public System.Drawing.Image GetCabExitImage(int width, int height)
        {
            myPlot.Clear();
            if (orbital.WF.Dimensions == 1 && orbital.WF is IWF_1D)
            {
                var DimX = ((IWF_1D)orbital.WF).DimX;
                IWavefunction super = orbital.CabExits.FirstOrDefault()?.wavefunction;
                if (super == null)
                    return null;
                for (int i = 1; i < orbital.CabExits.Count; i++)
                    super += orbital.CabExits.ElementAt(i).wavefunction;
                List<double> x = new List<double>();
                for (int i = 0; i < DimX; i++)
                    x.Add(i);
                List<double> y = new List<double>();
                for (int i = 0; i < DimX; i++)
                    y.Add((double)super.getNorm(i));
                myPlot.Add.Bars(x, y);
            }
            else if (orbital.WF.Dimensions == 2 && orbital.WF is IWF_2D)
            {
                var formatted = (IWF_2D)orbital.WF;
                IWavefunction super = orbital.CabExits.FirstOrDefault()?.wavefunction;
                if (super == null)
                    return null;
                for (int i = 1; i < orbital.CabExits.Count; i++)
                    super += (orbital.CabExits.ElementAt(i).wavefunction);
                double[,] data = new double[formatted.DimX, formatted.DimY];
                for (int i = 0; i < formatted.DimX; i++)
                    for (int j = 0; j < formatted.DimY; j++)
                        data[i, j] = ((IWF_2D)super).getNorm(i, j);
                var hm1 = myPlot.Add.Heatmap(data);
                hm1.Colormap = new ScottPlot.Colormaps.Turbo();
                myPlot.Add.ColorBar(hm1);
            }
            else
                return null;
            var img = System.Drawing.Image.FromStream(new MemoryStream(myPlot.GetImage(width, height).GetImageBytes()));
            return img;
        }
    }
}