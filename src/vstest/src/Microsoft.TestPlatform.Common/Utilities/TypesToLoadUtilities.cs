// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.Common.Utilities;

internal static class TypesToLoadUtilities
{
    public const string TypesToLoadAttributeFullName = "Microsoft.VisualStudio.TestPlatform.TestExtensionTypesAttribute";

    internal static IEnumerable<Type> GetTypesToLoad(Assembly assembly)
    {
        ValidateArg.NotNull(assembly, nameof(assembly));
        var typesToLoad = assembly
            .GetCustomAttributes(TypesToLoadAttributeFullName)
            .SelectMany(i => GetTypesToLoad(i));

        return typesToLoad;
    }

    private static IEnumerable<Type> GetTypesToLoad(Attribute attribute)
    {
        if (attribute == null)
            return Enumerable.Empty<Type>();

        var type = attribute.GetType();
        var typesProperty = type.GetProperty("Types");

        return typesProperty?.GetValue(attribute) as Type[] ?? Enumerable.Empty<Type>();
    }
}
