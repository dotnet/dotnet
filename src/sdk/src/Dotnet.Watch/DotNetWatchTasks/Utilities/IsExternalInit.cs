// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NETSTANDARD || NETFRAMEWORK

using System.ComponentModel;

<<<<<<<< HEAD:src/sdk/src/Dotnet.Watch/DotNetWatchTasks/Utilities/IsExternalInit.cs
namespace System.Runtime.CompilerServices;

/// <summary>
/// Reserved to be used by the compiler for tracking metadata.
/// This class should not be used by developers in source code.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class IsExternalInit
{
========
// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Reserved to be used by the compiler for tracking metadata.
    /// This class should not be used by developers in source code.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit;
>>>>>>>> darc/forward/8433b4b-5034ed5:src/sdk/src/TemplateEngine/Shared/IsExternalInit.cs
}

#endif
