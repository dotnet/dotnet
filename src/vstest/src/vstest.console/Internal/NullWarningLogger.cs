// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;

internal class NullWarningLogger : IWarningLogger
{
    public void LogWarning(string message)
    {
        //TODO: Implement registrar in StartTestSession instead of passing null, and stop using this class.
    }
}
