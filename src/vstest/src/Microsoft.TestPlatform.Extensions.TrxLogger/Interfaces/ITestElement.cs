// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

internal interface ITestElement
{
    TestId Id { get; }
    string Name { get; set; }
    string Owner { get; set; }
    string Storage { get; set; }
    string Adapter { get; }
    int Priority { get; set; }
    bool IsRunnable { get; }
    TestExecId ExecutionId { get; set; }
    TestExecId ParentExecutionId { get; set; }
    TestListCategoryId CategoryId { get; set; }
    TestCategoryItemCollection TestCategories { get; }
    WorkItemCollection WorkItems { get; set; }
    TestType TestType { get; }
}
