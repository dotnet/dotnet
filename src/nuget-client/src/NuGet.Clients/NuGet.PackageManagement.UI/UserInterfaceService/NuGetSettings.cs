// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// The user settings that are persisted in suo file
    /// </summary>
    [Serializable]
    internal class NuGetSettings
    {
        public Dictionary<string, UserSettings> WindowSettings { get; private set; }

        public NuGetSettings()
        {
            WindowSettings = new Dictionary<string, UserSettings>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
