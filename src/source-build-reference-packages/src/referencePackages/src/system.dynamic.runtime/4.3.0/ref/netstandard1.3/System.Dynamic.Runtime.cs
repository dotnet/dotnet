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
[assembly: System.Reflection.AssemblyTitle("System.Dynamic.Runtime")]
[assembly: System.Reflection.AssemblyDescription("System.Dynamic.Runtime")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Dynamic.Runtime")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.10.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Runtime.CompilerServices.ConditionalWeakTable<,>))]
namespace System.Dynamic
{
    public abstract partial class BinaryOperationBinder : DynamicMetaObjectBinder
    {
        protected BinaryOperationBinder(Linq.Expressions.ExpressionType operation) { }

        public Linq.Expressions.ExpressionType Operation { get { throw null; } }

        public sealed override Type ReturnType { get { throw null; } }

        public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }

        public abstract DynamicMetaObject FallbackBinaryOperation(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion);
        public DynamicMetaObject FallbackBinaryOperation(DynamicMetaObject target, DynamicMetaObject arg) { throw null; }
    }

    public abstract partial class BindingRestrictions
    {
        internal BindingRestrictions() { }

        public static readonly BindingRestrictions Empty;
        public static BindingRestrictions Combine(Collections.Generic.IList<DynamicMetaObject> contributingObjects) { throw null; }

        public static BindingRestrictions GetExpressionRestriction(Linq.Expressions.Expression expression) { throw null; }

        public static BindingRestrictions GetInstanceRestriction(Linq.Expressions.Expression expression, object instance) { throw null; }

        public static BindingRestrictions GetTypeRestriction(Linq.Expressions.Expression expression, Type type) { throw null; }

        public BindingRestrictions Merge(BindingRestrictions restrictions) { throw null; }

        public Linq.Expressions.Expression ToExpression() { throw null; }
    }

    public sealed partial class CallInfo
    {
        public CallInfo(int argCount, Collections.Generic.IEnumerable<string> argNames) { }

        public CallInfo(int argCount, params string[] argNames) { }

        public int ArgumentCount { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<string> ArgumentNames { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public abstract partial class ConvertBinder : DynamicMetaObjectBinder
    {
        protected ConvertBinder(Type type, bool @explicit) { }

        public bool Explicit { get { throw null; } }

        public sealed override Type ReturnType { get { throw null; } }

        public Type Type { get { throw null; } }

        public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }

        public abstract DynamicMetaObject FallbackConvert(DynamicMetaObject target, DynamicMetaObject errorSuggestion);
        public DynamicMetaObject FallbackConvert(DynamicMetaObject target) { throw null; }
    }

    public abstract partial class CreateInstanceBinder : DynamicMetaObjectBinder
    {
        protected CreateInstanceBinder(CallInfo callInfo) { }

        public CallInfo CallInfo { get { throw null; } }

        public sealed override Type ReturnType { get { throw null; } }

        public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }

        public abstract DynamicMetaObject FallbackCreateInstance(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion);
        public DynamicMetaObject FallbackCreateInstance(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }
    }

    public abstract partial class DeleteIndexBinder : DynamicMetaObjectBinder
    {
        protected DeleteIndexBinder(CallInfo callInfo) { }

        public CallInfo CallInfo { get { throw null; } }

        public sealed override Type ReturnType { get { throw null; } }

        public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }

        public abstract DynamicMetaObject FallbackDeleteIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject errorSuggestion);
        public DynamicMetaObject FallbackDeleteIndex(DynamicMetaObject target, DynamicMetaObject[] indexes) { throw null; }
    }

    public abstract partial class DeleteMemberBinder : DynamicMetaObjectBinder
    {
        protected DeleteMemberBinder(string name, bool ignoreCase) { }

        public bool IgnoreCase { get { throw null; } }

        public string Name { get { throw null; } }

        public sealed override Type ReturnType { get { throw null; } }

        public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }

        public abstract DynamicMetaObject FallbackDeleteMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion);
        public DynamicMetaObject FallbackDeleteMember(DynamicMetaObject target) { throw null; }
    }

    public partial class DynamicMetaObject
    {
        public static readonly DynamicMetaObject[] EmptyMetaObjects;
        public DynamicMetaObject(Linq.Expressions.Expression expression, BindingRestrictions restrictions, object value) { }

        public DynamicMetaObject(Linq.Expressions.Expression expression, BindingRestrictions restrictions) { }

        public Linq.Expressions.Expression Expression { get { throw null; } }

        public bool HasValue { get { throw null; } }

        public Type LimitType { get { throw null; } }

        public BindingRestrictions Restrictions { get { throw null; } }

        public Type RuntimeType { get { throw null; } }

        public object Value { get { throw null; } }

        public virtual DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg) { throw null; }

        public virtual DynamicMetaObject BindConvert(ConvertBinder binder) { throw null; }

        public virtual DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args) { throw null; }

        public virtual DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes) { throw null; }

        public virtual DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder) { throw null; }

        public virtual DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes) { throw null; }

        public virtual DynamicMetaObject BindGetMember(GetMemberBinder binder) { throw null; }

        public virtual DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args) { throw null; }

        public virtual DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args) { throw null; }

        public virtual DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value) { throw null; }

        public virtual DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value) { throw null; }

        public virtual DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder) { throw null; }

        public static DynamicMetaObject Create(object value, Linq.Expressions.Expression expression) { throw null; }

        public virtual Collections.Generic.IEnumerable<string> GetDynamicMemberNames() { throw null; }
    }

    public abstract partial class DynamicMetaObjectBinder : Runtime.CompilerServices.CallSiteBinder
    {
        public virtual Type ReturnType { get { throw null; } }

        public abstract DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args);
        public sealed override Linq.Expressions.Expression Bind(object[] args, Collections.ObjectModel.ReadOnlyCollection<Linq.Expressions.ParameterExpression> parameters, Linq.Expressions.LabelTarget returnLabel) { throw null; }

        public DynamicMetaObject Defer(DynamicMetaObject target, params DynamicMetaObject[] args) { throw null; }

        public DynamicMetaObject Defer(params DynamicMetaObject[] args) { throw null; }

        public Linq.Expressions.Expression GetUpdateExpression(Type type) { throw null; }
    }

    public partial class DynamicObject : IDynamicMetaObjectProvider
    {
        protected DynamicObject() { }

        public virtual Collections.Generic.IEnumerable<string> GetDynamicMemberNames() { throw null; }

        public virtual DynamicMetaObject GetMetaObject(Linq.Expressions.Expression parameter) { throw null; }

        public virtual bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result) { throw null; }

        public virtual bool TryConvert(ConvertBinder binder, out object result) { throw null; }

        public virtual bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out object result) { throw null; }

        public virtual bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes) { throw null; }

        public virtual bool TryDeleteMember(DeleteMemberBinder binder) { throw null; }

        public virtual bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result) { throw null; }

        public virtual bool TryGetMember(GetMemberBinder binder, out object result) { throw null; }

        public virtual bool TryInvoke(InvokeBinder binder, object[] args, out object result) { throw null; }

        public virtual bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) { throw null; }

        public virtual bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value) { throw null; }

        public virtual bool TrySetMember(SetMemberBinder binder, object value) { throw null; }

        public virtual bool TryUnaryOperation(UnaryOperationBinder binder, out object result) { throw null; }
    }

    public sealed partial class ExpandoObject : Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, object>>, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object>>, Collections.IEnumerable, Collections.Generic.IDictionary<string, object>, ComponentModel.INotifyPropertyChanged, IDynamicMetaObjectProvider
    {
        int Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, object>>.Count { get { throw null; } }

        bool Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, object>>.IsReadOnly { get { throw null; } }

        object Collections.Generic.IDictionary<string, object>.this[string key] { get { throw null; } set { } }

        Collections.Generic.ICollection<string> Collections.Generic.IDictionary<string, object>.Keys { get { throw null; } }

        Collections.Generic.ICollection<object> Collections.Generic.IDictionary<string, object>.Values { get { throw null; } }

        void Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, object>>.Add(Collections.Generic.KeyValuePair<string, object> item) { }

        void Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, object>>.Clear() { }

        bool Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, object>>.Contains(Collections.Generic.KeyValuePair<string, object> item) { throw null; }

        void Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, object>>.CopyTo(Collections.Generic.KeyValuePair<string, object>[] array, int arrayIndex) { }

        bool Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, object>>.Remove(Collections.Generic.KeyValuePair<string, object> item) { throw null; }

        void Collections.Generic.IDictionary<string, object>.Add(string key, object value) { }

        bool Collections.Generic.IDictionary<string, object>.ContainsKey(string key) { throw null; }

        bool Collections.Generic.IDictionary<string, object>.Remove(string key) { throw null; }

        bool Collections.Generic.IDictionary<string, object>.TryGetValue(string key, out object value) { throw null; }

        Collections.Generic.IEnumerator<Collections.Generic.KeyValuePair<string, object>> Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object>>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Linq.Expressions.Expression parameter) { throw null; }
    }

    public abstract partial class GetIndexBinder : DynamicMetaObjectBinder
    {
        protected GetIndexBinder(CallInfo callInfo) { }

        public CallInfo CallInfo { get { throw null; } }

        public sealed override Type ReturnType { get { throw null; } }

        public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }

        public abstract DynamicMetaObject FallbackGetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject errorSuggestion);
        public DynamicMetaObject FallbackGetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes) { throw null; }
    }

    public abstract partial class GetMemberBinder : DynamicMetaObjectBinder
    {
        protected GetMemberBinder(string name, bool ignoreCase) { }

        public bool IgnoreCase { get { throw null; } }

        public string Name { get { throw null; } }

        public sealed override Type ReturnType { get { throw null; } }

        public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }

        public abstract DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion);
        public DynamicMetaObject FallbackGetMember(DynamicMetaObject target) { throw null; }
    }

    public partial interface IDynamicMetaObjectProvider
    {
        DynamicMetaObject GetMetaObject(Linq.Expressions.Expression parameter);
    }

    public partial interface IInvokeOnGetBinder
    {
        bool InvokeOnGet { get; }
    }

    public abstract partial class InvokeBinder : DynamicMetaObjectBinder
    {
        protected InvokeBinder(CallInfo callInfo) { }

        public CallInfo CallInfo { get { throw null; } }

        public sealed override Type ReturnType { get { throw null; } }

        public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }

        public abstract DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion);
        public DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }
    }

    public abstract partial class InvokeMemberBinder : DynamicMetaObjectBinder
    {
        protected InvokeMemberBinder(string name, bool ignoreCase, CallInfo callInfo) { }

        public CallInfo CallInfo { get { throw null; } }

        public bool IgnoreCase { get { throw null; } }

        public string Name { get { throw null; } }

        public sealed override Type ReturnType { get { throw null; } }

        public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }

        public abstract DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion);
        public abstract DynamicMetaObject FallbackInvokeMember(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion);
        public DynamicMetaObject FallbackInvokeMember(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }
    }

    public abstract partial class SetIndexBinder : DynamicMetaObjectBinder
    {
        protected SetIndexBinder(CallInfo callInfo) { }

        public CallInfo CallInfo { get { throw null; } }

        public sealed override Type ReturnType { get { throw null; } }

        public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }

        public abstract DynamicMetaObject FallbackSetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject value, DynamicMetaObject errorSuggestion);
        public DynamicMetaObject FallbackSetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject value) { throw null; }
    }

    public abstract partial class SetMemberBinder : DynamicMetaObjectBinder
    {
        protected SetMemberBinder(string name, bool ignoreCase) { }

        public bool IgnoreCase { get { throw null; } }

        public string Name { get { throw null; } }

        public sealed override Type ReturnType { get { throw null; } }

        public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }

        public abstract DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion);
        public DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value) { throw null; }
    }

    public abstract partial class UnaryOperationBinder : DynamicMetaObjectBinder
    {
        protected UnaryOperationBinder(Linq.Expressions.ExpressionType operation) { }

        public Linq.Expressions.ExpressionType Operation { get { throw null; } }

        public sealed override Type ReturnType { get { throw null; } }

        public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args) { throw null; }

        public abstract DynamicMetaObject FallbackUnaryOperation(DynamicMetaObject target, DynamicMetaObject errorSuggestion);
        public DynamicMetaObject FallbackUnaryOperation(DynamicMetaObject target) { throw null; }
    }
}

