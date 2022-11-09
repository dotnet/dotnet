// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

public interface ICrashDumperFactory
{
    ICrashDumper Create(string targetFramework);
}
