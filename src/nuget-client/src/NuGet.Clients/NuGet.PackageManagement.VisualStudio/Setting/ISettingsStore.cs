// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.VisualStudio
{
    public interface ISettingsStore
    {
        bool CollectionExists(string collection);
        bool GetBoolean(string collection, string propertyName, bool defaultValue);
        int GetInt32(string collection, string propertyName, int defaultValue);
        string GetString(string collection, string propertyName, string defaultValue);
    }
}
