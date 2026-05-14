// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

/// <summary>
/// Specifies the command line options for CrashDumpType, the values here should be a subset of HangDumpType enum, because DumpType option on command line can also be used to specify the hang dump type.
/// </summary>
internal enum CrashDumpType
{
    Mini,
    Full
}
