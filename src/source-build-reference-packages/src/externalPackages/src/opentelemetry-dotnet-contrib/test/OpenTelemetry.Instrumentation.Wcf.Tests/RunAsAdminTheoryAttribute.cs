// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Security.Principal;
using Xunit;

namespace OpenTelemetry.Instrumentation.Wcf.Tests;

/// <summary>
/// Attribute that is applied to a method to indicate that it is a fact that should be run by the test runner
/// if the user account running the test has administrative privileges. This class cannot be inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class RunAsAdminTheoryAttribute : TheoryAttribute
{
    public RunAsAdminTheoryAttribute()
        : base()
    {
        this.Skip = IsCurrentUserAdmin(out string name) ? null : $"The current user '{name}' does not have administrative privileges.";
    }

    internal static bool IsCurrentUserAdmin() => IsCurrentUserAdmin(out string _);

    private static bool IsCurrentUserAdmin(out string name)
    {
#if NET
        if (!OperatingSystem.IsWindows())
        {
            name = Environment.UserName;
            return false;
        }
#endif

        using var identity = WindowsIdentity.GetCurrent();
        name = identity.Name;

        return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
    }
}
