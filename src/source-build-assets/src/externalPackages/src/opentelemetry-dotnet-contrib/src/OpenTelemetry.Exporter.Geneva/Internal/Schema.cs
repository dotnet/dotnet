// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.Geneva;

internal static class Schema
{
    internal static class V21
    {
        internal static class PartA
        {
            internal const string IKey = ".iKey";
            internal const string Name = ".name";
            internal const string Ver = ".ver";
            internal const string Time = ".time";
            internal const string Cv = ".cv";
            internal const string Epoch = ".epoch";
            internal const string Flags = ".flags";
            internal const string PopSample = ".popSample";
            internal const string SeqNum = ".seqNum";

            internal static class Extensions
            {
                internal static class App
                {
                    internal const string Id = "app.id";
                    internal const string Ver = "app.ver";
                }

                internal static class Cloud
                {
                    internal const string Environment = "cloud.environment";
                    internal const string Location = "cloud.location";
                    internal const string Name = "cloud.name";
                    internal const string DeploymentUnit = "cloud.deploymentUnit";
                    internal const string Role = "cloud.role";
                    internal const string RoleInstance = "cloud.roleInstance";
                    internal const string RoleVer = "cloud.roleVer";
                    internal const string Ver = "cloud.ver";
                }

                internal static class Os
                {
                    internal const string Name = "os.name";
                    internal const string Ver = "os.ver";
                }
            }
        }
    }

    internal static class V40
    {
        internal static class PartA
        {
            internal const string IKey = ".iKey";
            internal const string Name = ".name";
            internal const string Ver = ".ver";
            internal const string Time = ".time";

            internal static class Extensions
            {
                internal static class App
                {
                    internal const string Id = "app.id";
                    internal const string Ver = "app.ver";
                }

                internal static class Cloud
                {
                    internal const string Role = "cloud.role";
                    internal const string RoleInstance = "cloud.roleInstance";
                    internal const string RoleVer = "cloud.roleVer";
                }

                internal static class Os
                {
                    internal const string Name = "os.name";
                    internal const string Ver = "os.ver";
                }

                internal static class Dt
                {
                    internal const string TraceId = "dt.traceId";
                    internal const string SpanId = "dt.spanId";
                }
            }
        }
    }
}
