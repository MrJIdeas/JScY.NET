using JScience.Physik.Simulationen.Spins.Enums;
using JScience.Physik.Simulationen.Wavefunctions.Interfaces;
using System;

namespace JScience.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF
{
    public struct WFInfo
    {
        public WFInfo(int dimX, int dimY, int dimZ, ELatticeBoundary boundaryInfo)
        {
            DimX = dimX;
            DimY = dimY;
            DimZ = dimZ;
            BoundaryInfo = boundaryInfo;
            if (dimZ > 1)
                Type = typeof(IWF_3D);
            else if (dimY > 1)
                Type = typeof(WF_2D);
            else
                Type = typeof(WF_1D);
        }

        public Type Type { get; private set; }

        public int DimX { get; private set; }
        public int DimY { get; private set; }
        public int DimZ { get; private set; }

        public ELatticeBoundary BoundaryInfo { get; private set; }
    }
}