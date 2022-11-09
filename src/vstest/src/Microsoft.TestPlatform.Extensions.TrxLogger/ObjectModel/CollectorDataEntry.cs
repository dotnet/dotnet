// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Xml;

using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;
using Microsoft.TestPlatform.Extensions.TrxLogger.XML;
using Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Data entry from a collector along with information what this collector is and what agent it was collected on.
/// Instances are created by Agent and sent to Controller. Then controller puts agent names in.
/// The user is not supposed to create instances of this.
/// </summary>
internal class CollectorDataEntry : IXmlTestStore
{
    /// <summary>
    /// List of data attachments. These attachments can be things such as files that the
    /// collector wants to make available to the publishers.
    /// </summary>
    private readonly List<IDataAttachment> _attachments = new();

    /// <summary>
    /// Name of the agent from which we received the data
    /// </summary>
    private readonly string _agentName;

    /// <summary>
    /// Display name of the agent from which we received the data
    /// </summary>
    private readonly string _agentDisplayName;

    /// <summary>
    /// Flag indicating whether this data is coming from a remote (not hosted) agent
    /// </summary>
    private readonly bool _isFromRemoteAgent;

    /// <summary>
    /// URI of the collector.
    /// </summary>
    private readonly Uri _uri;

    /// <summary>
    /// Name of the collector that should be displayed to the user.
    /// </summary>
    private readonly string _collectorDisplayName;

    /// <summary>
    /// Used by the aggregator to put collector Uri, agentName, string agentDisplayName, whether it's remote data, and
    /// Attachments.
    /// </summary>
    /// <param name="uri">
    /// The uri.
    /// </param>
    /// <param name="collectorDisplayName">
    /// The collector Display Name.
    /// </param>
    /// <param name="agentName">
    /// The agent Name.
    /// </param>
    /// <param name="agentDisplayName">
    /// The agent Display Name.
    /// </param>
    /// <param name="isFromRemoteAgent">
    /// Is From Remote Agent.
    /// </param>
    /// <param name="attachments">
    /// The attachments.
    /// </param>
    public CollectorDataEntry(Uri uri, string collectorDisplayName, string agentName, string agentDisplayName, bool isFromRemoteAgent, IList<IDataAttachment>? attachments)
    {
        EqtAssert.ParameterNotNull(uri, nameof(uri));
        EqtAssert.StringNotNullOrEmpty(collectorDisplayName, nameof(collectorDisplayName));
        EqtAssert.StringNotNullOrEmpty(agentName, nameof(agentName));
        EqtAssert.StringNotNullOrEmpty(agentDisplayName, nameof(agentDisplayName));

        if (null != attachments)
        {
            // Copy the attachments
            foreach (IDataAttachment attachment in attachments)
            {
                AddAttachment(attachment);
            }
        }

        // Note that the data can be null.
        _uri = uri;
        _collectorDisplayName = collectorDisplayName;
        _agentName = agentName.Trim();
        _agentDisplayName = agentDisplayName.Trim();
        _isFromRemoteAgent = isFromRemoteAgent;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CollectorDataEntry"/> class.
    /// </summary>
    /// <remarks>
    /// For XML persistence
    /// </remarks>
    internal CollectorDataEntry()
    {
        _agentName = null!;
        _agentDisplayName = null!;
        _uri = null!;
        _collectorDisplayName = null!;
    }

    /// <summary>
    /// Gets the read-only list of data attachments
    /// </summary>
    public IList<IDataAttachment> Attachments
    {
        get
        {
            return _attachments.AsReadOnly();
        }
    }


    #region IXmlTestStore Members

    /// <summary>
    /// Saves the state to the XML element
    /// </summary>
    /// <param name="element">The XML element to save to</param>
    /// <param name="parameters">Parameters to customize the save behavior</param>
    public void Save(XmlElement element, XmlTestStoreParameters? parameters)
    {
        EqtAssert.ParameterNotNull(element, nameof(element));

        XmlPersistence helper = new();
        helper.SaveSimpleField(element, "@agentName", _agentName, null);
        helper.SaveSimpleField(element, "@agentDisplayName", _agentDisplayName, _agentName);
        helper.SaveSimpleField(element, "@isFromRemoteAgent", _isFromRemoteAgent, false);
        helper.SaveSimpleField(element, "@uri", _uri.AbsoluteUri, null);
        helper.SaveSimpleField(element, "@collectorDisplayName", _collectorDisplayName, string.Empty);

        IList<UriDataAttachment> uriAttachments = new List<UriDataAttachment>();
        foreach (IDataAttachment attachment in Attachments)
        {
            if (attachment is UriDataAttachment uriAtt)
            {
                uriAttachments.Add(uriAtt);
            }
        }

        helper.SaveIEnumerable(uriAttachments, element, "UriAttachments", "A", "UriAttachment", parameters);
    }

    #endregion
    /// <summary>
    /// Adds a data attachment to the list of data attachments
    /// </summary>
    /// <param name="attachment">The attachment to add</param>
    internal void AddAttachment(IDataAttachment attachment)
    {
        ValidateArg.NotNull(attachment, nameof(attachment));
        _attachments.Add(attachment);
    }

    /// <summary>
    /// Clones the instance and attachments, with file paths in file attachments absolute or relative as specified
    /// </summary>
    /// <param name="resultsDirectory">The results directory to use to make paths in the data attachments relative or absolute</param>
    /// <param name="useAbsolutePaths">True to use absolute paths in this instance, false to use relative paths</param>
    /// <returns>A clone of the instance containing cloned attachments with file paths made absolute or relative</returns>
    internal CollectorDataEntry Clone(string resultsDirectory, bool useAbsolutePaths)
    {
        TPDebug.Assert(!resultsDirectory.IsNullOrEmpty(), "'resultsDirectory' is null or empty");
        TPDebug.Assert(resultsDirectory == resultsDirectory.Trim(), "'resultsDirectory' contains whitespace at the ends");

        var collector = new CollectorDataEntry(_uri, _collectorDisplayName, _agentName, _agentDisplayName, _isFromRemoteAgent, null);

        // Clone the attachments
        foreach (IDataAttachment attachment in _attachments)
        {
            TPDebug.Assert(attachment is not null, "'attachment' is null");

            if (attachment is UriDataAttachment uriDataAttachment)
            {
                collector._attachments.Add(uriDataAttachment.Clone(resultsDirectory, useAbsolutePaths));
            }
            else
            {
                collector._attachments.Add(attachment);
            }
        }

        return collector;
    }
}
