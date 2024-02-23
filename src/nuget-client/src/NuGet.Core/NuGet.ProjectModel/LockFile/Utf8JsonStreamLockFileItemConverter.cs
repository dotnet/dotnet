// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.Json;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// A <see cref="IUtf8JsonStreamReaderConverter{T}"/> to allow read JSON into <see cref="LockFileItem"/>
    /// </summary>
    /// <example>
    /// "path/to/the.dll": {
    ///     "property1": "val1",
    ///     "property2": 2
    ///     "property3": true
    ///     "property4": false
    /// }
    /// </example>
    internal class Utf8JsonStreamLockFileItemConverter<T> : IUtf8JsonStreamReaderConverter<T> where T : LockFileItem
    {
        private Func<string, T> _lockFileItemCreator;

        public Utf8JsonStreamLockFileItemConverter(Func<string, T> lockFileItemCreator)
        {
            _lockFileItemCreator = lockFileItemCreator;
        }

        public T Read(ref Utf8JsonStreamReader reader)
        {
            var genericType = typeof(T);

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected PropertyName, found " + reader.TokenType);
            }

            //We want to read the property name right away
            var lockItemPath = reader.GetString();
            LockFileItem lockFileItem = _lockFileItemCreator(lockItemPath);

            reader.Read();
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    lockFileItem.Properties[propertyName] = reader.ReadNextTokenAsString();
                }
            }

            return lockFileItem as T;
        }
    }
}
