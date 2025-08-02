using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;
using ScottPlot;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Classes
{
    public class CabLogger : IOrbitalWatcher<CabEntry>, ITimeLog, I_ImageGenerator
    {
        private Plot myPlot = new();

        private readonly Dictionary<Orbital, List<CabEntry>> orbitals = [];

        public List<CabEntry> GetEntries(Orbital orb) => orbitals[orb];

        public void Add(double t)
        {
            foreach (var orb in orbitals.Keys)
                foreach (var item in orb.CalcCab())
                {
                    orbitals[orb].Add(new CabEntry()
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
            foreach (var orb in orbitals.Keys)
            {
                var exits = orbitals[orb].Select(x => x.ExitName).Distinct();
                foreach (var exit in exits)
                {
                    myPlot.Clear();
                    myPlot.XLabel("Simulated time in steps");
                    myPlot.YLabel("Cab Value");
                    myPlot.Axes.Title.Label.Text = "Cab Analysis: " + exit;
                    List<double> x = [];
                    List<double> y = [];
                    foreach (var item in orbitals[orb].Where(x => x.ExitName.Equals(exit)).OrderBy(x => x.t))
                    {
                        x.Add(item.t);
                        y.Add((item.cab * Complex.Conjugate(item.cab)).Real);
                    }
                    myPlot.Add.Bars(x, y);
                    var img = System.Drawing.Image.FromStream(new MemoryStream(myPlot.GetImage(width, height).GetImageBytes()));
                    images.Add(img);
                }
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