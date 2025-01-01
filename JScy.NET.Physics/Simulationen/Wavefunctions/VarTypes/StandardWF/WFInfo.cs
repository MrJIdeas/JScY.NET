﻿using System;
using System.Collections.Generic;
using JScy.NET.Physics.Simulationen.Spins.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF
{
    public class WFInfo
    {
        private Dictionary<string, object> DynamicInfo { get; set; }

        public WFInfo(int dimX, int dimY, int dimZ, ELatticeBoundary boundaryInfo, EWaveType eWaveType)
        {
            waveType = eWaveType;
            DynamicInfo = new Dictionary<string, object>();
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

        public T GetAdditionalInfo<T>(string key) => DynamicInfo.ContainsKey(key) ? (T)DynamicInfo[key] : default;

        public Dictionary<string, object> GetAllAdditionalInfos() => DynamicInfo;

        internal List<CabExit> CabExits { get; set; }
    }
}