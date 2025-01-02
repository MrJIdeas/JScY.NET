using System.Collections.Generic;
using System.IO;
using System.Linq;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;
using ScottPlot;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Classes
{
    public class Normlogger : ILogger<NormEntry>
    {
        private readonly List<NormEntry> entries = [];
        private readonly Plot myPlot = new();

        public void Add(double t, Orbital value) => entries.Add(new NormEntry { t = t, Norm = value.WF.Norm(), WFName = value.Bezeichnung });

        public List<NormEntry> GetEntries() => entries;

        public List<System.Drawing.Image> GetImage(int width, int height)
        {
            List<System.Drawing.Image> images = [];
            var exits = entries.Select(x => x.WFName).Distinct();
            foreach (var exit in exits)
            {
                myPlot.Clear();
                List<double> x = [];
                List<double> y = [];
                foreach (var item in entries.Where(x => x.WFName.Equals(exit)).OrderBy(x => x.t))
                {
                    x.Add(item.t);
                    y.Add(item.Norm);
                }
                myPlot.Add.ScatterLine(x, y);
                myPlot.Axes.SetLimits(x.Min(), x.Max(), y.Min(), y.Max());

                static string CustomFormatter(double position) => position.ToString("E16");

                ScottPlot.TickGenerators.NumericAutomatic myTickGenerator = new()
                {
                    LabelFormatter = CustomFormatter
                };

                // tell an axis to use the custom tick generator
                myPlot.Axes.Left.TickGenerator = myTickGenerator;
                var img = System.Drawing.Image.FromStream(new MemoryStream(myPlot.GetImage(width, height).GetImageBytes()));
                images.Add(img);
            }
            entries.Clear();
            return images;
        }
    }
}