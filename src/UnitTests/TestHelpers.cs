namespace UnitTests
{
    using nanoFramework.TestFramework;
    using TinyGPSPlusNF;

    internal static class TestHelpers
    {
        /// <summary>
        /// Encodes an nmea sentence using a <see cref="TinyGPSPlus"/> instance.
        /// </summary>
        /// <param name="gps">The <see cref="TinyGPSPlus"/> instance.</param>
        /// <param name="nmea">The nmea sentence.</param>
        /// <param name="assertChecksum">Run assert on <c>FailedChecksum</c> property when set to <c>true</c>.</param>
        public static void Encode(TinyGPSPlus gps, string nmea, bool assertChecksum = true)
        {
            foreach (char c in nmea)
            {
                if (gps.Encode(c))
                {
                    break;
                }
            }

            if (assertChecksum)
            {
                Assert.Equal(gps.FailedChecksum, 0);
            }
        }

        /// <summary>
        /// Builds a sentence with the given nmea command by adding the leading $, trailing * and checksum.
        /// </summary>
        /// <param name="nmea">Command contents</param>
        /// <returns>Full sentence with special chars and checksum</returns>
        public static string BuildSentence(string nmea)
        {
            int checksum = 0;

            for (int i = 0; i < nmea.Length; i++)
            {
                checksum ^= (byte)nmea[i];
            }

            string hexsum = checksum.ToString("X2").ToUpper();

            return string.Concat("$", nmea, "*", hexsum, "\r\n");
        }
    }
}
