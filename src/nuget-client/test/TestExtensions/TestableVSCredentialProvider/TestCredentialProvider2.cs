using System.ComponentModel.Composition;
using NuGet.VisualStudio;

namespace NuGet.Test.TestExtensions.TestableVSCredentialProvider
{

    [Export(typeof(IVsCredentialProvider))]
    public sealed class TestCredentialProvider2
        : TestCredentialProvider
    {
    }
}
