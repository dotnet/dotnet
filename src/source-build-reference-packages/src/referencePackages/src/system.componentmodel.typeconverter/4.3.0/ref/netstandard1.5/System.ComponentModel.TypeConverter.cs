// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.ComponentModel.TypeConverter")]
[assembly: System.Reflection.AssemblyDescription("System.ComponentModel.TypeConverter")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.ComponentModel.TypeConverter")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.1.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System
{
    public partial class UriTypeConverter : ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ComponentModel.ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }
}

namespace System.ComponentModel
{
    public partial class ArrayConverter : CollectionConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) { throw null; }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context) { throw null; }
    }

    public partial class AttributeCollection : Collections.ICollection, Collections.IEnumerable
    {
        public static readonly AttributeCollection Empty;
        protected AttributeCollection() { }

        public AttributeCollection(params Attribute[] attributes) { }

        protected virtual Attribute[] Attributes { get { throw null; } }

        public int Count { get { throw null; } }

        public virtual Attribute this[int index] { get { throw null; } }

        public virtual Attribute this[Type attributeType] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public bool Contains(Attribute attribute) { throw null; }

        public bool Contains(Attribute[] attributes) { throw null; }

        public void CopyTo(Array array, int index) { }

        public static AttributeCollection FromExisting(AttributeCollection existing, params Attribute[] newAttributes) { throw null; }

        protected Attribute GetDefaultAttribute(Type attributeType) { throw null; }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        public bool Matches(Attribute attribute) { throw null; }

        public bool Matches(Attribute[] attributes) { throw null; }
    }

    public partial class AttributeProviderAttribute : Attribute
    {
        public AttributeProviderAttribute(string typeName, string propertyName) { }

        public AttributeProviderAttribute(string typeName) { }

        public AttributeProviderAttribute(Type type) { }

        public string PropertyName { get { throw null; } }

        public string TypeName { get { throw null; } }
    }

    public abstract partial class BaseNumberConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type t) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class BooleanConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) { throw null; }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { throw null; }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { throw null; }
    }

    public partial class ByteConverter : BaseNumberConverter
    {
    }

    public delegate void CancelEventHandler(object sender, CancelEventArgs e);
    public partial class CharConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public enum CollectionChangeAction
    {
        Add = 1,
        Remove = 2,
        Refresh = 3
    }

    public partial class CollectionChangeEventArgs : EventArgs
    {
        public CollectionChangeEventArgs(CollectionChangeAction action, object element) { }

        public virtual CollectionChangeAction Action { get { throw null; } }

        public virtual object Element { get { throw null; } }
    }

    public delegate void CollectionChangeEventHandler(object sender, CollectionChangeEventArgs e);
    public partial class CollectionConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) { throw null; }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context) { throw null; }
    }

    public abstract partial class CustomTypeDescriptor : ICustomTypeDescriptor
    {
        protected CustomTypeDescriptor() { }

        protected CustomTypeDescriptor(ICustomTypeDescriptor parent) { }

        public virtual AttributeCollection GetAttributes() { throw null; }

        public virtual string GetClassName() { throw null; }

        public virtual string GetComponentName() { throw null; }

        public virtual TypeConverter GetConverter() { throw null; }

        public virtual EventDescriptor GetDefaultEvent() { throw null; }

        public virtual PropertyDescriptor GetDefaultProperty() { throw null; }

        public virtual object GetEditor(Type editorBaseType) { throw null; }

        public virtual EventDescriptorCollection GetEvents() { throw null; }

        public virtual EventDescriptorCollection GetEvents(Attribute[] attributes) { throw null; }

        public virtual PropertyDescriptorCollection GetProperties() { throw null; }

        public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes) { throw null; }

        public virtual object GetPropertyOwner(PropertyDescriptor pd) { throw null; }
    }

    public partial class DateTimeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class DateTimeOffsetConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class DecimalConverter : BaseNumberConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public sealed partial class DefaultEventAttribute : Attribute
    {
        public static readonly DefaultEventAttribute Default;
        public DefaultEventAttribute(string name) { }

        public string Name { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public sealed partial class DefaultPropertyAttribute : Attribute
    {
        public static readonly DefaultPropertyAttribute Default;
        public DefaultPropertyAttribute(string name) { }

        public string Name { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class DoubleConverter : BaseNumberConverter
    {
    }

    public partial class EnumConverter : TypeConverter
    {
        public EnumConverter(Type type) { }

        protected virtual Collections.IComparer Comparer { get { throw null; } }

        protected Type EnumType { get { throw null; } }

        protected StandardValuesCollection Values { get { throw null; } set { } }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) { throw null; }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { throw null; }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { throw null; }

        public override bool IsValid(ITypeDescriptorContext context, object value) { throw null; }
    }

    public abstract partial class EventDescriptor : MemberDescriptor
    {
        protected EventDescriptor(MemberDescriptor descr, Attribute[] attrs) : base(default(MemberDescriptor)!) { }

        protected EventDescriptor(MemberDescriptor descr) : base(default(MemberDescriptor)!) { }

        protected EventDescriptor(string name, Attribute[] attrs) : base(default(MemberDescriptor)!) { }

        public abstract Type ComponentType { get; }
        public abstract Type EventType { get; }
        public abstract bool IsMulticast { get; }

        public abstract void AddEventHandler(object component, Delegate value);
        public abstract void RemoveEventHandler(object component, Delegate value);
    }

    public partial class EventDescriptorCollection : Collections.ICollection, Collections.IEnumerable, Collections.IList
    {
        public static readonly EventDescriptorCollection Empty;
        public EventDescriptorCollection(EventDescriptor[] events, bool readOnly) { }

        public EventDescriptorCollection(EventDescriptor[] events) { }

        public int Count { get { throw null; } }

        public virtual EventDescriptor this[int index] { get { throw null; } }

        public virtual EventDescriptor this[string name] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        bool Collections.IList.IsFixedSize { get { throw null; } }

        bool Collections.IList.IsReadOnly { get { throw null; } }

        object System.Collections.IList.this[int index] { get { throw null; } set { } }

        public int Add(EventDescriptor value) { throw null; }

        public void Clear() { }

        public bool Contains(EventDescriptor value) { throw null; }

        public virtual EventDescriptor Find(string name, bool ignoreCase) { throw null; }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        public int IndexOf(EventDescriptor value) { throw null; }

        public void Insert(int index, EventDescriptor value) { }

        protected void InternalSort(Collections.IComparer sorter) { }

        protected void InternalSort(string[] names) { }

        public void Remove(EventDescriptor value) { }

        public void RemoveAt(int index) { }

        public virtual EventDescriptorCollection Sort() { throw null; }

        public virtual EventDescriptorCollection Sort(Collections.IComparer comparer) { throw null; }

        public virtual EventDescriptorCollection Sort(string[] names, Collections.IComparer comparer) { throw null; }

        public virtual EventDescriptorCollection Sort(string[] names) { throw null; }

        void Collections.ICollection.CopyTo(Array array, int index) { }

        int Collections.IList.Add(object value) { throw null; }

        bool Collections.IList.Contains(object value) { throw null; }

        int Collections.IList.IndexOf(object value) { throw null; }

        void Collections.IList.Insert(int index, object value) { }

        void Collections.IList.Remove(object value) { }
    }

    public sealed partial class ExtenderProvidedPropertyAttribute : Attribute
    {
        public PropertyDescriptor ExtenderProperty { get { throw null; } }

        public IExtenderProvider Provider { get { throw null; } }

        public Type ReceiverType { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class GuidConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class HandledEventArgs : EventArgs
    {
        public HandledEventArgs() { }

        public HandledEventArgs(bool defaultHandledValue) { }

        public bool Handled { get { throw null; } set { } }
    }

    public delegate void HandledEventHandler(object sender, HandledEventArgs e);
    public partial interface ICustomTypeDescriptor
    {
        AttributeCollection GetAttributes();
        string GetClassName();
        string GetComponentName();
        TypeConverter GetConverter();
        EventDescriptor GetDefaultEvent();
        PropertyDescriptor GetDefaultProperty();
        object GetEditor(Type editorBaseType);
        EventDescriptorCollection GetEvents();
        EventDescriptorCollection GetEvents(Attribute[] attributes);
        PropertyDescriptorCollection GetProperties();
        PropertyDescriptorCollection GetProperties(Attribute[] attributes);
        object GetPropertyOwner(PropertyDescriptor pd);
    }

    public partial interface IExtenderProvider
    {
        bool CanExtend(object extendee);
    }

    public partial interface IListSource
    {
        bool ContainsListCollection { get; }

        Collections.IList GetList();
    }

    public partial class Int16Converter : BaseNumberConverter
    {
    }

    public partial class Int32Converter : BaseNumberConverter
    {
    }

    public partial class Int64Converter : BaseNumberConverter
    {
    }

    public partial class InvalidAsynchronousStateException : ArgumentException
    {
        public InvalidAsynchronousStateException() { }

        public InvalidAsynchronousStateException(string message, Exception innerException) { }

        public InvalidAsynchronousStateException(string message) { }
    }

    public partial interface ITypeDescriptorContext : IServiceProvider
    {
        IContainer Container { get; }

        object Instance { get; }

        PropertyDescriptor PropertyDescriptor { get; }

        void OnComponentChanged();
        bool OnComponentChanging();
    }

    public partial interface ITypedList
    {
        PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors);
        string GetListName(PropertyDescriptor[] listAccessors);
    }

    public abstract partial class MemberDescriptor
    {
        protected MemberDescriptor(MemberDescriptor oldMemberDescriptor, Attribute[] newAttributes) { }

        protected MemberDescriptor(MemberDescriptor descr) { }

        protected MemberDescriptor(string name, Attribute[] attributes) { }

        protected MemberDescriptor(string name) { }

        protected virtual Attribute[] AttributeArray { get { throw null; } set { } }

        public virtual AttributeCollection Attributes { get { throw null; } }

        public virtual string Category { get { throw null; } }

        public virtual string Description { get { throw null; } }

        public virtual bool DesignTimeOnly { get { throw null; } }

        public virtual string DisplayName { get { throw null; } }

        public virtual bool IsBrowsable { get { throw null; } }

        public virtual string Name { get { throw null; } }

        protected virtual int NameHashCode { get { throw null; } }

        protected virtual AttributeCollection CreateAttributeCollection() { throw null; }

        public override bool Equals(object obj) { throw null; }

        protected virtual void FillAttributes(Collections.IList attributeList) { }

        protected static Reflection.MethodInfo FindMethod(Type componentClass, string name, Type[] args, Type returnType, bool publicOnly) { throw null; }

        protected static Reflection.MethodInfo FindMethod(Type componentClass, string name, Type[] args, Type returnType) { throw null; }

        public override int GetHashCode() { throw null; }

        protected virtual object GetInvocationTarget(Type type, object instance) { throw null; }

        protected static ISite GetSite(object component) { throw null; }
    }

    public partial class MultilineStringConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) { throw null; }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context) { throw null; }
    }

    public partial class NullableConverter : TypeConverter
    {
        public NullableConverter(Type type) { }

        public Type NullableType { get { throw null; } }

        public Type UnderlyingType { get { throw null; } }

        public TypeConverter UnderlyingTypeConverter { get { throw null; } }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }

        public override object CreateInstance(ITypeDescriptorContext context, Collections.IDictionary propertyValues) { throw null; }

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) { throw null; }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) { throw null; }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context) { throw null; }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) { throw null; }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { throw null; }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { throw null; }

        public override bool IsValid(ITypeDescriptorContext context, object value) { throw null; }
    }

    public abstract partial class PropertyDescriptor : MemberDescriptor
    {
        protected PropertyDescriptor(MemberDescriptor descr, Attribute[] attrs) : base(default(MemberDescriptor)!) { }

        protected PropertyDescriptor(MemberDescriptor descr) : base(default(MemberDescriptor)!) { }

        protected PropertyDescriptor(string name, Attribute[] attrs) : base(default(MemberDescriptor)!) { }

        public abstract Type ComponentType { get; }

        public virtual TypeConverter Converter { get { throw null; } }

        public virtual bool IsLocalizable { get { throw null; } }

        public abstract bool IsReadOnly { get; }
        public abstract Type PropertyType { get; }

        public DesignerSerializationVisibility SerializationVisibility { get { throw null; } }

        public virtual bool SupportsChangeEvents { get { throw null; } }

        public virtual void AddValueChanged(object component, EventHandler handler) { }

        public abstract bool CanResetValue(object component);
        protected object CreateInstance(Type type) { throw null; }

        public override bool Equals(object obj) { throw null; }

        protected override void FillAttributes(Collections.IList attributeList) { }

        public PropertyDescriptorCollection GetChildProperties() { throw null; }

        public PropertyDescriptorCollection GetChildProperties(Attribute[] filter) { throw null; }

        public virtual PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter) { throw null; }

        public PropertyDescriptorCollection GetChildProperties(object instance) { throw null; }

        public virtual object GetEditor(Type editorBaseType) { throw null; }

        public override int GetHashCode() { throw null; }

        protected override object GetInvocationTarget(Type type, object instance) { throw null; }

        protected Type GetTypeFromName(string typeName) { throw null; }

        public abstract object GetValue(object component);
        protected internal EventHandler GetValueChangedHandler(object component) { throw null; }

        protected virtual void OnValueChanged(object component, EventArgs e) { }

        public virtual void RemoveValueChanged(object component, EventHandler handler) { }

        public abstract void ResetValue(object component);
        public abstract void SetValue(object component, object value);
        public abstract bool ShouldSerializeValue(object component);
    }

    public partial class PropertyDescriptorCollection : Collections.ICollection, Collections.IEnumerable, Collections.IDictionary, Collections.IList
    {
        public static readonly PropertyDescriptorCollection Empty;
        public PropertyDescriptorCollection(PropertyDescriptor[] properties, bool readOnly) { }

        public PropertyDescriptorCollection(PropertyDescriptor[] properties) { }

        public int Count { get { throw null; } }

        public virtual PropertyDescriptor this[int index] { get { throw null; } }

        public virtual PropertyDescriptor this[string name] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        bool Collections.IDictionary.IsFixedSize { get { throw null; } }

        bool Collections.IDictionary.IsReadOnly { get { throw null; } }

        object System.Collections.IDictionary.this[object key] { get { throw null; } set { } }

        Collections.ICollection Collections.IDictionary.Keys { get { throw null; } }

        Collections.ICollection Collections.IDictionary.Values { get { throw null; } }

        bool Collections.IList.IsFixedSize { get { throw null; } }

        bool Collections.IList.IsReadOnly { get { throw null; } }

        object System.Collections.IList.this[int index] { get { throw null; } set { } }

        public int Add(PropertyDescriptor value) { throw null; }

        public void Clear() { }

        public bool Contains(PropertyDescriptor value) { throw null; }

        public void CopyTo(Array array, int index) { }

        public virtual PropertyDescriptor Find(string name, bool ignoreCase) { throw null; }

        public virtual Collections.IEnumerator GetEnumerator() { throw null; }

        public int IndexOf(PropertyDescriptor value) { throw null; }

        public void Insert(int index, PropertyDescriptor value) { }

        protected void InternalSort(Collections.IComparer sorter) { }

        protected void InternalSort(string[] names) { }

        public void Remove(PropertyDescriptor value) { }

        public void RemoveAt(int index) { }

        public virtual PropertyDescriptorCollection Sort() { throw null; }

        public virtual PropertyDescriptorCollection Sort(Collections.IComparer comparer) { throw null; }

        public virtual PropertyDescriptorCollection Sort(string[] names, Collections.IComparer comparer) { throw null; }

        public virtual PropertyDescriptorCollection Sort(string[] names) { throw null; }

        void Collections.IDictionary.Add(object key, object value) { }

        bool Collections.IDictionary.Contains(object key) { throw null; }

        Collections.IDictionaryEnumerator Collections.IDictionary.GetEnumerator() { throw null; }

        void Collections.IDictionary.Remove(object key) { }

        int Collections.IList.Add(object value) { throw null; }

        bool Collections.IList.Contains(object value) { throw null; }

        int Collections.IList.IndexOf(object value) { throw null; }

        void Collections.IList.Insert(int index, object value) { }

        void Collections.IList.Remove(object value) { }
    }

    public sealed partial class ProvidePropertyAttribute : Attribute
    {
        public ProvidePropertyAttribute(string propertyName, string receiverTypeName) { }

        public ProvidePropertyAttribute(string propertyName, Type receiverType) { }

        public string PropertyName { get { throw null; } }

        public string ReceiverTypeName { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class RefreshEventArgs : EventArgs
    {
        public RefreshEventArgs(object componentChanged) { }

        public RefreshEventArgs(Type typeChanged) { }

        public object ComponentChanged { get { throw null; } }

        public Type TypeChanged { get { throw null; } }
    }

    public delegate void RefreshEventHandler(RefreshEventArgs e);
    public partial class SByteConverter : BaseNumberConverter
    {
    }

    public partial class SingleConverter : BaseNumberConverter
    {
    }

    public partial class StringConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }
    }

    public partial class TimeSpanConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class TypeConverter
    {
        public virtual bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public bool CanConvertFrom(Type sourceType) { throw null; }

        public virtual bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public bool CanConvertTo(Type destinationType) { throw null; }

        public virtual object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public object ConvertFrom(object value) { throw null; }

        public object ConvertFromInvariantString(ITypeDescriptorContext context, string text) { throw null; }

        public object ConvertFromInvariantString(string text) { throw null; }

        public object ConvertFromString(ITypeDescriptorContext context, Globalization.CultureInfo culture, string text) { throw null; }

        public object ConvertFromString(ITypeDescriptorContext context, string text) { throw null; }

        public object ConvertFromString(string text) { throw null; }

        public virtual object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }

        public object ConvertTo(object value, Type destinationType) { throw null; }

        public string ConvertToInvariantString(ITypeDescriptorContext context, object value) { throw null; }

        public string ConvertToInvariantString(object value) { throw null; }

        public string ConvertToString(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public string ConvertToString(ITypeDescriptorContext context, object value) { throw null; }

        public string ConvertToString(object value) { throw null; }

        public object CreateInstance(Collections.IDictionary propertyValues) { throw null; }

        public virtual object CreateInstance(ITypeDescriptorContext context, Collections.IDictionary propertyValues) { throw null; }

        protected Exception GetConvertFromException(object value) { throw null; }

        protected Exception GetConvertToException(object value, Type destinationType) { throw null; }

        public bool GetCreateInstanceSupported() { throw null; }

        public virtual bool GetCreateInstanceSupported(ITypeDescriptorContext context) { throw null; }

        public virtual PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) { throw null; }

        public PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value) { throw null; }

        public PropertyDescriptorCollection GetProperties(object value) { throw null; }

        public bool GetPropertiesSupported() { throw null; }

        public virtual bool GetPropertiesSupported(ITypeDescriptorContext context) { throw null; }

        public Collections.ICollection GetStandardValues() { throw null; }

        public virtual StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) { throw null; }

        public bool GetStandardValuesExclusive() { throw null; }

        public virtual bool GetStandardValuesExclusive(ITypeDescriptorContext context) { throw null; }

        public bool GetStandardValuesSupported() { throw null; }

        public virtual bool GetStandardValuesSupported(ITypeDescriptorContext context) { throw null; }

        public virtual bool IsValid(ITypeDescriptorContext context, object value) { throw null; }

        public bool IsValid(object value) { throw null; }

        protected PropertyDescriptorCollection SortProperties(PropertyDescriptorCollection props, string[] names) { throw null; }

        protected abstract partial class SimplePropertyDescriptor : PropertyDescriptor
        {
            protected SimplePropertyDescriptor(Type componentType, string name, Type propertyType, Attribute[] attributes) : base(default!) { }

            protected SimplePropertyDescriptor(Type componentType, string name, Type propertyType) : base(default!) { }

            public override Type ComponentType { get { throw null; } }

            public override bool IsReadOnly { get { throw null; } }

            public override Type PropertyType { get { throw null; } }

            public override bool CanResetValue(object component) { throw null; }

            public override void ResetValue(object component) { }

            public override bool ShouldSerializeValue(object component) { throw null; }
        }

        public partial class StandardValuesCollection : Collections.ICollection, Collections.IEnumerable
        {
            public StandardValuesCollection(Collections.ICollection values) { }

            public int Count { get { throw null; } }

            public object this[int index] { get { throw null; } }

            bool Collections.ICollection.IsSynchronized { get { throw null; } }

            object Collections.ICollection.SyncRoot { get { throw null; } }

            public void CopyTo(Array array, int index) { }

            public Collections.IEnumerator GetEnumerator() { throw null; }
        }
    }

    public abstract partial class TypeDescriptionProvider
    {
        protected TypeDescriptionProvider() { }

        protected TypeDescriptionProvider(TypeDescriptionProvider parent) { }

        public virtual object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args) { throw null; }

        public virtual Collections.IDictionary GetCache(object instance) { throw null; }

        public virtual ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance) { throw null; }

        protected internal virtual IExtenderProvider[] GetExtenderProviders(object instance) { throw null; }

        public virtual string GetFullComponentName(object component) { throw null; }

        public Type GetReflectionType(object instance) { throw null; }

        public virtual Type GetReflectionType(Type objectType, object instance) { throw null; }

        public Type GetReflectionType(Type objectType) { throw null; }

        public virtual Type GetRuntimeType(Type reflectionType) { throw null; }

        public ICustomTypeDescriptor GetTypeDescriptor(object instance) { throw null; }

        public virtual ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance) { throw null; }

        public ICustomTypeDescriptor GetTypeDescriptor(Type objectType) { throw null; }

        public virtual bool IsSupportedType(Type type) { throw null; }
    }

    public sealed partial class TypeDescriptionProviderAttribute : Attribute
    {
        public TypeDescriptionProviderAttribute(string typeName) { }

        public TypeDescriptionProviderAttribute(Type type) { }

        public string TypeName { get { throw null; } }
    }

    public sealed partial class TypeDescriptor
    {
        internal TypeDescriptor() { }

        public static Type InterfaceType { get { throw null; } }

        public static event RefreshEventHandler Refreshed { add { } remove { } }

        public static TypeDescriptionProvider AddAttributes(object instance, params Attribute[] attributes) { throw null; }

        public static TypeDescriptionProvider AddAttributes(Type type, params Attribute[] attributes) { throw null; }

        public static void AddEditorTable(Type editorBaseType, Collections.Hashtable table) { }

        public static void AddProvider(TypeDescriptionProvider provider, object instance) { }

        public static void AddProvider(TypeDescriptionProvider provider, Type type) { }

        public static void AddProviderTransparent(TypeDescriptionProvider provider, object instance) { }

        public static void AddProviderTransparent(TypeDescriptionProvider provider, Type type) { }

        public static void CreateAssociation(object primary, object secondary) { }

        public static EventDescriptor CreateEvent(Type componentType, EventDescriptor oldEventDescriptor, params Attribute[] attributes) { throw null; }

        public static EventDescriptor CreateEvent(Type componentType, string name, Type type, params Attribute[] attributes) { throw null; }

        public static object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args) { throw null; }

        public static PropertyDescriptor CreateProperty(Type componentType, PropertyDescriptor oldPropertyDescriptor, params Attribute[] attributes) { throw null; }

        public static PropertyDescriptor CreateProperty(Type componentType, string name, Type type, params Attribute[] attributes) { throw null; }

        public static object GetAssociation(Type type, object primary) { throw null; }

        public static AttributeCollection GetAttributes(object component, bool noCustomTypeDesc) { throw null; }

        public static AttributeCollection GetAttributes(object component) { throw null; }

        public static AttributeCollection GetAttributes(Type componentType) { throw null; }

        public static string GetClassName(object component, bool noCustomTypeDesc) { throw null; }

        public static string GetClassName(object component) { throw null; }

        public static string GetClassName(Type componentType) { throw null; }

        public static string GetComponentName(object component, bool noCustomTypeDesc) { throw null; }

        public static string GetComponentName(object component) { throw null; }

        public static TypeConverter GetConverter(object component, bool noCustomTypeDesc) { throw null; }

        public static TypeConverter GetConverter(object component) { throw null; }

        public static TypeConverter GetConverter(Type type) { throw null; }

        public static EventDescriptor GetDefaultEvent(object component, bool noCustomTypeDesc) { throw null; }

        public static EventDescriptor GetDefaultEvent(object component) { throw null; }

        public static EventDescriptor GetDefaultEvent(Type componentType) { throw null; }

        public static PropertyDescriptor GetDefaultProperty(object component, bool noCustomTypeDesc) { throw null; }

        public static PropertyDescriptor GetDefaultProperty(object component) { throw null; }

        public static PropertyDescriptor GetDefaultProperty(Type componentType) { throw null; }

        public static object GetEditor(object component, Type editorBaseType, bool noCustomTypeDesc) { throw null; }

        public static object GetEditor(object component, Type editorBaseType) { throw null; }

        public static object GetEditor(Type type, Type editorBaseType) { throw null; }

        public static EventDescriptorCollection GetEvents(object component, Attribute[] attributes, bool noCustomTypeDesc) { throw null; }

        public static EventDescriptorCollection GetEvents(object component, Attribute[] attributes) { throw null; }

        public static EventDescriptorCollection GetEvents(object component, bool noCustomTypeDesc) { throw null; }

        public static EventDescriptorCollection GetEvents(object component) { throw null; }

        public static EventDescriptorCollection GetEvents(Type componentType, Attribute[] attributes) { throw null; }

        public static EventDescriptorCollection GetEvents(Type componentType) { throw null; }

        public static string GetFullComponentName(object component) { throw null; }

        public static PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes, bool noCustomTypeDesc) { throw null; }

        public static PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes) { throw null; }

        public static PropertyDescriptorCollection GetProperties(object component, bool noCustomTypeDesc) { throw null; }

        public static PropertyDescriptorCollection GetProperties(object component) { throw null; }

        public static PropertyDescriptorCollection GetProperties(Type componentType, Attribute[] attributes) { throw null; }

        public static PropertyDescriptorCollection GetProperties(Type componentType) { throw null; }

        public static TypeDescriptionProvider GetProvider(object instance) { throw null; }

        public static TypeDescriptionProvider GetProvider(Type type) { throw null; }

        public static Type GetReflectionType(object instance) { throw null; }

        public static Type GetReflectionType(Type type) { throw null; }

        public static void Refresh(object component) { }

        public static void Refresh(Reflection.Assembly assembly) { }

        public static void Refresh(Reflection.Module module) { }

        public static void Refresh(Type type) { }

        public static void RemoveAssociation(object primary, object secondary) { }

        public static void RemoveAssociations(object primary) { }

        public static void RemoveProvider(TypeDescriptionProvider provider, object instance) { }

        public static void RemoveProvider(TypeDescriptionProvider provider, Type type) { }

        public static void RemoveProviderTransparent(TypeDescriptionProvider provider, object instance) { }

        public static void RemoveProviderTransparent(TypeDescriptionProvider provider, Type type) { }

        public static void SortDescriptorArray(Collections.IList infos) { }
    }

    public abstract partial class TypeListConverter : TypeConverter
    {
        protected TypeListConverter(Type[] types) { }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) { throw null; }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { throw null; }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { throw null; }
    }

    public partial class UInt16Converter : BaseNumberConverter
    {
    }

    public partial class UInt32Converter : BaseNumberConverter
    {
    }

    public partial class UInt64Converter : BaseNumberConverter
    {
    }
}