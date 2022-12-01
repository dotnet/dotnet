// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;

namespace Microsoft.VisualStudio.Extensibility.Testing;

internal partial class EditorInProcess
{
    public async Task DismissCompletionSessionsAsync(CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var view = await GetActiveTextViewAsync(cancellationToken);

        var asyncBroker = await GetComponentModelServiceAsync<IAsyncCompletionBroker>(cancellationToken);
        var session = asyncBroker.GetSession(view);
        if (session is not null && !session.IsDismissed)
        {
            session.Dismiss();
        }
    }
}
