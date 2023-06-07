// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using TrxObjectModel = Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.XML;

/// <summary>
/// The xml persistence class.
/// </summary>
internal class XmlPersistence
{
    #region Types

    /// <summary>
    /// The exception type that is thrown when a duplicate key is added to a hashtable or
    /// dictionary
    /// </summary>
    public class DuplicateKeyLoadException : Exception
    {
        /// <summary>
        /// Initializes the instance
        /// </summary>
        /// <param name="key">Key that was a duplicate</param>
        /// <param name="message">The duplicate-key exception message</param>
        public DuplicateKeyLoadException(object key, string message)
            : this(key, message, null)
        {
        }

        /// <summary>
        /// Initializes the instance
        /// </summary>
        /// <param name="key">Key that was a duplicate</param>
        /// <param name="message">The duplicate-key exception message</param>
        /// <param name="innerException">The inner exception</param>
        public DuplicateKeyLoadException(object key, string message, Exception? innerException)
            : base(message, innerException)
        {
            Key = key;
        }

        /// <summary>
        /// Gets the key that was a duplicate
        /// </summary>
        public object Key
        {
            get;
            private set;
        }

    }

    #endregion

    /// <summary>This is how we persist date time except DateTime.MinValue.</summary>
    private const string DateTimePersistenceFormat = "yyyy'-'MM'-'ddTHH':'mm':'ss'.'fffffffzzz";

    /// <summary>Special case to persist DateTime.MinValue.</summary>
    private const string DateTimeUtcPersistenceFormat = "u";

    private const string DefaultNamespacePrefixEquivalent = "dpe";

    private static readonly string EmptyGuidString = Guid.Empty.ToString();

    private static readonly Type BoolType = typeof(bool);
    private static readonly Type ByteArrayType = typeof(byte[]);
    private static readonly Type DateTimeType = typeof(DateTime);

    /// <summary>
    /// this is the top level cache: Type->field information
    /// </summary>
    private static readonly Dictionary<Type, IEnumerable<FieldPersistenceInfo>> TypeToPersistenceInfoCache =
        new();

    /// <summary>
    /// Optimization: avoid re-parsing same query multiple times
    /// </summary>
    private static readonly Dictionary<string, string> QueryCache = new();

    private readonly string _prefix;
    private readonly string _namespaceUri;
    private readonly XmlNamespaceManager _xmlNamespaceManager = new(new NameTable());

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlPersistence"/> class.
    /// </summary>
    public XmlPersistence()
    {
        _prefix = string.Empty;
        _namespaceUri = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";

        if (!string.IsNullOrEmpty(_prefix))
        {
            // Register the specified prefix with the specified namespace
            _xmlNamespaceManager.AddNamespace(_prefix, _namespaceUri);
        }

        if (!string.IsNullOrEmpty(_namespaceUri))
        {
            // Register a prefix for the namespace. This is needed for XPath queries, since an element in an XPath
            // expression without a prefix is assumed to be in the empty namespace, and NOT in the provided XML namespace
            // manager's default namespace (for some reason). So, we need to register a prefix that we will prepend to
            // elements that are in this namespace (if it is not already the empty namespace) in XPath queries so that we
            // will be able to find elements, and so that callers will not need to provide the appropriate prefixes.
            //
            // See the documentation for the 'XmlNamespaceManager.AddNamespace' method, specifically the Note for the
            // 'prefix' parameter.
            _xmlNamespaceManager.AddNamespace(DefaultNamespacePrefixEquivalent, _namespaceUri);
        }
    }

    /// <summary>
    /// Create root element.
    /// </summary>
    /// <param name="name">
    /// Name of the root element
    /// </param>
    /// <returns>
    /// The <see cref="XmlElement"/>.
    /// </returns>
    public XmlElement CreateRootElement(string name)
    {
        return CreateRootElement(name, _namespaceUri);
    }

    private XmlElement CreateRootElement(string name, string namespaceUri)
    {
        namespaceUri ??= _namespaceUri;

        XmlDocument dom = new();
        dom.AppendChild(dom.CreateXmlDeclaration("1.0", "UTF-8", null));
        return (XmlElement)dom.AppendChild(dom.CreateElement(_prefix, name, namespaceUri))!;
    }

    #region PublicSaveDataInTrx

