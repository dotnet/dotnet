// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage.MessageInterception;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Export(typeof(IRazorCSharpInterceptionMiddleLayer))]
internal class RazorCSharpInterceptionMiddleLayer : IRazorCSharpInterceptionMiddleLayer
{
    private readonly InterceptionMiddleLayer _underlyingMiddleLayer;

    [ImportingConstructor]
    public RazorCSharpInterceptionMiddleLayer(InterceptorManager interceptorManager)
    {
        _underlyingMiddleLayer = new InterceptionMiddleLayer(interceptorManager, RazorLSPConstants.CSharpContentTypeName);
    }

    public bool CanHandle(string methodName)
        => _underlyingMiddleLayer.CanHandle(methodName);

    public Task HandleNotificationAsync(string methodName, JToken methodParam, Func<JToken, Task> sendNotification)
        => _underlyingMiddleLayer.HandleNotificationAsync(methodName, methodParam, sendNotification);

    public Task<JToken?> HandleRequestAsync(string methodName, JToken methodParam, Func<JToken, Task<JToken?>> sendRequest)
        => _underlyingMiddleLayer.HandleRequestAsync(methodName, methodParam, sendRequest);
}
