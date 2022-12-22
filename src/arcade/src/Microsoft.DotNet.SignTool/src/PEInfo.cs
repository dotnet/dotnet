using System.Runtime.InteropServices;

namespace Microsoft.DotNet.SignTool
{
    internal class PEInfo
    {
        internal bool IsManaged { get; }
        internal bool IsCrossgened { get; }
        internal string Copyright { get; }
        internal string PublicKeyToken { get; }
        internal string TargetFramework { get; }

        public PEInfo(bool isManaged, string copyright) : this(isManaged, false, copyright, null, null) { }

        public PEInfo(bool isManaged, bool isCrossgened, string copyright, string publicKeyToken, string targetFramework)
        {
            IsManaged = isManaged;
            IsCrossgened = isCrossgened;
            Copyright = copyright;
            PublicKeyToken = publicKeyToken;
            TargetFramework = targetFramework;
        }
    }
}
