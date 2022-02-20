namespace TinyGPSPlusNF
{
    using System;
    using System.Reflection;

    /// <summary>
    /// This class allows parsing NMEA sentences provided by GPS modules.
    /// </summary>
    public class TinyGPSPlus
    {
        internal const float _GPS_MPH_PER_KNOT = 1.15077945f;
        internal const float _GPS_MPS_PER_KNOT = 0.51444444f;
        internal const float _GPS_KMPH_PER_KNOT = 1.852f;
        internal const double _GPS_MILES_PER_METER = 0.00062137112;
        internal const float _GPS_KM_PER_METER = 0.001f;
        internal const float _GPS_FEET_PER_METER = 3.2808399f;
        internal const byte _GPS_MAX_FIELD_SIZE = 15;

        private const string _GPRMCSentenceIdentifier = "GPRMC";
        private const string _GPGGASentenceIdentifier = "GPGGA";
        private const string _GNRMCSentenceIdentifier = "GNRMC";
        private const string _GNGGASentenceIdentifier = "GNGGA";

        readonly char[] _term;
        private byte _parity;
        bool _isChecksumTerm;
        private GpsSentenceIdentifier _curSentenceType;
        private byte _curTermNumber;
        private byte _curTermOffset;
        private bool _sentenceHasFix;

        /// <summary>
        /// The latest position fix.
        /// </summary>
        public readonly TinyGPSLocation Location;

        /// <summary>
        /// The latest date fix (UTC).
        /// </summary>
        public readonly TinyGPSDate Date;

        /// <summary>
        /// The latest time fix (UTC).
        /// </summary>
        public readonly TinyGPSTime Time;

        /// <summary>
        /// Current ground speed.
        /// </summary>
        public readonly TinyGPSSpeed Speed;

        /// <summary>
        /// Current ground course.
        /// </summary>
        public readonly TinyGPSCourse Course;

        /// <summary>
        /// Latest latitude fix.
        /// </summary>
        public readonly TinyGPSAltitude Altitude;

        /// <summary>
        /// The number of visible, participating satellites.
        /// </summary>
        public readonly TinyGPSInteger Satellites;

        /// <summary>
        /// Horizontal Dilution of Precision.
        /// </summary>
        public readonly TinyGPSHDOP Hdop;

        private TinyGPSCustom _firstCustomElt;
        private TinyGPSCustom _firstCustomCandidate;

        /// <summary>
        /// Gets current version of the library.
        /// </summary>
        public static string LibraryVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// The total number of characters received by the object.
        /// </summary>
        /// <remarks>
        /// For debug purposes.
        /// </remarks>
        public uint CharsProcessed { get; private set; }

        /// <summary>
        /// The number of $GPRMC or $GPGGA sentences that had a fix.
        /// </summary>
        /// <remarks>
        /// For debug purposes.
        /// </remarks>
        public uint SentencesWithFix { get; private set; }

        /// <summary>
        /// The number of sentences of all types that failed the checksum test.
        /// </summary>
        /// <remarks>
        /// For debug purposes.
        /// </remarks>
        public uint FailedChecksum { get; private set; }

        /// <summary>
        /// The number of sentences of all types that passed the checksum test.
        /// </summary>
        /// <remarks>
        /// For debug purposes.
        /// </remarks>
        public uint PassedChecksum { get; private set; }

        /// <summary>
        /// Returns distance in meters between two positions, both specified as
        /// signed decimal-degrees latitude and longitude. Uses great-circle
        /// distance computation for hypothetical sphere of radius 6372795 meters.
        /// Because Earth is no exact sphere, rounding errors may be up to 0.5%.
        /// </summary>
        /// <remarks>Courtesy of Maarten Lamers.</remarks>
        /// <param name="lat1">Latitude of first coordinates</param>
        /// <param name="long1">Longitude of first coordinates</param>
        /// <param name="lat2">Latitude of second coordinates</param>
        /// <param name="long2">Longitude of second coordinates</param>
        /// <returns>Distance in meters.</returns>
        public static float DistanceBetween(float lat1, float long1, float lat2, float long2)
        {
            double delta = Radians(long1 - long2);
            double sdlong = Math.Sin(delta);
            double cdlong = Math.Cos(delta);
            double radlat1 = Radians(lat1);
            double radlat2 = Radians(lat2);
            double slat1 = Math.Sin(radlat1);
            double clat1 = Math.Cos(radlat1);
            double slat2 = Math.Sin(radlat2);
            double clat2 = Math.Cos(radlat2);
            delta = (clat1 * slat2) - (slat1 * clat2 * cdlong);
            delta = Math.Pow(delta, 2);
            delta += Math.Pow(clat2 * sdlong, 2);
            delta = Math.Sqrt(delta);
            double denom = (slat1 * slat2) + (clat1 * clat2 * cdlong);
            delta = Math.Atan2(delta, denom);
            return (float)delta * 6372795;
        }

        /// <summary>
        /// Returns course in degrees (North=0, West=270) from position 1 to
        /// position 2, both specified as signed decimal-degrees latitude and
        /// longitude. Because Earth is no exact sphere, calculated course may
        /// be off by a tiny fraction.
        /// </summary>
        /// <remarks>Courtesy of Maarten Lamers</remarks>
        /// <param name="lat1">Latitude of first coordinates</param>
        /// <param name="long1">Longitude of first coordinates</param>
        /// <param name="lat2">Latitude of second coordinates</param>
        /// <param name="long2">Longitude of second coordinates</param>
        /// <returns>Course in degrees.</returns>
        public static float CourseTo(float lat1, float long1, float lat2, float long2)
        {
            double dlon = Radians(long2 - long1);
            double radlat1 = Radians(lat1);
            double radlat2 = Radians(lat2);
            double a1 = Math.Sin(dlon) * Math.Cos(radlat2);
            double a2 = Math.Sin(radlat1) * Math.Cos(radlat2) * Math.Cos(dlon);
            a2 = Math.Cos(radlat1) * Math.Sin(radlat2) - a2;
            a2 = Math.Atan2(a1, a2);

            if (a2 < 0.0)
            {
                a2 += Math.PI * 2;
            }

            return (float)Degrees(a2);
        }

        /// <summary>
        /// Display the course in a friendly, human-readable compass directions.
        /// </summary>
        /// <param name="course">Input course value</param>
        /// <returns>Compass direction.</returns>
        public static string Cardinal(float course)
        {
            int index = (int)((course + 11.25) / 22.5) % 16;

            return index switch
            {
                0 => "N",
                1 => "NNE",
                2 => "NE",
                3 => "ENE",
                4 => "E",
                5 => "ESE",
                6 => "SE",
                7 => "SSE",
                8 => "S",
                9 => "SSW",
                10 => "SW",
                11 => "WSW",
                12 => "W",
                13 => "WNW",
                14 => "NW",
                15 => "NNW",
                _ => "N",
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyGPSPlus"/> class.
        /// </summary>
        public TinyGPSPlus()
        {
            this._term = new char[_GPS_MAX_FIELD_SIZE];
            this._term[0] = '\0';

            this._parity = 0;
            this._isChecksumTerm = false;
            this._curSentenceType = GpsSentenceIdentifier.OTHER;
            this._curTermNumber = 0;
            this._curTermOffset = 0;
            this._sentenceHasFix = false;

            this.Location = new();
            this.Date = new();
            this.Time = new();
            this.Speed = new();
            this.Course = new();
            this.Altitude = new();
            this.Satellites = new();
            this.Hdop = new();

            this.CharsProcessed = 0;
            this.SentencesWithFix = 0;
            this.FailedChecksum = 0;
            this.PassedChecksum = 0;

            this._firstCustomElt = null;
            this._firstCustomCandidate = null;
        }

        /// <summary>
        /// Feeds NMEA sentence from the GPS module.
        /// </summary>
        /// <param name="s">NMEA sentence.</param>
        /// <returns>Value <c>true</c> when a sentence is complete and valid, <c>false</c> otherwise.</returns>
        public bool Encode(string s)
        {
            foreach (char c in s)
            {
                if (this.Encode(c))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Feeds NMEA characters from the GPS module.
        /// </summary>
        /// <param name="c">Character from an NMEA sentence.</param>
        /// <returns>Value <c>true</c> when a sentence is complete and valid, <c>false</c> otherwise.</returns>
        public bool Encode(char c)
        {
            this.CharsProcessed++;

            switch (c)
            {
                case ',': // term terminators
                case '\r':
                case '\n':
                case '*':

                    if (c == ',')
                    {
                        this._parity ^= (byte)c;
                    }

                    bool isValidSentence = false;

                    if (this._curTermOffset < this._term.Length)
                    {
                        this._term[this._curTermOffset] = '\0';
                        isValidSentence = this.EndOfTermHandler();
                    }

                    ++this._curTermNumber;
                    this._curTermOffset = 0;
                    this._isChecksumTerm = c == '*';

                    return isValidSentence;

                case '$': // sentence begin
                    this._curTermNumber = this._curTermOffset = 0;
                    this._parity = 0;
                    this._curSentenceType = GpsSentenceIdentifier.OTHER;
                    this._isChecksumTerm = false;
                    this._sentenceHasFix = false;

                    return false;

                default: // ordinary characters
                    if (this._curTermOffset < this._term.Length - 1)
                    {
                        this._term[this._curTermOffset++] = c;
                    }

                    if (!this._isChecksumTerm)
                    {
                        this._parity ^= (byte)c;
                    }

                    return false;
            }
        }

        /// <summary>
        /// Add custom NMEA sentence extractor to this instance.
        /// </summary>
        /// <param name="elt"></param>
        public void InsertCustom(TinyGPSCustom elt)
        {
            TinyGPSCustom current;
            TinyGPSCustom previous = this._firstCustomElt;

            for (current = this._firstCustomElt; current != null; current = current.Next)
            {
                int cmp = string.Compare(elt.SentenceName, current.SentenceName);

                if (cmp < 0)
                {
                    break;
                }

                if (cmp == 0 && elt.TermNumber < current.TermNumber)
                {
                    break;
                }

                previous = current;
            }

            elt.Next = current;

            if (this._firstCustomElt == null || current == this._firstCustomElt)
            {
                this._firstCustomElt = elt;
            }
            else if (previous == this._firstCustomElt)
            {
                this._firstCustomElt.Next = elt;
            }
            else
            {
                previous.Next = elt;
            }
        }

        private bool ValidateChecksumAndCommit()
        {
            int checksum = 16 * FromHex(this._term[0]) + FromHex(this._term[1]);

            if (checksum == this._parity)
            {
                this.PassedChecksum++;

                if (this._sentenceHasFix)
                {
                    this.SentencesWithFix++;
                }

                switch (this._curSentenceType)
                {
                    case GpsSentenceIdentifier.GPGGA:
                        this.Time.Commit();

                        if (this._sentenceHasFix)
                        {
                            this.Location.Commit();
                            this.Altitude.Commit();
                        }

                        this.Satellites.Commit();
                        this.Hdop.Commit();

                        break;

                    case GpsSentenceIdentifier.GPRMC:
                        this.Date.Commit();
                        this.Time.Commit();

                        if (this._sentenceHasFix)
                        {
                            this.Location.Commit();
                            this.Speed.Commit();
                            this.Course.Commit();
                        }

                        break;
                }

                // Commit all custom listeners of this sentence type
                for (TinyGPSCustom cc = this._firstCustomCandidate
                    ; cc != null && cc.SentenceName == this._firstCustomCandidate.SentenceName
                    ; cc = cc.Next)
                {
                    cc.Commit();
                }

                return true;
            }
            else
            {
                this.FailedChecksum++;
            }

            return false;
        }

        private bool SetSentenceType(string term)
        {
            if (term == _GPRMCSentenceIdentifier || term == _GNRMCSentenceIdentifier)
            {
                this._curSentenceType = GpsSentenceIdentifier.GPRMC;
            }
            else if (term == _GPGGASentenceIdentifier || term == _GNGGASentenceIdentifier)
            {
                this._curSentenceType = GpsSentenceIdentifier.GPGGA;
            }
            else
            {
                this._curSentenceType = GpsSentenceIdentifier.OTHER;
            }

            // Any custom candidates of this sentence type?
            for (this._firstCustomCandidate = this._firstCustomElt
                ; this._firstCustomCandidate != null && string.Compare(this._firstCustomCandidate.SentenceName, term) < 0
                ; this._firstCustomCandidate = this._firstCustomCandidate.Next) ;

            if (this._firstCustomCandidate != null && string.Compare(this._firstCustomCandidate.SentenceName, term) > 0)
            {
                this._firstCustomCandidate = null;
            }

            return false;
        }

        private void SetStandardValues(string term)
        {
            if (this.Is(GpsSentenceIdentifier.GPRMC, 1) || this.Is(GpsSentenceIdentifier.GPGGA, 1))
            {
                this.Time.Set(term);
            }
            else if (this.Is(GpsSentenceIdentifier.GPRMC, 2))
            {
                // GPRMC validity
                this._sentenceHasFix = this._term[0] == 'A';
            }
            else if (this.Is(GpsSentenceIdentifier.GPRMC, 3) || this.Is(GpsSentenceIdentifier.GPGGA, 2))
            {
                this.Location.Latitude.Set(term);
            }
            else if (this.Is(GpsSentenceIdentifier.GPRMC, 4) || this.Is(GpsSentenceIdentifier.GPGGA, 3))
            {
                // N/S
                this.Location.Latitude.SetSign(this._term[0] == 'S');
            }
            else if (this.Is(GpsSentenceIdentifier.GPRMC, 5) || this.Is(GpsSentenceIdentifier.GPGGA, 4))
            {
                this.Location.Longitude.Set(term);
            }
            else if (this.Is(GpsSentenceIdentifier.GPRMC, 6) || this.Is(GpsSentenceIdentifier.GPGGA, 5))
            {
                // E/W
                this.Location.Longitude.SetSign(this._term[0] == 'W');
            }
            else if (this.Is(GpsSentenceIdentifier.GPRMC, 7))
            {
                this.Speed.Set(term);
            }
            else if (this.Is(GpsSentenceIdentifier.GPRMC, 8))
            {
                this.Course.Set(term);
            }
            else if (this.Is(GpsSentenceIdentifier.GPRMC, 9))
            {
                this.Date.Set(term);
            }
            else if (this.Is(GpsSentenceIdentifier.GPGGA, 6))
            {
                // Fix data
                this._sentenceHasFix = this._term[0] > '0';
            }
            else if (this.Is(GpsSentenceIdentifier.GPGGA, 7))
            {
                this.Satellites.Set(term);
            }
            else if (this.Is(GpsSentenceIdentifier.GPGGA, 8))
            {
                this.Hdop.Set(term);
            }
            else if (this.Is(GpsSentenceIdentifier.GPGGA, 9))
            {
                this.Altitude.Set(term);
            }
        }

        private void SetCustomValues(string term)
        {
            for (TinyGPSCustom cc = this._firstCustomCandidate
                ; cc != null && cc.SentenceName == this._firstCustomCandidate.SentenceName && cc.TermNumber <= this._curTermNumber
                ; cc = cc.Next)
            {
                if (cc.TermNumber == this._curTermNumber)
                {
                    cc.Set(term);
                }
            }
        }

        private bool EndOfTermHandler()
        {
            if (this._isChecksumTerm)
            {
                return this.ValidateChecksumAndCommit();
            }

            string term = new(this._term, 0, this._curTermOffset);

            // The first term determines the sentence type
            if (this._curTermNumber == 0)
            {
                return this.SetSentenceType(term);
            }

            if (this._curSentenceType != GpsSentenceIdentifier.OTHER && this._term[0] != '\0')
            {
                this.SetStandardValues(term);
            }

            this.SetCustomValues(term);

            return false;
        }

        /// <summary>
        /// Checks whether the current sentence type and the current term number are identical to
        /// the ones given as parameters.
        /// </summary>
        /// <param name="sentenceType">Sentence to match</param>
        /// <param name="termNumber">Term number to match</param>
        /// <returns>Value <c>true</c> when it is a match, <c>false</c> when it's not.</returns>
        private bool Is(GpsSentenceIdentifier sentenceType, int termNumber)
        {
            return this._curSentenceType == sentenceType
                && this._curTermNumber == termNumber;
        }

        private static double Radians(float degree)
        {
            return degree * (Math.PI / 180);
        }

        private static double Degrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        private static int FromHex(char a)
        {
            if (a >= 'A' && a <= 'F')
            {
                return a - 'A' + 10;
            }
            else if (a >= 'a' && a <= 'f')
            {
                return a - 'a' + 10;
            }
            else
            {
                return a - '0';
            }
        }
    }
}
