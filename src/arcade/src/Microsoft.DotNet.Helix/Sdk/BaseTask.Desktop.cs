using Microsoft.Arcade.Common.Desktop;

namespace Microsoft.DotNet.Helix
{
    public partial class BaseTask
    {
        static BaseTask()
        {
            AssemblyResolver.Enable();
        }
    }
}
