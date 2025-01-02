using System.Drawing;

namespace JScy.NET.Interfaces
{
    public interface IPlotter
    {
        Image GetImage(int width, int height);
    }
}