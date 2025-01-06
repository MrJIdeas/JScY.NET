using System.Collections.Generic;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Interfaces
{
    public interface I_ImageGenerator
    {
        List<System.Drawing.Image> GetImage(int width, int height);
    }
}