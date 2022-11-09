// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.VisualStudio
{
    public interface IWritableSettingsStore : ISettingsStore
    {
        void DeleteCollection(string collection);
        void CreateCollection(string collection);
        bool DeleteProperty(string collection, string propertyName);

        void SetBoolean(string collection, string propertyName, bool value);
        void SetInt32(string collection, string propertyName, int value);
        void SetString(string collection, string propertyName, string value);
    }
}