namespace System.Linq.Expressions
{
    public partial class DynamicExpression : Expression, IArgumentProvider, IDynamicExpression
    {
        internal DynamicExpression() { }

        public Collections.ObjectModel.ReadOnlyCollection<Expression> Arguments { get { throw null; } }

        public Runtime.CompilerServices.CallSiteBinder Binder { get { throw null; } }

        public Type DelegateType { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        int IArgumentProvider.ArgumentCount { get { throw null; } }

        public override Type Type { get { throw null; } }

        protected override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public static DynamicExpression Dynamic(Runtime.CompilerServices.CallSiteBinder binder, Type returnType, Collections.Generic.IEnumerable<Expression> arguments) { throw null; }

        public static DynamicExpression Dynamic(Runtime.CompilerServices.CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1, Expression arg2, Expression arg3) { throw null; }

        public static DynamicExpression Dynamic(Runtime.CompilerServices.CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1, Expression arg2) { throw null; }

        public static DynamicExpression Dynamic(Runtime.CompilerServices.CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1) { throw null; }

        public static DynamicExpression Dynamic(Runtime.CompilerServices.CallSiteBinder binder, Type returnType, Expression arg0) { throw null; }

        public static DynamicExpression Dynamic(Runtime.CompilerServices.CallSiteBinder binder, Type returnType, params Expression[] arguments) { throw null; }

        public static DynamicExpression MakeDynamic(Type delegateType, Runtime.CompilerServices.CallSiteBinder binder, Collections.Generic.IEnumerable<Expression> arguments) { throw null; }

        public static DynamicExpression MakeDynamic(Type delegateType, Runtime.CompilerServices.CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2, Expression arg3) { throw null; }

        public static DynamicExpression MakeDynamic(Type delegateType, Runtime.CompilerServices.CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2) { throw null; }

        public static DynamicExpression MakeDynamic(Type delegateType, Runtime.CompilerServices.CallSiteBinder binder, Expression arg0, Expression arg1) { throw null; }

        public static DynamicExpression MakeDynamic(Type delegateType, Runtime.CompilerServices.CallSiteBinder binder, Expression arg0) { throw null; }

        public static DynamicExpression MakeDynamic(Type delegateType, Runtime.CompilerServices.CallSiteBinder binder, params Expression[] arguments) { throw null; }

        Expression IArgumentProvider.GetArgument(int index) { throw null; }

        object IDynamicExpression.CreateCallSite() { throw null; }

        Expression IDynamicExpression.Rewrite(Expression[] args) { throw null; }

        public DynamicExpression Update(Collections.Generic.IEnumerable<Expression> arguments) { throw null; }
    }

