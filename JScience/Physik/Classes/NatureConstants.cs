using JScience.AttributesCustom;
using System;

namespace JScience.Physik.Classes
{
    /// <summary>
    /// Static Class With Values for nature constants in SI-Units
    /// </summary>
    public static class NatureConstants
    {
        [Unit("kg")]
        [Sign("u")]
        public static double AtomicMassUnit = 1.66053873e-27;

        [Unit("1/mol")]
        [Sign("N_A")]
        public static double AvogadroConstant = 6.0221353e23;

        [Unit("J/K")]
        [Sign("k")]
        public static double BoltzmannConstant = 1.3806503e-23;

        [Unit("m")]
        [Sign("lambda_C")]
        public static double ComptonWaveLength = 2.426310215e-12;

        [Unit("C/(V*m)")]
        [Sign("epsilon_0")]
        public static double ElectricFieldConstant = 8.854187817e-12;

        [Unit("C")]
        [Sign("e")]
        public static double ElementaryCharge = 1.602176462e-19;

        [Unit("C/mol")]
        [Sign("F")]
        public static double FaradayConstant = 96485.3415;

        [Unit("m^3/(kg*s^2)")]
        [Sign("G")]
        public static double GravityConstant = 6.673e-11;

        [Unit("m/s")]
        [Sign("c")]
        public static int LightspeedVacuum = 299792458;

        [Unit("1/m^3")]
        [Sign("n_0")]
        public static double LoschmidtConstant = 2.6867775e25;

        [Unit("H/m")]
        [Sign("mu_0")]
        public static double MagneticConstant = Math.PI * 4e-7;

        [Unit("m^3/mol")]
        [Sign("V_m,0")]
        public static double IdealGasNormVolume = 0.0224141;

        [Unit("J*s")]
        [Sign("h")]
        public static double PlanckConstant = 6.62606875e-34;

        [Unit("1/m")]
        [Sign("R_inf")]
        public static double RydbergConstant = 10973731.568549;

        [Unit("W/(m^2*K^4)")]
        [Sign("sigma")]
        public static double StefanBoltzmannConstant = 5.670400e-8;

        [Unit("K")]
        [Sign("T_tr")]
        public static double TriplePointWater = 273.16;

        [Unit("J/(K*mol)")]
        [Sign("R")]
        public static double UniversalGasConstant = 8.314472;

        [Unit("kg")]
        [Sign("m_e")]
        public static double Mass_Electron = 9.10938188e-31;

        [Unit("kg")]
        [Sign("m_p")]
        public static double Mass_Proton = 1.67262158e-27;

        [Unit("kg")]
        [Sign("m_n")]
        public static double Mass_Neutron = 1.67492716e-27;

        [Unit("C/kg")]
        [Sign("e/m_e")]
        public static double SpecificCharge_Electron = ElementaryCharge / Mass_Electron;
    }
}