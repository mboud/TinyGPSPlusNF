using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using nanoFramework.Hardware.Esp32;
using TinyGPSPlusNF;

namespace SatelliteTracker
{
    public struct Sat
    {
        public bool Active { get; set; }
        public int Elevation { get; set; }
        public int Azimuth { get; set; }
        public int Snr { get; set; }
    }

    public class Program
    {
        private const int MAX_SATELLITES = 40;

        private static TinyGPSPlus s_gps;
        private static SerialPort s_serial;
        private static TinyGPSCustom s_totalGPGSVMessages; // $GPGSV sentence, first element
        private static TinyGPSCustom s_messageNumber; // $GPGSV sentence, second element
        private static TinyGPSCustom[] s_satNumber;
        private static TinyGPSCustom[] s_elevation;
        private static TinyGPSCustom[] s_azimuth;
        private static TinyGPSCustom[] s_snr;
        private static Sat[] s_sats;

        /* 
            From http://aprs.gids.nl/nmea/:
   
            $GPGSV
  
            GPS Satellites in view
  
            eg. $GPGSV,3,1,11,03,03,111,00,04,15,270,00,06,01,010,00,13,06,292,00*74
                $GPGSV,3,2,11,14,25,170,00,16,57,208,39,18,67,296,40,19,40,246,00*74
                $GPGSV,3,3,11,22,42,067,42,24,14,311,43,27,05,244,00,,,,*4D
            1    = Total number of messages of this type in this cycle
            2    = Message number
            3    = Total number of SVs in view
            4    = SV PRN number
            5    = Elevation in degrees, 90 maximum
            6    = Azimuth, degrees from true north, 000 to 359
            7    = SNR, 00-99 dB (null when not tracking)
            8-11 = Information about second SV, same as field 4-7
            12-15= Information about third SV, same as field 4-7
            16-19= Information about fourth SV, same as field 4-7
        */

        public static void Main()
        {
            s_sats = new Sat[MAX_SATELLITES];

            for (int i = 0; i < MAX_SATELLITES; i++)
            {
                s_sats[i] = new Sat();
            }

            s_gps = new();

            InitCustom();

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

            Debug.WriteLine("SatelliteTracker");
            Debug.WriteLine("Monitoring satellite location and signal strength using TinyGPSCustom");
            Debug.Write("Testing TinyGPSPlusNF library v");
            Debug.WriteLine(TinyGPSPlus.LibraryVersion);
            Debug.WriteLine(string.Empty);

            while (true)
            {
                // Now we wait
            }
        }

        private static void InitCustom()
        {
            s_totalGPGSVMessages = new TinyGPSCustom(s_gps, "GPGSV", 1, true);
            s_messageNumber = new TinyGPSCustom(s_gps, "GPGSV", 2, true);

            s_satNumber = new TinyGPSCustom[4];
            s_elevation = new TinyGPSCustom[4];
            s_azimuth = new TinyGPSCustom[4];
            s_snr = new TinyGPSCustom[4];

            for (int i = 0; i < 4; ++i)
            {
                s_satNumber[i] = new TinyGPSCustom(s_gps, "GPGSV", 4 + 4 * i, true); // offsets 4, 8, 12, 16
                s_elevation[i] = new TinyGPSCustom(s_gps, "GPGSV", 5 + 4 * i, true); // offsets 4, 8, 12, 16
                s_azimuth[i] = new TinyGPSCustom(s_gps, "GPGSV", 6 + 4 * i, true); // offsets 4, 8, 12, 16
                s_snr[i] = new TinyGPSCustom(s_gps, "GPGSV", 7 + 4 * i, true); // offsets 4, 8, 12, 16
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
            if (s_totalGPGSVMessages.IsUpdated)
            {
                for (int i = 0; i < 4; ++i)
                {
                    int no = (int)s_satNumber[i].NumericValue.Value;

                    if (no >= 1 && no <= MAX_SATELLITES)
                    {
                        s_sats[no - 1].Elevation = (int)s_elevation[i].NumericValue.Value;
                        s_sats[no - 1].Azimuth = (int)s_azimuth[i].NumericValue.Value;
                        s_sats[no - 1].Snr = (int)s_snr[i].NumericValue.Value;
                        s_sats[no - 1].Active = true;
                    }
                }

                int totalMessages = (int)s_totalGPGSVMessages.NumericValue.Value;
                int currentMessage = (int)s_messageNumber.NumericValue.Value;

                if (totalMessages == currentMessage)
                {
                    Debug.Write("Sats=");
                    Debug.Write(s_gps.Satellites.Value.ToString());

                    Debug.Write(" ; Nums=");
                    for (int i = 0; i < MAX_SATELLITES; ++i)
                    {
                        if (s_sats[i].Active)
                        {
                            Debug.Write(" ");
                            Debug.Write((i + 1).ToString());
                        }
                    }

                    Debug.Write(" ; Elevation=");
                    for (int i = 0; i < MAX_SATELLITES; ++i)
                    {
                        if (s_sats[i].Active)
                        {
                            Debug.Write(" ");
                            Debug.Write(s_sats[i].Elevation.ToString());
                        }
                    }

                    Debug.Write(" ; Azimuth=");
                    for (int i = 0; i < MAX_SATELLITES; ++i)
                    {
                        if (s_sats[i].Active)
                        {
                            Debug.Write(" ");
                            Debug.Write(s_sats[i].Azimuth.ToString());
                        }
                    }

                    Debug.Write(" ; SNR=");
                    for (int i = 0; i < MAX_SATELLITES; ++i)
                    {
                        if (s_sats[i].Active)
                        {
                            Debug.Write(" ");
                            Debug.Write(s_sats[i].Snr.ToString());
                        }
                    }

                    Debug.WriteLine(string.Empty);

                    for (int i = 0; i < MAX_SATELLITES; ++i)
                    {
                        s_sats[i].Active = false;
                    }
                }
            }
        }
    }
}