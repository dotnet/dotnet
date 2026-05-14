// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <summary>
/// This interface holds additional values for a request handler when rewriting paths is used.
/// This is in case when --local-path and --remote-path parameters are provided and testhost is running
/// in a remote deployment. This interface is used only to avoid changes to public API of TestRequestHandler.
/// </summary>
internal interface IDeploymentAwareTestRequestHandler
{
    string? LocalPath { get; set; }
    string? RemotePath { get; set; }
}
