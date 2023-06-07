// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Language;

public abstract class BoundAttributeParameterDescriptorBuilder
{
    public abstract string Name { get; set; }

    public abstract string TypeName { get; set; }

    public abstract bool IsEnum { get; set; }

    public abstract string Documentation { get; set; }

    public abstract string DisplayName { get; set; }

    public abstract IDictionary<string, string> Metadata { get; }

    public abstract RazorDiagnosticCollection Diagnostics { get; }

#nullable enable

    internal virtual void SetDocumentation(string? text)
    {
        throw new NotImplementedException();
    }

    internal virtual void SetDocumentation(DocumentationDescriptor? documentation)
    {
        throw new NotImplementedException();
    }

    public virtual void SetMetadata(MetadataCollection metadata)
    {
        throw new NotImplementedException();
    }

    public virtual bool TryGetMetadataValue(string key, [NotNullWhen(true)] out string? value)
    {
        throw new NotImplementedException();
    }
}