    /// <summary>
    /// Save single fields.
    /// </summary>
    /// <param name="parentXml">
    /// The parent xml.
    /// </param>
    /// <param name="instance">
    /// The instance.
    /// </param>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    public void SaveSingleFields(XmlElement parentXml, object instance, XmlTestStoreParameters? parameters)
    {
        SaveSingleFields(parentXml, instance, null, parameters);
    }

    /// <summary>
    /// Based on the StoreXml* attributes saves simple fields
    /// </summary>
    /// <param name="parentXml">
    /// Parent xml
    /// </param>
    /// <param name="instance">
    /// object to save
    /// </param>
    /// <param name="requestedType">
    /// The requested Type.
    /// </param>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    public void SaveSingleFields(XmlElement parentXml, object? instance, Type? requestedType, XmlTestStoreParameters? parameters)
    {
        if (instance == null)
        {
            return; // nothing to do
        }

        Type type = requestedType ?? instance.GetType();

        foreach (FieldPersistenceInfo info in GetFieldInfos(type))
        {
            object? fieldValue = info.FieldInfo.GetValue(instance);
            if (fieldValue == null)
            {
                continue;
            }

            if (info.FieldAttribute != null)
            {
                SaveObject(fieldValue, parentXml, info.Location, parameters);
            }
            else if (info.SimpleFieldAttribute != null)
            {
                SaveSimpleField(parentXml, info.Location!, fieldValue, info.SimpleFieldAttribute.DefaultValue);
            }
        }
    }

    /// <summary>
    /// Based on the StoreXml* attributes saves simple fields
    /// </summary>
    /// <param name="objectToSave">
    /// The object to save.
    /// </param>
    /// <param name="parentXml">
    /// The parent xml.
    /// </param>
    /// <param name="location">
    /// The location.
    /// </param>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    public void SaveObject(object? objectToSave, XmlElement parentXml, string? location, XmlTestStoreParameters? parameters)
    {
        if (objectToSave == null || location == null)
        {
            return;
        }

        string? nameSpaceUri = _namespaceUri;
        if (objectToSave is IXmlTestStoreCustom customStore)
        {
            nameSpaceUri = customStore.NamespaceUri;
        }

        XmlNode? xmlNode = EnsureLocationExists(parentXml, location, nameSpaceUri);
        TPDebug.Assert(xmlNode != null, "EnsureLocationExists should have returned a node");
        SaveObject(objectToSave, xmlNode, parameters);

        if (xmlNode is XmlElement element &&
            !element.HasAttributes &&
            !element.HasChildNodes &&
            element.InnerText.IsNullOrEmpty())
        {
            element.ParentNode!.RemoveChild(element);    // get rid of empty elements to keep the xml clean
        }
    }

    /// <summary>
    /// Save object.
    /// </summary>
    /// <param name="objectToSave">
    /// The object to save.
    /// </param>
    /// <param name="nodeToSaveAt">
    /// The node to save at.
    /// </param>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    public static void SaveObject(object objectToSave, XmlNode nodeToSaveAt, XmlTestStoreParameters? parameters)
    {
        SaveObject(objectToSave, nodeToSaveAt, parameters, null);
    }

    /// <summary>
    /// Save the object.
    /// </summary>
    /// <param name="objectToSave">
    /// The object to save.
    /// </param>
    /// <param name="nodeToSaveAt">
    /// The node to save at.
    /// </param>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <param name="defaultValue">
    /// The default value.
    /// </param>
    public static void SaveObject(object? objectToSave, XmlNode nodeToSaveAt, XmlTestStoreParameters? parameters, object? defaultValue)
    {
        if (objectToSave == null)
        {
            return;
        }

        if (objectToSave is IXmlTestStore persistable)
        {
            persistable.Save((XmlElement)nodeToSaveAt, parameters);
        }
        else
        {
            SaveSimpleData(objectToSave, nodeToSaveAt, defaultValue);
        }
    }

    /// <summary>
    /// Save simple field.
    /// </summary>
    /// <param name="xml">
    /// The xml.
    /// </param>
    /// <param name="location">
    /// The location.
    /// </param>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <param name="defaultValue">
    /// The default value.
    /// </param>
    public void SaveSimpleField(XmlElement xml, string location, object? value, object? defaultValue)
    {
        if (value == null || value.Equals(defaultValue))
        {
            return;
        }

        XmlNode? saveTarget = EnsureLocationExists(xml, location);
        TPDebug.Assert(saveTarget != null, "EnsureLocationExists should have returned a node");
        SaveSimpleData(value, saveTarget, defaultValue);
    }

