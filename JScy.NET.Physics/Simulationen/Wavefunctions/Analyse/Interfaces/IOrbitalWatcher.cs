using System.Collections.Generic;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Interfaces
{
    public interface IOrbitalWatcher<T>
    {
        void WatchOrbital(Orbital orb);

        List<T> GetEntries(Orbital orb);
    }
}