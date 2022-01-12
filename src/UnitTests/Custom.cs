namespace UnitTests
{
    using nanoFramework.TestFramework;
    using TinyGPSPlusNF;

    [TestClass]
    public class Custom
    {
        [TestMethod]
        public void Custom_CustomWithInvalidTermNumber_CustomIsNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMB,A,4.08,L,EGLL,EGLM,5130.02,N,00046.34,W,004.6,213.9,122.9,A");

            TinyGPSPlus gps = new();
            TinyGPSCustom steerDirection = new(gps, "GPRMB", 123);

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(steerDirection.IsValid);
        }

        [TestMethod]
        public void Custom_CustomWithInvalidSentenceName_CustomIsNotValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMB,A,4.08,L,EGLL,EGLM,5130.02,N,00046.34,W,004.6,213.9,122.9,A");

            TinyGPSPlus gps = new();
            TinyGPSCustom steerDirection = new(gps, "AZERTY", 3);

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.False(steerDirection.IsValid);
        }

        [TestMethod]
        public void Custom_SentenceWithValidSteerDirection_SteerDirectionIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMB,A,4.08,L,EGLL,EGLM,5130.02,N,00046.34,W,004.6,213.9,122.9,A");
            string expectedSteerDirection = "L";

            TinyGPSPlus gps = new();
            TinyGPSCustom steerDirection = new(gps, "GPRMB", 3);

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(steerDirection.IsValid);
            Assert.Equal(steerDirection.Value, expectedSteerDirection);
        }

        [TestMethod]
        public void Custom_SentenceWithValidRange_RangeIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMB,A,4.08,L,EGLL,EGLM,5130.02,N,00046.34,W,004.6,213.9,122.9,A");
            string expectedRange = "004.6";

            TinyGPSPlus gps = new();
            TinyGPSCustom range = new(gps, "GPRMB", 10);

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(range.IsValid);
            Assert.Equal(range.Value, expectedRange);
        }

        [TestMethod]
        public void Custom_WhenCustomIsNumeric_SentenceWithValidDecimalRange_RangeIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMB,A,4.08,L,EGLL,EGLM,5130.02,N,00046.34,W,004.6,213.9,122.9,A");
            string expectedRange = "004.6";
            double expectedNumericRange = 4.6;

            TinyGPSPlus gps = new();
            TinyGPSCustom range = new(gps, "GPRMB", 10, true);

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(range.IsValid);
            Assert.Equal(range.Value, expectedRange);

            Assert.True(range.NumericValue.IsValid);
            Assert.Equal(range.NumericValue.Value, expectedNumericRange);
        }

        [TestMethod]
        public void Custom_WhenCustomIsNumeric_SentenceWithValidIntegerRange_RangeIsValid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMB,A,4.08,L,EGLL,EGLM,5130.02,N,00046.34,W,0046,213.9,122.9,A");
            string expectedRange = "0046";
            double expectedNumericRange = 46;

            TinyGPSPlus gps = new();
            TinyGPSCustom range = new(gps, "GPRMB", 10, true);

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(range.IsValid);
            Assert.Equal(range.Value, expectedRange);

            Assert.True(range.NumericValue.IsValid);
            Assert.Equal(range.NumericValue.Value, expectedNumericRange);
        }

        [TestMethod]
        public void Custom_WhenCustomIsNumeric_SentenceWithInvalidRange_StringValueIsValidAndNumericValueIsInvalid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMB,A,4.08,L,EGLL,EGLM,5130.02,N,00046.34,W,AZE.6,213.9,122.9,A");
            string expectedRange = "AZE.6";

            TinyGPSPlus gps = new();
            TinyGPSCustom range = new(gps, "GPRMB", 10, true);

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(range.IsValid);
            Assert.Equal(range.Value, expectedRange);
            Assert.False(range.NumericValue.IsValid);
        }

        [TestMethod]
        public void Custom_WhenUsingTwoCustomsOnTheSameSentenceName_SentenceWithValidValues_BothValuesAreOk()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMB,A,4.08,L,EGLL,EGLM,5130.02,N,00046.34,W,004.6,213.9,122.9,A");
            string expectedSteerDirection = "L";
            string expectedRange = "004.6";

            TinyGPSPlus gps = new();
            TinyGPSCustom steerDirection = new(gps, "GPRMB", 3);
            TinyGPSCustom range = new(gps, "GPRMB", 10);

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(steerDirection.IsValid);
            Assert.Equal(steerDirection.Value, expectedSteerDirection);

            Assert.True(range.IsValid);
            Assert.Equal(range.Value, expectedRange);
        }

        [TestMethod]
        public void Custom_WhenUsingDuplicateCustomsOnTheSameSentenceName_SentenceWithValidValues_BothValuesAreOk()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMB,A,4.08,L,EGLL,EGLM,5130.02,N,00046.34,W,004.6,213.9,122.9,A");
            string expectedSteerDirection = "L";

            TinyGPSPlus gps = new();
            TinyGPSCustom steerDirection = new(gps, "GPRMB", 3);
            TinyGPSCustom steerDirectionDuplicate = new(gps, "GPRMB", 3);

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(steerDirection.IsValid);
            Assert.Equal(steerDirection.Value, expectedSteerDirection);

            Assert.True(steerDirectionDuplicate.IsValid);
            Assert.Equal(steerDirectionDuplicate.Value, expectedSteerDirection);
        }

        [TestMethod]
        public void Custom_WhenUsingOneValidAndOneInvalidCustom_SentenceWithValidValues_OneCustomIsValidAndOneIsInvalid()
        {
            // Arrange
            string nmea = TestHelpers.BuildSentence("GPRMB,A,4.08,L,EGLL,EGLM,5130.02,N,00046.34,W,004.6,213.9,122.9,A");
            string expectedSteerDirection = "L";

            TinyGPSPlus gps = new();
            TinyGPSCustom steerDirection = new(gps, "GPRMB", 3);
            TinyGPSCustom range = new(gps, "AZERTY", 10);

            // Act
            TestHelpers.Encode(gps, nmea);

            // Assert
            Assert.True(steerDirection.IsValid);
            Assert.Equal(steerDirection.Value, expectedSteerDirection);

            Assert.False(range.IsValid);
        }

        [TestMethod]
        public void Custom_WhenUsingTwoCustomsOnDifferentSentenceNames_SentencesWithValidValues_BothValuesAreOk()
        {
            // Arrange
            string[] sentences =
            {
                TestHelpers.BuildSentence("GPRMB,A,4.08,L,EGLL,EGLM,5130.02,N,00046.34,W,004.6,213.9,122.9,A"),
                TestHelpers.BuildSentence("PGRME,15.0,M,45.0,M,25.0,M"),
            };

            string expectedSteerDirection = "L";
            double expectedHorizontalPositionError = 15;

            TinyGPSPlus gps = new();
            TinyGPSCustom steerDirection = new(gps, "GPRMB", 3);
            TinyGPSCustom horizontalPositionError = new(gps, "PGRME", 1, true);

            // Act
            foreach (string nmea in sentences)
            {
                TestHelpers.Encode(gps, nmea);
            }

            // Assert
            Assert.True(steerDirection.IsValid);
            Assert.Equal(steerDirection.Value, expectedSteerDirection);

            Assert.True(horizontalPositionError.NumericValue.IsValid);
            Assert.Equal(horizontalPositionError.NumericValue.Value, expectedHorizontalPositionError);
        }
    }
}
