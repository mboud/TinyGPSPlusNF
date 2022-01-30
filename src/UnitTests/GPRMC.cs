namespace UnitTests
{
    using nanoFramework.TestFramework;
    using TinyGPSPlusNF;

    [TestClass]
    public class GPRMC
    {
        [TestMethod]
        public void GPRMC_SentenceWithValidSpeed_SpeedIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMC,045103.000,A,3014.1984,N,09749.2872,W,0.67,161.46,030913,,,A");
            float expectedSpeedInKnots = 0.67f;
            float expectedSpeedInMph = 0.77f;
            float expectedSpeedInKph = 1.24f;
            float expectedSpeedInMps = 0.34f;

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(gps.Speed.IsValid);
            Assert.Equal(gps.Speed.Value, expectedSpeedInKnots);
            Assert.Equal(gps.Speed.Knots, expectedSpeedInKnots);
            Assert.Equal(gps.Speed.Mph, expectedSpeedInMph);
            Assert.Equal(gps.Speed.Kmph, expectedSpeedInKph);
            Assert.Equal(gps.Speed.Mps, expectedSpeedInMps);
        }

        [TestMethod]
        public void GPRMC_SentenceWithInvalidSpeed_SpeedIsNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMC,045103.000,A,3014.1984,N,09749.2872,W,A.ZE,161.46,030913,,,A");

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Speed.IsValid);
        }

        [TestMethod]
        public void GPRMC_SentenceWithValidCourse_CourseIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMC,045103.000,A,3014.1984,N,09749.2872,W,0.67,161.46,030913,,,A");
            float expectedCourse = 161.46f;

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(gps.Course.IsValid);
            Assert.Equal(gps.Course.Degrees, expectedCourse);
            Assert.Equal(gps.Course.Value, expectedCourse);
        }

        [TestMethod]
        public void GPRMC_SentenceWithInvalidCourse_CourseIsNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMC,045103.000,A,3014.1984,N,09749.2872,W,0.67,AZE.46,030913,,,A");

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Course.IsValid);
        }

        [TestMethod]
        public void GPRMC_SentenceWithValidLatAndLng_LatAndLngAreValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMC,045103.000,A,3014.1984,N,09749.2872,W,0.67,161.46,030913,,,A");
            float expectedLatitude = 30.23664f;
            uint expectedLatBillionth = 236640000;
            ushort expectedLatDeg = 30;
            bool expectedLatNeg = false;

            float expectedLongitude = -97.821453f;
            uint expectedLngBillionth = 821453333;
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
        public void GPRMC_SentenceWithInvalidLatAndValidLng_LatIsNotValidAndLngIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMC,045103.000,A,3AZE.1984,N,09749.2872,W,0.67,161.46,030913,,,A");

            float expectedLongitude = -97.821453f;
            uint expectedLngBillionth = 821453333;
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
        public void GPRMC_SentenceWithValidLatAndInvalidLng_LatIsValidAndLngIsNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMC,045103.000,A,3014.1984,N,09AZE.2872,W,0.67,161.46,030913,,,A");
            float expectedLatitude = 30.23664f;
            uint expectedLatBillionth = 236640000;
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
        public void GPRMC_SentenceWithInvalidLatAndLng_LatAndLngAreNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMC,045103.000,A,3AZE.1984,N,09RTY.2872,W,0.67,161.46,030913,,,A");

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Location.IsValid);
            Assert.False(gps.Location.IsValid);
            Assert.False(gps.Location.IsValid);
        }

        [TestMethod]
        public void GPRMC_SentenceWithValidDate_DateIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMC,045103.000,A,3014.1984,N,09749.2872,W,0.67,161.46,030913,,,A");
            uint expectedDateValue = 30913;
            byte expectedDay = 3;
            byte expectedMonth = 9;
            ushort expectedYear = 2013;

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(gps.Date.IsValid);
            Assert.Equal(gps.Date.Value, expectedDateValue);
            Assert.Equal(gps.Date.Day, expectedDay);
            Assert.Equal(gps.Date.Month, expectedMonth);
            Assert.Equal(gps.Date.Year, expectedYear);
        }

        [TestMethod]
        public void GPRMC_SentenceWithInvalidDate_DateIsNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMC,045103.000,A,3014.1984,N,09749.2872,W,0.67,161.46,030AZE,,,A");

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Date.IsValid);
        }

        [TestMethod]
        public void GPRMC_SentenceWithValidTime_TimeIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMC,045103.123,A,3014.1984,N,09749.2872,W,0.67,161.46,030913,,,A");
            uint expectedRawValue = 4510312;
            byte expectedHour = 4;
            byte expectedMinute = 51;
            byte expectedSecond = 3;
            byte expectedCentisecond = 12;

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
        public void GPRMC_SentenceWithInvalidTime_TimeIsNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMC,045Z03.123,A,3014.1984,N,09749.2872,W,0.67,161.46,030913,,,A");

            TinyGPSPlus gps = new();

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(gps.Time.IsValid);
        }
    }
}
