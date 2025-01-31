// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Configuration.ConfigurationManager")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides types that support using configuration files.\r\n\r\nCommonly Used Types:\r\nSystem.Configuration.Configuration\r\nSystem.Configuration.ConfigurationManager")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Configuration.ConfigurationManager")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System
{
    public enum UriIdnScope
    {
        None = 0,
        AllExceptIntranet = 1,
        All = 2
    }
}

namespace System.Configuration
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class ApplicationScopedSettingAttribute : SettingAttribute
    {
    }

    public abstract partial class ApplicationSettingsBase : SettingsBase, ComponentModel.INotifyPropertyChanged
    {
        protected ApplicationSettingsBase() { }

        protected ApplicationSettingsBase(ComponentModel.IComponent owner, string settingsKey) { }

        protected ApplicationSettingsBase(ComponentModel.IComponent owner) { }

        protected ApplicationSettingsBase(string settingsKey) { }

        [ComponentModel.Browsable(false)]
        public override SettingsContext Context { get { throw null; } }

        public override object this[string propertyName] { get { throw null; } set { } }

        [ComponentModel.Browsable(false)]
        public override SettingsPropertyCollection Properties { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public override SettingsPropertyValueCollection PropertyValues { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public override SettingsProviderCollection Providers { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public string SettingsKey { get { throw null; } set { } }

        public event ComponentModel.PropertyChangedEventHandler PropertyChanged { add { } remove { } }

        public event SettingChangingEventHandler SettingChanging { add { } remove { } }

        public event SettingsLoadedEventHandler SettingsLoaded { add { } remove { } }

        public event SettingsSavingEventHandler SettingsSaving { add { } remove { } }

        public object GetPreviousVersion(string propertyName) { throw null; }

        protected virtual void OnPropertyChanged(object sender, ComponentModel.PropertyChangedEventArgs e) { }

        protected virtual void OnSettingChanging(object sender, SettingChangingEventArgs e) { }

        protected virtual void OnSettingsLoaded(object sender, SettingsLoadedEventArgs e) { }

        protected virtual void OnSettingsSaving(object sender, ComponentModel.CancelEventArgs e) { }

        public void Reload() { }

        public void Reset() { }

        public override void Save() { }

        public virtual void Upgrade() { }
    }

    public sealed partial class ApplicationSettingsGroup : ConfigurationSectionGroup
    {
    }

    public partial class AppSettingsReader
    {
        public object GetValue(string key, Type type) { throw null; }
    }

    public sealed partial class AppSettingsSection : ConfigurationSection
    {
        public string File { get { throw null; } set { } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public KeyValueConfigurationCollection Settings { get { throw null; } }

        protected internal override void DeserializeElement(Xml.XmlReader reader, bool serializeCollectionKey) { }

        protected internal override object GetRuntimeObject() { throw null; }

        protected internal override void Reset(ConfigurationElement parentSection) { }
    }

    public sealed partial class CallbackValidator : ConfigurationValidatorBase
    {
        public CallbackValidator(Type type, ValidatorCallback callback) { }

        public override bool CanValidate(Type type) { throw null; }

        public override void Validate(object value) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class CallbackValidatorAttribute : ConfigurationValidatorAttribute
    {
        public string CallbackMethodName { get { throw null; } set { } }

        public Type Type { get { throw null; } set { } }

        public override ConfigurationValidatorBase ValidatorInstance { get { throw null; } }
    }

    public sealed partial class ClientSettingsSection : ConfigurationSection
    {
        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public SettingElementCollection Settings { get { throw null; } }
    }

    public sealed partial class CommaDelimitedStringCollection : Collections.Specialized.StringCollection
    {
        public bool IsModified { get { throw null; } }

        public new bool IsReadOnly { get { throw null; } }

        public new string this[int index] { get { throw null; } set { } }

        public new void Add(string value) { }

        public new void AddRange(string[] range) { }

        public new void Clear() { }

        public CommaDelimitedStringCollection Clone() { throw null; }

        public new void Insert(int index, string value) { }

        public new void Remove(string value) { }

        public void SetReadOnly() { }

        public override string ToString() { throw null; }
    }

    public sealed partial class CommaDelimitedStringCollectionConverter : ConfigurationConverterBase
    {
        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object data) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object value, Type type) { throw null; }
    }

    public sealed partial class Configuration
    {
        internal Configuration() { }

        public AppSettingsSection AppSettings { get { throw null; } }

        public Func<string, string> AssemblyStringTransformer { get { throw null; } set { } }

        public ConnectionStringsSection ConnectionStrings { get { throw null; } }

        public ContextInformation EvaluationContext { get { throw null; } }

        public string FilePath { get { throw null; } }

        public bool HasFile { get { throw null; } }

        public ConfigurationLocationCollection Locations { get { throw null; } }

        public bool NamespaceDeclared { get { throw null; } set { } }

        public ConfigurationSectionGroup RootSectionGroup { get { throw null; } }

        public ConfigurationSectionGroupCollection SectionGroups { get { throw null; } }

        public ConfigurationSectionCollection Sections { get { throw null; } }

        public Runtime.Versioning.FrameworkName TargetFramework { get { throw null; } set { } }

        public Func<string, string> TypeStringTransformer { get { throw null; } set { } }

        public ConfigurationSection GetSection(string sectionName) { throw null; }

        public ConfigurationSectionGroup GetSectionGroup(string sectionGroupName) { throw null; }

        public void Save() { }

        public void Save(ConfigurationSaveMode saveMode, bool forceSaveAll) { }

        public void Save(ConfigurationSaveMode saveMode) { }

        public void SaveAs(string filename, ConfigurationSaveMode saveMode, bool forceSaveAll) { }

        public void SaveAs(string filename, ConfigurationSaveMode saveMode) { }

        public void SaveAs(string filename) { }
    }

    public enum ConfigurationAllowDefinition
    {
        MachineOnly = 0,
        MachineToWebRoot = 100,
        MachineToApplication = 200,
        Everywhere = 300
    }

    public enum ConfigurationAllowExeDefinition
    {
        MachineOnly = 0,
        MachineToApplication = 100,
        MachineToRoamingUser = 200,
        MachineToLocalUser = 300
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed partial class ConfigurationCollectionAttribute : Attribute
    {
        public ConfigurationCollectionAttribute(Type itemType) { }

        public string AddItemName { get { throw null; } set { } }

        public string ClearItemsName { get { throw null; } set { } }

        public ConfigurationElementCollectionType CollectionType { get { throw null; } set { } }

        public Type ItemType { get { throw null; } }

        public string RemoveItemName { get { throw null; } set { } }
    }

    public abstract partial class ConfigurationConverterBase : ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext ctx, Type type) { throw null; }

        public override bool CanConvertTo(ComponentModel.ITypeDescriptorContext ctx, Type type) { throw null; }
    }

    public abstract partial class ConfigurationElement
    {
        public Configuration CurrentConfiguration { get { throw null; } }

        public ElementInformation ElementInformation { get { throw null; } }

        protected internal virtual ConfigurationElementProperty ElementProperty { get { throw null; } }

        protected ContextInformation EvaluationContext { get { throw null; } }

        protected bool HasContext { get { throw null; } }

        protected internal object this[ConfigurationProperty prop] { get { throw null; } set { } }

        protected internal object this[string propertyName] { get { throw null; } set { } }

        public ConfigurationLockCollection LockAllAttributesExcept { get { throw null; } }

        public ConfigurationLockCollection LockAllElementsExcept { get { throw null; } }

        public ConfigurationLockCollection LockAttributes { get { throw null; } }

        public ConfigurationLockCollection LockElements { get { throw null; } }

        public bool LockItem { get { throw null; } set { } }

        protected internal virtual ConfigurationPropertyCollection Properties { get { throw null; } }

        protected internal virtual void DeserializeElement(Xml.XmlReader reader, bool serializeCollectionKey) { }

        public override bool Equals(object compareTo) { throw null; }

        public override int GetHashCode() { throw null; }

        protected virtual string GetTransformedAssemblyString(string assemblyName) { throw null; }

        protected virtual string GetTransformedTypeString(string typeName) { throw null; }

        protected internal virtual void Init() { }

        protected internal virtual void InitializeDefault() { }

        protected internal virtual bool IsModified() { throw null; }

        public virtual bool IsReadOnly() { throw null; }

        protected virtual void ListErrors(Collections.IList errorList) { }

        protected virtual bool OnDeserializeUnrecognizedAttribute(string name, string value) { throw null; }

        protected virtual bool OnDeserializeUnrecognizedElement(string elementName, Xml.XmlReader reader) { throw null; }

        protected virtual object OnRequiredPropertyNotFound(string name) { throw null; }

        protected virtual void PostDeserialize() { }

        protected virtual void PreSerialize(Xml.XmlWriter writer) { }

        protected internal virtual void Reset(ConfigurationElement parentElement) { }

        protected internal virtual void ResetModified() { }

        protected internal virtual bool SerializeElement(Xml.XmlWriter writer, bool serializeCollectionKey) { throw null; }

        protected internal virtual bool SerializeToXmlElement(Xml.XmlWriter writer, string elementName) { throw null; }

        protected void SetPropertyValue(ConfigurationProperty prop, object value, bool ignoreLocks) { }

        protected internal virtual void SetReadOnly() { }

        protected internal virtual void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode) { }
    }

    public abstract partial class ConfigurationElementCollection : ConfigurationElement, Collections.ICollection, Collections.IEnumerable
    {
        protected ConfigurationElementCollection() { }

        protected ConfigurationElementCollection(Collections.IComparer comparer) { }

        protected internal string AddElementName { get { throw null; } set { } }

        protected internal string ClearElementName { get { throw null; } set { } }

        public virtual ConfigurationElementCollectionType CollectionType { get { throw null; } }

        public int Count { get { throw null; } }

        protected virtual string ElementName { get { throw null; } }

        public bool EmitClear { get { throw null; } set { } }

        public bool IsSynchronized { get { throw null; } }

        protected internal string RemoveElementName { get { throw null; } set { } }

        public object SyncRoot { get { throw null; } }

        protected virtual bool ThrowOnDuplicate { get { throw null; } }

        protected internal void BaseAdd(ConfigurationElement element, bool throwIfExists) { }

        protected virtual void BaseAdd(ConfigurationElement element) { }

        protected virtual void BaseAdd(int index, ConfigurationElement element) { }

        protected internal void BaseClear() { }

        protected internal ConfigurationElement BaseGet(int index) { throw null; }

        protected internal ConfigurationElement BaseGet(object key) { throw null; }

        protected internal object[] BaseGetAllKeys() { throw null; }

        protected internal object BaseGetKey(int index) { throw null; }

        protected int BaseIndexOf(ConfigurationElement element) { throw null; }

        protected internal bool BaseIsRemoved(object key) { throw null; }

        protected internal void BaseRemove(object key) { }

        protected internal void BaseRemoveAt(int index) { }

        public void CopyTo(ConfigurationElement[] array, int index) { }

        protected abstract ConfigurationElement CreateNewElement();
        protected virtual ConfigurationElement CreateNewElement(string elementName) { throw null; }

        public override bool Equals(object compareTo) { throw null; }

        protected abstract object GetElementKey(ConfigurationElement element);
        public Collections.IEnumerator GetEnumerator() { throw null; }

        public override int GetHashCode() { throw null; }

        protected virtual bool IsElementName(string elementName) { throw null; }

        protected virtual bool IsElementRemovable(ConfigurationElement element) { throw null; }

        protected internal override bool IsModified() { throw null; }

        public override bool IsReadOnly() { throw null; }

        protected override bool OnDeserializeUnrecognizedElement(string elementName, Xml.XmlReader reader) { throw null; }

        protected internal override void Reset(ConfigurationElement parentElement) { }

        protected internal override void ResetModified() { }

        protected internal override bool SerializeElement(Xml.XmlWriter writer, bool serializeCollectionKey) { throw null; }

        protected internal override void SetReadOnly() { }

        void Collections.ICollection.CopyTo(Array arr, int index) { }

        protected internal override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode) { }
    }

    public enum ConfigurationElementCollectionType
    {
        BasicMap = 0,
        AddRemoveClearMap = 1,
        BasicMapAlternate = 2,
        AddRemoveClearMapAlternate = 3
    }

    public sealed partial class ConfigurationElementProperty
    {
        public ConfigurationElementProperty(ConfigurationValidatorBase validator) { }

        public ConfigurationValidatorBase Validator { get { throw null; } }
    }

    public partial class ConfigurationErrorsException : ConfigurationException
    {
        public ConfigurationErrorsException() { }

        protected ConfigurationErrorsException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public ConfigurationErrorsException(string message, Exception inner, string filename, int line) { }

        public ConfigurationErrorsException(string message, Exception inner, Xml.XmlNode node) { }

        public ConfigurationErrorsException(string message, Exception inner, Xml.XmlReader reader) { }

        public ConfigurationErrorsException(string message, Exception inner) { }

        public ConfigurationErrorsException(string message, string filename, int line) { }

        public ConfigurationErrorsException(string message, Xml.XmlNode node) { }

        public ConfigurationErrorsException(string message, Xml.XmlReader reader) { }

        public ConfigurationErrorsException(string message) { }

        public Collections.ICollection Errors { get { throw null; } }

        public override string Filename { get { throw null; } }

        public override int Line { get { throw null; } }

        public override string Message { get { throw null; } }

        public static string GetFilename(Xml.XmlNode node) { throw null; }

        public static string GetFilename(Xml.XmlReader reader) { throw null; }

        public static int GetLineNumber(Xml.XmlNode node) { throw null; }

        public static int GetLineNumber(Xml.XmlReader reader) { throw null; }

        public override void GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }
    }

    public partial class ConfigurationException : SystemException
    {
        [Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
        public ConfigurationException() { }

        protected ConfigurationException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        [Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
        public ConfigurationException(string message, Exception inner, string filename, int line) { }

        [Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
        public ConfigurationException(string message, Exception inner, Xml.XmlNode node) { }

        [Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
        public ConfigurationException(string message, Exception inner) { }

        [Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
        public ConfigurationException(string message, string filename, int line) { }

        [Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
        public ConfigurationException(string message, Xml.XmlNode node) { }

        [Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
        public ConfigurationException(string message) { }

        public virtual string BareMessage { get { throw null; } }

        public virtual string Filename { get { throw null; } }

        public virtual int Line { get { throw null; } }

        public override string Message { get { throw null; } }

        public override void GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        [Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException.GetFilename instead.")]
        public static string GetXmlNodeFilename(Xml.XmlNode node) { throw null; }

        [Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException.GetFilename instead.")]
        public static int GetXmlNodeLineNumber(Xml.XmlNode node) { throw null; }
    }

    public partial class ConfigurationFileMap : ICloneable
    {
        public ConfigurationFileMap() { }

        public ConfigurationFileMap(string machineConfigFilename) { }

        public string MachineConfigFilename { get { throw null; } set { } }

        public virtual object Clone() { throw null; }
    }

    public partial class ConfigurationLocation
    {
        internal ConfigurationLocation() { }

        public string Path { get { throw null; } }

        public Configuration OpenConfiguration() { throw null; }
    }

    public partial class ConfigurationLocationCollection : Collections.ReadOnlyCollectionBase
    {
        internal ConfigurationLocationCollection() { }

        public ConfigurationLocation this[int index] { get { throw null; } }
    }

    public sealed partial class ConfigurationLockCollection : Collections.ICollection, Collections.IEnumerable
    {
        internal ConfigurationLockCollection() { }

        public string AttributeList { get { throw null; } }

        public int Count { get { throw null; } }

        public bool HasParentElements { get { throw null; } }

        public bool IsModified { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public object SyncRoot { get { throw null; } }

        public void Add(string name) { }

        public void Clear() { }

        public bool Contains(string name) { throw null; }

        public void CopyTo(string[] array, int index) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        public bool IsReadOnly(string name) { throw null; }

        public void Remove(string name) { }

        public void SetFromList(string attributeList) { }

        void Collections.ICollection.CopyTo(Array array, int index) { }
    }

    public static partial class ConfigurationManager
    {
        public static Collections.Specialized.NameValueCollection AppSettings { get { throw null; } }

        public static ConnectionStringSettingsCollection ConnectionStrings { get { throw null; } }

        public static object GetSection(string sectionName) { throw null; }

        public static Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel) { throw null; }

        public static Configuration OpenExeConfiguration(string exePath) { throw null; }

        public static Configuration OpenMachineConfiguration() { throw null; }

        public static Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel, bool preLoad) { throw null; }

        public static Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel) { throw null; }

        public static Configuration OpenMappedMachineConfiguration(ConfigurationFileMap fileMap) { throw null; }

        public static void RefreshSection(string sectionName) { }
    }

    public sealed partial class ConfigurationProperty
    {
        public ConfigurationProperty(string name, Type type, object defaultValue, ComponentModel.TypeConverter typeConverter, ConfigurationValidatorBase validator, ConfigurationPropertyOptions options, string description) { }

        public ConfigurationProperty(string name, Type type, object defaultValue, ComponentModel.TypeConverter typeConverter, ConfigurationValidatorBase validator, ConfigurationPropertyOptions options) { }

        public ConfigurationProperty(string name, Type type, object defaultValue, ConfigurationPropertyOptions options) { }

        public ConfigurationProperty(string name, Type type, object defaultValue) { }

        public ConfigurationProperty(string name, Type type) { }

        public ComponentModel.TypeConverter Converter { get { throw null; } }

        public object DefaultValue { get { throw null; } }

        public string Description { get { throw null; } }

        public bool IsAssemblyStringTransformationRequired { get { throw null; } }

        public bool IsDefaultCollection { get { throw null; } }

        public bool IsKey { get { throw null; } }

        public bool IsRequired { get { throw null; } }

        public bool IsTypeStringTransformationRequired { get { throw null; } }

        public bool IsVersionCheckRequired { get { throw null; } }

        public string Name { get { throw null; } }

        public Type Type { get { throw null; } }

        public ConfigurationValidatorBase Validator { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class ConfigurationPropertyAttribute : Attribute
    {
        public ConfigurationPropertyAttribute(string name) { }

        public object DefaultValue { get { throw null; } set { } }

        public bool IsDefaultCollection { get { throw null; } set { } }

        public bool IsKey { get { throw null; } set { } }

        public bool IsRequired { get { throw null; } set { } }

        public string Name { get { throw null; } }

        public ConfigurationPropertyOptions Options { get { throw null; } set { } }
    }

    public partial class ConfigurationPropertyCollection : Collections.ICollection, Collections.IEnumerable
    {
        public int Count { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public ConfigurationProperty this[string name] { get { throw null; } }

        public object SyncRoot { get { throw null; } }

        public void Add(ConfigurationProperty property) { }

        public void Clear() { }

        public bool Contains(string name) { throw null; }

        public void CopyTo(ConfigurationProperty[] array, int index) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        public bool Remove(string name) { throw null; }

        void Collections.ICollection.CopyTo(Array array, int index) { }
    }

    [Flags]
    public enum ConfigurationPropertyOptions
    {
        None = 0,
        IsDefaultCollection = 1,
        IsRequired = 2,
        IsKey = 4,
        IsTypeStringTransformationRequired = 8,
        IsAssemblyStringTransformationRequired = 16,
        IsVersionCheckRequired = 32
    }

    public enum ConfigurationSaveMode
    {
        Modified = 0,
        Minimal = 1,
        Full = 2
    }

    public abstract partial class ConfigurationSection : ConfigurationElement
    {
        public SectionInformation SectionInformation { get { throw null; } }

        protected internal virtual void DeserializeSection(Xml.XmlReader reader) { }

        protected internal virtual object GetRuntimeObject() { throw null; }

        protected internal override bool IsModified() { throw null; }

        protected internal override void ResetModified() { }

        protected internal virtual string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode) { throw null; }

        protected internal virtual bool ShouldSerializeElementInTargetVersion(ConfigurationElement element, string elementName, Runtime.Versioning.FrameworkName targetFramework) { throw null; }

        protected internal virtual bool ShouldSerializePropertyInTargetVersion(ConfigurationProperty property, string propertyName, Runtime.Versioning.FrameworkName targetFramework, ConfigurationElement parentConfigurationElement) { throw null; }

        protected internal virtual bool ShouldSerializeSectionInTargetVersion(Runtime.Versioning.FrameworkName targetFramework) { throw null; }
    }

    public sealed partial class ConfigurationSectionCollection : Collections.Specialized.NameObjectCollectionBase
    {
        internal ConfigurationSectionCollection() { }

        public ConfigurationSection this[int index] { get { throw null; } }

        public ConfigurationSection this[string name] { get { throw null; } }

        public void Add(string name, ConfigurationSection section) { }

        public void Clear() { }

        public void CopyTo(ConfigurationSection[] array, int index) { }

        public ConfigurationSection Get(int index) { throw null; }

        public ConfigurationSection Get(string name) { throw null; }

        public override Collections.IEnumerator GetEnumerator() { throw null; }

        public string GetKey(int index) { throw null; }

        public void Remove(string name) { }

        public void RemoveAt(int index) { }
    }

    public partial class ConfigurationSectionGroup
    {
        public bool IsDeclarationRequired { get { throw null; } }

        public bool IsDeclared { get { throw null; } }

        public string Name { get { throw null; } }

        public string SectionGroupName { get { throw null; } }

        public ConfigurationSectionGroupCollection SectionGroups { get { throw null; } }

        public ConfigurationSectionCollection Sections { get { throw null; } }

        public string Type { get { throw null; } set { } }

        public void ForceDeclaration() { }

        public void ForceDeclaration(bool force) { }

        protected internal virtual bool ShouldSerializeSectionGroupInTargetVersion(Runtime.Versioning.FrameworkName targetFramework) { throw null; }
    }

    public sealed partial class ConfigurationSectionGroupCollection : Collections.Specialized.NameObjectCollectionBase
    {
        internal ConfigurationSectionGroupCollection() { }

        public ConfigurationSectionGroup this[int index] { get { throw null; } }

        public ConfigurationSectionGroup this[string name] { get { throw null; } }

        public void Add(string name, ConfigurationSectionGroup sectionGroup) { }

        public void Clear() { }

        public void CopyTo(ConfigurationSectionGroup[] array, int index) { }

        public ConfigurationSectionGroup Get(int index) { throw null; }

        public ConfigurationSectionGroup Get(string name) { throw null; }

        public override Collections.IEnumerator GetEnumerator() { throw null; }

        public string GetKey(int index) { throw null; }

        public void Remove(string name) { }

        public void RemoveAt(int index) { }
    }

    public sealed partial class ConfigurationSettings
    {
        internal ConfigurationSettings() { }

        [Obsolete("ConfigurationSettings.AppSettings has been deprecated. Use System.Configuration.ConfigurationManager.AppSettings instead.")]
        public static Collections.Specialized.NameValueCollection AppSettings { get { throw null; } }

        [Obsolete("ConfigurationSettings.GetConfig has been deprecated. Use System.Configuration.ConfigurationManager.GetSection instead.")]
        public static object GetConfig(string sectionName) { throw null; }
    }

    public enum ConfigurationUserLevel
    {
        None = 0,
        PerUserRoaming = 10,
        PerUserRoamingAndLocal = 20
    }

    [AttributeUsage(AttributeTargets.Property)]
    public partial class ConfigurationValidatorAttribute : Attribute
    {
        protected ConfigurationValidatorAttribute() { }

        public ConfigurationValidatorAttribute(Type validator) { }

        public virtual ConfigurationValidatorBase ValidatorInstance { get { throw null; } }

        public Type ValidatorType { get { throw null; } }
    }

    public abstract partial class ConfigurationValidatorBase
    {
        public virtual bool CanValidate(Type type) { throw null; }

        public abstract void Validate(object value);
    }

    public sealed partial class ConfigXmlDocument : Xml.XmlDocument, Internal.IConfigErrorInfo
    {
        public string Filename { get { throw null; } }

        public int LineNumber { get { throw null; } }

        string Internal.IConfigErrorInfo.Filename { get { throw null; } }

        int Internal.IConfigErrorInfo.LineNumber { get { throw null; } }

        public override Xml.XmlAttribute CreateAttribute(string prefix, string localName, string namespaceUri) { throw null; }

        public override Xml.XmlCDataSection CreateCDataSection(string data) { throw null; }

        public override Xml.XmlComment CreateComment(string data) { throw null; }

        public override Xml.XmlElement CreateElement(string prefix, string localName, string namespaceUri) { throw null; }

        public override Xml.XmlSignificantWhitespace CreateSignificantWhitespace(string data) { throw null; }

        public override Xml.XmlText CreateTextNode(string text) { throw null; }

        public override Xml.XmlWhitespace CreateWhitespace(string data) { throw null; }

        public override void Load(string filename) { }

        public void LoadSingleElement(string filename, Xml.XmlTextReader sourceReader) { }
    }

    public sealed partial class ConnectionStringSettings : ConfigurationElement
    {
        public ConnectionStringSettings() { }

        public ConnectionStringSettings(string name, string connectionString, string providerName) { }

        public ConnectionStringSettings(string name, string connectionString) { }

        public string ConnectionString { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public string ProviderName { get { throw null; } set { } }

        public override string ToString() { throw null; }
    }

    [ConfigurationCollection(typeof(ConnectionStringSettings))]
    public sealed partial class ConnectionStringSettingsCollection : ConfigurationElementCollection
    {
        public ConnectionStringSettings this[int index] { get { throw null; } set { } }

        public ConnectionStringSettings this[string name] { get { throw null; } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public void Add(ConnectionStringSettings settings) { }

        protected override void BaseAdd(int index, ConfigurationElement element) { }

        public void Clear() { }

        protected override ConfigurationElement CreateNewElement() { throw null; }

        protected override object GetElementKey(ConfigurationElement element) { throw null; }

        public int IndexOf(ConnectionStringSettings settings) { throw null; }

        public void Remove(ConnectionStringSettings settings) { }

        public void Remove(string name) { }

        public void RemoveAt(int index) { }
    }

    public sealed partial class ConnectionStringsSection : ConfigurationSection
    {
        public ConnectionStringSettingsCollection ConnectionStrings { get { throw null; } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        protected internal override object GetRuntimeObject() { throw null; }
    }

    public sealed partial class ContextInformation
    {
        internal ContextInformation() { }

        public object HostingContext { get { throw null; } }

        public bool IsMachineLevel { get { throw null; } }

        public object GetSection(string sectionName) { throw null; }
    }

    public sealed partial class DefaultSection : ConfigurationSection
    {
        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        protected internal override void DeserializeSection(Xml.XmlReader xmlReader) { }

        protected internal override bool IsModified() { throw null; }

        protected internal override void Reset(ConfigurationElement parentSection) { }

        protected internal override void ResetModified() { }

        protected internal override string SerializeSection(ConfigurationElement parentSection, string name, ConfigurationSaveMode saveMode) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class DefaultSettingValueAttribute : Attribute
    {
        public DefaultSettingValueAttribute(string value) { }

        public string Value { get { throw null; } }
    }

    public sealed partial class DefaultValidator : ConfigurationValidatorBase
    {
        public override bool CanValidate(Type type) { throw null; }

        public override void Validate(object value) { }
    }

    public partial class DictionarySectionHandler : IConfigurationSectionHandler
    {
        protected virtual string KeyAttributeName { get { throw null; } }

        protected virtual string ValueAttributeName { get { throw null; } }

        public virtual object Create(object parent, object context, Xml.XmlNode section) { throw null; }
    }

    public sealed partial class DpapiProtectedConfigurationProvider : ProtectedConfigurationProvider
    {
        public bool UseMachineProtection { get { throw null; } }

        public override Xml.XmlNode Decrypt(Xml.XmlNode encryptedNode) { throw null; }

        public override Xml.XmlNode Encrypt(Xml.XmlNode node) { throw null; }

        public override void Initialize(string name, Collections.Specialized.NameValueCollection configurationValues) { }
    }

    public sealed partial class ElementInformation
    {
        internal ElementInformation() { }

        public Collections.ICollection Errors { get { throw null; } }

        public bool IsCollection { get { throw null; } }

        public bool IsLocked { get { throw null; } }

        public bool IsPresent { get { throw null; } }

        public int LineNumber { get { throw null; } }

        public PropertyInformationCollection Properties { get { throw null; } }

        public string Source { get { throw null; } }

        public Type Type { get { throw null; } }

        public ConfigurationValidatorBase Validator { get { throw null; } }
    }

    public sealed partial class ExeConfigurationFileMap : ConfigurationFileMap
    {
        public ExeConfigurationFileMap() { }

        public ExeConfigurationFileMap(string machineConfigFileName) { }

        public string ExeConfigFilename { get { throw null; } set { } }

        public string LocalUserConfigFilename { get { throw null; } set { } }

        public string RoamingUserConfigFilename { get { throw null; } set { } }

        public override object Clone() { throw null; }
    }

    public sealed partial class ExeContext
    {
        internal ExeContext() { }

        public string ExePath { get { throw null; } }

        public ConfigurationUserLevel UserLevel { get { throw null; } }
    }

    public sealed partial class GenericEnumConverter : ConfigurationConverterBase
    {
        public GenericEnumConverter(Type typeEnum) { }

        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object data) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object value, Type type) { throw null; }
    }

    public partial interface IApplicationSettingsProvider
    {
        SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property);
        void Reset(SettingsContext context);
        void Upgrade(SettingsContext context, SettingsPropertyCollection properties);
    }

    public partial interface IConfigurationSectionHandler
    {
        object Create(object parent, object configContext, Xml.XmlNode section);
    }

    public partial interface IConfigurationSystem
    {
        object GetConfig(string configKey);
        void Init();
    }

    public sealed partial class IdnElement : ConfigurationElement
    {
        public UriIdnScope Enabled { get { throw null; } set { } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }
    }

    public sealed partial class IgnoreSection : ConfigurationSection
    {
        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        protected internal override void DeserializeSection(Xml.XmlReader xmlReader) { }

        protected internal override bool IsModified() { throw null; }

        protected internal override void Reset(ConfigurationElement parentSection) { }

        protected internal override void ResetModified() { }

        protected internal override string SerializeSection(ConfigurationElement parentSection, string name, ConfigurationSaveMode saveMode) { throw null; }
    }

    public partial class IgnoreSectionHandler : IConfigurationSectionHandler
    {
        public virtual object Create(object parent, object configContext, Xml.XmlNode section) { throw null; }
    }

    public sealed partial class InfiniteIntConverter : ConfigurationConverterBase
    {
        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object data) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object value, Type type) { throw null; }
    }

    public sealed partial class InfiniteTimeSpanConverter : ConfigurationConverterBase
    {
        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object data) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object value, Type type) { throw null; }
    }

    public partial class IntegerValidator : ConfigurationValidatorBase
    {
        public IntegerValidator(int minValue, int maxValue, bool rangeIsExclusive, int resolution) { }

        public IntegerValidator(int minValue, int maxValue, bool rangeIsExclusive) { }

        public IntegerValidator(int minValue, int maxValue) { }

        public override bool CanValidate(Type type) { throw null; }

        public override void Validate(object value) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class IntegerValidatorAttribute : ConfigurationValidatorAttribute
    {
        public bool ExcludeRange { get { throw null; } set { } }

        public int MaxValue { get { throw null; } set { } }

        public int MinValue { get { throw null; } set { } }

        public override ConfigurationValidatorBase ValidatorInstance { get { throw null; } }
    }

    public partial interface IPersistComponentSettings
    {
        bool SaveSettings { get; set; }

        string SettingsKey { get; set; }

        void LoadComponentSettings();
        void ResetComponentSettings();
        void SaveComponentSettings();
    }

    public sealed partial class IriParsingElement : ConfigurationElement
    {
        public bool Enabled { get { throw null; } set { } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }
    }

    public partial interface ISettingsProviderService
    {
        SettingsProvider GetSettingsProvider(SettingsProperty property);
    }

    [ConfigurationCollection(typeof(KeyValueConfigurationElement))]
    public partial class KeyValueConfigurationCollection : ConfigurationElementCollection
    {
        public string[] AllKeys { get { throw null; } }

        public KeyValueConfigurationElement this[string key] { get { throw null; } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        protected override bool ThrowOnDuplicate { get { throw null; } }

        public void Add(KeyValueConfigurationElement keyValue) { }

        public void Add(string key, string value) { }

        public void Clear() { }

        protected override ConfigurationElement CreateNewElement() { throw null; }

        protected override object GetElementKey(ConfigurationElement element) { throw null; }

        public void Remove(string key) { }
    }

    public partial class KeyValueConfigurationElement : ConfigurationElement
    {
        public KeyValueConfigurationElement(string key, string value) { }

        public string Key { get { throw null; } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public string Value { get { throw null; } set { } }

        protected internal override void Init() { }
    }

    public partial class LocalFileSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        public override string ApplicationName { get { throw null; } set { } }

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property) { throw null; }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties) { throw null; }

        public override void Initialize(string name, Collections.Specialized.NameValueCollection values) { }

        public void Reset(SettingsContext context) { }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection values) { }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties) { }
    }

    public partial class LongValidator : ConfigurationValidatorBase
    {
        public LongValidator(long minValue, long maxValue, bool rangeIsExclusive, long resolution) { }

        public LongValidator(long minValue, long maxValue, bool rangeIsExclusive) { }

        public LongValidator(long minValue, long maxValue) { }

        public override bool CanValidate(Type type) { throw null; }

        public override void Validate(object value) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class LongValidatorAttribute : ConfigurationValidatorAttribute
    {
        public bool ExcludeRange { get { throw null; } set { } }

        public long MaxValue { get { throw null; } set { } }

        public long MinValue { get { throw null; } set { } }

        public override ConfigurationValidatorBase ValidatorInstance { get { throw null; } }
    }

    [ConfigurationCollection(typeof(NameValueConfigurationElement))]
    public sealed partial class NameValueConfigurationCollection : ConfigurationElementCollection
    {
        public string[] AllKeys { get { throw null; } }

        public NameValueConfigurationElement this[string name] { get { throw null; } set { } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public void Add(NameValueConfigurationElement nameValue) { }

        public void Clear() { }

        protected override ConfigurationElement CreateNewElement() { throw null; }

        protected override object GetElementKey(ConfigurationElement element) { throw null; }

        public void Remove(NameValueConfigurationElement nameValue) { }

        public void Remove(string name) { }
    }

    public sealed partial class NameValueConfigurationElement : ConfigurationElement
    {
        public NameValueConfigurationElement(string name, string value) { }

        public string Name { get { throw null; } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public string Value { get { throw null; } set { } }
    }

    public partial class NameValueFileSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, Xml.XmlNode section) { throw null; }
    }

    public partial class NameValueSectionHandler : IConfigurationSectionHandler
    {
        protected virtual string KeyAttributeName { get { throw null; } }

        protected virtual string ValueAttributeName { get { throw null; } }

        public object Create(object parent, object context, Xml.XmlNode section) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class NoSettingsVersionUpgradeAttribute : Attribute
    {
    }

    public enum OverrideMode
    {
        Inherit = 0,
        Allow = 1,
        Deny = 2
    }

    public partial class PositiveTimeSpanValidator : ConfigurationValidatorBase
    {
        public override bool CanValidate(Type type) { throw null; }

        public override void Validate(object value) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class PositiveTimeSpanValidatorAttribute : ConfigurationValidatorAttribute
    {
        public override ConfigurationValidatorBase ValidatorInstance { get { throw null; } }
    }

    public sealed partial class PropertyInformation
    {
        internal PropertyInformation() { }

        public ComponentModel.TypeConverter Converter { get { throw null; } }

        public object DefaultValue { get { throw null; } }

        public string Description { get { throw null; } }

        public bool IsKey { get { throw null; } }

        public bool IsLocked { get { throw null; } }

        public bool IsModified { get { throw null; } }

        public bool IsRequired { get { throw null; } }

        public int LineNumber { get { throw null; } }

        public string Name { get { throw null; } }

        public string Source { get { throw null; } }

        public Type Type { get { throw null; } }

        public ConfigurationValidatorBase Validator { get { throw null; } }

        public object Value { get { throw null; } set { } }

        public PropertyValueOrigin ValueOrigin { get { throw null; } }
    }

    public sealed partial class PropertyInformationCollection : Collections.Specialized.NameObjectCollectionBase
    {
        internal PropertyInformationCollection() { }

        public PropertyInformation this[string propertyName] { get { throw null; } }

        public void CopyTo(PropertyInformation[] array, int index) { }

        public override Collections.IEnumerator GetEnumerator() { throw null; }
    }

    public enum PropertyValueOrigin
    {
        Default = 0,
        Inherited = 1,
        SetHere = 2
    }

    public static partial class ProtectedConfiguration
    {
        public const string DataProtectionProviderName = "DataProtectionConfigurationProvider";
        public const string ProtectedDataSectionName = "configProtectedData";
        public const string RsaProviderName = "RsaProtectedConfigurationProvider";
        public static string DefaultProvider { get { throw null; } }

        public static ProtectedConfigurationProviderCollection Providers { get { throw null; } }
    }

    public abstract partial class ProtectedConfigurationProvider : Provider.ProviderBase
    {
        public abstract Xml.XmlNode Decrypt(Xml.XmlNode encryptedNode);
        public abstract Xml.XmlNode Encrypt(Xml.XmlNode node);
    }

    public partial class ProtectedConfigurationProviderCollection : Provider.ProviderCollection
    {
        public new ProtectedConfigurationProvider this[string name] { get { throw null; } }

        public override void Add(Provider.ProviderBase provider) { }
    }

    public sealed partial class ProtectedConfigurationSection : ConfigurationSection
    {
        public string DefaultProvider { get { throw null; } set { } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public ProviderSettingsCollection Providers { get { throw null; } }
    }

    public partial class ProtectedProviderSettings : ConfigurationElement
    {
        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public ProviderSettingsCollection Providers { get { throw null; } }
    }

    public sealed partial class ProviderSettings : ConfigurationElement
    {
        public ProviderSettings() { }

        public ProviderSettings(string name, string type) { }

        public string Name { get { throw null; } set { } }

        public Collections.Specialized.NameValueCollection Parameters { get { throw null; } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public string Type { get { throw null; } set { } }

        protected internal override bool IsModified() { throw null; }

        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value) { throw null; }

        protected internal override void Reset(ConfigurationElement parentElement) { }

        protected internal override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode) { }
    }

    [ConfigurationCollection(typeof(ProviderSettings))]
    public sealed partial class ProviderSettingsCollection : ConfigurationElementCollection
    {
        public ProviderSettings this[int index] { get { throw null; } set { } }

        public ProviderSettings this[string key] { get { throw null; } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public void Add(ProviderSettings provider) { }

        public void Clear() { }

        protected override ConfigurationElement CreateNewElement() { throw null; }

        protected override object GetElementKey(ConfigurationElement element) { throw null; }

        public void Remove(string name) { }
    }

    public partial class RegexStringValidator : ConfigurationValidatorBase
    {
        public RegexStringValidator(string regex) { }

        public override bool CanValidate(Type type) { throw null; }

        public override void Validate(object value) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class RegexStringValidatorAttribute : ConfigurationValidatorAttribute
    {
        public RegexStringValidatorAttribute(string regex) { }

        public string Regex { get { throw null; } }

        public override ConfigurationValidatorBase ValidatorInstance { get { throw null; } }
    }

    public sealed partial class RsaProtectedConfigurationProvider : ProtectedConfigurationProvider
    {
        public string CspProviderName { get { throw null; } }

        public string KeyContainerName { get { throw null; } }

        public Security.Cryptography.RSAParameters RsaPublicKey { get { throw null; } }

        public bool UseFIPS { get { throw null; } }

        public bool UseMachineContainer { get { throw null; } }

        public bool UseOAEP { get { throw null; } }

        public void AddKey(int keySize, bool exportable) { }

        public override Xml.XmlNode Decrypt(Xml.XmlNode encryptedNode) { throw null; }

        public void DeleteKey() { }

        public override Xml.XmlNode Encrypt(Xml.XmlNode node) { throw null; }

        public void ExportKey(string xmlFileName, bool includePrivateParameters) { }

        public void ImportKey(string xmlFileName, bool exportable) { }
    }

    public sealed partial class SchemeSettingElement : ConfigurationElement
    {
        public GenericUriParserOptions GenericUriParserOptions { get { throw null; } }

        public string Name { get { throw null; } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }
    }

    [ConfigurationCollection(typeof(SchemeSettingElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap, AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
    public sealed partial class SchemeSettingElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType { get { throw null; } }

        public SchemeSettingElement this[int index] { get { throw null; } }

        public SchemeSettingElement this[string name] { get { throw null; } }

        protected override ConfigurationElement CreateNewElement() { throw null; }

        protected override object GetElementKey(ConfigurationElement element) { throw null; }

        public int IndexOf(SchemeSettingElement element) { throw null; }
    }

    public sealed partial class SectionInformation
    {
        internal SectionInformation() { }

        public ConfigurationAllowDefinition AllowDefinition { get { throw null; } set { } }

        public ConfigurationAllowExeDefinition AllowExeDefinition { get { throw null; } set { } }

        public bool AllowLocation { get { throw null; } set { } }

        public bool AllowOverride { get { throw null; } set { } }

        public string ConfigSource { get { throw null; } set { } }

        public bool ForceSave { get { throw null; } set { } }

        public bool InheritInChildApplications { get { throw null; } set { } }

        public bool IsDeclarationRequired { get { throw null; } }

        public bool IsDeclared { get { throw null; } }

        public bool IsLocked { get { throw null; } }

        public bool IsProtected { get { throw null; } }

        public string Name { get { throw null; } }

        public OverrideMode OverrideMode { get { throw null; } set { } }

        public OverrideMode OverrideModeDefault { get { throw null; } set { } }

        public OverrideMode OverrideModeEffective { get { throw null; } }

        public ProtectedConfigurationProvider ProtectionProvider { get { throw null; } }

        public bool RequirePermission { get { throw null; } set { } }

        public bool RestartOnExternalChanges { get { throw null; } set { } }

        public string SectionName { get { throw null; } }

        public string Type { get { throw null; } set { } }

        public void ForceDeclaration() { }

        public void ForceDeclaration(bool force) { }

        public ConfigurationSection GetParentSection() { throw null; }

        public string GetRawXml() { throw null; }

        public void ProtectSection(string protectionProvider) { }

        public void RevertToParent() { }

        public void SetRawXml(string rawXml) { }

        public void UnprotectSection() { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public partial class SettingAttribute : Attribute
    {
    }

    public partial class SettingChangingEventArgs : ComponentModel.CancelEventArgs
    {
        public SettingChangingEventArgs(string settingName, string settingClass, string settingKey, object newValue, bool cancel) { }

        public object NewValue { get { throw null; } }

        public string SettingClass { get { throw null; } }

        public string SettingKey { get { throw null; } }

        public string SettingName { get { throw null; } }
    }

    public delegate void SettingChangingEventHandler(object sender, SettingChangingEventArgs e);
    public sealed partial class SettingElement : ConfigurationElement
    {
        public SettingElement() { }

        public SettingElement(string name, SettingsSerializeAs serializeAs) { }

        public string Name { get { throw null; } set { } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public SettingsSerializeAs SerializeAs { get { throw null; } set { } }

        public SettingValueElement Value { get { throw null; } set { } }

        public override bool Equals(object settings) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public sealed partial class SettingElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType { get { throw null; } }

        protected override string ElementName { get { throw null; } }

        public void Add(SettingElement element) { }

        public void Clear() { }

        protected override ConfigurationElement CreateNewElement() { throw null; }

        public SettingElement Get(string elementKey) { throw null; }

        protected override object GetElementKey(ConfigurationElement element) { throw null; }

        public void Remove(SettingElement element) { }
    }

    public partial class SettingsAttributeDictionary : Collections.Hashtable
    {
        public SettingsAttributeDictionary() { }

        public SettingsAttributeDictionary(SettingsAttributeDictionary attributes) { }

        protected SettingsAttributeDictionary(Runtime.Serialization.SerializationInfo serializationInfo, Runtime.Serialization.StreamingContext streamingContext) { }
    }

    public abstract partial class SettingsBase
    {
        public virtual SettingsContext Context { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public bool IsSynchronized { get { throw null; } }

        public virtual object this[string propertyName] { get { throw null; } set { } }

        public virtual SettingsPropertyCollection Properties { get { throw null; } }

        public virtual SettingsPropertyValueCollection PropertyValues { get { throw null; } }

        public virtual SettingsProviderCollection Providers { get { throw null; } }

        public void Initialize(SettingsContext context, SettingsPropertyCollection properties, SettingsProviderCollection providers) { }

        public virtual void Save() { }

        public static SettingsBase Synchronized(SettingsBase settingsBase) { throw null; }
    }

    public partial class SettingsContext : Collections.Hashtable
    {
        public SettingsContext() { }

        protected SettingsContext(Runtime.Serialization.SerializationInfo serializationInfo, Runtime.Serialization.StreamingContext streamingContext) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class SettingsDescriptionAttribute : Attribute
    {
        public SettingsDescriptionAttribute(string description) { }

        public string Description { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed partial class SettingsGroupDescriptionAttribute : Attribute
    {
        public SettingsGroupDescriptionAttribute(string description) { }

        public string Description { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed partial class SettingsGroupNameAttribute : Attribute
    {
        public SettingsGroupNameAttribute(string groupName) { }

        public string GroupName { get { throw null; } }
    }

    public partial class SettingsLoadedEventArgs : EventArgs
    {
        public SettingsLoadedEventArgs(SettingsProvider provider) { }

        public SettingsProvider Provider { get { throw null; } }
    }

    public delegate void SettingsLoadedEventHandler(object sender, SettingsLoadedEventArgs e);
    public enum SettingsManageability
    {
        Roaming = 0
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed partial class SettingsManageabilityAttribute : Attribute
    {
        public SettingsManageabilityAttribute(SettingsManageability manageability) { }

        public SettingsManageability Manageability { get { throw null; } }
    }

    public partial class SettingsProperty
    {
        public SettingsProperty(SettingsProperty propertyToCopy) { }

        public SettingsProperty(string name, Type propertyType, SettingsProvider provider, bool isReadOnly, object defaultValue, SettingsSerializeAs serializeAs, SettingsAttributeDictionary attributes, bool throwOnErrorDeserializing, bool throwOnErrorSerializing) { }

        public SettingsProperty(string name) { }

        public virtual SettingsAttributeDictionary Attributes { get { throw null; } }

        public virtual object DefaultValue { get { throw null; } set { } }

        public virtual bool IsReadOnly { get { throw null; } set { } }

        public virtual string Name { get { throw null; } set { } }

        public virtual Type PropertyType { get { throw null; } set { } }

        public virtual SettingsProvider Provider { get { throw null; } set { } }

        public virtual SettingsSerializeAs SerializeAs { get { throw null; } set { } }

        public bool ThrowOnErrorDeserializing { get { throw null; } set { } }

        public bool ThrowOnErrorSerializing { get { throw null; } set { } }
    }

    public partial class SettingsPropertyCollection : Collections.IEnumerable, ICloneable, Collections.ICollection
    {
        public int Count { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public SettingsProperty this[string name] { get { throw null; } }

        public object SyncRoot { get { throw null; } }

        public void Add(SettingsProperty property) { }

        public void Clear() { }

        public object Clone() { throw null; }

        public void CopyTo(Array array, int index) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        protected virtual void OnAdd(SettingsProperty property) { }

        protected virtual void OnAddComplete(SettingsProperty property) { }

        protected virtual void OnClear() { }

        protected virtual void OnClearComplete() { }

        protected virtual void OnRemove(SettingsProperty property) { }

        protected virtual void OnRemoveComplete(SettingsProperty property) { }

        public void Remove(string name) { }

        public void SetReadOnly() { }
    }

    public partial class SettingsPropertyIsReadOnlyException : Exception
    {
        public SettingsPropertyIsReadOnlyException() { }

        protected SettingsPropertyIsReadOnlyException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public SettingsPropertyIsReadOnlyException(string message, Exception innerException) { }

        public SettingsPropertyIsReadOnlyException(string message) { }
    }

    public partial class SettingsPropertyNotFoundException : Exception
    {
        public SettingsPropertyNotFoundException() { }

        protected SettingsPropertyNotFoundException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public SettingsPropertyNotFoundException(string message, Exception innerException) { }

        public SettingsPropertyNotFoundException(string message) { }
    }

    public partial class SettingsPropertyValue
    {
        public SettingsPropertyValue(SettingsProperty property) { }

        public bool Deserialized { get { throw null; } set { } }

        public bool IsDirty { get { throw null; } set { } }

        public string Name { get { throw null; } }

        public SettingsProperty Property { get { throw null; } }

        public object PropertyValue { get { throw null; } set { } }

        public object SerializedValue { get { throw null; } set { } }

        public bool UsingDefaultValue { get { throw null; } }
    }

    public partial class SettingsPropertyValueCollection : Collections.IEnumerable, ICloneable, Collections.ICollection
    {
        public int Count { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public SettingsPropertyValue this[string name] { get { throw null; } }

        public object SyncRoot { get { throw null; } }

        public void Add(SettingsPropertyValue property) { }

        public void Clear() { }

        public object Clone() { throw null; }

        public void CopyTo(Array array, int index) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        public void Remove(string name) { }

        public void SetReadOnly() { }
    }

    public partial class SettingsPropertyWrongTypeException : Exception
    {
        public SettingsPropertyWrongTypeException() { }

        protected SettingsPropertyWrongTypeException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public SettingsPropertyWrongTypeException(string message, Exception innerException) { }

        public SettingsPropertyWrongTypeException(string message) { }
    }

    public abstract partial class SettingsProvider : Provider.ProviderBase
    {
        public abstract string ApplicationName { get; set; }

        public abstract SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection);
        public abstract void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection);
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed partial class SettingsProviderAttribute : Attribute
    {
        public SettingsProviderAttribute(string providerTypeName) { }

        public SettingsProviderAttribute(Type providerType) { }

        public string ProviderTypeName { get { throw null; } }
    }

    public partial class SettingsProviderCollection : Provider.ProviderCollection
    {
        public new SettingsProvider this[string name] { get { throw null; } }

        public override void Add(Provider.ProviderBase provider) { }
    }

    public delegate void SettingsSavingEventHandler(object sender, ComponentModel.CancelEventArgs e);
    public enum SettingsSerializeAs
    {
        String = 0,
        Xml = 1,
        Binary = 2,
        ProviderSpecific = 3
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed partial class SettingsSerializeAsAttribute : Attribute
    {
        public SettingsSerializeAsAttribute(SettingsSerializeAs serializeAs) { }

        public SettingsSerializeAs SerializeAs { get { throw null; } }
    }

    public sealed partial class SettingValueElement : ConfigurationElement
    {
        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public Xml.XmlNode ValueXml { get { throw null; } set { } }

        protected internal override void DeserializeElement(Xml.XmlReader reader, bool serializeCollectionKey) { }

        public override bool Equals(object settingValue) { throw null; }

        public override int GetHashCode() { throw null; }

        protected internal override bool IsModified() { throw null; }

        protected internal override void Reset(ConfigurationElement parentElement) { }

        protected internal override void ResetModified() { }

        protected internal override bool SerializeToXmlElement(Xml.XmlWriter writer, string elementName) { throw null; }

        protected internal override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode) { }
    }

    public partial class SingleTagSectionHandler : IConfigurationSectionHandler
    {
        public virtual object Create(object parent, object context, Xml.XmlNode section) { throw null; }
    }

    public enum SpecialSetting
    {
        ConnectionString = 0,
        WebServiceUrl = 1
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed partial class SpecialSettingAttribute : Attribute
    {
        public SpecialSettingAttribute(SpecialSetting specialSetting) { }

        public SpecialSetting SpecialSetting { get { throw null; } }
    }

    public partial class StringValidator : ConfigurationValidatorBase
    {
        public StringValidator(int minLength, int maxLength, string invalidCharacters) { }

        public StringValidator(int minLength, int maxLength) { }

        public StringValidator(int minLength) { }

        public override bool CanValidate(Type type) { throw null; }

        public override void Validate(object value) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class StringValidatorAttribute : ConfigurationValidatorAttribute
    {
        public string InvalidCharacters { get { throw null; } set { } }

        public int MaxLength { get { throw null; } set { } }

        public int MinLength { get { throw null; } set { } }

        public override ConfigurationValidatorBase ValidatorInstance { get { throw null; } }
    }

    public sealed partial class SubclassTypeValidator : ConfigurationValidatorBase
    {
        public SubclassTypeValidator(Type baseClass) { }

        public override bool CanValidate(Type type) { throw null; }

        public override void Validate(object value) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class SubclassTypeValidatorAttribute : ConfigurationValidatorAttribute
    {
        public SubclassTypeValidatorAttribute(Type baseClass) { }

        public Type BaseClass { get { throw null; } }

        public override ConfigurationValidatorBase ValidatorInstance { get { throw null; } }
    }

    public partial class TimeSpanMinutesConverter : ConfigurationConverterBase
    {
        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object data) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object value, Type type) { throw null; }
    }

    public sealed partial class TimeSpanMinutesOrInfiniteConverter : TimeSpanMinutesConverter
    {
        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object data) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object value, Type type) { throw null; }
    }

    public partial class TimeSpanSecondsConverter : ConfigurationConverterBase
    {
        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object data) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object value, Type type) { throw null; }
    }

    public sealed partial class TimeSpanSecondsOrInfiniteConverter : TimeSpanSecondsConverter
    {
        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object data) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object value, Type type) { throw null; }
    }

    public partial class TimeSpanValidator : ConfigurationValidatorBase
    {
        public TimeSpanValidator(TimeSpan minValue, TimeSpan maxValue, bool rangeIsExclusive, long resolutionInSeconds) { }

        public TimeSpanValidator(TimeSpan minValue, TimeSpan maxValue, bool rangeIsExclusive) { }

        public TimeSpanValidator(TimeSpan minValue, TimeSpan maxValue) { }

        public override bool CanValidate(Type type) { throw null; }

        public override void Validate(object value) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class TimeSpanValidatorAttribute : ConfigurationValidatorAttribute
    {
        public const string TimeSpanMaxValue = "10675199.02:48:05.4775807";
        public const string TimeSpanMinValue = "-10675199.02:48:05.4775808";
        public bool ExcludeRange { get { throw null; } set { } }

        public TimeSpan MaxValue { get { throw null; } }

        public string MaxValueString { get { throw null; } set { } }

        public TimeSpan MinValue { get { throw null; } }

        public string MinValueString { get { throw null; } set { } }

        public override ConfigurationValidatorBase ValidatorInstance { get { throw null; } }
    }

    public sealed partial class TypeNameConverter : ConfigurationConverterBase
    {
        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object data) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object value, Type type) { throw null; }
    }

    public sealed partial class UriSection : ConfigurationSection
    {
        public IdnElement Idn { get { throw null; } }

        public IriParsingElement IriParsing { get { throw null; } }

        protected internal override ConfigurationPropertyCollection Properties { get { throw null; } }

        public SchemeSettingElementCollection SchemeSettings { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed partial class UserScopedSettingAttribute : SettingAttribute
    {
    }

    public sealed partial class UserSettingsGroup : ConfigurationSectionGroup
    {
    }

    public delegate void ValidatorCallback(object value);
    public sealed partial class WhiteSpaceTrimStringConverter : ConfigurationConverterBase
    {
        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object data) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext ctx, Globalization.CultureInfo ci, object value, Type type) { throw null; }
    }
}

namespace System.Configuration.Internal
{
    public partial class DelegatingConfigHost : IInternalConfigHost
    {
        protected DelegatingConfigHost() { }

        public virtual bool HasLocalConfig { get { throw null; } }

        public virtual bool HasRoamingConfig { get { throw null; } }

        protected IInternalConfigHost Host { get { throw null; } set { } }

        public virtual bool IsAppConfigHttp { get { throw null; } }

        public virtual bool IsRemote { get { throw null; } }

        public virtual bool SupportsChangeNotifications { get { throw null; } }

        public virtual bool SupportsLocation { get { throw null; } }

        public virtual bool SupportsPath { get { throw null; } }

        public virtual bool SupportsRefresh { get { throw null; } }

        public virtual object CreateConfigurationContext(string configPath, string locationSubPath) { throw null; }

        public virtual object CreateDeprecatedConfigContext(string configPath) { throw null; }

        public virtual string DecryptSection(string encryptedXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection) { throw null; }

        public virtual void DeleteStream(string streamName) { }

        public virtual string EncryptSection(string clearTextXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection) { throw null; }

        public virtual string GetConfigPathFromLocationSubPath(string configPath, string locationSubPath) { throw null; }

        public virtual Type GetConfigType(string typeName, bool throwOnError) { throw null; }

        public virtual string GetConfigTypeName(Type t) { throw null; }

        public virtual void GetRestrictedPermissions(IInternalConfigRecord configRecord, out Security.PermissionSet permissionSet, out bool isHostReady) { throw null; }

        public virtual string GetStreamName(string configPath) { throw null; }

        public virtual string GetStreamNameForConfigSource(string streamName, string configSource) { throw null; }

        public virtual object GetStreamVersion(string streamName) { throw null; }

        public virtual IDisposable Impersonate() { throw null; }

        public virtual void Init(IInternalConfigRoot configRoot, params object[] hostInitParams) { }

        public virtual void InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath, IInternalConfigRoot configRoot, params object[] hostInitConfigurationParams) { throw null; }

        public virtual bool IsAboveApplication(string configPath) { throw null; }

        public virtual bool IsConfigRecordRequired(string configPath) { throw null; }

        public virtual bool IsDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition) { throw null; }

        public virtual bool IsFile(string streamName) { throw null; }

        public virtual bool IsFullTrustSectionWithoutAptcaAllowed(IInternalConfigRecord configRecord) { throw null; }

        public virtual bool IsInitDelayed(IInternalConfigRecord configRecord) { throw null; }

        public virtual bool IsLocationApplicable(string configPath) { throw null; }

        public virtual bool IsSecondaryRoot(string configPath) { throw null; }

        public virtual bool IsTrustedConfigPath(string configPath) { throw null; }

        public virtual IO.Stream OpenStreamForRead(string streamName, bool assertPermissions) { throw null; }

        public virtual IO.Stream OpenStreamForRead(string streamName) { throw null; }

        public virtual IO.Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext, bool assertPermissions) { throw null; }

        public virtual IO.Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext) { throw null; }

        public virtual bool PrefetchAll(string configPath, string streamName) { throw null; }

        public virtual bool PrefetchSection(string sectionGroupName, string sectionName) { throw null; }

        public virtual void RefreshConfigPaths() { }

        public virtual void RequireCompleteInit(IInternalConfigRecord configRecord) { }

        public virtual object StartMonitoringStreamForChanges(string streamName, StreamChangeCallback callback) { throw null; }

        public virtual void StopMonitoringStreamForChanges(string streamName, StreamChangeCallback callback) { }

        public virtual void VerifyDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition, IConfigErrorInfo errorInfo) { }

        public virtual void WriteCompleted(string streamName, bool success, object writeContext, bool assertPermissions) { }

        public virtual void WriteCompleted(string streamName, bool success, object writeContext) { }
    }

    public partial interface IConfigErrorInfo
    {
        string Filename { get; }

        int LineNumber { get; }
    }

    public partial interface IConfigSystem
    {
        IInternalConfigHost Host { get; }

        IInternalConfigRoot Root { get; }

        void Init(Type typeConfigHost, params object[] hostInitParams);
    }

    public partial interface IConfigurationManagerHelper
    {
        void EnsureNetConfigLoaded();
    }

    public partial interface IConfigurationManagerInternal
    {
        string ApplicationConfigUri { get; }

        string ExeLocalConfigDirectory { get; }

        string ExeLocalConfigPath { get; }

        string ExeProductName { get; }

        string ExeProductVersion { get; }

        string ExeRoamingConfigDirectory { get; }

        string ExeRoamingConfigPath { get; }

        string MachineConfigPath { get; }

        bool SetConfigurationSystemInProgress { get; }

        bool SupportsUserConfig { get; }

        string UserConfigFilename { get; }
    }

    public partial interface IInternalConfigClientHost
    {
        string GetExeConfigPath();
        string GetLocalUserConfigPath();
        string GetRoamingUserConfigPath();
        bool IsExeConfig(string configPath);
        bool IsLocalUserConfig(string configPath);
        bool IsRoamingUserConfig(string configPath);
    }

    public partial interface IInternalConfigConfigurationFactory
    {
        Configuration Create(Type typeConfigHost, params object[] hostInitConfigurationParams);
        string NormalizeLocationSubPath(string subPath, IConfigErrorInfo errorInfo);
    }

    public partial interface IInternalConfigHost
    {
        bool IsRemote { get; }

        bool SupportsChangeNotifications { get; }

        bool SupportsLocation { get; }

        bool SupportsPath { get; }

        bool SupportsRefresh { get; }

        object CreateConfigurationContext(string configPath, string locationSubPath);
        object CreateDeprecatedConfigContext(string configPath);
        string DecryptSection(string encryptedXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection);
        void DeleteStream(string streamName);
        string EncryptSection(string clearTextXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection);
        string GetConfigPathFromLocationSubPath(string configPath, string locationSubPath);
        Type GetConfigType(string typeName, bool throwOnError);
        string GetConfigTypeName(Type t);
        void GetRestrictedPermissions(IInternalConfigRecord configRecord, out Security.PermissionSet permissionSet, out bool isHostReady);
        string GetStreamName(string configPath);
        string GetStreamNameForConfigSource(string streamName, string configSource);
        object GetStreamVersion(string streamName);
        IDisposable Impersonate();
        void Init(IInternalConfigRoot configRoot, params object[] hostInitParams);
        void InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath, IInternalConfigRoot configRoot, params object[] hostInitConfigurationParams);
        bool IsAboveApplication(string configPath);
        bool IsConfigRecordRequired(string configPath);
        bool IsDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition);
        bool IsFile(string streamName);
        bool IsFullTrustSectionWithoutAptcaAllowed(IInternalConfigRecord configRecord);
        bool IsInitDelayed(IInternalConfigRecord configRecord);
        bool IsLocationApplicable(string configPath);
        bool IsSecondaryRoot(string configPath);
        bool IsTrustedConfigPath(string configPath);
        IO.Stream OpenStreamForRead(string streamName, bool assertPermissions);
        IO.Stream OpenStreamForRead(string streamName);
        IO.Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext, bool assertPermissions);
        IO.Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext);
        bool PrefetchAll(string configPath, string streamName);
        bool PrefetchSection(string sectionGroupName, string sectionName);
        void RequireCompleteInit(IInternalConfigRecord configRecord);
        object StartMonitoringStreamForChanges(string streamName, StreamChangeCallback callback);
        void StopMonitoringStreamForChanges(string streamName, StreamChangeCallback callback);
        void VerifyDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition, IConfigErrorInfo errorInfo);
        void WriteCompleted(string streamName, bool success, object writeContext, bool assertPermissions);
        void WriteCompleted(string streamName, bool success, object writeContext);
    }

    public partial interface IInternalConfigRecord
    {
        string ConfigPath { get; }

        bool HasInitErrors { get; }

        string StreamName { get; }

        object GetLkgSection(string configKey);
        object GetSection(string configKey);
        void RefreshSection(string configKey);
        void Remove();
        void ThrowIfInitErrors();
    }

    public partial interface IInternalConfigRoot
    {
        bool IsDesignTime { get; }

        event InternalConfigEventHandler ConfigChanged;
        event InternalConfigEventHandler ConfigRemoved;
        IInternalConfigRecord GetConfigRecord(string configPath);
        object GetSection(string section, string configPath);
        string GetUniqueConfigPath(string configPath);
        IInternalConfigRecord GetUniqueConfigRecord(string configPath);
        void Init(IInternalConfigHost host, bool isDesignTime);
        void RemoveConfig(string configPath);
    }

    public partial interface IInternalConfigSettingsFactory
    {
        void CompleteInit();
        void SetConfigurationSystem(IInternalConfigSystem internalConfigSystem, bool initComplete);
    }

    public partial interface IInternalConfigSystem
    {
        bool SupportsUserConfig { get; }

        object GetSection(string configKey);
        void RefreshConfig(string sectionName);
    }

    public sealed partial class InternalConfigEventArgs : EventArgs
    {
        public InternalConfigEventArgs(string configPath) { }

        public string ConfigPath { get { throw null; } set { } }
    }

    public delegate void InternalConfigEventHandler(object sender, InternalConfigEventArgs e);
    public delegate void StreamChangeCallback(string streamName);
}

namespace System.Configuration.Provider
{
    public abstract partial class ProviderBase
    {
        public virtual string Description { get { throw null; } }

        public virtual string Name { get { throw null; } }

        public virtual void Initialize(string name, Collections.Specialized.NameValueCollection config) { }
    }

    public partial class ProviderCollection : Collections.ICollection, Collections.IEnumerable
    {
        public int Count { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public ProviderBase this[string name] { get { throw null; } }

        public object SyncRoot { get { throw null; } }

        public virtual void Add(ProviderBase provider) { }

        public void Clear() { }

        public void CopyTo(ProviderBase[] array, int index) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        public void Remove(string name) { }

        public void SetReadOnly() { }

        void Collections.ICollection.CopyTo(Array array, int index) { }
    }

    public partial class ProviderException : Exception
    {
        public ProviderException() { }

        protected ProviderException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public ProviderException(string message, Exception innerException) { }

        public ProviderException(string message) { }
    }
}

namespace System.Drawing.Configuration
{
    public sealed partial class SystemDrawingSection : System.Configuration.ConfigurationSection
    {
        public string BitmapSuffix { get { throw null; } set { } }

        protected internal override System.Configuration.ConfigurationPropertyCollection Properties { get { throw null; } }
    }
}