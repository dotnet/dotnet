// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;

public class EnvironmentHelper
{
    public const string VstestConnectionTimeout = "VSTEST_CONNECTION_TIMEOUT";
    public const int DefaultConnectionTimeout = 90; // seconds

    /// <summary>
    /// Get timeout based on environment variable VSTEST_CONNECTION_TIMEOUT.
    /// </summary>
    public static int GetConnectionTimeout()
    {
        var envVarValue = Environment.GetEnvironmentVariable(VstestConnectionTimeout);

        if (!envVarValue.IsNullOrEmpty() && int.TryParse(envVarValue, out int value) && value >= 0)
        {
            EqtTrace.Info("EnvironmentHelper.GetConnectionTimeout: {0} value set to {1}.", VstestConnectionTimeout, value);
        }
        else
        {
            value = DefaultConnectionTimeout;
        }

        return value;
    }
}
