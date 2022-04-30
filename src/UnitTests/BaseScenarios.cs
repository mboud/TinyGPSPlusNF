namespace UnitTests
{
    using nanoFramework.TestFramework;
    using TinyGPSPlusNF;

    [TestClass]
    public class BaseScenarios
    {
        [TestMethod]
        public void BaseScenarios_EncodeValidSentence_ReturnsTrue()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMC,045103.000,A,3014.1984,N,09749.2872,W,0.67,161.46,030913,,,A");

            TinyGPSPlus gps = new();
            bool encoded = false;

            // Act
            foreach (char c in nmea)
            {
                encoded = gps.Encode(c);

                if (encoded)
                {
                    break;
                }
            }

            // Assert
            Assert.True(encoded);
        }

        [TestMethod]
        public void BaseScenarios_EncodeValidSentence_MissingDataIsInvalid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045104.321,3014.1985,N,09749.2873,W,1,09,1.2,211.6,M,-22.5,M,,0000");

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Date.IsValid);
            Assert.False(gps.Speed.IsValid);
            Assert.False(gps.Course.IsValid);
        }

        [TestMethod]
        public void BaseScenarios_EncodeValidSentence_DataPersistedBetweenSentences()
        {
            // Arrange
            string[] sentences =
            {
                TestHelpers.BuildSentence("GPRMC,045103.123,A,3014.1984,N,09749.2872,W,0.67,161.46,030913,,,A"),
                TestHelpers.BuildSentence("GPGGA,045104.321,3014.1985,N,09749.2873,W,1,09,1.2,211.6,M,-22.5,M,,0000"),
            };

            TinyGPSPlus gps = new();

            // Act
            foreach (string nmea in sentences)
            {
                TestHelpers.Encode(gps, nmea);
            }

            // Assert
            Assert.True(gps.Date.IsValid);
            Assert.True(gps.Speed.IsValid);
            Assert.True(gps.Course.IsValid);
        }

        [TestMethod]
        public void BaseScenarios_EncodeSentenceWithInvalidAndValidData_ValidDataShouldBeReadable()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPGGA,045104.321,3014.1985,N,09749.2873,W,1,09,Z.2,211.6,M,-22.5,M,,0000");
            float expectedAltitudeInMeters = 211.6f;

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Hdop.IsValid);
            Assert.Equal(gps.Hdop.Value, 0);
            Assert.True(gps.Altitude.IsValid);
            Assert.Equal(gps.Altitude.Meters, expectedAltitudeInMeters);
        }
    }
}
