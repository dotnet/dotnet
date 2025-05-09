namespace Microsoft.ApplicationInsights.AspNetCore.Tests
{
    using System.IO;

    using Moq;

    using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

    /// <summary>
    /// The type <see cref="IHostingEnvironment"/> was marked Obsolete starting in NetCore3.
    /// Here I'm abstracting it's use into a helper method to simplify some of the tests.
    /// </summary>
    public static class EnvironmentHelper
    {
#pragma warning disable CS0618 // Type or member is obsolete
        /// <summary>
        /// Get an instance of <see cref="IHostingEnvironment"/>.
        /// <see cref="IHostingEnvironment.EnvironmentName"/> is set to "UnitTest".
        /// <see cref="IHostingEnvironment.ContentRootPath"/> is set to <see cref="Directory.GetCurrentDirectory"/>.
        /// </summary>
        /// <returns></returns>
        public static IHostingEnvironment GetIHostingEnvironment()
        {
            var mockEnvironment = new Mock<IHostingEnvironment>();
            mockEnvironment.Setup(x => x.EnvironmentName).Returns("UnitTest");
            mockEnvironment.Setup(x => x.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            return mockEnvironment.Object;
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
