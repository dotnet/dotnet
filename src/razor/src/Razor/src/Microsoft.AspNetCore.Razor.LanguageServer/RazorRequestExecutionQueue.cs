// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RazorRequestExecutionQueue : RequestExecutionQueue<RazorRequestContext>
{
    private CultureInfo? _cultureInfo;
    private readonly CapabilitiesManager _capabilitiesManager;

    public RazorRequestExecutionQueue(AbstractLanguageServer<RazorRequestContext> languageServer, ILspLogger logger, AbstractHandlerProvider handlerProvider)
        : base(languageServer, logger, handlerProvider)
    {
        _capabilitiesManager = languageServer.GetLspServices().GetRequiredService<CapabilitiesManager>();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD003:Avoid awaiting foreign Tasks", Justification = "<Pending>")]
    public override Task WrapStartRequestTaskAsync(Task nonMutatingRequestTask, bool rethrowExceptions)
    {
        CultureInfo.CurrentUICulture = GetCultureForRequest();

        return nonMutatingRequestTask;
    }

    private CultureInfo GetCultureForRequest()
    {
        // Mostly copied from Roslyn: https://github.com/dotnet/roslyn/blob/6faeaaa5c10472c0ef34c6714659712cd83894b9/src/Features/LanguageServer/Protocol/RoslynRequestExecutionQueue.cs#L45
        if (_cultureInfo is not null)
        {
            return _cultureInfo;
        }

        if (!_capabilitiesManager.HasInitialized)
        {
            // Initialize has not been called yet, no culture to set
            return CultureInfo.CurrentUICulture;
        }

        var initializeParams = _capabilitiesManager.GetInitializeParams();
        var locale = initializeParams.Locale;
        if (string.IsNullOrWhiteSpace(locale))
        {
            // The client did not provide a culture, use the OS configured value
            // and remember that so we can short-circuit from now on.
            _cultureInfo = CultureInfo.CurrentUICulture;
            return _cultureInfo;
        }

        try
        {
            // Parse the LSP locale into a culture and remember it for future requests.
            _cultureInfo = CultureInfo.CreateSpecificCulture(locale);
        }
        catch (CultureNotFoundException)
        {
            // We couldn't parse the culture, log a warning and fallback to the OS configured value.
            // Also remember the fallback so we don't warn on every request.
            _logger.LogWarning($"Culture {locale} was not found, falling back to OS culture");
            _cultureInfo = CultureInfo.CurrentUICulture;
        }

        return _cultureInfo;
    }

    // Internal for testing
    internal new TestAccessor GetTestAccessor()
    {
        return new TestAccessor(this);
    }

    internal new class TestAccessor
    {
        private RazorRequestExecutionQueue _queue;

        public TestAccessor(RazorRequestExecutionQueue queue)
        {
            _queue = queue;
        }

        public CultureInfo? GetCultureInfo()
        {
            return _queue._cultureInfo;
        }
    }
}
