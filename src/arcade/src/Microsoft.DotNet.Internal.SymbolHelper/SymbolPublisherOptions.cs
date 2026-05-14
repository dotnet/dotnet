// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable enable

using Azure.Core;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;

namespace Microsoft.DotNet.Internal.SymbolHelper;

/// <summary>
/// Represents the options for a symbol client's publishing.
/// </summary>
public sealed class SymbolPublisherOptions
{
    /// <param name="azdoOrg">The Azure DevOps organization to publish to.</param>
    /// <param name="credential">The token credential with symbol write scope.</param>
    /// <param name="packageFileExcludeList">The list of package files to exclude from package publishing. Doesn't contribute to loose file publishing. Empty by default.</param>
    /// <param name="convertPortablePdbs">A flag indicating whether to convert portable PDBs to windows PDBs. Defaults to false. </param>
    /// <param name="treatPdbConversionIssuesAsInfo">A flag indicating whether to treat PDB conversion issues as informational rather than warn/error. Defaults to false.</param>
    /// <param name="pdbConversionTreatAsWarning">The list of PDB conversion issue IDs to treat as warnings. Defaults to empty.</param>
    /// <param name="operationTimeoutInMins">Symbol client per-operation timeout in minutes. Defaults to 10 mins.</param>
    /// <param name="dotnetInternalPublishSpecialClrFiles">A flag indicating whether to publish CLR files under their special diagnostic indexes. Defaults to false.</param>
    /// <param name="verboseClient">A flag indicating whether to enable verbose client logging. Defaults to false.</param>
    /// <param name="isDryRun">A flag indicating whether to perform a dry run. Defaults to false.</param>
    public SymbolPublisherOptions(
        string azdoOrg,
        TokenCredential credential,
        IEnumerable<string>? packageFileExcludeList = null,
        bool convertPortablePdbs = false,
        bool treatPdbConversionIssuesAsInfo = false,
        IEnumerable<int>? pdbConversionTreatAsWarning = null,
        uint operationTimeoutInMins = 10,
        bool dotnetInternalPublishSpecialClrFiles = false,
        bool verboseClient = false,
        bool isDryRun = false)
    {
        AzdoOrg = azdoOrg is not null and not "" ? azdoOrg : throw new ArgumentException("Organization can't be null or empty", nameof(azdoOrg));
        Credential = credential ?? throw new ArgumentNullException(nameof(credential));
        PackageFileExcludeList = packageFileExcludeList is null ? FrozenSet<string>.Empty : packageFileExcludeList.ToFrozenSet();
        ConvertPortablePdbs = convertPortablePdbs;
        TreatPdbConversionIssuesAsInfo = treatPdbConversionIssuesAsInfo;
        PdbConversionTreatAsWarning = pdbConversionTreatAsWarning is null ? FrozenSet<int>.Empty : pdbConversionTreatAsWarning.ToFrozenSet();
        OperationTimeoutInMins = operationTimeoutInMins;
        DotnetInternalPublishSpecialClrFiles = dotnetInternalPublishSpecialClrFiles;
        VerboseClient = verboseClient;
        IsDryRun = isDryRun;
    }

    /// <summary>
    /// The Azure DevOps organization a symbol upload targets.
    /// </summary>
    public string AzdoOrg { get; }

    /// <summary>
    /// The token credential with vso.symbols_write perms to the associated Azure DevOps org.
    /// </summary>
    public TokenCredential Credential { get; }

    /// <summary>
    /// List of package-root-relative files to exclude from publishing if found in symbol packages.
    /// </summary>
    public FrozenSet<string> PackageFileExcludeList { get; }

    /// <summary>
    /// A flag indicating whether the client should try to convert portable PDBs to classic on upload.
    /// </summary>
    public bool ConvertPortablePdbs { get; }

    /// <summary>
    /// A flag indicating whether to treat PDB conversion issues as information.
    /// </summary>
    public bool TreatPdbConversionIssuesAsInfo { get; }

    /// <summary>
    /// List of PDB conversion issue IDs to treat as warnings.
    /// </summary>
    public FrozenSet<int> PdbConversionTreatAsWarning { get; }

    /// <summary>
    /// Symbol client per-operation timeout in minutes.
    /// </summary>
    public uint OperationTimeoutInMins { get; }

    /// <summary>
    /// Flag indicating whether to publish special CLR files for dotnet internal builds.
    /// </summary>
    public bool DotnetInternalPublishSpecialClrFiles { get; }

    /// <summary>
    /// Flag indicating whether to enable verbose client logging.
    /// </summary>
    public bool VerboseClient { get; }

    /// <summary>
    /// Flag indicating whether to perform a dry run, unwrapping packages and logging commands and files to be uploaded without executing uploading agent.
    /// </summary>
    public bool IsDryRun { get; }
}
