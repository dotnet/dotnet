// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

// Enable IAP at method level with as many threads as possible based on CPU and core count.
// Method level is safe because integration tests offload work to child processes (vstest.console, testhost).
[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
