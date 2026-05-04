// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace OpenTelemetry.Instrumentation;

internal static class SqlParameterProcessor
{
    public static void AddQueryParameters(Activity activity, object? command)
    {
        if (command is not IDbCommand { Parameters.Count: > 0 } dbCommand)
        {
            return;
        }

        int index = 0;

        foreach (var parameter in dbCommand.Parameters)
        {
            if (parameter is IDbDataParameter dbDataParameter)
            {
                // If a query parameter has no name and instead is referenced only by index, then {key} SHOULD be the 0-based index.
                var key = string.IsNullOrEmpty(dbDataParameter.ParameterName)
                    ? index.ToString(CultureInfo.InvariantCulture)
                    : dbDataParameter.ParameterName;

                activity.SetTag($"db.query.parameter.{key}", dbDataParameter.Value);
            }

            index++;
        }
    }
}
