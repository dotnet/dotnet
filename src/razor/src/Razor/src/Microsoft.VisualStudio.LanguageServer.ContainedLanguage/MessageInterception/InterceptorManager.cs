// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage.MessageInterception;

public abstract class InterceptorManager
{
    /// <summary>
    /// Returns whether there is an interceptor available for the given message name.
    /// </summary>
    public abstract bool HasInterceptor(string messageName, string contentType);

    /// <summary>
    /// Takes a message token and returns it with any transforms applied.  To block the message completely, return null.
    /// </summary>
    /// <param name="methodName">The LSP method being intercepted</param>
    /// <param name="message">The LSP message payload</param>
    /// <param name="contentType">The content type name of the contained language where the message originated</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The message token with any applicable modifications, or null to block the message.</returns>
    public abstract Task<JToken?> ProcessInterceptorsAsync(string methodName, JToken message, string contentType, CancellationToken cancellationToken);
}