    /// <summary>
    /// Save GUID.
    /// </summary>
    /// <param name="xml">
    /// The xml.
    /// </param>
    /// <param name="location">
    /// The location.
    /// </param>
    /// <param name="guid">
    /// The GUID.
    /// </param>
    public void SaveGuid(XmlElement xml, string location, Guid guid)
    {
        SaveSimpleField(xml, location, guid.ToString(), EmptyGuidString);
    }

    public void SaveHashtable(Hashtable ht, XmlElement element, string location, string keyLocation, string? valueLocation, string itemElementName, XmlTestStoreParameters? parameters)
    {
        if (ht == null || ht.Count <= 0)
        {
            return;
        }

        XmlElement? dictionaryElement = (XmlElement?)EnsureLocationExists(element, location);
        TPDebug.Assert(dictionaryElement != null, "EnsureLocationExists should have returned a node");
        foreach (DictionaryEntry de in ht)
        {
            XmlElement itemXml = CreateElement(dictionaryElement, itemElementName);

            SaveObject(de.Key, itemXml, keyLocation, parameters);
            SaveObject(de.Value, itemXml, valueLocation, parameters);
        }
    }

    public void SaveStringDictionary(StringDictionary dict, XmlElement element, string location, string keyLocation, string valueLocation, string itemElementName, XmlTestStoreParameters parameters)
    {
        if (dict == null || dict.Count <= 0)
        {
            return;
        }

        XmlElement? dictionaryElement = (XmlElement?)EnsureLocationExists(element, location);
        TPDebug.Assert(dictionaryElement != null, "EnsureLocationExists should have returned a node");
        foreach (DictionaryEntry de in dict)
        {
            XmlElement itemXml = CreateElement(dictionaryElement, itemElementName);

            SaveObject(de.Key, itemXml, keyLocation, parameters);
            SaveObject(de.Value, itemXml, valueLocation, parameters);
        }
    }

    #region Lists
    /// <summary>
    /// Save list of object .
    /// </summary>
    /// <param name="list">
    /// The list.
    /// </param>
    /// <param name="element">
    /// The parent element.
    /// </param>
    /// <param name="listXmlElement">
    /// The list xml element.
    /// </param>
    /// <param name="itemLocation">
    /// The item location.
    /// </param>
    /// <param name="itemElementName">
    /// The item element name.
    /// </param>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    public void SaveIEnumerable(IEnumerable? list, XmlElement element, string listXmlElement, string itemLocation, string? itemElementName, XmlTestStoreParameters? parameters)
    {
        if (list == null || !list.GetEnumerator().MoveNext())
        {
            return;
        }

        XmlElement? listElement = (XmlElement?)EnsureLocationExists(element, listXmlElement);
        TPDebug.Assert(listElement != null, "EnsureLocationExists should have returned a node");
        foreach (object item in list)
        {
            XmlElement itemXml = CreateElement(listElement, itemElementName, item);
            SaveObject(item, itemXml, itemLocation, parameters);
        }
    }

    /// <summary>
    /// Save list.
    /// </summary>
    /// <param name="list">
    /// The list.
    /// </param>
    /// <param name="element">
    /// The element.
    /// </param>
    /// <param name="listXmlElement">
    /// The list xml element.
    /// </param>
    /// <param name="itemLocation">
    /// The item location.
    /// </param>
    /// <param name="itemElementName">
    /// The item element name.
    /// </param>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <typeparam name="V"> Generic parameter
    /// </typeparam>
    public void SaveList<V>(IList<V> list, XmlElement element, string listXmlElement, string itemLocation, string itemElementName, XmlTestStoreParameters parameters)
        where V : notnull
    {
        if (list == null || list.Count <= 0)
        {
            return;
        }

        XmlElement? listElement = (XmlElement?)EnsureLocationExists(element, listXmlElement);
        TPDebug.Assert(listElement != null, "EnsureLocationExists should have returned a node");
        foreach (V item in list)
        {
            XmlElement itemXml = CreateElement(listElement, itemElementName, item);
            SaveObject(item, itemXml, itemLocation, parameters);
        }
    }

