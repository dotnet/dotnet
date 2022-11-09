using System;
using NuGet.VisualStudio.Implementation.Extensibility;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test.Extensibility
{
    public class VsSemanticVersionComparerTests
    {
        [Fact]
        public void VsSemanticVersionComparer_CompareGreaterThan()
        {
            // Arrange
            var target = new VsSemanticVersionComparer();
            var a = "3.1.0-beta-001";
            var b = "2.9.0.0";

            // Act
            var actual = target.Compare(a, b);

            // Assert
            Assert.True(actual > 0, $"{actual} should be greater than zero.");
        }

        [Fact]
        public void VsSemanticVersionComparer_CompareLessThan()
        {
            // Arrange
            var target = new VsSemanticVersionComparer();
            var a = "2.9.0.0";
            var b = "3.1.0-beta-001";

            // Act
            var actual = target.Compare(a, b);

            // Assert
            Assert.True(actual < 0, $"{actual} should be less than zero.");
        }

        [Fact]
        public void VsSemanticVersionComparer_CompareEquivalent()
        {
            // Arrange
            var target = new VsSemanticVersionComparer();
            var a = "2.9.0.0";
            var b = "2.9.0";

            // Act
            var actual = target.Compare(a, b);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public void VsSemanticVersionComparer_CompareEqual()
        {
            // Arrange
            var target = new VsSemanticVersionComparer();
            var a = "2.9.0.0";

            // Act
            var actual = target.Compare(a, a);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public void VsSemanticVersionComparer_InvalidA()
        {
            // Arrange
            var target = new VsSemanticVersionComparer();
            var a = "a.b";
            var b = "2.9.0.0";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => target.Compare(a, b));
        }

        [Fact]
        public void VsSemanticVersionComparer_InvalidB()
        {
            // Arrange
            var target = new VsSemanticVersionComparer();
            var a = "2.9.0.0";
            var b = "a.b";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => target.Compare(a, b));
        }

        [Fact]
        public void VsSemanticVersionComparer_NullA()
        {
            // Arrange
            var target = new VsSemanticVersionComparer();
            string a = null;
            var b = "2.9.0.0";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => target.Compare(a, b));
        }

        [Fact]
        public void VsSemanticVersionComparer_NullB()
        {
            // Arrange
            var target = new VsSemanticVersionComparer();
            var a = "2.9.0.0";
            string b = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => target.Compare(a, b));
        }
    }
}
