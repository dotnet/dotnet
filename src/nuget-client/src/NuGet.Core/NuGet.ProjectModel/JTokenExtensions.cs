// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NuGet.ProjectModel
{
    [Obsolete("This class is unused and will be removed in a future release.")]
    public static class JTokenExtensions
    {
        public static T?[] ValueAsArray<T>(this JToken jToken)
        {
            return jToken.Select(a => a.Value<T>()).ToArray();
        }

        public static T?[]? ValueAsArray<T>(this JToken jToken, string name)
        {
            return jToken?[name]?.ValueAsArray<T>();
        }

        public static T? GetValue<T>(this JToken token, string name)
        {
            if (token == null)
            {
                return default(T);
            }

            var obj = token[name];

            if (obj == null)
            {
                return default(T);
            }

            return obj.Value<T>();
        }
    }
}
