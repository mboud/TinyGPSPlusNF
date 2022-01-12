namespace TinyGPSPlusNF
{
    using System;

    internal static class TryParse
    {
        public static bool Int32(string s, out int value)
        {
            bool success;

            try
            {
                value = int.Parse(s);
                success = true;
            }
            catch (Exception)
            {
                value = default;
                success = false;
            }

            return success;
        }

        public static bool Int16(string s, out short value)
        {
            bool success;

            try
            {
                value = short.Parse(s);
                success = true;
            }
            catch (Exception)
            {
                value = default;
                success = false;
            }

            return success;
        }

        public static bool Double(string s, out double value)
        {
            bool success;

            try
            {
                value = double.Parse(s);
                success = true;
            }
            catch (Exception)
            {
                value = default;
                success = false;
            }

            return success;
        }
    }
}
