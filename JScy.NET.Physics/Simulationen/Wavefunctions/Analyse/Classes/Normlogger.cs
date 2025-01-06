using System.Collections.Generic;
using System.IO;
using System.Linq;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;
using ScottPlot;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Classes
{
    public class Normlogger : ITimeLog, IOrbitalWatcher<NormEntry>, I_ImageGenerator
    {
        private readonly Plot myPlot = new();

        private Dictionary<Orbital, List<NormEntry>> orbitals = [];

        public void Add(double t)
        {
            foreach (var orb in orbitals.Keys)
                orbitals[orb].Add(new NormEntry { t = t, Norm = orb.WF.Norm(), WFName = orb.Bezeichnung });
        }

        public List<NormEntry> GetEntries(Orbital orb) => orbitals[orb];

        public List<System.Drawing.Image> GetImage(int width, int height)
        {
            List<System.Drawing.Image> images = [];
            foreach (var orb in orbitals.Keys)
            {
                var exits = orbitals[orb].Select(x => x.WFName).Distinct();
                foreach (var exit in exits)
                {
                    myPlot.Clear();
                    myPlot.XLabel("Simulated time in steps");
                    myPlot.YLabel("Norm");
                    myPlot.Axes.Title.Label.Text = "Norm Analysis: " + exit;
                    List<double> x = [];
                    List<double> y = [];
                    foreach (var item in orbitals[orb].Where(x => x.WFName.Equals(exit)).OrderBy(x => x.t))
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
                orbitals[orb].Clear();
            }
            return images;
        }

        public void WatchOrbital(Orbital orb)
        {
            if (!orbitals.ContainsKey(orb))
                orbitals.Add(orb, []);
        }
    }
}