// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml;

using Microsoft.TestPlatform.Extensions.TrxLogger.XML;
using Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// This is a record about one run-level event that happened at run execution or around it.
/// </summary>
internal sealed class RunInfo : IXmlTestStore
{
    [StoreXmlSimpleField("Text", "")]
    private readonly string _text;

    private readonly Exception? _exception;

    [StoreXmlSimpleField("@computerName", "")]
    private readonly string _computer;

    [StoreXmlSimpleField("@outcome")]
    private readonly TestOutcome _outcome;

    [StoreXmlSimpleField("@timestamp")]
    private readonly DateTime _timestamp;

    /// <summary>
    /// Initializes a new instance of the <see cref="RunInfo"/> class.
    /// </summary>
    /// <param name="textMessage">
    /// The text message.
    /// </param>
    /// <param name="ex">
    /// The exception
    /// </param>
    /// <param name="computer">
    /// The computer.
    /// </param>
    /// <param name="outcome">
    /// The outcome.
    /// </param>
    public RunInfo(string textMessage, Exception? ex, string computer, TestOutcome outcome)
    {
        TPDebug.Assert(computer != null, "computer is null");

        _text = textMessage;
        _exception = ex;
        _computer = computer;
        _outcome = outcome;
        _timestamp = DateTime.UtcNow;
    }

    #region IXmlTestStore Members

    /// <summary>
    /// Saves the class under the XmlElement..
    /// </summary>
    /// <param name="element">
    /// The parent xml.
    /// </param>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    public void Save(XmlElement element, XmlTestStoreParameters? parameters)
    {
        XmlPersistence helper = new();
        helper.SaveSingleFields(element, this, parameters);
        helper.SaveSimpleField(element, "Exception", _exception, null);
    }

    #endregion
}
