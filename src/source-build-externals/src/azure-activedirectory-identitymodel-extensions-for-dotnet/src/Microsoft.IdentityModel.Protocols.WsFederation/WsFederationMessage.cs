// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Protocols.WsFederation
{
    /// <summary>
    /// Provides access to common WsFederation message parameters.
    /// </summary>
    [type: SuppressMessage("Microsoft.Naming", "CA1704")]
    public class WsFederationMessage : AuthenticationProtocolMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WsFederationMessage"/> class.
        /// </summary>
        public WsFederationMessage()
        {
        }

        /// <summary>
        /// Creates a <see cref="WsFederationMessage"/> from the contents of a query string.
        /// </summary>
        /// <param name="queryString"> query string to extract parameters.</param>
        /// <returns>An instance of <see cref="WsFederationMessage"/>.</returns>
        /// <remarks>If 'queryString' is null or whitespace, a default <see cref="WsFederationMessage"/> is returned. Parameters are parsed from <see cref="Uri.Query"/>.</remarks>
        public static WsFederationMessage FromQueryString(string queryString)
        {
            if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                LogHelper.LogVerbose(FormatInvariant(LogMessages.IDX22900, queryString));

            var wsFederationMessage = new WsFederationMessage();
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                foreach(var keyValuePair in QueryHelper.ParseQuery(queryString))
                {
                    foreach(var value in keyValuePair.Value)
                    {
                        wsFederationMessage.SetParameter(keyValuePair.Key, value);
                    }
                }
            } 

            return wsFederationMessage;
        }

        /// <summary>
        /// Creates a <see cref="WsFederationMessage"/> from the contents of a <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">uri containing parameters.</param>
        /// <returns>An instance of <see cref="WsFederationMessage"/>.</returns>
        /// <remarks><see cref="WsFederationMessage"/>.IssuerAddress is NOT set/>. Parameters are parsed from <see cref="Uri.Query"/>.</remarks>
        public static WsFederationMessage FromUri(Uri uri)
        {
            if (uri != null && uri.Query.Length > 1)
            {
                if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                    LogHelper.LogVerbose(FormatInvariant(LogMessages.IDX22901, uri.ToString()));

                return FromQueryString(uri.Query.Substring(1));
            }

            return new WsFederationMessage();
        }
       
        /// <summary>
        /// Initializes a new instance of the <see cref="WsFederationMessage"/> class.
        /// </summary>
        /// <param name="wsFederationMessage"> message to copy.</param>        
        public WsFederationMessage(WsFederationMessage wsFederationMessage)
        {
            if (wsFederationMessage == null)
            {
                if (LogHelper.IsEnabled(EventLogLevel.Warning))
                    LogHelper.LogWarning(FormatInvariant(LogMessages.IDX22000, LogHelper.MarkAsNonPII(nameof(wsFederationMessage))));

                return;
            }

            foreach (KeyValuePair<string, string> keyValue in wsFederationMessage.Parameters)
                SetParameter(keyValue.Key, keyValue.Value);

            IssuerAddress = wsFederationMessage.IssuerAddress;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WsFederationMessage"/> class.
        /// </summary>
        /// <param name="parameters">Enumeration of key value pairs.</param>        
        public WsFederationMessage(IEnumerable<KeyValuePair<string, string[]>> parameters)
        {
            if (parameters == null)
            {
                if (LogHelper.IsEnabled(EventLogLevel.Warning))
                    LogHelper.LogWarning(FormatInvariant(LogMessages.IDX22000, LogHelper.MarkAsNonPII(nameof(parameters))));

                return;
            }

            foreach (KeyValuePair<string, string[]> keyValue in parameters)
            {
                foreach (string strValue in keyValue.Value)
                {
                    SetParameter(keyValue.Key, strValue);
                }
            }
        }

        /// <summary>
        /// Creates a 'wsignin1.0' message using the current contents of this <see cref="WsFederationMessage"/>.
        /// </summary>
        /// <returns>The uri to use for a redirect.</returns>

        public string CreateSignInUrl()
        {
            return (new WsFederationMessage(this)
            {
                Wa = WsFederationConstants.WsFederationActions.SignIn
            }).BuildRedirectUrl();
        }

        /// <summary>
        /// Creates a 'wsignout1.0' message using the current contents of this <see cref="WsFederationMessage"/>.
        /// </summary>
        /// <returns>The uri to use for a redirect.</returns>
        public string CreateSignOutUrl()
        {
            return (new WsFederationMessage(this)
            {
                Wa = WsFederationConstants.WsFederationActions.SignOut
            }).BuildRedirectUrl();
        }

        /// <summary>
        /// Reads the 'wresult' and returns the embedded security token.
        /// </summary>
        /// <returns>the 'SecurityToken'.</returns>
        /// <exception cref="WsFederationException">if exception occurs while reading security token.</exception>
        public virtual string? GetToken()
        {
            return GetTokenUsingXmlReader();
        }

        /// <summary>
        /// Processes the 'Wresult' and returns the first 'RequestedSecurityToken' found.
        /// This method is called only for netstandard 1.4 targets as XmlTextReader is not available and
        /// XmlDictionaryReader.CreateTextReader normalizes the XML causing signature failures.
        /// This is only called after it is determined the Wresult is well formed xml. A successful call the GetTokenUsingXmlReader should be made first.
        /// </summary>
        /// <returns>the string version of the security token.</returns>
        internal static string? GetToken(string wresult)
        {
            if (string.IsNullOrEmpty(wresult))
            {
                if (LogHelper.IsEnabled(EventLogLevel.Warning))
                    LogHelper.LogWarning(FormatInvariant(LogMessages.IDX22000, LogHelper.MarkAsNonPII(nameof(wresult))));

                return null;
            }

            // find first <RequestedSecurityToken>
            var tokenStartIndex = wresult.IndexOf(WsTrustConstants.Elements.RequestedSecurityToken, StringComparison.Ordinal);
            if (tokenStartIndex == -1)
            {
                LogHelper.LogWarning(LogMessages.IDX22904);
                return null;
            }

            // skip ahead for known string
            tokenStartIndex += WsTrustConstants.Elements.RequestedSecurityToken.Length;

            // move forward until start element, assume its the token
            while (tokenStartIndex < wresult.Length)
            {
                if (wresult[tokenStartIndex] == '<')
                    break;

                tokenStartIndex++;
            }

            // sanity check
            if (tokenStartIndex >= wresult.Length)
            {
                LogHelper.LogWarning(LogMessages.IDX22904);
                return null;
            }

            // find matching </RequestedSecurityToken>
            var tokenEndIndex = wresult.IndexOf(WsTrustConstants.Elements.RequestedSecurityToken, tokenStartIndex, StringComparison.InvariantCulture);
            if (tokenEndIndex == -1)
            {
                LogHelper.LogWarning(LogMessages.IDX22904);
                return null;
            }

            // move backwards until hitting the end element for token
            while (tokenEndIndex > tokenStartIndex)
            {
                if (wresult[tokenEndIndex] == '>')
                    break;

                tokenEndIndex--;
            }

            // sanity check
            if (tokenEndIndex <= tokenStartIndex)
            {
                LogHelper.LogWarning(LogMessages.IDX22904);
                return null;
            }

            // +1 to account for zero index
            return wresult.Substring(tokenStartIndex, tokenEndIndex - tokenStartIndex + 1);
        }

        /// <summary>
        /// Reads the 'wresult' and returns the embedded security token.
        /// </summary>
        /// <returns>the 'SecurityToken'.</returns>
        /// <exception cref="WsFederationException">if exception occurs while reading security token.</exception>
        public virtual string? GetTokenUsingXmlReader()
        {
            if (Wresult == null)
            {
                if (LogHelper.IsEnabled(EventLogLevel.Warning))
                    LogHelper.LogWarning(FormatInvariant(LogMessages.IDX22000, LogHelper.MarkAsNonPII(nameof(Wresult))));

                return null;
            }

            string? token = null;
            using (var sr = new StringReader(Wresult))
            using (var xmlReader = new XmlTextReader(sr) { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null })
            {
                // Read StartElement <RequestSecurityTokenResponseCollection> this is possible for wstrust 1.3 and 1.4
                if (XmlUtil.IsStartElement(xmlReader, WsTrustConstants.Elements.RequestSecurityTokenResponseCollection, WsTrustNamespaceNon2005List))
                    xmlReader.ReadStartElement();

                while (xmlReader.IsStartElement())
                {
                    // Read <RequestSecurityTokenResponse>
                    if (XmlUtil.IsStartElement(xmlReader, WsTrustConstants.Elements.RequestSecurityTokenResponse, WsTrustNamespaceList))
                    {
                        // <RequestSecurityTokenResponse>
                        xmlReader.ReadStartElement();

                        // while we are not on <RequestedSecurityToken> skip
                        while (xmlReader.IsStartElement())
                        {
                            if (XmlUtil.IsStartElement(xmlReader, WsTrustConstants.Elements.RequestedSecurityToken, WsTrustNamespaceList))
                            {
                                // Multiple tokens were found in the RequestSecurityTokenCollection. Only a single token is supported.
                                if (token != null)
                                    throw new WsFederationException(LogMessages.IDX22903);

                                // <RequestedSecurityToken>
                                xmlReader.ReadStartElement();

                                // once RequestedSecurityToken element is found, it's written into a token.
                                // as the current node might not be a content node, the reader should skip ahead to the next content node.
                                xmlReader.MoveToContent();

                                using (var ms = new MemoryStream())
                                {
                                    using (var writer = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, false))
                                    {
                                        writer.WriteNode(xmlReader, true);
                                        writer.Flush();
                                    }
                                    token = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
                                }

                                // </RequestedSecurityToken>
                                xmlReader.ReadEndElement();
                            }
                            else
                            {
                                // skip over everything but <RequestedSecurityToken>
                                xmlReader.Skip();
                            }
                        }

                        // <RequestSecurityTokenResponse>
                        xmlReader.ReadEndElement();
                    }
                    else
                    {
                        xmlReader.Skip();
                    }
                }

                if (token == null)
                    throw LogExceptionMessage(new WsFederationException(LogMessages.IDX22902));

                return token;
            }
        }

        /// <summary>
        /// Gets a boolean representing if the <see cref="WsFederationMessage"/> is a 'sign-in-message'.
        /// </summary>
        public bool IsSignInMessage
        {
            get => Wa == WsFederationConstants.WsFederationActions.SignIn;
        }
        
        /// <summary>
        /// Gets a boolean representing if the <see cref="WsFederationMessage"/> is a 'sign-out-message'.
        /// </summary>
        public bool IsSignOutMessage
        {
            get => Wa == WsFederationConstants.WsFederationActions.SignOut;
        }

        /// <summary>
        /// Gets or sets 'wa'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Wa")]
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wa 
        { 
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wa); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wa, value); }
        }

        /// <summary>
        /// Gets or sets 'wattr'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wattr
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wattr); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wattr, value); }
        }

        /// <summary>
        /// Gets or sets 'wattrptr'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wattrptr
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wattrptr); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wattrptr, value); }
        }

        /// <summary>
        /// Gets or sets 'wauth'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wauth
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wauth); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wauth, value); }
        }

        /// <summary>
        /// Gets or sets 'wct'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wct
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wct); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wct, value); }
        }

        /// <summary>
        /// Gets or sets 'wctx'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wctx
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wctx); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wctx, value); }
        }

        /// <summary>
        /// Gets or sets 'wencoding'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wencoding
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wencoding); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wencoding, value); }
        }

        /// <summary>
        /// Gets or sets 'wfed'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wfed
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wfed); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wfed, value); }
        }

        /// <summary>
        /// Gets or sets 'wfresh'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wfresh
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wfresh); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wfresh, value); }
        }

        /// <summary>
        /// Gets or sets 'whr'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Whr
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Whr); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Whr, value); }
        }

        /// <summary>
        /// Gets or sets 'wp'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        [property: SuppressMessage("Microsoft.Naming", "CA1709")]
        public string Wp
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wp); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wp, value); }
        }

        /// <summary>
        /// Gets or sets 'wpseudo'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wpseudo
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wpseudo); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wpseudo, value); }
        }

        /// <summary>
        /// Gets or sets 'wpseudoptr'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wpseudoptr
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wpseudoptr); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wpseudoptr, value); }
        }

        /// <summary>
        /// Gets or sets 'wreply'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wreply
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wreply); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wreply, value); }
        }

        /// <summary>
        /// Gets or sets 'wreq'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wreq
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wreq); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wreq, value); }
        }

        /// <summary>
        /// Gets or sets 'wreqptr'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wreqptr
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wreqptr); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wreqptr, value); }
        }

        /// <summary>
        /// Gets or sets 'wres'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wres
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wres); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wres, value); }
        }

        /// <summary>
        /// Gets or sets 'wresult'.
        /// </summary>
        public string Wresult
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wresult); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wresult, value); }
        }

        /// <summary>
        /// Gets or sets 'wresultptr'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wresultptr
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wresultptr); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wresultptr, value); }
        }

        /// <summary>
        /// Gets or sets 'wtrealm'.
        /// </summary>
        [property: SuppressMessage("Microsoft.Naming", "CA1704")]
        public string Wtrealm
        {
            get { return GetParameter(WsFederationConstants.WsFederationParameterNames.Wtrealm); }
            set { SetParameter(WsFederationConstants.WsFederationParameterNames.Wtrealm, value); }
        }

        private static List<string> WsTrustNamespaceList = new List<string>() { WsTrustConstants.Namespaces.WsTrust2005, WsTrustConstants.Namespaces.WsTrust1_3, WsTrustConstants.Namespaces.WsTrust1_4 };

        private static List<string> WsTrustNamespaceNon2005List = new List<string>() { WsTrustConstants.Namespaces.WsTrust1_3, WsTrustConstants.Namespaces.WsTrust1_4 };
    }
}
