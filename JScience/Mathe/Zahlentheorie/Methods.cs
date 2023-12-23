using System.Numerics;

namespace JScience.Mathe.Zahlentheorie
{
    public static class Methods
    {
        public static BigInteger Fakultaet(BigInteger x)
        {
            BigInteger erg = 1;
            for (BigInteger i = 2; i <= x; i++)
            {
                erg *= i;
            }
            return erg;
        }
    }
}