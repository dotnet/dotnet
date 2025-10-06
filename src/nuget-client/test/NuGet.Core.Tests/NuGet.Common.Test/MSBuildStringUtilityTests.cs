using Xunit;

namespace NuGet.Common.Test
{
    public class MSBuildStringUtilityTests
    {
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
