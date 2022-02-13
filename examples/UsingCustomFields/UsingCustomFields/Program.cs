using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using nanoFramework.Hardware.Esp32;
using TinyGPSPlusNF;

namespace UsingCustomFields
{
    public class Program
    {
        private static TinyGPSPlus s_gps;
        private static SerialPort s_serial;

        private static TinyGPSCustom s_pdop;
        private static TinyGPSCustom s_hdop;
        private static TinyGPSCustom s_vdop;

        public static void Main()
        {
            s_gps = new();

            /*
                By declaring TinyGPSCustom objects like this, we announce that we
                are interested in the 15th, 16th, and 17th fields in the $GPGSA 
                sentence, respectively the PDOP ("positional dilution of precision"),
                HDOP ("horizontal..."), and VDOP ("vertical...").
                
                Counting starts with the field immediately following the sentence name, 
                i.e. $GPGSA. For more information on NMEA sentences, consult your
                GPS module's documentation and/or http://aprs.gids.nl/nmea/.
                
                If your GPS module doesn't support the $GPGSA sentence, then you 
                won't get any output from this program.

                Please note that the HDOP value is a native field of TinyGPSPlusNF,
                it is fetched using a TinyGPSCustom object for the sole purpose of
                illustrating how to use these objects.

                The following TinyGPSCustom objects could be declared with the
                constructor parameter "isNumeric" set to true because these are
                numeric values. Their value is only for display here, so this is
                not necessary to treat them as numeric and it avoids the overhead
                of casting the string values.
                
                If these were declared as numeric, accessing their value would be
                possible using the property "NumericValue".
            */

            s_pdop = new TinyGPSCustom(s_gps, "GPGSA", 15); // $GPGSA sentence, 15th element
            s_hdop = new TinyGPSCustom(s_gps, "GPGSA", 16); // $GPGSA sentence, 16th element
            s_vdop = new TinyGPSCustom(s_gps, "GPGSA", 17); // $GPGSA sentence, 17th element

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

            Debug.WriteLine("UsingCustomFields");
            Debug.WriteLine("Demonstrating how to extract any NMEA field using TinyGPSCustom");
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
            if (s_gps.Altitude.IsUpdated)
            {
                Debug.Write("ALT=");
                Debug.Write(s_gps.Altitude.Meters.ToString());
                Debug.WriteLine(" meters");
            }

            if (s_gps.Satellites.IsUpdated)
            {
                Debug.Write("SATS=");
                Debug.WriteLine(s_gps.Satellites.Value.ToString());
            }

            if (s_pdop.IsUpdated)
            {
                Debug.Write("PDOP=");
                Debug.WriteLine(s_pdop.Value);
            }

            if (s_hdop.IsUpdated)
            {
                Debug.Write("HDOP=");
                Debug.WriteLine(s_hdop.Value);
            }

            if (s_vdop.IsUpdated)
            {
                Debug.Write("VDOP=");
                Debug.WriteLine(s_vdop.Value);
            }
        }
    }
}