    public abstract partial class DynamicExpressionVisitor : ExpressionVisitor
    {
        protected virtual Expression VisitDynamic(DynamicExpression node) { throw null; }
    }
}

namespace System.Runtime.CompilerServices
{
    public partial class CallSite
    {
        internal CallSite() { }

        public CallSiteBinder Binder { get { throw null; } }

        public static CallSite Create(Type delegateType, CallSiteBinder binder) { throw null; }
    }

    public abstract partial class CallSiteBinder
    {
        public static Linq.Expressions.LabelTarget UpdateLabel { get { throw null; } }

        public abstract Linq.Expressions.Expression Bind(object[] args, Collections.ObjectModel.ReadOnlyCollection<Linq.Expressions.ParameterExpression> parameters, Linq.Expressions.LabelTarget returnLabel);
        public virtual T BindDelegate<T>(CallSite<T> site, object[] args)
            where T : class { throw null; }

        protected void CacheTarget<T>(T target)
            where T : class { }
    }

    public static partial class CallSiteHelpers
    {
        public static bool IsInternalFrame(Reflection.MethodBase mb) { throw null; }
    }

    public partial class CallSite<T> : CallSite where T : class
    {
        internal CallSite() { }

        public T Target;
        public T Update { get { throw null; } }

        public static CallSite<T> Create(CallSiteBinder binder) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public sealed partial class DynamicAttribute : Attribute
    {
        public DynamicAttribute() { }

        public DynamicAttribute(bool[] transformFlags) { }

        public Collections.Generic.IList<bool> TransformFlags { get { throw null; } }
    }
}