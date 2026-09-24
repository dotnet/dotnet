// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Copilot;

namespace NuGetVSExtension
{
    /// <summary>
    /// Holds an active Copilot thread and the available MCP functions.
    /// Disposing this session releases all underlying Copilot/MCP resources.
    /// </summary>
    /// <remarks>
    /// If a future caller needs to keep this <see cref="Thread"/> (or the underlying
    /// <see cref="ICopilotService"/> proxy) alive for a long time, subscribe to
    /// <see cref="Microsoft.ServiceHub.Framework.IServiceBroker.AvailabilityChanged"/> (filtered
    /// to <c>CopilotDescriptors.CopilotService.Moniker</c>) and dispose this session in response.
    /// </remarks>
    internal sealed class CopilotToolSession : IAsyncDisposable
    {
        private readonly ICopilotService _copilotService;
        private readonly IDisposable? _copilotServiceDisposable;
        private readonly CopilotSessionId? _harnessSessionId;

        private CopilotToolSession(
            ICopilotService copilotService,
            CopilotThread thread,
            IReadOnlyList<CopilotFunctionDescriptor> functions,
            IDisposable? copilotServiceDisposable)
        {
            _copilotService = copilotService;
            Thread = thread;
            Functions = functions;
            _copilotServiceDisposable = copilotServiceDisposable;
        }

        private CopilotToolSession(
            ICopilotService copilotService,
            CopilotSessionId harnessSessionId,
            IDisposable? copilotServiceDisposable)
        {
            _copilotService = copilotService;
            _harnessSessionId = harnessSessionId;
            Functions = Array.Empty<CopilotFunctionDescriptor>();
            _copilotServiceDisposable = copilotServiceDisposable;
        }

        public CopilotThread? Thread { get; }

        public IReadOnlyList<CopilotFunctionDescriptor> Functions { get; }

        internal static CopilotToolSession CreateLegacy(
            ICopilotService copilotService,
            CopilotThread thread,
            IReadOnlyList<CopilotFunctionDescriptor> functions)
        {
            return new CopilotToolSession(copilotService, thread, functions, copilotService as IDisposable);
        }

        internal static CopilotToolSession CreateHarness(
            ICopilotService copilotService,
            CopilotSessionId harnessSessionId)
        {
            return new CopilotToolSession(copilotService, harnessSessionId, copilotService as IDisposable);
        }

        internal async Task SendRequestAsync(
            CopilotRequest legacyRequest,
            CopilotUserMessage harnessRequest,
            CancellationToken cancellationToken)
        {
            if (_harnessSessionId is CopilotSessionId harnessSessionId)
            {
#pragma warning disable VSCOPILOT_BACKEND // Experimental SDK harness contracts.
                CopilotMessageResponse response = await _copilotService.SendRequestAsync(harnessSessionId, harnessRequest, cancellationToken);
                EnsureSuccessfulHarnessResponse(response);
#pragma warning restore VSCOPILOT_BACKEND
                return;
            }

            _ = await Thread!.Session.SendRequestAsync(legacyRequest, cancellationToken);
        }

        internal static void EnsureSuccessfulHarnessResponse(CopilotMessageResponse response)
        {
            if (response.Status == CopilotResponseStatus.Success)
            {
                return;
            }

            string? errorMessage = response.Error?.Message;
            if (response.Status == CopilotResponseStatus.UserHasNoChatAccess)
            {
                throw new UnauthorizedAccessException(errorMessage);
            }

            throw new CopilotRequestException(response.Status, errorMessage);
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (_harnessSessionId is null && Thread is not null)
                {
                    await Thread.DisposeAsync();
                }
            }
            finally
            {
                _copilotServiceDisposable?.Dispose();
            }
        }
    }

    internal sealed class CopilotRequestException : Exception
    {
        internal CopilotRequestException(CopilotResponseStatus status, string? errorMessage)
            : base(CreateMessage(status, errorMessage))
        {
            Status = status;
        }

        internal CopilotResponseStatus Status { get; }

        private static string CreateMessage(CopilotResponseStatus status, string? errorMessage)
        {
            string message = $"Copilot request failed with status '{status}'.";
            return string.IsNullOrEmpty(errorMessage) ? message : $"{message} {errorMessage}";
        }
    }
}
