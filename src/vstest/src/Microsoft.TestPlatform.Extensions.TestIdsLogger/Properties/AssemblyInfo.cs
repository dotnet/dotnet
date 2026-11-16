// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform;
using Microsoft.VisualStudio.TestPlatform.Extensions.TestIdsLogger;

// The logger type is experimental. Naming it here is how the extension framework discovers it, so
// this reference is mandatory rather than a use of the API by a consumer.
#pragma warning disable VSTEST001
[assembly: TestExtensionTypes(typeof(TestIdsLogger))]
#pragma warning restore VSTEST001
