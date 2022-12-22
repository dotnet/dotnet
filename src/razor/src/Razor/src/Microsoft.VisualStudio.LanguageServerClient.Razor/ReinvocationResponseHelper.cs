// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal static class ReinvocationResponseHelper
{
    public static bool TryExtractResultOrLog<TResponseType>(
        ReinvocationResponse<TResponseType>? response,
        ILogger logger,
        string fromLanguageServerName,
        [NotNullWhen(true)] out TResponseType? result)
    {
        if (response is null)
        {
            logger.LogInformation("Could not make a request against language server {0}.", fromLanguageServerName);
            result = default;
            return false;
        }

        if (response.Response is null)
        {
            logger.LogInformation("Language server {0} returned a `null` result.", fromLanguageServerName);
            result = default;
            return false;
        }

        result = response.Response;
        return true;
    }
}
