#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Azure
{
    using System;
    using System.IO;

    using Microsoft.ApplicationInsights.WindowsServer.Azure.Emulation;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;

    internal class ServiceRuntimeHelper
    {
        static ServiceRuntimeHelper()
        {
            DeploymentId = Guid.NewGuid().ToString("N");
            RoleName = "MyTestRole_" + Guid.NewGuid().ToString("N");
            RoleInstanceOrdinal = new Random().Next(0, 10);
            IsAvailable = true;

            // Will set up the IPC channel to which our mirror Azure SDK will connect (from the secondary app domain),
            // interception points for the root level methods we're interested in intercepting, and set up a test folder containing AI.DLL, 
            // AI.Platform.DLL, Azure.ServiceRuntime.DLL (mirror) and Azure.ServiceRuntime.DLL.mirrorConfig.
            
            // enable the looking glass
            WindowsAzure.ServiceRuntime.Mirror.LookingGlass.Enabled = true;
            WindowsAzure.ServiceRuntime.Mirror.LookingGlass.EnableRemoteLookingGlass = false;

            // register interceptors for all implemented properties on RoleEnvironment. Child objects are returned as serialized rather than MarshalByRef 
            // and as such don't need to be intercepted.
            WindowsAzure.ServiceRuntime.Mirror.LookingGlass.Register<bool, bool>(
                                    source: typeof(WindowsAzure.ServiceRuntime.RoleEnvironment).GetProperty("IsAvailable"),
                                    handler: b => IsAvailable);

            WindowsAzure.ServiceRuntime.Mirror.LookingGlass.Register<string, string>(
                                    source: typeof(WindowsAzure.ServiceRuntime.RoleEnvironment).GetProperty("DeploymentId"),
                                    handler: s => DeploymentId);

            WindowsAzure.ServiceRuntime.Mirror.LookingGlass.Register<WindowsAzure.ServiceRuntime.RoleInstance, WindowsAzure.ServiceRuntime.RoleInstance>(
                                    source: typeof(WindowsAzure.ServiceRuntime.RoleEnvironment).GetProperty("CurrentRoleInstance"),
                                    handler: r =>
                                    {
                                        TestRole testRole = new TestRole(RoleName);
                                        TestRoleInstance testRoleInstance = new TestRoleInstance(testRole, RoleInstanceOrdinal);
                                        return testRoleInstance;
                                    });

            // create a temp path first.
            TestWithServiceRuntimePath = Path.GetDirectoryName(typeof(ServiceRuntimeTests).Assembly.Location);
        }

        public static string DeploymentId { get; set; }

        public static string RoleName { get; set; }

        public static int RoleInstanceOrdinal { get; set; }

        public static bool IsAvailable { get; set; }

        public static string TestWithServiceRuntimePath { get; set; }
    }
}
#endif