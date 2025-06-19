// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Microsoft.IdentityModel.Xml
// Range: 30000 - 30999

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Log messages and codes for XmlProcessing
    /// </summary>
    internal static class LogMessages
    {
#pragma warning disable 1591

        // XML reading
        internal const string IDX30011 = "IDX30011: Unable to read XML. Expecting XmlReader to be at ns.element: '{0}.{1}', found: '{2}.{3}'.";
        internal const string IDX30013 = "IDX30013: Unable to read XML. While reading element '{0}', Required attribute was not found : '{1}'.";
        internal const string IDX30015 = "IDX30015: Only a single '{0}' element is supported. Found more than one.";
        internal const string IDX30016 = "IDX30016: Exception thrown while reading '{0}'. See inner exception for more details.";
        internal const string IDX30017 = "IDX30017: Exception thrown while reading '{0}'. Caught exception: '{1}'.";
        internal const string IDX30019 = "IDX30019: Unable to read XML. A second <Signature> element was found. The EnvelopedSignatureReader can only process one <Signature>.";
        internal const string IDX30020 = "IDX30020: Unable to read XML. A second <Reference> element was found. The EnvelopedSignatures can only have one <Reference>.";
        internal const string IDX30022 = "IDX30022: Unable to read XML. Expecting XmlReader to be at a StartElement, NodeType is: '{0}'.";
        internal const string IDX30024 = "IDX30024: Unable to read XML. Expecting XmlReader to be at element: '{0}', found: '{1}'.";
        internal const string IDX30025 = "IDX30025: Unable to read XML. Expecting XmlReader to be at EndElement: '{0}'. Found XmlNode 'type.name': '{1}.{2}'.";
        internal const string IDX30026 = "IDX30026: The reader must be pointing to a StartElement. NodeType is: '{0}'.";
        internal const string IDX30027 = "IDX30027: InnerReader is null. It is necessary to set InnerReader before making calls to DelegatingXmlDictionaryReader.";
        internal const string IDX30028 = "IDX30028: InnerWriter is null. It is necessary to set InnerWriter before making calls to DelegatingXmlDictionaryWriter.";
        internal const string IDX30029 = "IDX30029: The Reference '{0}' has exceeded the number of Transforms that will be processed: '{1}'. If there is a need to increase the number of Transforms, the DSigSerializer.MaximumReferenceTransforms can be increased. The default value is 10.";

        // XML structure, supported exceptions
        internal const string IDX30100 = "IDX30100: Unable to process the {0} element. This canonicalization method is not supported: '{1}'. Supported methods are: '{2}', '{3}'.";
        internal const string IDX30105 = "IDX30105: Transform must specify an algorithm none was found.";
        internal const string IDX30107 = "IDX30107: 'InclusiveNamespaces' is not supported.";
        internal const string IDX30108 = "IDX30108: 'X509Data' cannot be empty.";

        // signature validation
        internal const string IDX30200 = "IDX30200: The 'Signature' did not validate. CryptoProviderFactory: '{0}', SecurityKey: '{1}'.";
        internal const string IDX30201 = "IDX30201: The 'Reference' did not validate: '{0}'.";
        internal const string IDX30202 = "IDX30202: The Reference does not have a XmlTokenStream set: '{0}'.";
        internal const string IDX30203 = "IDX30203: The CryptoProviderFactory: '{0}', CreateForVerifying returned null for key: '{1}', SignatureMethod: '{2}'.";
        internal const string IDX30204 = "IDX30204: Canonicalization algorithm is not supported: '{0}'. Supported methods are: '{1}', '{2}'.";
        internal const string IDX30206 = "IDX30206: The reference '{0}' did not contain a digest.";
        internal const string IDX30207 = "IDX30207: SignatureMethod is not supported: '{0}'. CryptoProviderFactory: '{1}'.";
        internal const string IDX30208 = "IDX30208: DigestMethod is not supported: '{0}'. CryptoProviderFactory: '{1}'.";
        internal const string IDX30209 = "IDX30209: The CryptoProviderFactory: '{0}', CreateHashAlgorithm, returned null for DigestMethod: '{1}'.";
        internal const string IDX30210 = "IDX30210: The TransformFactory does not support the transform: '{0}'.";
        internal const string IDX30211 = "IDX30211: The TransfromFactory does not support the canonicalizing transform: '{0}'.";
        internal const string IDX30212 = "IDX30212: Unable to verify Signature as Signature.SignedInfo is null.";
        internal const string IDX30213 = "IDX30213: The CryptoProviderFactory: '{0}', CreateForSigning returned null for key: '{1}', SignatureMethod: '{2}'.";

        // logging messages
        internal const string IDX30300 = "IDX30300: KeyInfo skipped unknown element: '{0}'.";

        // XML writing
        internal const string IDX30401 = "IDX30401: Unable to write XML. {0}.{1} is null or empty.";
        internal const string IDX30403 = "IDX30403: Unable to write XML. One of the values in Reference.Transforms is null or empty.";
        internal const string IDX30404 = "IDX30404: Unable to write XML. Signature.SignedInfo is null.";
        internal const string IDX30405 = "IDX30405: Unable to write XML. SignedInfo.Reference is null.";
        internal const string IDX30406 = "IDX30406: Unsupported NodeType: {0}.";

        // XML validation
        internal const string IDX30500 = "IDX30500: xsi:type attribute was not found. Expected: '{0}':'{1}'.";
        internal const string IDX30501 = "IDX30501: xsi:type attribute was did not match. Expected: '{0}':'{1}', Found: '{2}':'{3}'.";

        // Setting values on types
        internal const string IDX30600 = "IDX30600: MaximumReferenceTransforms can not be a negative value. value: '{0}'.";

#pragma warning restore 1591
    }
}
