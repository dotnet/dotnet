using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuGet.Common.Test
{
    public class MSBuildStringUtilityTests
    {
        [Fact]
        public void GetDistinctNuGetLogCodesOrDefault_SameLogCodes()
        {
            // Arrange
            var logCodes1 = new List<NuGetLogCode>() { NuGetLogCode.NU1000, NuGetLogCode.NU1001 };
            var logCodes2 = new List<NuGetLogCode>() { NuGetLogCode.NU1001, NuGetLogCode.NU1000, };

            var logCodesList = new List<IEnumerable<NuGetLogCode>>() { logCodes1, logCodes2 };

            // Act
            var result = MSBuildStringUtility.GetDistinctNuGetLogCodesOrDefault(logCodesList);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.True(result.All(logCodes2.Contains));
        }

        [Fact]
        public void GetDistinctNuGetLogCodesOrDefault_EmptyLogCodes()
        {
            // Arrange
            var logCodesList = new List<IEnumerable<NuGetLogCode>>();

            // Act
            var result = MSBuildStringUtility.GetDistinctNuGetLogCodesOrDefault(logCodesList);

            // Assert
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetDistinctNuGetLogCodesOrDefault_DiffLogCodes()
        {
            // Arrange
            var logCodes1 = new List<NuGetLogCode>() { NuGetLogCode.NU1000 };
            var logCodes2 = new List<NuGetLogCode>() { NuGetLogCode.NU1001, NuGetLogCode.NU1000 };

            var logCodesList = new List<IEnumerable<NuGetLogCode>>() { logCodes1, logCodes2 };

            // Act
            var result = MSBuildStringUtility.GetDistinctNuGetLogCodesOrDefault(logCodesList);

            // Assert
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetDistinctNuGetLogCodesOrDefault_OneNullCode()
        {
            // Arrange
            var logCodes1 = new List<NuGetLogCode>() { NuGetLogCode.NU1001, NuGetLogCode.NU1000 };

            var logCodesList = new List<IEnumerable<NuGetLogCode>>() { null!, logCodes1 };

            // Act
            var result = MSBuildStringUtility.GetDistinctNuGetLogCodesOrDefault(logCodesList);

            // Assert
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetDistinctNuGetLogCodesOrDefault_AllNullCodes()
        {
            // Arrange
            var logCodesList = new List<IEnumerable<NuGetLogCode>>() { null!, null! };

            // Act
            var result = MSBuildStringUtility.GetDistinctNuGetLogCodesOrDefault(logCodesList);

            // Assert
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetDistinctNuGetLogCodesOrDefault_NullCodesAfterFirst()
        {
            // Arrange
            var logCodes1 = new List<NuGetLogCode>() { NuGetLogCode.NU1001, NuGetLogCode.NU1000 };
            var logCodesList = new List<IEnumerable<NuGetLogCode>>() { logCodes1, null! };

            // Act
            var result = MSBuildStringUtility.GetDistinctNuGetLogCodesOrDefault(logCodesList);

            // Assert
            Assert.Equal(0, result.Count());
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("", null)]
        [InlineData(null, null)]
        public void GetBooleanOrNullTests(string value, bool? expected)
        {
            // Act
            bool? result = MSBuildStringUtility.GetBooleanOrNull(value);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
