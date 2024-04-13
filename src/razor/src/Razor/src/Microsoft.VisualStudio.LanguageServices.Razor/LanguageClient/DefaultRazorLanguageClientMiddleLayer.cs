// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

[Export(typeof(RazorLanguageClientMiddleLayer))]
internal class DefaultRazorLanguageClientMiddleLayer : RazorLanguageClientMiddleLayer
{
    public override bool CanHandle(string methodName) => false;

    public override Task HandleNotificationAsync(string methodName, JToken methodParam, Func<JToken, Task> sendNotification)
    {
        return Task.CompletedTask;
    }

    public override Task<JToken?> HandleRequestAsync(string methodName, JToken methodParam, Func<JToken, Task<JToken?>> sendRequest)
    {
        throw new NotImplementedException();
    }
}
