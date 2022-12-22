// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Details of EndPoint address for connection b/w test runtime, test runner
/// </summary>
public struct TestHostConnectionInfo
{
    /// <summary>
    /// Endpoint where the service is hosted, This endpoint is specific to Transport
    /// e.g. 127.0.0.0:8080 for sockets
    /// </summary>
    public string Endpoint
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the role played by TestRuntime during communication.
    /// </summary>
    public ConnectionRole Role
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the transport protocol supported by test runtime
    /// </summary>
    public Transport Transport
    {
        get;
        set;
    }
}

/// <summary>
/// Specifies the role of application, whether as host or client
/// </summary>
public enum ConnectionRole
{
    /// <summary>
    /// Act as server host
    /// </summary>
    Host = 0,

    /// <summary>
    /// Act as client
    /// </summary>
    Client = 1
}

/// <summary>
/// Specifies the underneath Transport channel to be used
/// E.g. For UWP/Desktop it is Sockets, for Android it could be SSH
/// </summary>
public enum Transport
{
    /// <summary>
    /// Act as server host
    /// </summary>
    Sockets = 0,
}

/// <summary>
/// Connection information for a test host to communicate with test runner.
/// </summary>
public struct TestRunnerConnectionInfo
{
    /// <summary>
    /// Gets or sets the port for runner to connect to
    /// Needed for backward compatibility
    /// </summary>
    public int Port
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the details of endpoint by test runner for host communication.
    /// </summary>
    public TestHostConnectionInfo ConnectionInfo
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the diagnostics log file.
    /// </summary>
    public string? LogFile
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the trace level of logs.
    /// </summary>
    public int TraceLevel
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the runner process id.
    /// </summary>
    public int RunnerProcessId
    {
        get;
        set;
    }
}
