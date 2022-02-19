using System.Diagnostics;
using System.IO.Ports;
using nanoFramework.Hardware.Esp32;
using TinyGPSPlusNF;

namespace DeviceExample
{
    public class Program
    {
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

            Debug.WriteLine("DeviceExample");
            Debug.WriteLine("A simple demonstration of TinyGPSPlus with an attached GPS module");
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
