namespace Microsoft.ApplicationInsights.Tests
{
    using System;
#if NETCOREAPP
    using System.Reflection;
#endif
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
#if !NETCOREAPP
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    /// <summary>
    /// Part of the <see cref="QuickPulseTestHelper"/> class for all .NET Frameworks.
    /// </summary>
    internal static class QuickPulseTestHelper
    {
        public static void ClearEnvironment()
        {
            SetPrivateStaticField(typeof(TelemetryConfiguration), "active", null);
            TelemetryModules.Instance.Modules.Clear();
        }

        private static void SetPrivateStaticField(Type type, string fieldName, object value)
        {
#if NETCOREAPP
            TypeInfo typeInfo = type.GetTypeInfo();
            FieldInfo field = typeInfo.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            field.SetValue(null, value);
#else
            PrivateType t = new PrivateType(type);
            t.SetStaticField(fieldName, value);
#endif
        }
    }
}