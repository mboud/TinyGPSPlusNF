namespace UnitTests
{
    using nanoFramework.TestFramework;
    using TinyGPSPlusNF;

    [TestClass]
    public class GPGGA
    {
        [TestMethod]
        public void GPGGA_SentenceWithValidLatAndLng_LatAndLngAreValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045104.321,3014.1985,N,09749.2873,W,1,09,1.2,211.6,M,-22.5,M,,0000");
            float expectedLatitude = 30.236641f;
            uint expectedLatBillionth = 236641667;
            ushort expectedLatDeg = 30;
            bool expectedLatNeg = false;

            float expectedLongitude = -97.821455f;
            uint expectedLngBillionth = 821455000;
            ushort expectedLngDeg = 97;
            bool expectedLngNeg = true;

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(gps.Location.IsValid);

            Assert.Equal(gps.Location.Latitude.Degrees, expectedLatitude);
            Assert.Equal(gps.Location.Latitude.Billionths, expectedLatBillionth);
            Assert.Equal(gps.Location.Latitude.HoleDegrees, expectedLatDeg);
            Assert.Equal(gps.Location.Latitude.Negative, expectedLatNeg);

            Assert.Equal(gps.Location.Longitude.Degrees, expectedLongitude);
            Assert.Equal(gps.Location.Longitude.Billionths, expectedLngBillionth);
            Assert.Equal(gps.Location.Longitude.HoleDegrees, expectedLngDeg);
            Assert.Equal(gps.Location.Longitude.Negative, expectedLngNeg);
        }

        [TestMethod]
        public void GPGGA_SentenceWithInvalidLatAndValidLng_LatIsNotValidAndLngIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045104.321,3AZE.1985,N,09749.2873,W,1,09,1.2,211.6,M,-22.5,M,,0000");

            float expectedLongitude = -97.821455f;
            uint expectedLngBillionth = 821455000;
            ushort expectedLngDeg = 97;
            bool expectedLngNeg = true;

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Location.IsValid);
            Assert.False(gps.Location.Latitude.IsValid);

            Assert.True(gps.Location.Longitude.IsValid);
            Assert.Equal(gps.Location.Longitude.Degrees, expectedLongitude);
            Assert.Equal(gps.Location.Longitude.Billionths, expectedLngBillionth);
            Assert.Equal(gps.Location.Longitude.HoleDegrees, expectedLngDeg);
            Assert.Equal(gps.Location.Longitude.Negative, expectedLngNeg);
        }

        [TestMethod]
        public void GPGGA_SentenceWithValidLatAndInvalidLng_LatIsValidAndLngIsNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045104.321,3014.1985,N,09AZE.2873,W,1,09,1.2,211.6,M,-22.5,M,,0000");
            float expectedLatitude = 30.236641f;
            uint expectedLatBillionth = 236641667;
            ushort expectedLatDeg = 30;
            bool expectedLatNeg = false;

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Location.IsValid);
            Assert.False(gps.Location.Longitude.IsValid);

            Assert.True(gps.Location.Latitude.IsValid);
            Assert.Equal(gps.Location.Latitude.Degrees, expectedLatitude);
            Assert.Equal(gps.Location.Latitude.Billionths, expectedLatBillionth);
            Assert.Equal(gps.Location.Latitude.HoleDegrees, expectedLatDeg);
            Assert.Equal(gps.Location.Latitude.Negative, expectedLatNeg);
        }

        [TestMethod]
        public void GPGGA_SentenceWithInvalidLatAndLng_LatAndLngAreNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045104.321,3AZE.1985,N,09RTY.2873,W,1,09,1.2,211.6,M,-22.5,M,,0000");

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Location.IsValid);
            Assert.False(gps.Location.Latitude.IsValid);
            Assert.False(gps.Location.Longitude.IsValid);
        }

        [TestMethod]
        public void GPGGA_SentenceWithValidTime_TimeIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045104.321,3014.1985,N,09749.2873,W,1,09,1.2,211.6,M,-22.5,M,,0000");
            uint expectedRawValue = 4510432;
            byte expectedHour = 4;
            byte expectedMinute = 51;
            byte expectedSecond = 4;
            byte expectedCentisecond = 32;

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(gps.Time.IsValid);
            Assert.Equal(gps.Time.Value, expectedRawValue);
            Assert.Equal(gps.Time.Hour, expectedHour);
            Assert.Equal(gps.Time.Minute, expectedMinute);
            Assert.Equal(gps.Time.Second, expectedSecond);
            Assert.Equal(gps.Time.Centisecond, expectedCentisecond);
        }

        [TestMethod]
        public void GPGGA_SentenceWithInvalidTime_TimeIsNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045Z04.321,3014.1985,N,09749.2873,W,1,09,1.2,211.6,M,-22.5,M,,0000");

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Time.IsValid);
        }

        [TestMethod]
        public void GPGGA_SentenceWithValidHdop_HdopIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045104.321,3014.1985,N,09749.2873,W,1,09,1.2,211.6,M,-22.5,M,,0000");
            float expectedHdopValue = 1.2f;

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(gps.Hdop.IsValid);
            Assert.Equal(gps.Hdop.Value, expectedHdopValue);
        }

        [TestMethod]
        public void GPGGA_SentenceWithInvalidHdop_HdopIsNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045104.321,3014.1985,N,09749.2873,W,1,09,1Z2,211.6,M,-22.5,M,,0000");

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Hdop.IsValid);
        }

        [TestMethod]
        public void GPGGA_SentenceWithValidSatellites_SatellitesCountIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045104.321,3014.1985,N,09749.2873,W,1,09,1.2,211.6,M,-22.5,M,,0000");
            int expectedSatellitesValue = 9;

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(gps.Satellites.IsValid);
            Assert.Equal(gps.Satellites.Value, expectedSatellitesValue);
        }

        [TestMethod]
        public void GPGGA_SentenceWithInvalidSatellitesCount_SatellitesIsNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045104.321,3014.1985,N,09749.2873,W,1,K9,1.2,211.6,M,-22.5,M,,0000");

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Satellites.IsValid);
        }

        [TestMethod]
        public void GPGGA_SentenceWithValidAltitude_AltitudeIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045104.321,3014.1985,N,09749.2873,W,1,09,1.2,211.6,M,-22.5,M,,0000");
            float expectedAltitudeInMeters = 211.6f;
            float expectedAltitudeInKms = 0.21f;
            float expectedAltitudeInFeet = 694.22f;
            float expectedAltitudeInMiles = 0.13f;

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(gps.Altitude.IsValid);
            Assert.Equal(gps.Altitude.Value, expectedAltitudeInMeters);
            Assert.Equal(gps.Altitude.Meters, expectedAltitudeInMeters);
            Assert.Equal(gps.Altitude.Kilometers, expectedAltitudeInKms);
            Assert.Equal(gps.Altitude.Feet, expectedAltitudeInFeet);
            Assert.Equal(gps.Altitude.Miles, expectedAltitudeInMiles);
        }

        [TestMethod]
        public void GPGGA_SentenceWithInvalidAltitude_AltitudeIsNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045104.321,3014.1985,N,09749.2873,W,1,09,1.2,AZE.6,M,-22.5,M,,0000");

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Altitude.IsValid);
        }
    }
}
