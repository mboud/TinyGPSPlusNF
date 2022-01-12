namespace TinyGPSPlusNF
{
    using System;

    internal static class Utils
    {
        public static double ToFixed(double value, int digits)
        {
            var step = Math.Pow(10, digits);
            return Math.Truncate(step * value) / step;
        }

        public static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }
    }
}
