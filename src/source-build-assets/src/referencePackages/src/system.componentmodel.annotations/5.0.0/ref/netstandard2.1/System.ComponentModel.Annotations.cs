// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName = "")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.ComponentModel.Annotations")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.ComponentModel.Annotations")]
[assembly: System.Reflection.AssemblyFileVersion("5.0.20.51904")]
[assembly: System.Reflection.AssemblyInformationalVersion("5.0.0+cf258a14b70ad9069470a108f13765e0e5988f51")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.ComponentModel.Annotations")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "git://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("5.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.ComponentModel.DataAnnotations
{
    public partial class AssociatedMetadataTypeTypeDescriptionProvider : TypeDescriptionProvider
    {
        public AssociatedMetadataTypeTypeDescriptionProvider(Type type, Type associatedMetadataType) { }

        public AssociatedMetadataTypeTypeDescriptionProvider(Type type) { }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    [Obsolete("This attribute is no longer in use and will be ignored if applied.")]
    public sealed partial class AssociationAttribute : Attribute
    {
        public AssociationAttribute(string name, string thisKey, string otherKey) { }

        public bool IsForeignKey { get { throw null; } set { } }

        public string Name { get { throw null; } }

        public string OtherKey { get { throw null; } }

        public Collections.Generic.IEnumerable<string> OtherKeyMembers { get { throw null; } }

        public string ThisKey { get { throw null; } }

        public Collections.Generic.IEnumerable<string> ThisKeyMembers { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public partial class CompareAttribute : ValidationAttribute
    {
        public CompareAttribute(string otherProperty) { }

        public string OtherProperty { get { throw null; } }

        public string? OtherPropertyDisplayName { get { throw null; } }

        public override bool RequiresValidationContext { get { throw null; } }

        public override string FormatErrorMessage(string name) { throw null; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed partial class ConcurrencyCheckAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed partial class CreditCardAttribute : DataTypeAttribute
    {
        public CreditCardAttribute() : base(default(DataType)) { }

        public override bool IsValid(object? value) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed partial class CustomValidationAttribute : ValidationAttribute
    {
        public CustomValidationAttribute(Type validatorType, string method) { }

        public string Method { get { throw null; } }

        public Type ValidatorType { get { throw null; } }

        public override string FormatErrorMessage(string name) { throw null; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) { throw null; }
    }

    public enum DataType
    {
        Custom = 0,
        DateTime = 1,
        Date = 2,
        Time = 3,
        Duration = 4,
        PhoneNumber = 5,
        Currency = 6,
        Text = 7,
        Html = 8,
        MultilineText = 9,
        EmailAddress = 10,
        Password = 11,
        Url = 12,
        ImageUrl = 13,
        CreditCard = 14,
        PostalCode = 15,
        Upload = 16
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public partial class DataTypeAttribute : ValidationAttribute
    {
        public DataTypeAttribute(DataType dataType) { }

        public DataTypeAttribute(string customDataType) { }

        public string? CustomDataType { get { throw null; } }

        public DataType DataType { get { throw null; } }

        public DisplayFormatAttribute? DisplayFormat { get { throw null; } protected set { } }

        public virtual string GetDataTypeName() { throw null; }

        public override bool IsValid(object? value) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed partial class DisplayAttribute : Attribute
    {
        public bool AutoGenerateField { get { throw null; } set { } }

        public bool AutoGenerateFilter { get { throw null; } set { } }

        public string? Description { get { throw null; } set { } }

        public string? GroupName { get { throw null; } set { } }

        public string? Name { get { throw null; } set { } }

        public int Order { get { throw null; } set { } }

        public string? Prompt { get { throw null; } set { } }

        public Type? ResourceType { get { throw null; } set { } }

        public string? ShortName { get { throw null; } set { } }

        public bool? GetAutoGenerateField() { throw null; }

        public bool? GetAutoGenerateFilter() { throw null; }

        public string? GetDescription() { throw null; }

        public string? GetGroupName() { throw null; }

        public string? GetName() { throw null; }

        public int? GetOrder() { throw null; }

        public string? GetPrompt() { throw null; }

        public string? GetShortName() { throw null; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public partial class DisplayColumnAttribute : Attribute
    {
        public DisplayColumnAttribute(string displayColumn, string? sortColumn, bool sortDescending) { }

        public DisplayColumnAttribute(string displayColumn, string? sortColumn) { }

        public DisplayColumnAttribute(string displayColumn) { }

        public string DisplayColumn { get { throw null; } }

        public string? SortColumn { get { throw null; } }

        public bool SortDescending { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public partial class DisplayFormatAttribute : Attribute
    {
        public bool ApplyFormatInEditMode { get { throw null; } set { } }

        public bool ConvertEmptyStringToNull { get { throw null; } set { } }

        public string? DataFormatString { get { throw null; } set { } }

        public bool HtmlEncode { get { throw null; } set { } }

        public string? NullDisplayText { get { throw null; } set { } }

        public Type? NullDisplayTextResourceType { get { throw null; } set { } }

        public string? GetNullDisplayText() { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed partial class EditableAttribute : Attribute
    {
        public EditableAttribute(bool allowEdit) { }

        public bool AllowEdit { get { throw null; } }

        public bool AllowInitialValue { get { throw null; } set { } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed partial class EmailAddressAttribute : DataTypeAttribute
    {
        public EmailAddressAttribute() : base(default(DataType)) { }

        public override bool IsValid(object? value) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed partial class EnumDataTypeAttribute : DataTypeAttribute
    {
        public EnumDataTypeAttribute(Type enumType) : base(default(DataType)) { }

        public Type EnumType { get { throw null; } }

        public override bool IsValid(object? value) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed partial class FileExtensionsAttribute : DataTypeAttribute
    {
        public FileExtensionsAttribute() : base(default(DataType)) { }

        public string Extensions { get { throw null; } set { } }

        public override string FormatErrorMessage(string name) { throw null; }

        public override bool IsValid(object? value) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    [Obsolete("This attribute is no longer in use and will be ignored if applied.")]
    public sealed partial class FilterUIHintAttribute : Attribute
    {
        public FilterUIHintAttribute(string filterUIHint, string? presentationLayer, params object?[] controlParameters) { }

        public FilterUIHintAttribute(string filterUIHint, string? presentationLayer) { }

        public FilterUIHintAttribute(string filterUIHint) { }

        public Collections.Generic.IDictionary<string, object?> ControlParameters { get { throw null; } }

        public string FilterUIHint { get { throw null; } }

        public string? PresentationLayer { get { throw null; } }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial interface IValidatableObject
    {
        Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed partial class KeyAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public partial class MaxLengthAttribute : ValidationAttribute
    {
        public MaxLengthAttribute() { }

        public MaxLengthAttribute(int length) { }

        public int Length { get { throw null; } }

        public override string FormatErrorMessage(string name) { throw null; }

        public override bool IsValid(object? value) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed partial class MetadataTypeAttribute : Attribute
    {
        public MetadataTypeAttribute(Type metadataClassType) { }

        public Type MetadataClassType { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public partial class MinLengthAttribute : ValidationAttribute
    {
        public MinLengthAttribute(int length) { }

        public int Length { get { throw null; } }

        public override string FormatErrorMessage(string name) { throw null; }

        public override bool IsValid(object? value) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed partial class PhoneAttribute : DataTypeAttribute
    {
        public PhoneAttribute() : base(default(DataType)) { }

        public override bool IsValid(object? value) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public partial class RangeAttribute : ValidationAttribute
    {
        public RangeAttribute(double minimum, double maximum) { }

        public RangeAttribute(int minimum, int maximum) { }

        public RangeAttribute(Type type, string minimum, string maximum) { }

        public bool ConvertValueInInvariantCulture { get { throw null; } set { } }

        public object Maximum { get { throw null; } }

        public object Minimum { get { throw null; } }

        public Type OperandType { get { throw null; } }

        public bool ParseLimitsInInvariantCulture { get { throw null; } set { } }

        public override string FormatErrorMessage(string name) { throw null; }

        public override bool IsValid(object? value) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public partial class RegularExpressionAttribute : ValidationAttribute
    {
        public RegularExpressionAttribute(string pattern) { }

        public int MatchTimeoutInMilliseconds { get { throw null; } set { } }

        public string Pattern { get { throw null; } }

        public override string FormatErrorMessage(string name) { throw null; }

        public override bool IsValid(object? value) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public partial class RequiredAttribute : ValidationAttribute
    {
        public bool AllowEmptyStrings { get { throw null; } set { } }

        public override bool IsValid(object? value) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public partial class ScaffoldColumnAttribute : Attribute
    {
        public ScaffoldColumnAttribute(bool scaffold) { }

        public bool Scaffold { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public partial class StringLengthAttribute : ValidationAttribute
    {
        public StringLengthAttribute(int maximumLength) { }

        public int MaximumLength { get { throw null; } }

        public int MinimumLength { get { throw null; } set { } }

        public override string FormatErrorMessage(string name) { throw null; }

        public override bool IsValid(object? value) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed partial class TimestampAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public partial class UIHintAttribute : Attribute
    {
        public UIHintAttribute(string uiHint, string? presentationLayer, params object?[]? controlParameters) { }

        public UIHintAttribute(string uiHint, string? presentationLayer) { }

        public UIHintAttribute(string uiHint) { }

        public Collections.Generic.IDictionary<string, object?> ControlParameters { get { throw null; } }

        public string? PresentationLayer { get { throw null; } }

        public string UIHint { get { throw null; } }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed partial class UrlAttribute : DataTypeAttribute
    {
        public UrlAttribute() : base(default(DataType)) { }

        public override bool IsValid(object? value) { throw null; }
    }

    public abstract partial class ValidationAttribute : Attribute
    {
        protected ValidationAttribute() { }

        protected ValidationAttribute(Func<string> errorMessageAccessor) { }

        protected ValidationAttribute(string errorMessage) { }

        public string? ErrorMessage { get { throw null; } set { } }

        public string? ErrorMessageResourceName { get { throw null; } set { } }

        public Type? ErrorMessageResourceType { get { throw null; } set { } }

        protected string ErrorMessageString { get { throw null; } }

        public virtual bool RequiresValidationContext { get { throw null; } }

        public virtual string FormatErrorMessage(string name) { throw null; }

        public ValidationResult? GetValidationResult(object? value, ValidationContext validationContext) { throw null; }

        protected virtual ValidationResult? IsValid(object? value, ValidationContext validationContext) { throw null; }

        public virtual bool IsValid(object? value) { throw null; }

        public void Validate(object? value, ValidationContext validationContext) { }

        public void Validate(object? value, string name) { }
    }

    public sealed partial class ValidationContext : IServiceProvider
    {
        public ValidationContext(object instance, Collections.Generic.IDictionary<object, object?>? items) { }

        public ValidationContext(object instance, IServiceProvider? serviceProvider, Collections.Generic.IDictionary<object, object?>? items) { }

        public ValidationContext(object instance) { }

        public string DisplayName { get { throw null; } set { } }

        public Collections.Generic.IDictionary<object, object?> Items { get { throw null; } }

        public string? MemberName { get { throw null; } set { } }

        public object ObjectInstance { get { throw null; } }

        public Type ObjectType { get { throw null; } }

        public object? GetService(Type serviceType) { throw null; }

        public void InitializeServiceProvider(Func<Type, object?> serviceProvider) { }
    }

    public partial class ValidationException : Exception
    {
        public ValidationException() { }

        public ValidationException(ValidationResult validationResult, ValidationAttribute? validatingAttribute, object? value) { }

        protected ValidationException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public ValidationException(string? errorMessage, ValidationAttribute? validatingAttribute, object? value) { }

        public ValidationException(string? message, Exception? innerException) { }

        public ValidationException(string? message) { }

        public ValidationAttribute? ValidationAttribute { get { throw null; } }

        public ValidationResult ValidationResult { get { throw null; } }

        public object? Value { get { throw null; } }
    }

    public partial class ValidationResult
    {
        public static readonly ValidationResult? Success;
        protected ValidationResult(ValidationResult validationResult) { }

        public ValidationResult(string? errorMessage, Collections.Generic.IEnumerable<string>? memberNames) { }

        public ValidationResult(string? errorMessage) { }

        public string? ErrorMessage { get { throw null; } set { } }

        public Collections.Generic.IEnumerable<string> MemberNames { get { throw null; } }

        public override string ToString() { throw null; }
    }

    public static partial class Validator
    {
        public static bool TryValidateObject(object instance, ValidationContext validationContext, Collections.Generic.ICollection<ValidationResult>? validationResults, bool validateAllProperties) { throw null; }

        public static bool TryValidateObject(object instance, ValidationContext validationContext, Collections.Generic.ICollection<ValidationResult>? validationResults) { throw null; }

        public static bool TryValidateProperty(object? value, ValidationContext validationContext, Collections.Generic.ICollection<ValidationResult>? validationResults) { throw null; }

        public static bool TryValidateValue(object value, ValidationContext validationContext, Collections.Generic.ICollection<ValidationResult>? validationResults, Collections.Generic.IEnumerable<ValidationAttribute> validationAttributes) { throw null; }

        public static void ValidateObject(object instance, ValidationContext validationContext, bool validateAllProperties) { }

        public static void ValidateObject(object instance, ValidationContext validationContext) { }

        public static void ValidateProperty(object? value, ValidationContext validationContext) { }

        public static void ValidateValue(object value, ValidationContext validationContext, Collections.Generic.IEnumerable<ValidationAttribute> validationAttributes) { }
    }
}

namespace System.ComponentModel.DataAnnotations.Schema
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public partial class ColumnAttribute : Attribute
    {
        public ColumnAttribute() { }

        public ColumnAttribute(string name) { }

        public string? Name { get { throw null; } }

        public int Order { get { throw null; } set { } }

        public string? TypeName { get { throw null; } set { } }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public partial class ComplexTypeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public partial class DatabaseGeneratedAttribute : Attribute
    {
        public DatabaseGeneratedAttribute(DatabaseGeneratedOption databaseGeneratedOption) { }

        public DatabaseGeneratedOption DatabaseGeneratedOption { get { throw null; } }
    }

    public enum DatabaseGeneratedOption
    {
        None = 0,
        Identity = 1,
        Computed = 2
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public partial class ForeignKeyAttribute : Attribute
    {
        public ForeignKeyAttribute(string name) { }

        public string Name { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public partial class InversePropertyAttribute : Attribute
    {
        public InversePropertyAttribute(string property) { }

        public string Property { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public partial class NotMappedAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public partial class TableAttribute : Attribute
    {
        public TableAttribute(string name) { }

        public string Name { get { throw null; } }

        public string? Schema { get { throw null; } set { } }
    }
}