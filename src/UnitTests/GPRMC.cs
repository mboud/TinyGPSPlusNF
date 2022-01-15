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
            double expectedSpeedInKnots = 0.67;
            double expectedSpeedInMph = 0.77;
            double expectedSpeedInKph = 1.24;
            double expectedSpeedInMps = 0.34;

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
            double expectedCourse = 161.46;

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
            double expectedLatitude = 30.23664;
            long expectedLatBillionth = 236640000;
            int expectedLatDeg = 30;
            bool expectedLatNeg = false;

            double expectedLongitude = -97.821453;
            long expectedLngBillionth = 821453333;
            int expectedLngDeg = 97;
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

            double expectedLongitude = -97.821453;
            long expectedLngBillionth = 821453333;
            int expectedLngDeg = 97;
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
            double expectedLatitude = 30.23664;
            long expectedLatBillionth = 236640000;
            int expectedLatDeg = 30;
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
            int expectedDateValue = 30913;
            int expectedDay = 3;
            int expectedMonth = 9;
            int expectedYear = 2013;

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
            int expectedRawValue = 4510312;
            int expectedHour = 4;
            int expectedMinute = 51;
            int expectedSecond = 3;
            int expectedCentisecond = 12;

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
