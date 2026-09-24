// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.Copilot;
using NuGetVSExtension;
using Xunit;

namespace NuGet.Tools.Test
{
    public class CopilotToolSessionTests
    {
        [Fact]
        public void EnsureSuccessfulHarnessResponse_WithSuccess_DoesNotThrow()
        {
            var response = new CopilotMessageResponse
            {
                Content = string.Empty,
                Model = string.Empty,
                Status = CopilotResponseStatus.Success,
            };

            CopilotToolSession.EnsureSuccessfulHarnessResponse(response);
        }

        [Fact]
        public void EnsureSuccessfulHarnessResponse_WithFailure_ThrowsTypedExceptionWithStatusAndError()
        {
            var response = new CopilotMessageResponse
            {
                Content = string.Empty,
                Model = string.Empty,
                Status = CopilotResponseStatus.QuotaExceeded,
                Error = new CopilotMessageError
                {
                    Message = "Quota has been exhausted.",
                },
            };

            CopilotRequestException exception = Assert.Throws<CopilotRequestException>(
                () => CopilotToolSession.EnsureSuccessfulHarnessResponse(response));

            Assert.Equal(CopilotResponseStatus.QuotaExceeded, exception.Status);
            Assert.Contains(nameof(CopilotResponseStatus.QuotaExceeded), exception.Message);
            Assert.Contains("Quota has been exhausted.", exception.Message);
        }

        [Fact]
        public void EnsureSuccessfulHarnessResponse_WithoutChatAccess_ThrowsUnauthorizedAccessException()
        {
            var response = new CopilotMessageResponse
            {
                Content = string.Empty,
                Model = string.Empty,
                Status = CopilotResponseStatus.UserHasNoChatAccess,
                Error = new CopilotMessageError
                {
                    Message = "Chat access is unavailable.",
                },
            };

            UnauthorizedAccessException exception = Assert.Throws<UnauthorizedAccessException>(
                () => CopilotToolSession.EnsureSuccessfulHarnessResponse(response));

            Assert.Equal("Chat access is unavailable.", exception.Message);
        }
    }
}
