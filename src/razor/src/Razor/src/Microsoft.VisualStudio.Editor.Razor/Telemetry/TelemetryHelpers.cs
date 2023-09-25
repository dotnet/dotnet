// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.VisualStudio.Telemetry;

namespace Microsoft.AspNetCore.Razor.Telemetry;

internal static class TelemetryHelpers
{
    public static string GetTelemetryName(string name) => "dotnet/razor/" + name;

    public static string GetPropertyName(string name) => "dotnet.razor." + name;

    public static string GetDescription(Exception exception)
    {
        const string CodeAnalysisNamespace = nameof(Microsoft) + "." + nameof(CodeAnalysis);
        const string AspNetCoreNamespace = nameof(Microsoft) + "." + nameof(AspNetCore);

        // Be resilient to failing here.  If we can't get a suitable name, just fall back to the standard name we
        // used to report.
        try
        {
            // walk up the stack looking for the first call from a type that isn't in the ErrorReporting namespace.
            var frames = new StackTrace(exception).GetFrames();

            // On the .NET Framework, GetFrames() can return null even though it's not documented as such.
            // At least one case here is if the exception's stack trace itself is null.
            if (frames != null)
            {
                foreach (var frame in frames)
                {
                    var method = frame?.GetMethod();
                    var methodName = method?.Name;
                    if (methodName is null)
                    {
                        continue;
                    }

                    var declaringTypeName = method?.DeclaringType?.FullName;
                    if (declaringTypeName == null)
                    {
                        continue;
                    }

                    if (!declaringTypeName.StartsWith(CodeAnalysisNamespace) &&
                        !declaringTypeName.StartsWith(AspNetCoreNamespace))
                    {
                        continue;
                    }

                    return declaringTypeName + "." + methodName;
                }
            }
        }
        catch
        {
        }

        // If we couldn't get a stack, do this
        return exception.Message;
    }

    public static TelemetrySeverity ToTelemetrySeverity(Severity severity)
        => severity switch
        {
            Severity.Normal => TelemetrySeverity.Normal,
            Severity.Low => TelemetrySeverity.Low,
            Severity.High => TelemetrySeverity.High,
            _ => throw new InvalidOperationException($"Unknown severity: {severity}")
        };

    public static bool IsNumeric(object? o)
        => o is not null &&
        !o.GetType().IsEnum &&
        Type.GetTypeCode(o.GetType()) switch
        {
            TypeCode.Char or
            TypeCode.SByte or
            TypeCode.Byte or
            TypeCode.Int16 or
            TypeCode.Int32 or
            TypeCode.Int64 or
            TypeCode.Double or
            TypeCode.Single or
            TypeCode.UInt16 or
            TypeCode.UInt32 or
            TypeCode.UInt64
            => true,
            _ => false
        };
}
