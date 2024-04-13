// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Client;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

#pragma warning disable CS0618 // Type or member is obsolete. Temporary until we remove ILanguageClientMiddleLayer
internal abstract class RazorLanguageClientMiddleLayer : ILanguageClientMiddleLayer
#pragma warning restore CS0618 // Type or member is obsolete
{
    public abstract bool CanHandle(string methodName);

    public abstract Task HandleNotificationAsync(string methodName, JToken methodParam, Func<JToken, Task> sendNotification);

    public abstract Task<JToken?> HandleRequestAsync(string methodName, JToken methodParam, Func<JToken, Task<JToken?>> sendRequest);
}
