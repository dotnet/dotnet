// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.VisualStudio.Razor.IntegrationTests.InProcess;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Extensibility.Testing;

[TestService]
internal partial class TaskListInProcess
{
    public async Task<string[]?> WaitForTasksAsync(int expectedCount, CancellationToken cancellationToken)
    {
        await TestServices.Shell.ExecuteCommandAsync("View.TaskList", cancellationToken);

        return await Helper.RetryAsync(async (cancellationToken) =>
        {
            var items = await GetTaskListItemsAsync(cancellationToken);
            if (items.Length != expectedCount)
            {
                return null;
            }

            return items;

        }, TimeSpan.FromSeconds(1), cancellationToken);
    }

    private async Task<string[]> GetTaskListItemsAsync(CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var taskList = await GetRequiredGlobalServiceAsync<SVsTaskList, IVsTaskList>(cancellationToken);

        taskList.EnumTaskItems(out var taskItemEnum);
        var taskItems = new IVsTaskItem[1];

        using var items = new PooledArrayBuilder<string>();
        while (ErrorHandler.Succeeded(taskItemEnum.Next(1, taskItems, null)))
        {
            var taskItem = taskItems[0];
            if (taskItem == null)
            {
                break;
            }

            taskItem.get_Text(out var text);
            items.Add(text);
        }

        return items.ToArray();
    }
}
