// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage.MessageInterception;

[Export(typeof(InterceptorManager))]
internal sealed class DefaultInterceptorManager : InterceptorManager
{
    private readonly IReadOnlyList<Lazy<MessageInterceptor, IInterceptMethodMetadata>> _lazyInterceptors;

    [ImportingConstructor]
    public DefaultInterceptorManager([ImportMany] IEnumerable<Lazy<MessageInterceptor, IInterceptMethodMetadata>> lazyInterceptors)
    {
        _ = lazyInterceptors ?? throw new ArgumentNullException(nameof(lazyInterceptors));
        _lazyInterceptors = lazyInterceptors.ToList().AsReadOnly();
    }

    public override bool HasInterceptor(string methodName, string contentType)
    {
        if (string.IsNullOrEmpty(methodName))
        {
            throw new ArgumentException("Cannot be empty", nameof(methodName));
        }

        foreach (var interceptor in _lazyInterceptors)
        {
            if (interceptor.Metadata.ContentTypes.Any(ct => contentType.Equals(ct, StringComparison.Ordinal)))
            {
                foreach (var method in interceptor.Metadata.InterceptMethods)
                {
                    if (method.Equals(methodName, StringComparison.Ordinal))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public override async Task<JToken?> ProcessInterceptorsAsync(string methodName, JToken message, string contentType, CancellationToken cancellationToken)
    {
        _ = message ?? throw new ArgumentNullException(nameof(message));
        if (string.IsNullOrEmpty(methodName))
        {
            throw new ArgumentException("Cannot be empty", nameof(methodName));
        }

        if (string.IsNullOrEmpty(contentType))
        {
            throw new ArgumentException("Cannot be empty", nameof(contentType));
        }

        for (var i = 0; i < _lazyInterceptors.Count; i++)
        {
            var interceptor = _lazyInterceptors[i];
            if (CanInterceptMessage(methodName, contentType, interceptor.Metadata))
            {
                var result = await interceptor.Value.ApplyChangesAsync(message, contentType, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if (result.UpdatedToken is null)
                {
                    // The interceptor has blocked this message
                    return null;
                }

                message = result.UpdatedToken;

                if (result.ChangedDocumentUri)
                {
                    // If the DocumentUri changes, we need to restart the loop
                    i = -1;
                    continue;
                }
            }
        }

        return message;

        static bool CanInterceptMessage(string methodName, string contentType, IInterceptMethodMetadata metadata)
        {
            var handledMessages = metadata.InterceptMethods;
            var contentTypes = metadata.ContentTypes;

            return handledMessages.Any(m => methodName.Equals(m, StringComparison.Ordinal))
                && contentTypes.Any(ct => contentType.Equals(ct, StringComparison.Ordinal));
        }
    }
}
