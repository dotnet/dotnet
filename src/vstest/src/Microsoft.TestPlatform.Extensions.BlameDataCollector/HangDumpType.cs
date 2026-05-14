// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

/// <summary>
/// Specifies the command line options for hang dump type. This should be superset of CrashDumpType, because crash and hang dumps share DumpType command line option, which can specify the type of dump to create to both.
/// </summary>
internal enum HangDumpType
{
    None,
    Mini,
    Full
}
