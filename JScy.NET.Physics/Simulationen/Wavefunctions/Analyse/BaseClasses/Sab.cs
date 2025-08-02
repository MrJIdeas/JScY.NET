using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;
using ScottPlot;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.BaseClasses
{
    public abstract class Sab : IOrbitalWatcher<SabEntry>, I_ImageGenerator
    {
        private readonly Plot myPlot = new();

        private readonly Dictionary<Orbital, List<SabEntry>> orbitals = [];

        public List<System.Drawing.Image> GetImage(int width, int height)
        {
            List<System.Drawing.Image> images = [];
            foreach (var orb in orbitals.Keys)
            {
                var exits = orbitals[orb].Select(x => x.ExitName).Distinct();
                foreach (var exit in exits)
                {
                    myPlot.Clear();
                    myPlot.XLabel("Energy Values");
                    myPlot.YLabel("Sab Value");
                    myPlot.Axes.Title.Label.Text = "Sab Analysis: " + exit;
                    List<double> x = [];
                    List<double> y = [];
                    foreach (var item in orbitals[orb].Where(x => x.ExitName.Equals(exit)).OrderBy(x => x.v))
                    {
                        x.Add(item.v);
                        y.Add((item.sab * Complex.Conjugate(item.sab)).Real);
                    }
                    myPlot.Add.Scatter(x, y);
                    var img = System.Drawing.Image.FromStream(new MemoryStream(myPlot.GetImage(width, height).GetImageBytes()));
                    images.Add(img);
                }
            }
            return images;
        }

        public List<SabEntry> CalcSab(CabLogger cabLogger, double vMin, double vMax)
        {
            if (cabLogger == null)
                return null;
            List<SabEntry> erg2 = [];
            foreach (var orb in orbitals.Keys)
            {
                List<SabEntry> erg = [];
                List<CabEntry> cabs = cabLogger.GetEntries(orb);
                var exits = cabs.Select(x => x.ExitName).Distinct().ToList();
                foreach (var (e, cablist, dv, dt) in from e in exits
                                                     let cablist = cabs.Where(x => x.ExitName.Equals(e)).OrderBy(x => x.t).ToList()
                                                     let dv = (vMax - vMin) / cablist.Count
                                                     let dt = (cablist.Last().t - cablist.First().t) / cablist.Count
                                                     select (e, cablist, dv, dt))
                {
                    for (int i = 0; i < cablist.Count; i++)
                    {
                        SabEntry sab = new()
                        {
                            v = vMin + dv * i,
                            ExitName = e
                        };
                        for (int j = 0; j < cablist.Count; j++)
                        {
                            sab.sab += cablist[j].cab * dt * Complex.Exp(Complex.ImaginaryOne * sab.v * cablist[j].t);
                        }
                        erg.Add(sab);
                    }
                }
                orbitals[orb] = erg;
                erg2.AddRange(erg);
            }
            return erg2;
        }

        public void WatchOrbital(Orbital orb)
        {
            if (!orbitals.ContainsKey(orb))
                orbitals.Add(orb, []);
        }

        public List<SabEntry> GetEntries(Orbital orb) => orbitals[orb];

        //public /*abstract*/ double GroupVelocity(double v);
    }
}