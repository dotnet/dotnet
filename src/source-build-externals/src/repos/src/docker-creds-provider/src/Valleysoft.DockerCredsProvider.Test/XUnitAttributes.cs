namespace Valleysoft.DockerCredsProvider.Test;

using System.Runtime.InteropServices;
using Xunit;

public sealed class WindowsFact : FactAttribute
{
    public WindowsFact() {
        if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            Skip = $"Test only runs on {OSPlatform.Windows}.";
        }
    }
}
