// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Intent;

public interface IRunLogger
{
    void WriteTestPassed(MethodInfo m, TimeSpan t);
    void WriteTestFailure(MethodInfo m, Exception ex, TimeSpan t);
    void WriteFrameworkError(Exception ex);
    void WriteSummary(int passed, List<(MethodInfo method, Exception exception, TimeSpan time)> failures, TimeSpan duration);
}
