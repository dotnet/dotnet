// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.VersionTools.Cli;

public interface IOperation
{
    enum ExitCode : int
    {
        Success = 0, // (0x0) The operation completed successfully.
        ErrorFileNotFount = 2, // (0x2) The system cannot find the file specified.
    }

    ExitCode Execute();
}
