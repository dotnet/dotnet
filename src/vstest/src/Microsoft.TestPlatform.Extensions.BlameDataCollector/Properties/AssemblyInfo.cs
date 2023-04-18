// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.Extensions.BlameDataCollector;
using Microsoft.VisualStudio.TestPlatform;

[assembly: TestExtensionTypes(typeof(BlameLogger), typeof(BlameCollector))]
