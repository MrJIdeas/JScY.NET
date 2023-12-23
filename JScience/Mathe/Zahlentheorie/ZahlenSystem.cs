namespace JScience.Mathe.Zahlentheorie
{
    public static class ZahlenSystem
    {
        #region Dezimal zu Zahlensystem

        public static string ToNBase(long Zahl10, long n)
        {
            long remainder;
            string result = string.Empty;
            while (Zahl10 > 0)
            {
                remainder = Zahl10 % n;
                Zahl10 /= n;
                result = remainder.ToString() + result;
            }

            return result;
        }

        public static string To2Base(long Zahl10) => ToNBase(Zahl10, 2);

        public static string To3Base(long Zahl10) => ToNBase(Zahl10, 3);

        public static string To4Base(long Zahl10) => ToNBase(Zahl10, 4);

        public static string To5Base(long Zahl10) => ToNBase(Zahl10, 5);

        public static string To6Base(long Zahl10) => ToNBase(Zahl10, 6);

        public static string To7Base(long Zahl10) => ToNBase(Zahl10, 7);

        public static string To8Base(long Zahl10) => ToNBase(Zahl10, 8);

        public static string To9Base(long Zahl10) => ToNBase(Zahl10, 9);

        #endregion Dezimal zu Zahlensystem
    }
}