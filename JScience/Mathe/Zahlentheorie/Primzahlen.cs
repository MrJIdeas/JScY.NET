using System.Collections.Generic;
using System.Linq;

namespace JScience.Mathe.Zahlentheorie
{
    public static class Primzahlen
    {
        public static bool IstPrimzahl(int zahl)
        {
            if (zahl < 2) return false;
            if (zahl == 2) return true;
            if (zahl % 2 == 0) return false;
            for (int i = 3; i < zahl - 1; i += 2)
                if (zahl % i == 0) return false;
            return true;
        }

        public static List<long> FindePrimzahl(long max)
        {
            if (max < 2) return null;
            List<long> primes = new List<long>();
            bool[] numbers = new bool[max + 1];

            for (long i = 0; i <= max; i++)
            {
                numbers[i] = true;
            }

            long p = 2;

            while (p * p <= max)
            {
                for (long i = p + p; i <= max; i += p)
                {
                    numbers[i] = false;
                }

                long x = p + 1;
                while (numbers[x] == false) x++;
                p = x;
            }

            for (long i = 2; i <= max; i++)
            {
                if (numbers[i] == true) primes.Add(i);
            }

            return primes;
        }

        public static Dictionary<long, long> GibPrimzahlenAbstaende(long max)
        {
            var erg = FindePrimzahl(max);
            var erglist = new Dictionary<long, long>();
            erglist.Add(erg.FirstOrDefault(), 0);
            for (int i = 1; i < erg.Count; i++)
            {
                erglist.Add(erg[i], erg[i] - erg[i - 1]);
            }
            return erglist;
        }
    }
}