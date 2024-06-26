// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage.MessageInterception;

/// <summary>
/// Intercepts an LSP message and applies changes to the payload.
/// </summary>
public abstract class GenericMessageInterceptor
{
    /// <summary>
    /// Applies changes to the message token, and signals if the document path has been changed.
    /// </summary>
    /// <typeparam name="TJsonToken">The type of the token used by the Json serialization engine (JToken or JsonElement normally)</typeparam>
    /// <param name="message">The message payload</param>
    /// <param name="containedLanguageName">The name of the content type for the contained language.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public abstract Task<GenericInterceptionResult<TJsonToken>> ApplyChangesAsync<TJsonToken>(TJsonToken message, string containedLanguageName, CancellationToken cancellationToken);
}
