// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage.MessageInterception;

/// <summary>
/// Contains an updated message token and a signal of whether the document Uri was changed.
/// </summary>
public struct GenericInterceptionResult<TJsonToken>
{
    public static readonly GenericInterceptionResult<TJsonToken> NoChange = new(default, false);

    public GenericInterceptionResult(TJsonToken? newToken, bool changedDocumentUri)
    {
        UpdatedToken = newToken;
        ChangedDocumentUri = changedDocumentUri;
    }

    public TJsonToken? UpdatedToken { get; }
    public bool ChangedDocumentUri { get; }
}
