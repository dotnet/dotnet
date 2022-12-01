// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Client;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

internal struct ReinvokeResponse<TOut>
{
    public ILanguageClient LanguageClient { get; }

    public TOut Result { get; }

    public bool IsSuccess => LanguageClient != default;

    public ReinvokeResponse(
        ILanguageClient languageClient,
        TOut result)
    {
        LanguageClient = languageClient;
        Result = result;
    }
}
