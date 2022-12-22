// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

internal class ReinvocationResponse<TResponseType>
{
    public ReinvocationResponse(string languageClientName, TResponseType? response)
    {
        LanguageClientName = languageClientName;
        Response = response;
    }

    public string LanguageClientName { get; }

    public TResponseType? Response { get; }
}
