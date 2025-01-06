using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;
using ScottPlot;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Classes
{
    public class CabLogger : I_ImageGenerator
    {
        private List<CabEntry> entries { get; set; }
        private Plot myPlot { get; set; }

        public CabLogger()
        {
            entries = [];
            myPlot = new Plot();
        }

        public List<CabEntry> GetEntries() => entries;

        public void Add(double t, Orbital value)
        {
            foreach (var item in value.CalcCab())
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
            List<System.Drawing.Image> images = [];
            var exits = entries.Select(x => x.ExitName).Distinct();
            foreach (var exit in exits)
            {
                myPlot.Clear();
                List<double> x = [];
                List<double> y = [];
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