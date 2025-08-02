using JScy.NET.Enums;
using JScy.NET.Physics.Simulationen.Spins.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using System;
using System.Collections.Generic;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF
{
    public class WFInfo
    {
        private readonly Dictionary<string, object> DynamicInfo = [];

        public readonly ECalculationMethod CalcMethod;

        public WFInfo(int dimX, int dimY, int dimZ, ELatticeBoundary boundaryInfo, EWaveType eWaveType, ECalculationMethod calculationMethod)
        {
            CalcMethod = calculationMethod;
            waveType = eWaveType;
            DimInfo = new DimensionInfo(dimX, dimY, dimZ);
            BoundaryInfo = boundaryInfo;
            if (dimZ > 1)
                Type = typeof(IWF_3D);
            else if (dimY > 1)
                Type = typeof(WF_2D);
            else
                Type = typeof(WF_1D);
        }

        public Type Type { get; private set; }

        public readonly DimensionInfo DimInfo;

        public ELatticeBoundary BoundaryInfo { get; private set; }

        public EWaveType waveType { get; private set; }

        public void AddAdditionalInfo<T>(string key, T val)
        {
            if (!DynamicInfo.ContainsKey(key))
                DynamicInfo.Add(key, val);
        }

        public T GetAdditionalInfo<T>(string key) => DynamicInfo.TryGetValue(key, out object value) ? (T)value : default;

        public Dictionary<string, object> GetAllAdditionalInfos() => DynamicInfo;

        internal List<CabExit> CabExits { get; set; }
    }
}