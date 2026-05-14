// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Collects the required and optional information needed for requesting a file transfer from a data collector.
/// </summary>
public abstract class BasicTransferInformation
{
    private string? _description;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasicTransferInformation"/> class.
    /// </summary>
    /// <param name="context">
    /// The data collection context for the transfer.
    /// </param>
    protected BasicTransferInformation(DataCollectionContext context)
    {
        //EqtAssert.ParameterNotNull(context, "context");
        Context = context;
        Description = string.Empty;
    }


    #region  Required Parameters.

    /// <summary>
    /// Gets the data collection context the transfer will be associated with.
    /// </summary>
    public DataCollectionContext Context { get; private set; }

    #endregion

    #region Optional Parameters.

    /// <summary>
    /// Gets or sets a short description of the data being sent.
    /// </summary>
    public string? Description
    {
        get => _description;

        set =>
            // If we don't have a description, use an empty string.
            _description = value ?? string.Empty;
    }

    /// <summary>
    /// Gets or sets the token which will be included with the callback to identify this file transfer.
    /// </summary>
    public object? UserToken { get; set; }

    /// <summary>
    /// Gets or sets the ID of the request that this file should be associated with. This is used
    /// for sending transient data which will be associated only with this
    /// data request and not the session or test cases that are currently running.
    /// </summary>
    public RequestId? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether cleanup should be performed after transferring the resource. This
    /// can be known by different names in the derived classes so it is protected internal
    /// so that we can refer to it in a consistent way.
    /// </summary>
    protected internal abstract bool PerformCleanup
    {
        get;
    }

    /// <summary>
    /// Gets the name of the file to use on the client machine. This
    /// can be known by different names in the derived classes so it is protected internal
    /// so that we can refer to it in a consistent way.
    /// </summary>
    protected internal abstract string FileName
    {
        get;
    }

    #endregion
}
