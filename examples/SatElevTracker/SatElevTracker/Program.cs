using System.Diagnostics;
using System.IO.Ports;
using nanoFramework.Hardware.Esp32;
using TinyGPSPlusNF;

namespace SatElevTracker
{
    public struct Sat
    {
        public int Elevation { get; set; }
        public bool Active { get; set; }
    }

    public class Program
    {
        private const int MAX_SATELLITES = 40;
        private const int PAGE_LENGTH = 40;

        private static TinyGPSPlus s_gps;
        private static SerialPort s_serial;
        private static TinyGPSCustom s_totalGPGSVMessages;
        private static TinyGPSCustom s_messageNumber;
        private static TinyGPSCustom[] s_satNumber;
        private static TinyGPSCustom[] s_elevation;
        private static Sat[] s_sats;

        private static bool s_anyChanges = false;
        private static ulong s_lineCount = 0;

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

            Debug.WriteLine("SatElevTracker");
            Debug.WriteLine("Displays GPS satellite elevations as they change");
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
            s_totalGPGSVMessages = new TinyGPSCustom(s_gps, "GPGSV", 1, true); // $GPGSV sentence, first element
            s_messageNumber = new TinyGPSCustom(s_gps, "GPGSV", 2, true); // $GPGSV sentence, second element

            s_satNumber = new TinyGPSCustom[4];
            s_elevation = new TinyGPSCustom[4];

            // Initialize all the uninitialized TinyGPSCustom objects
            for (int i = 0; i < 4; ++i)
            {
                s_satNumber[i] = new TinyGPSCustom(s_gps, "GPGSV", 4 + 4 * i, true); // offsets 4, 8, 12, 16
                s_elevation[i] = new TinyGPSCustom(s_gps, "GPGSV", 5 + 4 * i, true); // offsets 5, 9, 13, 17
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
            if (!s_totalGPGSVMessages.IsUpdated)
            {
                return;
            }

            for (int i = 0; i < 4; ++i)
            {
                int no = s_satNumber[i].IsValid ? (int)s_satNumber[i].NumericValue.Value : 0;

                if (no >= 1 && no <= MAX_SATELLITES)
                {
                    int elev = s_elevation[i].IsValid ? (int)s_elevation[i].NumericValue.Value : 0;
                    s_sats[no - 1].Active = true;

                    if (s_sats[no - 1].Elevation != elev)
                    {
                        s_sats[no - 1].Elevation = elev;
                        s_anyChanges = true;
                    }
                }
            }

            int totalMessages = s_totalGPGSVMessages.IsValid ? (int)s_totalGPGSVMessages.NumericValue.Value : 0;
            int currentMessage = s_messageNumber.IsValid ? (int)s_messageNumber.NumericValue.Value : 0;

            if (totalMessages == currentMessage && s_anyChanges)
            {
                if (s_lineCount++ % PAGE_LENGTH == 0)
                {
                    PrintHeader();
                }

                TimePrint();

                for (int i = 0; i < MAX_SATELLITES; ++i)
                {
                    Debug.Write(" ");

                    if (s_sats[i].Active)
                    {
                        IntPrint(s_sats[i].Elevation);
                    }
                    else
                    {
                        Debug.Write("   ");
                    }

                    s_sats[i].Active = false;
                }

                Debug.WriteLine(string.Empty);
                s_anyChanges = false;
            }
        }

        private static void IntPrint(int n)
        {
            Debug.Write(n.ToString().PadLeft(2));
            Debug.Write(" ");
            return;
        }

        private static void TimePrint()
        {
            if (s_gps.Time.IsValid)
            {
                Debug.Write(s_gps.Time.Hour.ToString().PadLeft(2, '0'));
                Debug.Write(":");
                Debug.Write(s_gps.Time.Minute.ToString().PadLeft(2, '0'));
                Debug.Write(":");
                Debug.Write(s_gps.Time.Second.ToString().PadLeft(2, '0'));
                Debug.Write(" ");
            }
            else
            {
                Debug.Write("(unknown)");
            }
        }

        private static void PrintHeader()
        {
            Debug.WriteLine(string.Empty);
            Debug.Write("Time     ");

            for (int i = 0; i < MAX_SATELLITES; ++i)
            {
                Debug.Write(" ");
                IntPrint(i + 1);
            }

            Debug.WriteLine(string.Empty);
            Debug.Write("---------");

            for (int i = 0; i < MAX_SATELLITES; ++i)
            {
                Debug.Write("----");
            }

            Debug.WriteLine(string.Empty);
        }
    }
}
