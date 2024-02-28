// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

namespace Microsoft.AspNetCore.Razor.Language;

public static class TagHelperMetadata
{
    public static class Common
    {
        public static readonly string PropertyName = "Common.PropertyName";

        public static readonly string TypeName = "Common.TypeName";

        public static readonly string TypeNamespace = "Common.TypeNamespace";

        public static readonly string TypeNameIdentifier = "Common.TypeNameIdentifier";

        public static readonly string GloballyQualifiedTypeName = "Common.GloballyQualifiedTypeName";

        public static readonly string ClassifyAttributesOnly = "Common.ClassifyAttributesOnly";
    }

    public static class Runtime
    {
        public static readonly string Name = "Runtime.Name";
    }
}
