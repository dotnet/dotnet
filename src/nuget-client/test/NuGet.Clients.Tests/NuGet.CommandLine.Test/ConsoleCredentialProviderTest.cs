using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class ConsoleCredentialProviderTest
    {
        private static readonly Uri Uri = new Uri("http://fake/");

        [Fact]
        public async Task ConsoleCredentialProvider_FailsWhenNonInteractive()
        {
            // Arrange
            var console = new Console();
            var provider = new ConsoleCredentialProvider(console);

            // Act
            var actual = await provider.GetAsync(
                Uri,
                proxy: null,
                type: CredentialRequestType.Proxy,
                message: null,
                isRetry: false,
                nonInteractive: true,
                cancellationToken: CancellationToken.None);

            // Assert
            Assert.Equal(Credentials.CredentialStatus.ProviderNotApplicable, actual.Status);
            Assert.Null(actual.Credentials);
        }
    }
}
