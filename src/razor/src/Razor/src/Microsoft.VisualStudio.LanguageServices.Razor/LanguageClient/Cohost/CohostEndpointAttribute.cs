// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal class CohostEndpointAttribute : RazorMethodAttribute
{
    public CohostEndpointAttribute(string method)
        : base(method, Constants.RazorLanguageName)
    {
    }
}
