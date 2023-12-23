namespace JScience.Mathe.Stochastik
{
    public static class Binom
    {
        public static ulong Koeffizient(ulong n, ulong k)
        {
            if (n < k) return 0L;
            if (n < 2 * k) k = n - k;
            if (k == 1) return n;
            if (k == 0) return 1;
            ulong nminusk = n - k;
            ulong bin = nminusk + 1;
            for (ulong i = 2; i <= k; i++)
                bin = bin * (nminusk + i) / i;
            return bin;
        }
    }
}