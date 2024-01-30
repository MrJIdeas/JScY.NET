namespace JScience.Mathe.ComplexNumbers.VarTypes
{
    public struct DecComplex
    {
        public DecComplex(decimal real, decimal imag)
        {
            Real = real;
            Imag = imag;
        }

        public decimal Real { get; set; }
        public decimal Imag { get; set; }

        public decimal Magnitude => Real * Real + Imag * Imag;

        public DecComplex Conj() => new DecComplex(Real, -Imag);

        #region Addition

        public static DecComplex operator +(DecComplex lhs, decimal rhs)
        {
            lhs.Real += rhs;
            return lhs;
        }

        public static DecComplex operator +(decimal rhs, DecComplex lhs) => lhs + rhs;

        public static DecComplex operator +(DecComplex lhs, DecComplex rhs)
        {
            lhs.Real += rhs.Real;
            lhs.Imag += rhs.Imag;
            return lhs;
        }

        #endregion Addition

        #region Subtraktion

        public static DecComplex operator -(DecComplex lhs, decimal rhs)
        {
            lhs.Real += rhs;
            return lhs;
        }

        public static DecComplex operator -(decimal rhs, DecComplex lhs) => -1 * lhs + rhs;

        public static DecComplex operator -(DecComplex lhs, DecComplex rhs)
        {
            lhs.Real -= rhs.Real;
            lhs.Imag -= rhs.Imag;
            return lhs;
        }

        #endregion Subtraktion

        #region Multiplikation

        public static DecComplex operator *(DecComplex lhs, decimal rhs)
        {
            lhs.Real *= rhs;
            lhs.Imag *= rhs;
            return lhs;
        }

        public static DecComplex operator *(decimal rhs, DecComplex lhs) => lhs * rhs;

        public static DecComplex operator *(DecComplex lhs, DecComplex rhs) => new DecComplex(lhs.Real * rhs.Real - lhs.Imag * rhs.Imag, lhs.Real * rhs.Imag + rhs.Real * lhs.Imag);

        #endregion Multiplikation

        #region Division

        public static DecComplex operator /(DecComplex lhs, decimal rhs)
        {
            lhs.Real /= rhs;
            lhs.Imag /= rhs;
            return lhs;
        }

        public static DecComplex operator /(DecComplex lhs, DecComplex rhs) => lhs * rhs.Conj() / rhs.Magnitude;

        #endregion Division
    }
}