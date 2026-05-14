// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// The base argument processor capabilities.
/// </summary>
internal abstract class BaseArgumentProcessorCapabilities : IArgumentProcessorCapabilities
{
    /// <summary>
    /// Gets a value indicating whether allow multiple.
    /// </summary>
    public virtual bool AllowMultiple => true;

    /// <summary>
    /// Gets a value indicating whether always execute.
    /// </summary>
    public virtual bool AlwaysExecute => false;

    /// <summary>
    /// Gets the command name.
    /// </summary>
    public abstract string CommandName { get; }

    /// <summary>
    /// Gets the short command name.
    /// </summary>
    public virtual string? ShortCommandName => null;

    /// <summary>
    /// Gets the help content resource name.
    /// </summary>
    public virtual string? HelpContentResourceName => null;

    /// <summary>
    /// Gets the help priority.
    /// </summary>
    public virtual HelpContentPriority HelpPriority => HelpContentPriority.None;

    /// <summary>
    /// Gets a value indicating whether is action.
    /// </summary>
    public virtual bool IsAction => false;

    /// <summary>
    /// Gets a value indicating whether is special command.
    /// </summary>
    public virtual bool IsSpecialCommand => false;

    /// <summary>
    /// Gets the priority.
    /// </summary>
    public virtual ArgumentProcessorPriority Priority => ArgumentProcessorPriority.Normal;
}
