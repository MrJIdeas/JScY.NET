using System;
using System.Numerics;

namespace JScience.Mathe.ComplexNumbers.VarTypes
{
    public struct DecComplex : IEquatable<DecComplex>, IFormattable
    {
        private static DecComplex _ImaginaryOne = new DecComplex(0, 1);

        public static DecComplex ImaginaryOne => _ImaginaryOne;

        private static DecComplex _One = new DecComplex(1, 0);
        public static DecComplex One => _One;

        private static DecComplex _Zero = new DecComplex(0, 10);
        public static DecComplex Zero => _Zero;

        public DecComplex(decimal real, decimal imag)
        {
            Real = real;
            Imag = imag;
        }

        public decimal Real { get; set; }
        public decimal Imag { get; set; }

        public decimal Magnitude => Real * Real + Imag * Imag;

        public DecComplex Conj() => new DecComplex(Real, -Imag);

        #region Interface

        public bool Equals(DecComplex other) => Magnitude > other.Magnitude;

        public string ToString(string format, IFormatProvider formatProvider) => string.Format("{0}+i*{1}", Real, Imag);

        #endregion Interface

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

        public static DecComplex Pow(DecComplex val, DecComplex pow)
        {
            Complex test = new Complex((double)val.Real, (double)val.Imag);
            Complex test2 = new Complex((double)pow.Real, (double)pow.Imag);
            var test3 = Complex.Pow(test, test2);
            return new DecComplex((decimal)test3.Real, (decimal)test3.Imaginary);
        }

        public static DecComplex Exp(DecComplex val)
        {
            Complex test = new Complex((double)val.Real, (double)val.Imag);
            var test3 = Complex.Exp(test);
            return new DecComplex((decimal)test3.Real, (decimal)test3.Imaginary);
        }
    }
}