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
[assembly: System.Reflection.AssemblyTitle("System.ComponentModel.Primitives")]
[assembly: System.Reflection.AssemblyDescription("System.ComponentModel.Primitives")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.ComponentModel.Primitives")]
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
namespace System.ComponentModel
{
    public sealed partial class BrowsableAttribute : Attribute
    {
        public static readonly BrowsableAttribute Default;
        public static readonly BrowsableAttribute No;
        public static readonly BrowsableAttribute Yes;
        public BrowsableAttribute(bool browsable) { }

        public bool Browsable { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class CategoryAttribute : Attribute
    {
        public CategoryAttribute() { }

        public CategoryAttribute(string category) { }

        public static CategoryAttribute Action { get { throw null; } }

        public static CategoryAttribute Appearance { get { throw null; } }

        public static CategoryAttribute Asynchronous { get { throw null; } }

        public static CategoryAttribute Behavior { get { throw null; } }

        public string Category { get { throw null; } }

        public static CategoryAttribute Data { get { throw null; } }

        public static CategoryAttribute Default { get { throw null; } }

        public static CategoryAttribute Design { get { throw null; } }

        public static CategoryAttribute DragDrop { get { throw null; } }

        public static CategoryAttribute Focus { get { throw null; } }

        public static CategoryAttribute Format { get { throw null; } }

        public static CategoryAttribute Key { get { throw null; } }

        public static CategoryAttribute Layout { get { throw null; } }

        public static CategoryAttribute Mouse { get { throw null; } }

        public static CategoryAttribute WindowStyle { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        protected virtual string GetLocalizedString(string value) { throw null; }
    }

    public partial class ComponentCollection
    {
        internal ComponentCollection() { }
    }

    public partial class DescriptionAttribute : Attribute
    {
        public static readonly DescriptionAttribute Default;
        public DescriptionAttribute() { }

        public DescriptionAttribute(string description) { }

        public virtual string Description { get { throw null; } }

        protected string DescriptionValue { get { throw null; } set { } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public sealed partial class DesignerCategoryAttribute : Attribute
    {
        public static readonly DesignerCategoryAttribute Component;
        public static readonly DesignerCategoryAttribute Default;
        public static readonly DesignerCategoryAttribute Form;
        public static readonly DesignerCategoryAttribute Generic;
        public DesignerCategoryAttribute() { }

        public DesignerCategoryAttribute(string category) { }

        public string Category { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public enum DesignerSerializationVisibility
    {
        Hidden = 0,
        Visible = 1,
        Content = 2
    }

    public sealed partial class DesignerSerializationVisibilityAttribute : Attribute
    {
        public static readonly DesignerSerializationVisibilityAttribute Content;
        public static readonly DesignerSerializationVisibilityAttribute Default;
        public static readonly DesignerSerializationVisibilityAttribute Hidden;
        public static readonly DesignerSerializationVisibilityAttribute Visible;
        public DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility visibility) { }

        public DesignerSerializationVisibility Visibility { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public sealed partial class DesignOnlyAttribute : Attribute
    {
        public static readonly DesignOnlyAttribute Default;
        public static readonly DesignOnlyAttribute No;
        public static readonly DesignOnlyAttribute Yes;
        public DesignOnlyAttribute(bool isDesignOnly) { }

        public bool IsDesignOnly { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class DisplayNameAttribute : Attribute
    {
        public static readonly DisplayNameAttribute Default;
        public DisplayNameAttribute() { }

        public DisplayNameAttribute(string displayName) { }

        public virtual string DisplayName { get { throw null; } }

        protected string DisplayNameValue { get { throw null; } set { } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public sealed partial class EventHandlerList : IDisposable
    {
        public Delegate this[object key] { get { throw null; } set { } }

        public void AddHandler(object key, Delegate value) { }

        public void AddHandlers(EventHandlerList listToAddFrom) { }

        public void Dispose() { }

        public void RemoveHandler(object key, Delegate value) { }
    }

    public partial interface IComponent : IDisposable
    {
        ISite Site { get; set; }

        event EventHandler Disposed;
    }

    public partial interface IContainer : IDisposable
    {
        ComponentCollection Components { get; }

        void Add(IComponent component, string name);
        void Add(IComponent component);
        void Remove(IComponent component);
    }

    public sealed partial class ImmutableObjectAttribute : Attribute
    {
        public static readonly ImmutableObjectAttribute Default;
        public static readonly ImmutableObjectAttribute No;
        public static readonly ImmutableObjectAttribute Yes;
        public ImmutableObjectAttribute(bool immutable) { }

        public bool Immutable { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public sealed partial class InitializationEventAttribute : Attribute
    {
        public InitializationEventAttribute(string eventName) { }

        public string EventName { get { throw null; } }
    }

    public partial interface ISite : IServiceProvider
    {
        IComponent Component { get; }

        IContainer Container { get; }

        bool DesignMode { get; }

        string Name { get; set; }
    }

    public sealed partial class LocalizableAttribute : Attribute
    {
        public static readonly LocalizableAttribute Default;
        public static readonly LocalizableAttribute No;
        public static readonly LocalizableAttribute Yes;
        public LocalizableAttribute(bool isLocalizable) { }

        public bool IsLocalizable { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public sealed partial class MergablePropertyAttribute : Attribute
    {
        public static readonly MergablePropertyAttribute Default;
        public static readonly MergablePropertyAttribute No;
        public static readonly MergablePropertyAttribute Yes;
        public MergablePropertyAttribute(bool allowMerge) { }

        public bool AllowMerge { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public sealed partial class NotifyParentPropertyAttribute : Attribute
    {
        public static readonly NotifyParentPropertyAttribute Default;
        public static readonly NotifyParentPropertyAttribute No;
        public static readonly NotifyParentPropertyAttribute Yes;
        public NotifyParentPropertyAttribute(bool notifyParent) { }

        public bool NotifyParent { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public sealed partial class ParenthesizePropertyNameAttribute : Attribute
    {
        public static readonly ParenthesizePropertyNameAttribute Default;
        public ParenthesizePropertyNameAttribute() { }

        public ParenthesizePropertyNameAttribute(bool needParenthesis) { }

        public bool NeedParenthesis { get { throw null; } }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public sealed partial class ReadOnlyAttribute : Attribute
    {
        public static readonly ReadOnlyAttribute Default;
        public static readonly ReadOnlyAttribute No;
        public static readonly ReadOnlyAttribute Yes;
        public ReadOnlyAttribute(bool isReadOnly) { }

        public bool IsReadOnly { get { throw null; } }

        public override bool Equals(object value) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public enum RefreshProperties
    {
        None = 0,
        All = 1,
        Repaint = 2
    }

    public sealed partial class RefreshPropertiesAttribute : Attribute
    {
        public static readonly RefreshPropertiesAttribute All;
        public static readonly RefreshPropertiesAttribute Default;
        public static readonly RefreshPropertiesAttribute Repaint;
        public RefreshPropertiesAttribute(RefreshProperties refresh) { }

        public RefreshProperties RefreshProperties { get { throw null; } }

        public override bool Equals(object value) { throw null; }

        public override int GetHashCode() { throw null; }
    }
}