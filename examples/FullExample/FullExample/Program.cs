using System;
using System.Diagnostics;
using System.IO.Ports;
using nanoFramework.Hardware.Esp32;
using TinyGPSPlusNF;

namespace FullExample
{
    public class Program
    {
        private const float LONDON_LAT = 51.508131f;
        private const float LONDON_LON = -0.128002f;

        private static TinyGPSPlus s_gps;
        private static SerialPort s_serial;

        public static void Main()
        {
            s_gps = new();

            // Example based on an ESP32 board (NodeMCU-32S ESP-WROOM-32) which requires pin configuration.
            Configuration.SetPinFunction(Gpio.IO05, DeviceFunction.COM3_RX);
            Configuration.SetPinFunction(Gpio.IO04, DeviceFunction.COM3_TX);

            // Example based on a NEO-6M module (GY-NEO6MV2) which operates at 9600 bauds by default.
            s_serial = new SerialPort("COM3", 9600)
            {
                Handshake = Handshake.None,
                ReadTimeout = 10,
            };

            s_serial.DataReceived += GpsDataReceived;
            s_serial.Open();

            Debug.WriteLine("FullExample");
            Debug.WriteLine("An extensive example of many interesting TinyGPSPlusNF features");
            Debug.Write("Testing TinyGPSPlusNF library v");
            Debug.WriteLine(TinyGPSPlus.LibraryVersion);
            Debug.WriteLine(string.Empty);

            Debug.WriteLine("Sats HDOP  Latitude   Longitude   Fix  Date       Time     Date Alt    Course Speed Card  Distance Course Card  Chars Sentences Checksum");
            Debug.WriteLine("           (deg)      (deg)       Age                      Age  (m)    --- from GPS ----  ---- to London  ----  RX    RX        Fail");
            Debug.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------");

            while (true)
            {
                // Now we wait
            }
        }

        private static void GpsDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (s_serial.BytesToRead == 0)
            {
                return;
            }

            byte[] buffer = new byte[s_serial.BytesToRead];
            int bytesRead = s_serial.Read(buffer, 0, buffer.Length);

            for (int i = 0; i < bytesRead; i++)
            {
                if (s_gps.Encode((char)buffer[i]))
                {
                    DisplayInfo();
                }
            }
        }

        private static void DisplayInfo()
        {
            PrintInt(s_gps.Satellites.Value, s_gps.Satellites.IsValid, 5);
            PrintFloat(s_gps.Hdop.Value, s_gps.Hdop.IsValid, 6, 1);
            PrintFloat(s_gps.Location.Latitude.Degrees, s_gps.Location.IsValid, 11, 6);
            PrintFloat(s_gps.Location.Longitude.Degrees, s_gps.Location.IsValid, 12, 6);
            PrintInt((int)s_gps.Location.Age, s_gps.Location.IsValid, 5);
            PrintDateTime(s_gps.Date, s_gps.Time);
            PrintFloat(s_gps.Altitude.Meters, s_gps.Altitude.IsValid, 7, 2);
            PrintFloat(s_gps.Course.Degrees, s_gps.Course.IsValid, 7, 2);
            PrintFloat(s_gps.Speed.Kmph, s_gps.Speed.IsValid, 6, 2);
            PrintStr(s_gps.Course.IsValid ? TinyGPSPlus.Cardinal(s_gps.Course.Degrees) : "    ", 6);

            float distanceKmToLondon = TinyGPSPlus.DistanceBetween(
                s_gps.Location.Latitude.Degrees,
                s_gps.Location.Longitude.Degrees,
                LONDON_LAT,
                LONDON_LON) / 1000;

            PrintFloat(distanceKmToLondon, s_gps.Location.IsValid, 9, 2);

            float courseToLondon = TinyGPSPlus.CourseTo(
                s_gps.Location.Latitude.Degrees,
                s_gps.Location.Longitude.Degrees,
                LONDON_LAT,
                LONDON_LON);

            PrintFloat(courseToLondon, s_gps.Location.IsValid, 7, 2);

            string cardinalToLondon = TinyGPSPlus.Cardinal(courseToLondon);

            PrintStr(s_gps.Location.IsValid ? cardinalToLondon : "    ", 6);

            PrintInt(s_gps.CharsProcessed, true, 6);
            PrintInt(s_gps.SentencesWithFix, true, 10);
            PrintInt(s_gps.FailedChecksum, true, 9);

            Debug.WriteLine(string.Empty);
        }

        private static void PrintInt(uint val, bool valid, int len)
        {
            PrintInt((int)val, valid, len);
        }

        private static void PrintInt(long val, bool valid, int len)
        {
            PrintInt((int)val, valid, len);
        }

        private static void PrintInt(int val, bool valid, int len)
        {
            string value;

            if (valid)
            {
                value = val.ToString();
            }
            else
            {
                value = string.Empty;
            }

            Console.Write(value.PadRight(len));
        }

        private static void PrintFloat(float val, bool valid, int len, int prec)
        {
            string value;

            if (valid)
            {
                value = val.ToString(string.Concat("N" + prec));
            }
            else
            {
                value = string.Empty;
            }

            Console.Write(value.PadRight(len));
        }

        private static void PrintStr(string val, int len)
        {
            Console.Write(val.PadRight(len));
        }

        private static void PrintDateTime(TinyGPSDate d, TinyGPSTime t)
        {
            string date;

            if (d.IsValid)
            {
                date = string.Concat(
                    d.Year.ToString(),
                    "/",
                    d.Month.ToString().PadLeft(2, '0'),
                    "/",
                    d.Day.ToString().PadLeft(2, '0'));
            }
            else
            {
                date = string.Empty;
            }

            Console.Write(date.PadRight(11));

            string time;

            if (t.IsValid)
            {
                time = string.Concat(
                    t.Hour.ToString().PadLeft(2, '0'),
                    ":",
                    t.Minute.ToString().PadLeft(2, '0'),
                    ":",
                    t.Second.ToString().PadLeft(2, '0'));
            }
            else
            {
                time = string.Empty;
            }

            Console.Write(time.PadRight(9));

            PrintInt(d.Age, d.IsValid, 5);
        }
    }
}