    #region Counters

    public void SaveCounters(XmlElement xml, string location, int[] counters)
    {
        xml = (XmlElement)LocationToXmlNode(xml, location)!;

        for (int i = 0; i < counters.Length; i++)
        {
            TrxObjectModel.TestOutcome outcome = (TrxObjectModel.TestOutcome)i;
            string attributeName = outcome.ToString();
            attributeName = attributeName.Substring(0, 1).ToLowerInvariant() + attributeName.Substring(1);

            xml.SetAttribute(attributeName, counters[i].ToString(CultureInfo.InvariantCulture));
        }
    }

    #endregion Counters

    #endregion List

    internal static void SaveUsingReflection(XmlElement element, object instance, Type? requestedType, XmlTestStoreParameters? parameters)
    {
        XmlPersistence helper = new();
        helper.SaveSingleFields(element, instance, requestedType, parameters);
    }

    #endregion PublicSaveDataInTrx

    #region Utilities

    #region Optimization: Reflection caching

    /// <summary>
    /// Updates the cache if needed and gets the field info collection
    /// </summary>
    /// <param name="type">
    /// The type.
    /// </param>
    /// <returns>
    /// The <see cref="IEnumerable"/>.
    /// </returns>
    private static IEnumerable<FieldPersistenceInfo> GetFieldInfos(Type type)
    {
        if (!TypeToPersistenceInfoCache.TryGetValue(type, out var toReturn))
        {
            toReturn = ReflectFields(type);
            lock (TypeToPersistenceInfoCache)
            {
                if (!TypeToPersistenceInfoCache.TryGetValue(type, out var checkCache))
                {
                    TypeToPersistenceInfoCache.Add(type, toReturn);
                }
            }
        }

        return toReturn;
    }

