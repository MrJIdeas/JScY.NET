using JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.VarTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace JScience.Physik.Simulationen.Wavefunctions.TimeEvolution.Classes
{
    public abstract class Sab
    {
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

            return erg;
        }

        public abstract double GroupVelocity(double v);
    }
}