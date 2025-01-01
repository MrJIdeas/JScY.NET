namespace JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF
{
    public struct DimensionInfo
    {
        public readonly int DimX;
        public readonly int DimY;
        public readonly int DimZ;

        public readonly int Dimensions;

        public DimensionInfo(int x, int y, int z)
        {
            DimX = x;
            DimY = y;
            DimZ = z;
            Dimensions = (x > 0 ? 1 : 0) + (y > 1 ? 1 : 0) + (z > 1 ? 1 : 0);
        }
    }
}