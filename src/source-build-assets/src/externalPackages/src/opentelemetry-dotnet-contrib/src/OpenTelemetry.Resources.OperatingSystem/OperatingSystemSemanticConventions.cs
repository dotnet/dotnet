// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Resources.OperatingSystem;

internal static class OperatingSystemSemanticConventions
{
    public const string AttributeOperatingSystemType = "os.type";
    public const string AttributeOperatingSystemBuildId = "os.build_id";
    public const string AttributeOperatingSystemDescription = "os.description";
    public const string AttributeOperatingSystemName = "os.name";
    public const string AttributeOperatingSystemVersion = "os.version";

    public static class OperatingSystemsValues
    {
        public const string Windows = "windows";

        public const string Linux = "linux";

        public const string Darwin = "darwin";
    }
}
