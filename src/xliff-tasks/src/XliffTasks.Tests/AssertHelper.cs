using Xunit;

namespace XliffTasks.Tests
{
    static class AssertEx
    {
        public static void EqualIgnoringLineEndings(string expected, string actual)
        {
            Assert.Equal(
                expected.Replace("\r", string.Empty),
                actual.Replace("\r", string.Empty));
        }
    }
}
