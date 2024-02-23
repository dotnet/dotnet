// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
namespace NuGet.ProjectModel
{
    /// <summary>
    /// An abstract class that defines a function for reading a <see cref="Utf8JsonStreamReader"/> into a <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IUtf8JsonStreamReaderConverter<T>
    {
        T Read(ref Utf8JsonStreamReader reader);
    }
}
