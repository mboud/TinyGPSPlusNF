using System.Diagnostics;
using System.Threading;
using TinyGPSPlusNF;

namespace BasicExample
{
    public class Program
    {
        private static readonly string[] gpsStream =
        {
          "$GPRMC,045103.000,A,3014.1984,N,09749.2872,W,0.67,161.46,030913,,,A*7C\r\n",
          "$GPGGA,045104.000,3014.1985,N,09749.2873,W,1,09,1.2,211.6,M,-22.5,M,,0000*62\r\n",
          "$GPRMC,045200.000,A,3014.3820,N,09748.9514,W,36.88,65.02,030913,,,A*77\r\n",
          "$GPGGA,045201.000,3014.3864,N,09748.9411,W,1,10,1.2,200.8,M,-22.5,M,,0000*6C\r\n",
          "$GPRMC,045251.000,A,3014.4275,N,09749.0626,W,0.51,217.94,030913,,,A*7D\r\n",
          "$GPGGA,045252.000,3014.4273,N,09749.0628,W,1,09,1.3,206.9,M,-22.5,M,,0000*6F\r\n",
        };

        private static TinyGPSPlus s_gps;

        public static void Main()
        {
            s_gps = new();

            Debug.WriteLine("BasicExample");
            Debug.WriteLine("Basic demonstration of TinyGPSPlusNF (no gps device needed)");
            Debug.Write("Testing TinyGPSPlus library v");
            Debug.WriteLine(TinyGPSPlus.LibraryVersion);
            Debug.WriteLine(string.Empty);

            foreach (string nmea in gpsStream)
            {
                if (s_gps.Encode(nmea))
                {
                    DisplayInfo();
                }
            }

            Debug.WriteLine(string.Empty);
            Debug.WriteLine("Done.");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void DisplayInfo()
        {
            Debug.Write("Location: ");
            if (s_gps.Location.IsValid)
            {
                Debug.Write(s_gps.Location.Latitude.Degrees.ToString());
                Debug.Write(",");
                Debug.Write(s_gps.Location.Longitude.Degrees.ToString());
            }
            else
            {
                Debug.Write("INVALID");
            }

            Debug.Write("  Date/Time: ");
            if (s_gps.Date.IsValid)
            {
                Debug.Write(s_gps.Date.Year.ToString());
                Debug.Write("/");
                Debug.Write(s_gps.Date.Month.ToString("D2"));
                Debug.Write("/");
                Debug.Write(s_gps.Date.Day.ToString("D2"));
            }
            else
            {
                Debug.Write("INVALID");
            }

            Debug.Write(" ");
            if (s_gps.Time.IsValid)
            {
                Debug.Write(s_gps.Time.Hour.ToString("D2"));
                Debug.Write(":");
                Debug.Write(s_gps.Time.Minute.ToString("D2"));
                Debug.Write(":");
                Debug.Write(s_gps.Time.Second.ToString("D2"));
                Debug.Write(".");
                Debug.Write(s_gps.Time.Centisecond.ToString("D2"));
            }
            else
            {
                Debug.Write("INVALID");
            }

            Debug.WriteLine(string.Empty);
        }
    }
}
