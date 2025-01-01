using JScy.NET.AttributesCustom;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.TopologicalInsulator.VarTypes
{
    public readonly struct TPI_MaterialInfo
    {
        public readonly string Bezeichnung;

        [Unit("eV")]
        public readonly double M;

        [Unit("eV*Angström")]
        public readonly double A1;

        [Unit("eV*Angström")]
        public readonly double A2;

        [Unit("eV*Angström^2")]
        public readonly double B1;

        [Unit("eV*Angström^2")]
        public readonly double B2;

        [Unit("eV*Angström")]
        public readonly double C;

        [Unit("eV*Angström^2")]
        public readonly double D1;

        [Unit("eV*Angström^2")]
        public readonly double D2;

        internal TPI_MaterialInfo(string bezeichnung, double m, double a1, double a2, double b1, double b2, double c, double d1, double d2)
        {
            Bezeichnung = bezeichnung;
            M = m;
            A1 = a1;
            A2 = a2;
            B1 = b1;
            B2 = b2;
            C = c;
            D1 = d1;
            D2 = d2;
        }
    }
}