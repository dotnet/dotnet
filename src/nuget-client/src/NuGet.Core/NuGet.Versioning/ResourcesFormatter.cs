// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Versioning
{
    /// <summary>Strings that contain {x} arguments might throw exceptions in string.Format when the number of
    /// arguments change, but the args to string.Format do not. By putting all calls to a particular resource in
    /// a C# class, we can use the compiler to check that all references pass the correct number of arguments.
    /// </summary>
    internal static class ResourcesFormatter
    {
        internal static ArgumentException TypeNotSupported(Type type, string paramName)
        {
            return new ArgumentException(
                message: string.Format(Resources.TypeNotSupported, type.FullName),
                paramName: paramName);
        }

        internal static ArgumentNullException CannotBeNullWhenParameterIsNull(string parameterThatIsNull, string parameterThisIsNotNull)
        {
            return new ArgumentNullException(
                message: string.Format(Resources.CannotBeNullWhenParameterIsNotNull, parameterThatIsNull, parameterThisIsNotNull),
                paramName: parameterThatIsNull);
        }
    }
}
