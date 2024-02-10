// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Configuration.Test
{
    internal static class TestConfigurationDefaults
    {
        public static ConfigurationDefaults NullInstance { get; } = new ConfigurationDefaults(NullSettings.Instance);
    }
}
