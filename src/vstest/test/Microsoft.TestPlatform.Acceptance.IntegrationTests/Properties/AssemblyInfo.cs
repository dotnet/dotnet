// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

// Enable IAP at class level with as many threads as possible based on CPU and core count.
[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.ClassLevel)]
