// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting;

#if DOTNET_BUILD_FROM_SOURCE
// MtpTestRuntimeProvider is compiled out of source-only builds, see dotnet/dotnet#8349.
[assembly: TestExtensionTypes(typeof(DefaultTestHostManager), typeof(DotnetTestHostManager))]
#else
[assembly: TestExtensionTypes(typeof(DefaultTestHostManager), typeof(DotnetTestHostManager), typeof(MtpTestRuntimeProvider))]
#endif
