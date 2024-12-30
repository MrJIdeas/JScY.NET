using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.VarTypes;
using ScottPlot;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.TimeEvolution.Classes
{
    public class CabLogger
    {
        private List<CabEntry> entries { get; set; }
        private Plot myPlot { get; set; }

        public CabLogger()
        {
            entries = new List<CabEntry>();
            myPlot = new Plot();
        }

        public List<CabEntry> GetEntries() => entries;

        public void AddCab(double t, IWavefunction wavefunction)
        {
            foreach (var item in wavefunction.CalcCab())
            {
                entries.Add(new CabEntry()
                {
                    ExitName = item.Key,
                    t = t,
                    cab = item.Value
                });
            }
        }

        public List<System.Drawing.Image> GetImage(int width, int height)
        {
            List<System.Drawing.Image> images = new List<System.Drawing.Image>();
            var exits = entries.Select(x => x.ExitName).Distinct();
            foreach (var exit in exits)
            {
                myPlot.Clear();
                List<double> x = new List<double>();
                List<double> y = new List<double>();
                foreach (var item in entries.Where(x => x.ExitName.Equals(exit)).OrderBy(x => x.t))
                {
                    x.Add(item.t);
                    y.Add((item.cab * Complex.Conjugate(item.cab)).Real);
                }
                myPlot.Add.Bars(x, y);
                var img = System.Drawing.Image.FromStream(new MemoryStream(myPlot.GetImage(width, height).GetImageBytes()));
                images.Add(img);
            }
            return images;
        }
    }
}