// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// A <see cref="IUtf8JsonStreamReaderConverter{T}"/> to allow read JSON into <see cref="LockFileTarget"/>
    /// </summary>
    /// <example>
    /// "net45/win8": {
    ///     <see cref="Utf8JsonStreamLockFileTargetLibraryConverter"/>,
    /// }
    /// </example>
    internal class Utf8JsonStreamLockFileTargetConverterV4 : IUtf8JsonStreamReaderConverter<LockFileTarget>
    {
        public LockFileTarget Read(ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected PropertyName, found " + reader.TokenType);
            }

            //We want to read the property name right away
            var propertyName = reader.GetString();
            var (targetFramework, runTimeFramework) = propertyName.SplitInTwo(LockFile.DirectorySeparatorChar);

            var lockFileTarget = new LockFileTarget
            {
                TargetAlias = targetFramework,
                RuntimeIdentifier = runTimeFramework,
                Name = string.IsNullOrEmpty(runTimeFramework) ? targetFramework : $"{targetFramework}/{runTimeFramework}"
            };

            reader.Read();
            lockFileTarget.Libraries = reader.ReadObjectAsList(Utf8JsonStreamLockFileConverters.LockFileTargetLibraryConverter);

            return lockFileTarget;
        }
    }
}
