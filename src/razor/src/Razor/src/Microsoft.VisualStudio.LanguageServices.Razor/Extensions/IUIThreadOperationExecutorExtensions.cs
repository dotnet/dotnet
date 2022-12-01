// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Extensions;

internal static class IUIThreadOperationExecutorExtensions
{
    public static T? Execute<T>(
        this IUIThreadOperationExecutor iUIThreadOperationExecutor,
        string title,
        string description,
        bool allowCancellation,
        bool showProgress,
        Func<CancellationToken, Task<T>> func,
        JoinableTaskFactory jtf)
    {
        T? obj = default;
        var result = iUIThreadOperationExecutor.Execute(title, description, allowCancellation, showProgress,
            (context) => jtf.Run(async () => obj = await func(context.UserCancellationToken)));

        if (result == UIThreadOperationStatus.Canceled)
        {
            return default;
        }

        return obj;
    }
}
