using JScience.AttributesCustom;
using System;

namespace JScience.Physik.Classes
{
    /// <summary>
    /// Static Class With Values for nature constants in SI-Units
    /// </summary>
    public static class NatureConstants
    {
        /// <summary>
        /// Atomare Masseneinheit.
        /// </summary>
        [Unit("kg")]
        [Sign("u")]
        public static double AtomicMassUnit = 1.66053873e-27;

        /// <summary>
        /// Avogadro-Konstante.
        /// </summary>
        [Unit("1/mol")]
        [Sign("N_A")]
        public static double AvogadroConstant = 6.0221353e23;

        /// <summary>
        /// Boltzmann-Konstante.
        /// </summary>
        [Unit("J/K")]
        [Sign("k")]
        public static double BoltzmannConstant = 1.3806503e-23;

        /// <summary>
        /// Compton-Wellenlänge.
        /// </summary>
        [Unit("m")]
        [Sign("lambda_C")]
        public static double ComptonWaveLength = 2.426310215e-12;

        /// <summary>
        /// Elektrische Feldkonstante.
        /// </summary>
        [Unit("C/(V*m)")]
        [Sign("epsilon_0")]
        public static double ElectricFieldConstant = 8.854187817e-12;

        /// <summary>
        /// Elementarladung.
        /// </summary>
        [Unit("C")]
        [Sign("e")]
        public static double ElementaryCharge = 1.602176462e-19;

        /// <summary>
        /// Faraday-Konstante.
        /// </summary>
        [Unit("C/mol")]
        [Sign("F")]
        public static double FaradayConstant = 96485.3415;

        /// <summary>
        /// Gravitationskonstante.
        /// </summary>
        [Unit("m^3/(kg*s^2)")]
        [Sign("G")]
        public static double GravityConstant = 6.673e-11;

        /// <summary>
        /// Lichtgeschindigkeit im Vakuum.
        /// </summary>
        [Unit("m/s")]
        [Sign("c")]
        public static int LightspeedVacuum = 299792458;

        /// <summary>
        /// Loschmidt-Konstante.
        /// </summary>
        [Unit("1/m^3")]
        [Sign("n_0")]
        public static double LoschmidtConstant = 2.6867775e25;

        /// <summary>
        /// Magnetische Feldkonstante.
        /// </summary>
        [Unit("H/m")]
        [Sign("mu_0")]
        public static double MagneticConstant = Math.PI * 4e-7;

        /// <summary>
        /// Ideale Gaskonstante.
        /// </summary>
        [Unit("m^3/mol")]
        [Sign("V_m,0")]
        public static double IdealGasNormVolume = 0.0224141;

        /// <summary>
        /// Plansches Wirkungsquantum.
        /// </summary>
        [Unit("J*s")]
        [Sign("h")]
        public static double PlanckConstant = 6.62606875e-34;

        /// <summary>
        /// Rydbergkonstante.
        /// </summary>
        [Unit("1/m")]
        [Sign("R_inf")]
        public static double RydbergConstant = 10973731.568549;

        /// <summary>
        /// Stefan-Boltzmann-Konstante.
        /// </summary>
        [Unit("W/(m^2*K^4)")]
        [Sign("sigma")]
        public static double StefanBoltzmannConstant = 5.670400e-8;

        /// <summary>
        /// Tripelpunkt von Wasser.
        /// </summary>
        [Unit("K")]
        [Sign("T_tr")]
        public static double TriplePointWater = 273.16;

        /// <summary>
        /// Universelle Gaskonstante.
        /// </summary>
        [Unit("J/(K*mol)")]
        [Sign("R")]
        public static double UniversalGasConstant = 8.314472;

        /// <summary>
        /// Masse des Elektrons.
        /// </summary>
        [Unit("kg")]
        [Sign("m_e")]
        public static double Mass_Electron = 9.10938188e-31;

        /// <summary>
        /// Masse des Protons.
        /// </summary>
        [Unit("kg")]
        [Sign("m_p")]
        public static double Mass_Proton = 1.67262158e-27;

        /// <summary>
        /// Masse des Neutrons.
        /// </summary>
        [Unit("kg")]
        [Sign("m_n")]
        public static double Mass_Neutron = 1.67492716e-27;

        /// <summary>
        /// Spezifische Ladung Elektron.
        /// </summary>
        [Unit("C/kg")]
        [Sign("e/m_e")]
        public static double SpecificCharge_Electron = ElementaryCharge / Mass_Electron;
    }
}