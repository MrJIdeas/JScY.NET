using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
using ScottPlot;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.BaseClasses
{
    public abstract class Sab
    {
        private List<SabEntry> entries;
        private readonly Plot myPlot = new();

        public List<System.Drawing.Image> GetImage(int width, int height)
        {
            List<System.Drawing.Image> images = new List<System.Drawing.Image>();
            var exits = entries.Select(x => x.ExitName).Distinct();
            foreach (var exit in exits)
            {
                myPlot.Clear();
                List<double> x = new List<double>();
                List<double> y = new List<double>();
                foreach (var item in entries.Where(x => x.ExitName.Equals(exit)).OrderBy(x => x.v))
                {
                    x.Add(item.v);
                    y.Add((item.sab * Complex.Conjugate(item.sab)).Real);
                }
                myPlot.Add.Scatter(x, y);
                var img = System.Drawing.Image.FromStream(new MemoryStream(myPlot.GetImage(width, height).GetImageBytes()));
                images.Add(img);
            }
            return images;
        }

        public List<SabEntry> CalcSab(CabLogger cabLogger, double vMin, double vMax)
        {
            if (cabLogger == null || cabLogger.GetEntries().Count <= 1)
                return null;
            List<SabEntry> erg = new List<SabEntry>();
            List<CabEntry> cabs = cabLogger.GetEntries();
            var exits = cabs.Select(x => x.ExitName).Distinct().ToList();
            foreach (var (e, cablist, dv, dt) in from e in exits
                                                 let cablist = cabs.Where(x => x.ExitName.Equals(e)).OrderBy(x => x.t).ToList()
                                                 let dv = (vMax - vMin) / cablist.Count()
                                                 let dt = (cablist.Last().t - cablist.First().t) / cablist.Count
                                                 select (e, cablist, dv, dt))
            {
                for (int i = 0; i < cablist.Count(); i++)
                {
                    SabEntry sab = new SabEntry();
                    sab.v = vMin + dv * i;
                    sab.ExitName = e;
                    for (int j = 0; j < cablist.Count(); j++)
                    {
                        sab.sab += cablist[j].cab * dt * Complex.Exp(Complex.ImaginaryOne * sab.v * cablist[j].t);
                    }
                    erg.Add(sab);
                }
            }
            entries = erg;
            return erg;
        }

        //public /*abstract*/ double GroupVelocity(double v);
    }
}