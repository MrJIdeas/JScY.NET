namespace JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF
{
    public readonly struct DimensionInfo(int x, int y, int z)
    {
        public readonly int DimX = x;
        public readonly int DimY = y;
        public readonly int DimZ = z;

        public readonly int Dimensions = (x > 0 ? 1 : 0) + (y > 1 ? 1 : 0) + (z > 1 ? 1 : 0);
    }
}