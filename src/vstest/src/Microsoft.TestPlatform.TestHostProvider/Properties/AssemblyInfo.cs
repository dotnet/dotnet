// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting;

[assembly: TestExtensionTypes(typeof(DefaultTestHostManager), typeof(DotnetTestHostManager))]
