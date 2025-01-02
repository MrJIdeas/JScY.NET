using System.Collections.Generic;
using System.Drawing;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Interfaces
{
    public interface ILogger<U>
    {
        void Add(double t, Orbital value);

        List<U> GetEntries();

        List<Image> GetImage(int width, int height);
    }
}