    /// <summary>
    /// EXPENSIVE! Uses reflection to build a cache item of information about the type and its fields
    /// </summary>
    /// <param name="type">the type to reflect on</param>
    /// <returns>collection of field information</returns>
    private static IEnumerable<FieldPersistenceInfo> ReflectFields(Type type)
    {
        List<FieldPersistenceInfo> toReturn = new();

        foreach (
            FieldInfo reflectedFieldInfo in
            type.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
        {
            FieldPersistenceInfo info = new(reflectedFieldInfo);

            // only fields with known location need to be persisted
            if (!string.IsNullOrEmpty(info.Location))
            {
                toReturn.Add(info);
            }
        }

        return toReturn;
    }

    #endregion Optimization: Reflection caching

    /// <summary>
    /// Convert dateTime to string.
    /// </summary>
    /// <param name="dateTime">
    /// The date time.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    private static string DateTimeToString(DateTime dateTime)
    {
        if (dateTime == DateTime.MinValue)
        {
            // DateTime.MinValue has Kind.Unspecified and thus it does not make any sense to convert it to local/universal.
            // Also note that we use format w/o time zones to persist DateTime.MinValue.
            return dateTime.ToString(DateTimeUtcPersistenceFormat, CultureInfo.InvariantCulture.DateTimeFormat);
        }
        else
        {
            // Ensure that the datetime value is in local time..
            // This is needed as the persistence format we use needs the datetime to be in local time..
            DateTime localDateTime = dateTime.ToLocalTime();
            return localDateTime.ToString(DateTimePersistenceFormat, CultureInfo.InvariantCulture.DateTimeFormat);
        }
    }

    private static string? GetFieldLocation(FieldInfo fieldInfo)
    {
        string? location = null;

        StoreXmlAttribute? locationAttribute = GetAttribute<StoreXmlAttribute>(fieldInfo);
        if (locationAttribute != null)
        {
            location = locationAttribute.Location ?? GetDefaultFieldLocation(fieldInfo);
        }

        return location;
    }

    private static string GetDefaultFieldLocation(FieldInfo fieldInfo)
    {
        string fieldName = fieldInfo.Name;
        string defaultFieldLocation = fieldName.StartsWith("m_", StringComparison.Ordinal) ? fieldName.Substring(2, fieldName.Length - 2) : fieldName;

        if (!ImplementsIXmlTestStore(fieldInfo.FieldType))
        {
            defaultFieldLocation = '@' + defaultFieldLocation;
        }

        return defaultFieldLocation;
    }

    private static bool ImplementsIXmlTestStore(Type type)
    {
        return type.GetInterface(typeof(IXmlTestStore).Name) != null;
    }

    private static T? GetAttribute<T>(FieldInfo fieldInfo) where T : Attribute
    {
        var attributes = fieldInfo.GetCustomAttributes(typeof(T), false).ToArray();

        return attributes.Length > 0 ? (T)attributes[0] : default;
    }

    private static void SaveSimpleData(object? value, XmlNode nodeToSaveAt, object? defaultValue)
    {
        if (value == null || value.Equals(defaultValue))
        {
            return;
        }

        Type valueType = value.GetType();

        string? valueToSave;
        if (valueType == BoolType)
        {
            valueToSave = value.ToString()!.ToLowerInvariant();
        }
        else if (valueType == ByteArrayType)
        {
            // Use only for Arrays, Collections and Lists. E.g. string is also IEnumerable.
            valueToSave = Convert.ToBase64String((byte[])value);
        }
        else if (valueType == DateTimeType)
        {
            // always save as a sortable date time with fractional seconds.
            valueToSave = DateTimeToString((DateTime)value);
        }
        else
        {
            // use the appropriate type converter to get a culture invariant string.
            try
            {
                TypeConverter convert = TypeDescriptor.GetConverter(valueType);
                valueToSave = convert.ConvertToInvariantString(value);
            }
            catch (NotSupportedException nosupportEx)
            {
                EqtTrace.Info("TypeConverter not supported for {0} : NotSupportedException: {1}", value.ToString(), nosupportEx);
                valueToSave = value.ToString();
            }
        }

        // Remove invalid char if any
        valueToSave = RemoveInvalidXmlChar(valueToSave);
        if (nodeToSaveAt is XmlElement elementToSaveAt)
        {
            TPDebug.Assert(valueToSave is not null, "valueToSave is null");
            elementToSaveAt.InnerText = valueToSave;
        }
        else
        {
            nodeToSaveAt.Value = valueToSave;
        }
    }

    public XmlNode? EnsureLocationExists(XmlElement xml, string location)
    {
        return EnsureLocationExists(xml, location, _namespaceUri);
    }

    private static string? RemoveInvalidXmlChar(string? str)
    {
        if (str == null)
        {
            return null;
        }

        // From xml spec (http://www.w3.org/TR/xml/#charsets) valid chars:
        // #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]

        // we are handling only #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD]
        // because C# support unicode character in range \u0000 to \uFFFF
        MatchEvaluator evaluator = new(ReplaceInvalidCharacterWithUniCodeEscapeSequence);
        string invalidChar = @"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD]";
        return Regex.Replace(str, invalidChar, evaluator);
    }

    private static string ReplaceInvalidCharacterWithUniCodeEscapeSequence(Match match)
    {
        char x = match.Value[0];
        return $@"\u{(ushort)x:x4}";
    }

    private XmlNode? EnsureLocationExists(XmlElement xml, string location, string? nameSpaceUri)
    {
        XmlNode? node = LocationToXmlNode(xml, location);
        if (node != null)
        {
            return node;
        }

        if (location.StartsWith("@", StringComparison.Ordinal))
        {
            string attributeName = location.Substring(1, location.Length - 1);
            return xml.HasAttribute(attributeName)
                ? xml.GetAttributeNode(attributeName)
                : (XmlNode)xml.Attributes.Append(xml.OwnerDocument.CreateAttribute(attributeName));
        }
        else
        {
            string[] parts = location.Split(new char[] { '/' }, 2);
            string firstPart = parts[0];

            XmlNode? firstChild = LocationToXmlNode(xml, firstPart);
            firstChild ??= CreateElement(xml, firstPart, GetNamespaceUriOrDefault(nameSpaceUri));

            return parts.Length > 1 ? EnsureLocationExists((XmlElement)firstChild, parts[1]) : firstChild;
        }
    }

    private string GetNamespaceUriOrDefault(string? nameSpaceUri)
    {
        return nameSpaceUri ?? _namespaceUri;
    }

    /// <summary>
    /// Creates a new element with the given name in the current (of this instance of XmlPersistence namespace)
    /// </summary>
    /// <param name="xml">parent xml</param>
    /// <param name="name">element name</param>
    /// <returns>a new XmlElement attached to the parent</returns>
    private XmlElement CreateElement(XmlElement xml, string name)
    {
        return CreateElement(xml, name, _namespaceUri);
    }

    private XmlElement CreateElement(XmlElement xml, string name, string? elementNamespaceUri)
    {
        return (XmlElement)xml.AppendChild(xml.OwnerDocument.CreateElement(_prefix, name, elementNamespaceUri))!;
    }

    /// <summary>
    /// Accepts name == null
    /// </summary>
    /// <param name="parent"> parent xml</param>
    /// <param name="name">The local name of the new element. </param>
    /// <param name="instance">the object for which element has to create</param>
    /// <returns>a new XmlElement attached to the parent</returns>
    private XmlElement CreateElement(XmlElement parent, string? name, object instance)
    {
        if (name != null)
        {
            return CreateElement(parent, name);
        }
        else
        {
            NewElementCreateData createData = GetElementCreateData(instance);
            TPDebug.Assert(createData.ElementName is not null, "createData.ElementName is null");
            return CreateElement(parent, createData.ElementName, createData.NamespaceUri);
        }
    }

    private NewElementCreateData GetElementCreateData(object persistee)
    {
        TPDebug.Assert(persistee != null, "persistee is null");

        NewElementCreateData toReturn = new();
        if (persistee is IXmlTestStoreCustom custom)
        {
            toReturn.ElementName = custom.ElementName;
            toReturn.NamespaceUri = custom.NamespaceUri;
        }

        toReturn.ElementName ??= persistee.GetType().Name;

        toReturn.NamespaceUri ??= _namespaceUri;

        return toReturn;
    }

    private XmlNode? LocationToXmlNode(XmlElement element, string location)
    {
        location = ProcessXPathQuery(location);

        try
        {
            return element.SelectSingleNode(location, _xmlNamespaceManager);
        }
        catch (System.Xml.XPath.XPathException e)
        {
            throw new Exception($"The persistence location is invalid. Element: '{element.Name}', location: '{location}'", e);
        }
    }

    private string ProcessXPathQuery(string queryIn)
    {
        // If we are working with the empty namespace, there is no need to decorate elements in the XPath expression, since
        // elements in the XPath expression that don't have a prefix will be searched for in the empty namespace.
        if (string.IsNullOrEmpty(_namespaceUri))
        {
            return queryIn;
        }

        if (QueryCache.ContainsKey(queryIn))
        {
            return QueryCache[queryIn];
        }

        // fix the empty namespaces to a temp prefix, so xpath query can understand them
        string[] parts = queryIn.Split(new char[] { '/' }, StringSplitOptions.None);

        StringBuilder query = new();

        foreach (string part in parts)
        {
            if (query.Length > 0 || queryIn.StartsWith("/", StringComparison.Ordinal))
            {
                query.Append('/');
            }

            if (part != "." && part != ".." && !part.Contains(":") && (part.Length > 0) && (!part.StartsWith("@", StringComparison.Ordinal)))
            {
                query.Append(DefaultNamespacePrefixEquivalent + ":");
            }

            query.Append(part);
        }

        string queryString = query.ToString();
        QueryCache[queryIn] = queryString;

        return queryString;
    }

    #endregion Utilities

    #region Types

    private class NewElementCreateData
    {
        public string? NamespaceUri { get; set; }

        public string? ElementName { get; set; }
    }

    /// <summary>
    /// caches information about a field
    /// </summary>
    private class FieldPersistenceInfo
    {
        internal readonly FieldInfo FieldInfo;

        internal readonly string? Location;

        internal readonly StoreXmlAttribute? Attribute;

        internal readonly StoreXmlSimpleFieldAttribute? SimpleFieldAttribute;

        internal readonly StoreXmlFieldAttribute? FieldAttribute;

        internal FieldPersistenceInfo(FieldInfo fieldInfo)
        {
            FieldInfo = fieldInfo;
            Location = GetFieldLocation(fieldInfo);

            Attribute = GetAttribute<StoreXmlAttribute>(fieldInfo);
            SimpleFieldAttribute = Attribute as StoreXmlSimpleFieldAttribute;
            FieldAttribute = Attribute as StoreXmlFieldAttribute;
        }
    }

    #endregion
}
