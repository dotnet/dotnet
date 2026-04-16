using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#pragma warning disable CS0436

[assembly: InternalsVisibleTo("Microsoft.ApplicationInsights.Web.Tests, PublicKey=" + AssemblyInfo.PublicKey)]

[assembly: InternalsVisibleTo("Microsoft.ApplicationInsights.DependencyCollector.Tests, PublicKey=" + AssemblyInfo.PublicKey)]

[assembly: InternalsVisibleTo("Microsoft.AI.PerformanceCollector.NetCore.Tests, PublicKey=" + AssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("Microsoft.AI.PerformanceCollector.NetFull.Tests, PublicKey=" + AssemblyInfo.PublicKey)]

[assembly: InternalsVisibleTo("Microsoft.ApplicationInsights.Web.Tests, PublicKey=" + AssemblyInfo.PublicKey)]

[assembly: InternalsVisibleTo("WindowsServer.Tests, PublicKey=" + AssemblyInfo.PublicKey)]

[assembly: InternalsVisibleTo("Microsoft.ApplicationInsights.Operation.Tests, PublicKey=" + AssemblyInfo.PublicKey)]

internal static class AssemblyInfo
{
#if PUBLIC_RELEASE
    // Public key; assemblies are delay signed.
    public const string PublicKey = "0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9";
#else
    // Internal key; assemblies are fully signed.
    public const string PublicKey = "0024000004800000940000000602000000240000525341310004000001000100319b35b21a993df850846602dae9e86d8fbb0528a0ad488ecd6414db798996534381825f94f90d8b16b72a51c4e7e07cf66ff3293c1046c045fafc354cfcc15fc177c748111e4a8c5a34d3940e7f3789dd58a928add6160d6f9cc219680253dcea88a034e7d472de51d4989c7783e19343839273e0e63a43b99ab338149dd59f";
#endif
}

#pragma warning restore CS0436