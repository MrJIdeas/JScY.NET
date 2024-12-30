using JScy.NET.Physik.Simulationen.Spins.Enums;
using JScy.NET.Physik.Simulationen.Wavefunctions.Enums;
using JScy.NET.Physik.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Collections.Generic;

namespace JScy.NET.Physik.Simulationen.Wavefunctions.VarTypes.StandardWF
{
    public class WFInfo
    {
        private Dictionary<string, object> DynamicInfo { get; set; }

        public WFInfo(int dimX, int dimY, int dimZ, ELatticeBoundary boundaryInfo, EWaveType eWaveType)
        {
            waveType = eWaveType;
            DynamicInfo = new Dictionary<string, object>();
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

        public EWaveType waveType { get; private set; }

        public void AddAdditionalInfo<T>(string key, T val)
        {
            if (!DynamicInfo.ContainsKey(key))
                DynamicInfo.Add(key, val);
        }

        public T GetAdditionalInfo<T>(string key) => DynamicInfo.ContainsKey(key) ? (T)DynamicInfo[key] : default;

        public Dictionary<string, object> GetAllAdditionalInfos() => DynamicInfo;

        internal Dictionary<string, IWavefunction> CabExits { get; set; }
    }
}