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
    /// Name of the environment variable that carries the connection secrets from the runner
    /// (vstest.console) to a child process it starts (testhost or datacollector). The value is the
    /// pair "childSecret:parentSecret", generated fresh for each child. The environment is used
    /// rather than the command line because the operating system only lets the same user read it.
    /// </summary>
    public const string VstestConnectionSecrets = "VSTEST_CONNECTION_SECRETS";

    /// <summary>
    /// Name of the environment variable that carries the connection secrets for the testhost to
    /// datacollector test-case-events channel. vstest.console generates this pair and gives it to
    /// both processes, so no secret has to travel back over a message. It is kept distinct from
    /// <see cref="VstestConnectionSecrets"/> so the two do not overwrite each other when the
    /// datacollector's environment is merged into the testhost process.
    /// </summary>
    public const string VstestDataCollectionConnectionSecrets = "VSTEST_DATACOLLECTION_CONNECTION_SECRETS";

    /// <summary>
    /// Name of the environment variable that carries the connection secrets for the translation
    /// layer to vstest.console (design mode) channel. The translation layer generates the pair and
    /// passes it to the vstest.console process it starts. It is kept distinct from
    /// <see cref="VstestConnectionSecrets"/> so the two do not overwrite each other when
    /// vstest.console starts a testhost that inherits its environment.
    /// </summary>
    public const string VstestDesignModeConnectionSecrets = "VSTEST_DESIGNMODE_CONNECTION_SECRETS";

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
