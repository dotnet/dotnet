// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage.MessageInterception;

/// <summary>
/// Contains an updated message token and a signal of whether the document Uri was changed.
/// </summary>
internal struct InterceptionResult
{
    public static readonly InterceptionResult NoChange = new(null, false);

    public InterceptionResult(JToken? newToken, bool changedDocumentUri)
    {
        UpdatedToken = newToken;
        ChangedDocumentUri = changedDocumentUri;
    }

    public JToken? UpdatedToken { get; }
    public bool ChangedDocumentUri { get; }
}
