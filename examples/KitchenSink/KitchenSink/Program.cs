using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using nanoFramework.Hardware.Esp32;
using TinyGPSPlusNF;

namespace KitchenSink
{
    public class Program
    {
        private const float LONDON_LAT = 51.508131f;
        private const float LONDON_LON = -0.128002f;

        private static TinyGPSPlus s_gps;
        private static SerialPort s_serial;

        private static long s_last = Environment.TickCount64; // For stats that happen every 5 seconds

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

            Debug.WriteLine("KitchenSink");
            Debug.WriteLine("Demonstrating nearly every feature of TinyGPSPlusNF");
            Debug.Write("Testing TinyGPSPlusNF library v");
            Debug.WriteLine(TinyGPSPlus.LibraryVersion);
            Debug.WriteLine(string.Empty);

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

            string nmea;

            try
            {
                byte[] buffer = new byte[s_serial.BytesToRead];
                int bytesRead = s_serial.Read(buffer, 0, buffer.Length);
                nmea = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            }
            catch (Exception)
            {
                return;
            }

            for (int i = 0; i < nmea.Length; i++)
            {
                if (s_gps.Encode(nmea[i]))
                {
                    DisplayInfo();
                }
            }
        }

        private static void DisplayInfo()
        {
            if (s_gps.Location.IsUpdated)
            {
                Debug.Write("LOCATION ; Fix Age=");
                Debug.Write(s_gps.Location.Age.ToString());
                Debug.Write("ms ; Raw Lat=");
                Debug.Write(s_gps.Location.Latitude.Negative ? "-" : string.Empty);
                Debug.Write(s_gps.Location.Latitude.HoleDegrees.ToString());
                Debug.Write("[+");
                Debug.Write(s_gps.Location.Latitude.Billionths.ToString());
                Debug.Write(" billionths] ; Raw Long=");
                Debug.Write(s_gps.Location.Longitude.Negative ? "-" : string.Empty);
                Debug.Write(s_gps.Location.Longitude.HoleDegrees.ToString());
                Debug.Write("[+");
                Debug.Write(s_gps.Location.Longitude.Billionths.ToString());
                Debug.Write(" billionths] ; Lat=");
                Debug.Write(s_gps.Location.Latitude.Degrees.ToString());
                Debug.Write(" ; Long=");
                Debug.WriteLine(s_gps.Location.Longitude.Degrees.ToString());
            }

            if (s_gps.Date.IsUpdated)
            {
                Debug.Write("DATE ; Fix Age=");
                Debug.Write(s_gps.Date.Age.ToString());
                Debug.Write("ms ; Raw=");
                Debug.Write(s_gps.Date.Value.ToString());
                Debug.Write(" ; Year=");
                Debug.Write(s_gps.Date.Year.ToString());
                Debug.Write(" ; Month=");
                Debug.Write(s_gps.Date.Month.ToString());
                Debug.Write(" ; Day=");
                Debug.WriteLine(s_gps.Date.Day.ToString());
            }

            if (s_gps.Time.IsUpdated)
            {
                Debug.Write("TIME ; Fix Age=");
                Debug.Write(s_gps.Time.Age.ToString());
                Debug.Write("ms ; Raw=");
                Debug.Write(s_gps.Time.Value.ToString());
                Debug.Write(" ; Hour=");
                Debug.Write(s_gps.Time.Hour.ToString());
                Debug.Write(" ; Minute=");
                Debug.Write(s_gps.Time.Minute.ToString());
                Debug.Write(" ; Second=");
                Debug.Write(s_gps.Time.Second.ToString());
                Debug.Write(" ; Hundredths=");
                Debug.WriteLine(s_gps.Time.Centisecond.ToString());
            }

            if (s_gps.Speed.IsUpdated)
            {
                Debug.Write("SPEED ; Fix Age=");
                Debug.Write(s_gps.Speed.Age.ToString());
                Debug.Write("ms ; Raw=");
                Debug.Write(s_gps.Speed.Value.ToString());
                Debug.Write(" ; Knots=");
                Debug.Write(s_gps.Speed.Knots.ToString());
                Debug.Write(" ; MPH=");
                Debug.Write(s_gps.Speed.Mph.ToString());
                Debug.Write(" ; m/s=");
                Debug.Write(s_gps.Speed.Mps.ToString());
                Debug.Write(" ; km/h=");
                Debug.WriteLine(s_gps.Speed.Kmph.ToString());
            }

            if (s_gps.Course.IsUpdated)
            {
                Debug.Write("COURSE ; Fix Age=");
                Debug.Write(s_gps.Course.Age.ToString());
                Debug.Write("ms ; Raw=");
                Debug.Write(s_gps.Course.Value.ToString());
                Debug.Write(" ; Deg=");
                Debug.WriteLine(s_gps.Course.Degrees.ToString());
            }

            if (s_gps.Altitude.IsUpdated)
            {
                Debug.Write("ALTITUDE ; Fix Age=");
                Debug.Write(s_gps.Altitude.Age.ToString());
                Debug.Write("ms ; Raw=");
                Debug.Write(s_gps.Altitude.Value.ToString());
                Debug.Write(" ; Meters=");
                Debug.Write(s_gps.Altitude.Meters.ToString());
                Debug.Write(" ; Miles=");
                Debug.Write(s_gps.Altitude.Miles.ToString());
                Debug.Write(" ; KM=");
                Debug.Write(s_gps.Altitude.Kilometers.ToString());
                Debug.Write(" ; Feet=");
                Debug.WriteLine(s_gps.Altitude.Feet.ToString());
            }

            if (s_gps.Satellites.IsUpdated)
            {
                Debug.Write("SATELLITES ; Fix Age=");
                Debug.Write(s_gps.Satellites.Age.ToString());
                Debug.Write("ms ; Value=");
                Debug.WriteLine(s_gps.Satellites.Value.ToString());
            }

            if (s_gps.Hdop.IsUpdated)
            {
                Debug.Write("HDOP ; Fix Age=");
                Debug.Write(s_gps.Hdop.Age.ToString());
                Debug.Write("ms ; hdop=");
                Debug.WriteLine(s_gps.Hdop.Value.ToString());
            }

            if (Environment.TickCount64 - s_last > 5000)
            {
                if (s_gps.Location.IsValid)
                {
                    float distanceToLondon = TinyGPSPlus.DistanceBetween(
                       s_gps.Location.Latitude.Degrees,
                       s_gps.Location.Longitude.Degrees,
                       LONDON_LAT,
                       LONDON_LON);

                    float courseToLondon = TinyGPSPlus.CourseTo(
                       s_gps.Location.Latitude.Degrees,
                       s_gps.Location.Longitude.Degrees,
                        LONDON_LAT,
                        LONDON_LON);

                    Debug.Write("LONDON ; Distance=");
                    Debug.Write((distanceToLondon / 1000).ToString("N2"));
                    Debug.Write(" km ; Course-to=");
                    Debug.Write(courseToLondon.ToString());
                    Debug.Write(" degrees [");
                    Debug.Write(TinyGPSPlus.Cardinal(courseToLondon));
                    Debug.WriteLine("]");
                }

                Debug.Write("DIAGS ; Chars=");
                Debug.Write(s_gps.CharsProcessed.ToString());
                Debug.Write(" ; Sentences-with-Fix=");
                Debug.Write(s_gps.SentencesWithFix.ToString());
                Debug.Write(" ; Failed-checksum=");
                Debug.Write(s_gps.FailedChecksum.ToString());
                Debug.Write(" ; Passed-checksum=");
                Debug.WriteLine(s_gps.PassedChecksum.ToString());

                if (s_gps.CharsProcessed < 10)
                {
                    Debug.WriteLine("WARNING: No GPS data (no fix or bad wiring)");
                }

                s_last = Environment.TickCount64;
            }
        }
    }
}
