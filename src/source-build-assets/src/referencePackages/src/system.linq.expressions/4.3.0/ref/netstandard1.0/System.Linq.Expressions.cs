// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.CLSCompliant(true)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Linq.Expressions.dll")]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.0.30319.17929")]
[assembly: System.Reflection.AssemblyDescription("System.Linq.Expressions.dll")]
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Reflection.AssemblyTitle("System.Linq.Expressions.dll")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.0.30319.17929")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Linq
{
    public partial interface IOrderedQueryable : IQueryable, Collections.IEnumerable
    {
    }

    public partial interface IOrderedQueryable<out T> : IOrderedQueryable, IQueryable<T>, Collections.Generic.IEnumerable<T>, IQueryable, Collections.IEnumerable
    {
    }

    public partial interface IQueryable : Collections.IEnumerable
    {
        Type ElementType { get; }

        Expressions.Expression Expression { get; }

        IQueryProvider Provider { get; }
    }

    public partial interface IQueryable<out T> : Collections.Generic.IEnumerable<T>, IQueryable, Collections.IEnumerable
    {
    }

    public partial interface IQueryProvider
    {
        IQueryable CreateQuery(Expressions.Expression expression);
        IQueryable<TElement> CreateQuery<TElement>(Expressions.Expression expression);
        object Execute(Expressions.Expression expression);
        TResult Execute<TResult>(Expressions.Expression expression);
    }
}

namespace System.Linq.Expressions
{
    public partial class BinaryExpression : Expression
    {
        internal BinaryExpression() { }

        public override bool CanReduce { get { throw null; } }

        public LambdaExpression Conversion { get { throw null; } }

        public bool IsLifted { get { throw null; } }

        public bool IsLiftedToNull { get { throw null; } }

        public Expression Left { get { throw null; } }

        public Reflection.MethodInfo Method { get { throw null; } }

        public Expression Right { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public override Expression Reduce() { throw null; }

        public BinaryExpression Update(Expression left, LambdaExpression conversion, Expression right) { throw null; }
    }

    public partial class BlockExpression : Expression
    {
        internal BlockExpression() { }

        public Collections.ObjectModel.ReadOnlyCollection<Expression> Expressions { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public Expression Result { get { throw null; } }

        public override Type Type { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<ParameterExpression> Variables { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public BlockExpression Update(Collections.Generic.IEnumerable<ParameterExpression> variables, Collections.Generic.IEnumerable<Expression> expressions) { throw null; }
    }

    public sealed partial class CatchBlock
    {
        internal CatchBlock() { }

        public Expression Body { get { throw null; } }

        public Expression Filter { get { throw null; } }

        public Type Test { get { throw null; } }

        public ParameterExpression Variable { get { throw null; } }

        public override string ToString() { throw null; }

        public CatchBlock Update(ParameterExpression variable, Expression filter, Expression body) { throw null; }
    }

    public partial class ConditionalExpression : Expression
    {
        internal ConditionalExpression() { }

        public Expression IfFalse { get { throw null; } }

        public Expression IfTrue { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public Expression Test { get { throw null; } }

        public override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public ConditionalExpression Update(Expression test, Expression ifTrue, Expression ifFalse) { throw null; }
    }

    public partial class ConstantExpression : Expression
    {
        internal ConstantExpression() { }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public override Type Type { get { throw null; } }

        public object Value { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }
    }

    public partial class DebugInfoExpression : Expression
    {
        internal DebugInfoExpression() { }

        public SymbolDocumentInfo Document { get { throw null; } }

        public virtual int EndColumn { get { throw null; } }

        public virtual int EndLine { get { throw null; } }

        public virtual bool IsClear { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public virtual int StartColumn { get { throw null; } }

        public virtual int StartLine { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }
    }

    public sealed partial class DefaultExpression : Expression
    {
        internal DefaultExpression() { }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }
    }

    public sealed partial class ElementInit
    {
        internal ElementInit() { }

        public Reflection.MethodInfo AddMethod { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<Expression> Arguments { get { throw null; } }

        public override string ToString() { throw null; }

        public ElementInit Update(Collections.Generic.IEnumerable<Expression> arguments) { throw null; }
    }

    public abstract partial class Expression
    {
        public virtual bool CanReduce { get { throw null; } }

        public virtual ExpressionType NodeType { get { throw null; } }

        public virtual Type Type { get { throw null; } }

        protected internal virtual Expression Accept(ExpressionVisitor visitor) { throw null; }

        public static BinaryExpression Add(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression Add(Expression left, Expression right) { throw null; }

        public static BinaryExpression AddAssign(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression AddAssign(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression AddAssign(Expression left, Expression right) { throw null; }

        public static BinaryExpression AddAssignChecked(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression AddAssignChecked(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression AddAssignChecked(Expression left, Expression right) { throw null; }

        public static BinaryExpression AddChecked(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression AddChecked(Expression left, Expression right) { throw null; }

        public static BinaryExpression And(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression And(Expression left, Expression right) { throw null; }

        public static BinaryExpression AndAlso(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression AndAlso(Expression left, Expression right) { throw null; }

        public static BinaryExpression AndAssign(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression AndAssign(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression AndAssign(Expression left, Expression right) { throw null; }

        public static IndexExpression ArrayAccess(Expression array, Collections.Generic.IEnumerable<Expression> indexes) { throw null; }

        public static IndexExpression ArrayAccess(Expression array, params Expression[] indexes) { throw null; }

        public static MethodCallExpression ArrayIndex(Expression array, Collections.Generic.IEnumerable<Expression> indexes) { throw null; }

        public static BinaryExpression ArrayIndex(Expression array, Expression index) { throw null; }

        public static MethodCallExpression ArrayIndex(Expression array, params Expression[] indexes) { throw null; }

        public static UnaryExpression ArrayLength(Expression array) { throw null; }

        public static BinaryExpression Assign(Expression left, Expression right) { throw null; }

        public static MemberAssignment Bind(Reflection.MemberInfo member, Expression expression) { throw null; }

        public static MemberAssignment Bind(Reflection.MethodInfo propertyAccessor, Expression expression) { throw null; }

        public static BlockExpression Block(Collections.Generic.IEnumerable<Expression> expressions) { throw null; }

        public static BlockExpression Block(Collections.Generic.IEnumerable<ParameterExpression> variables, Collections.Generic.IEnumerable<Expression> expressions) { throw null; }

        public static BlockExpression Block(Collections.Generic.IEnumerable<ParameterExpression> variables, params Expression[] expressions) { throw null; }

        public static BlockExpression Block(Expression arg0, Expression arg1, Expression arg2, Expression arg3, Expression arg4) { throw null; }

        public static BlockExpression Block(Expression arg0, Expression arg1, Expression arg2, Expression arg3) { throw null; }

        public static BlockExpression Block(Expression arg0, Expression arg1, Expression arg2) { throw null; }

        public static BlockExpression Block(Expression arg0, Expression arg1) { throw null; }

        public static BlockExpression Block(params Expression[] expressions) { throw null; }

        public static BlockExpression Block(Type type, Collections.Generic.IEnumerable<Expression> expressions) { throw null; }

        public static BlockExpression Block(Type type, Collections.Generic.IEnumerable<ParameterExpression> variables, Collections.Generic.IEnumerable<Expression> expressions) { throw null; }

        public static BlockExpression Block(Type type, Collections.Generic.IEnumerable<ParameterExpression> variables, params Expression[] expressions) { throw null; }

        public static BlockExpression Block(Type type, params Expression[] expressions) { throw null; }

        public static GotoExpression Break(LabelTarget target, Expression value, Type type) { throw null; }

        public static GotoExpression Break(LabelTarget target, Expression value) { throw null; }

        public static GotoExpression Break(LabelTarget target, Type type) { throw null; }

        public static GotoExpression Break(LabelTarget target) { throw null; }

        public static MethodCallExpression Call(Expression instance, Reflection.MethodInfo method, Collections.Generic.IEnumerable<Expression> arguments) { throw null; }

        public static MethodCallExpression Call(Expression instance, Reflection.MethodInfo method, Expression arg0, Expression arg1, Expression arg2) { throw null; }

        public static MethodCallExpression Call(Expression instance, Reflection.MethodInfo method, Expression arg0, Expression arg1) { throw null; }

        public static MethodCallExpression Call(Expression instance, Reflection.MethodInfo method, params Expression[] arguments) { throw null; }

        public static MethodCallExpression Call(Expression instance, Reflection.MethodInfo method) { throw null; }

        public static MethodCallExpression Call(Expression instance, string methodName, Type[] typeArguments, params Expression[] arguments) { throw null; }

        public static MethodCallExpression Call(Reflection.MethodInfo method, Collections.Generic.IEnumerable<Expression> arguments) { throw null; }

        public static MethodCallExpression Call(Reflection.MethodInfo method, Expression arg0, Expression arg1, Expression arg2, Expression arg3, Expression arg4) { throw null; }

        public static MethodCallExpression Call(Reflection.MethodInfo method, Expression arg0, Expression arg1, Expression arg2, Expression arg3) { throw null; }

        public static MethodCallExpression Call(Reflection.MethodInfo method, Expression arg0, Expression arg1, Expression arg2) { throw null; }

        public static MethodCallExpression Call(Reflection.MethodInfo method, Expression arg0, Expression arg1) { throw null; }

        public static MethodCallExpression Call(Reflection.MethodInfo method, Expression arg0) { throw null; }

        public static MethodCallExpression Call(Reflection.MethodInfo method, params Expression[] arguments) { throw null; }

        public static MethodCallExpression Call(Type type, string methodName, Type[] typeArguments, params Expression[] arguments) { throw null; }

        public static CatchBlock Catch(ParameterExpression variable, Expression body, Expression filter) { throw null; }

        public static CatchBlock Catch(ParameterExpression variable, Expression body) { throw null; }

        public static CatchBlock Catch(Type type, Expression body, Expression filter) { throw null; }

        public static CatchBlock Catch(Type type, Expression body) { throw null; }

        public static DebugInfoExpression ClearDebugInfo(SymbolDocumentInfo document) { throw null; }

        public static BinaryExpression Coalesce(Expression left, Expression right, LambdaExpression conversion) { throw null; }

        public static BinaryExpression Coalesce(Expression left, Expression right) { throw null; }

        public static ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse, Type type) { throw null; }

        public static ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse) { throw null; }

        public static ConstantExpression Constant(object value, Type type) { throw null; }

        public static ConstantExpression Constant(object value) { throw null; }

        public static GotoExpression Continue(LabelTarget target, Type type) { throw null; }

        public static GotoExpression Continue(LabelTarget target) { throw null; }

        public static UnaryExpression Convert(Expression expression, Type type, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression Convert(Expression expression, Type type) { throw null; }

        public static UnaryExpression ConvertChecked(Expression expression, Type type, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression ConvertChecked(Expression expression, Type type) { throw null; }

        public static DebugInfoExpression DebugInfo(SymbolDocumentInfo document, int startLine, int startColumn, int endLine, int endColumn) { throw null; }

        public static UnaryExpression Decrement(Expression expression, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression Decrement(Expression expression) { throw null; }

        public static DefaultExpression Default(Type type) { throw null; }

        public static BinaryExpression Divide(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression Divide(Expression left, Expression right) { throw null; }

        public static BinaryExpression DivideAssign(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression DivideAssign(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression DivideAssign(Expression left, Expression right) { throw null; }

        public static ElementInit ElementInit(Reflection.MethodInfo addMethod, Collections.Generic.IEnumerable<Expression> arguments) { throw null; }

        public static ElementInit ElementInit(Reflection.MethodInfo addMethod, params Expression[] arguments) { throw null; }

        public static DefaultExpression Empty() { throw null; }

        public static BinaryExpression Equal(Expression left, Expression right, bool liftToNull, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression Equal(Expression left, Expression right) { throw null; }

        public static BinaryExpression ExclusiveOr(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression ExclusiveOr(Expression left, Expression right) { throw null; }

        public static BinaryExpression ExclusiveOrAssign(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression ExclusiveOrAssign(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression ExclusiveOrAssign(Expression left, Expression right) { throw null; }

        public static MemberExpression Field(Expression expression, Reflection.FieldInfo field) { throw null; }

        public static MemberExpression Field(Expression expression, string fieldName) { throw null; }

        public static MemberExpression Field(Expression expression, Type type, string fieldName) { throw null; }

        public static Type GetActionType(params Type[] typeArgs) { throw null; }

        public static Type GetDelegateType(params Type[] typeArgs) { throw null; }

        public static Type GetFuncType(params Type[] typeArgs) { throw null; }

        public static GotoExpression Goto(LabelTarget target, Expression value, Type type) { throw null; }

        public static GotoExpression Goto(LabelTarget target, Expression value) { throw null; }

        public static GotoExpression Goto(LabelTarget target, Type type) { throw null; }

        public static GotoExpression Goto(LabelTarget target) { throw null; }

        public static BinaryExpression GreaterThan(Expression left, Expression right, bool liftToNull, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression GreaterThan(Expression left, Expression right) { throw null; }

        public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right, bool liftToNull, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right) { throw null; }

        public static ConditionalExpression IfThen(Expression test, Expression ifTrue) { throw null; }

        public static ConditionalExpression IfThenElse(Expression test, Expression ifTrue, Expression ifFalse) { throw null; }

        public static UnaryExpression Increment(Expression expression, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression Increment(Expression expression) { throw null; }

        public static InvocationExpression Invoke(Expression expression, Collections.Generic.IEnumerable<Expression> arguments) { throw null; }

        public static InvocationExpression Invoke(Expression expression, params Expression[] arguments) { throw null; }

        public static UnaryExpression IsFalse(Expression expression, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression IsFalse(Expression expression) { throw null; }

        public static UnaryExpression IsTrue(Expression expression, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression IsTrue(Expression expression) { throw null; }

        public static LabelTarget Label() { throw null; }

        public static LabelExpression Label(LabelTarget target, Expression defaultValue) { throw null; }

        public static LabelExpression Label(LabelTarget target) { throw null; }

        public static LabelTarget Label(string name) { throw null; }

        public static LabelTarget Label(Type type, string name) { throw null; }

        public static LabelTarget Label(Type type) { throw null; }

        public static LambdaExpression Lambda(Expression body, bool tailCall, Collections.Generic.IEnumerable<ParameterExpression> parameters) { throw null; }

        public static LambdaExpression Lambda(Expression body, bool tailCall, params ParameterExpression[] parameters) { throw null; }

        public static LambdaExpression Lambda(Expression body, Collections.Generic.IEnumerable<ParameterExpression> parameters) { throw null; }

        public static LambdaExpression Lambda(Expression body, params ParameterExpression[] parameters) { throw null; }

        public static LambdaExpression Lambda(Expression body, string name, bool tailCall, Collections.Generic.IEnumerable<ParameterExpression> parameters) { throw null; }

        public static LambdaExpression Lambda(Expression body, string name, Collections.Generic.IEnumerable<ParameterExpression> parameters) { throw null; }

        public static LambdaExpression Lambda(Type delegateType, Expression body, bool tailCall, Collections.Generic.IEnumerable<ParameterExpression> parameters) { throw null; }

        public static LambdaExpression Lambda(Type delegateType, Expression body, bool tailCall, params ParameterExpression[] parameters) { throw null; }

        public static LambdaExpression Lambda(Type delegateType, Expression body, Collections.Generic.IEnumerable<ParameterExpression> parameters) { throw null; }

        public static LambdaExpression Lambda(Type delegateType, Expression body, params ParameterExpression[] parameters) { throw null; }

        public static LambdaExpression Lambda(Type delegateType, Expression body, string name, bool tailCall, Collections.Generic.IEnumerable<ParameterExpression> parameters) { throw null; }

        public static LambdaExpression Lambda(Type delegateType, Expression body, string name, Collections.Generic.IEnumerable<ParameterExpression> parameters) { throw null; }

        public static Expression<TDelegate> Lambda<TDelegate>(Expression body, bool tailCall, Collections.Generic.IEnumerable<ParameterExpression> parameters) { throw null; }

        public static Expression<TDelegate> Lambda<TDelegate>(Expression body, bool tailCall, params ParameterExpression[] parameters) { throw null; }

        public static Expression<TDelegate> Lambda<TDelegate>(Expression body, Collections.Generic.IEnumerable<ParameterExpression> parameters) { throw null; }

        public static Expression<TDelegate> Lambda<TDelegate>(Expression body, params ParameterExpression[] parameters) { throw null; }

        public static Expression<TDelegate> Lambda<TDelegate>(Expression body, string name, bool tailCall, Collections.Generic.IEnumerable<ParameterExpression> parameters) { throw null; }

        public static Expression<TDelegate> Lambda<TDelegate>(Expression body, string name, Collections.Generic.IEnumerable<ParameterExpression> parameters) { throw null; }

        public static BinaryExpression LeftShift(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression LeftShift(Expression left, Expression right) { throw null; }

        public static BinaryExpression LeftShiftAssign(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression LeftShiftAssign(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression LeftShiftAssign(Expression left, Expression right) { throw null; }

        public static BinaryExpression LessThan(Expression left, Expression right, bool liftToNull, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression LessThan(Expression left, Expression right) { throw null; }

        public static BinaryExpression LessThanOrEqual(Expression left, Expression right, bool liftToNull, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression LessThanOrEqual(Expression left, Expression right) { throw null; }

        public static MemberListBinding ListBind(Reflection.MemberInfo member, Collections.Generic.IEnumerable<ElementInit> initializers) { throw null; }

        public static MemberListBinding ListBind(Reflection.MemberInfo member, params ElementInit[] initializers) { throw null; }

        public static MemberListBinding ListBind(Reflection.MethodInfo propertyAccessor, Collections.Generic.IEnumerable<ElementInit> initializers) { throw null; }

        public static MemberListBinding ListBind(Reflection.MethodInfo propertyAccessor, params ElementInit[] initializers) { throw null; }

        public static ListInitExpression ListInit(NewExpression newExpression, Collections.Generic.IEnumerable<ElementInit> initializers) { throw null; }

        public static ListInitExpression ListInit(NewExpression newExpression, Collections.Generic.IEnumerable<Expression> initializers) { throw null; }

        public static ListInitExpression ListInit(NewExpression newExpression, params ElementInit[] initializers) { throw null; }

        public static ListInitExpression ListInit(NewExpression newExpression, params Expression[] initializers) { throw null; }

        public static ListInitExpression ListInit(NewExpression newExpression, Reflection.MethodInfo addMethod, Collections.Generic.IEnumerable<Expression> initializers) { throw null; }

        public static ListInitExpression ListInit(NewExpression newExpression, Reflection.MethodInfo addMethod, params Expression[] initializers) { throw null; }

        public static LoopExpression Loop(Expression body, LabelTarget @break, LabelTarget @continue) { throw null; }

        public static LoopExpression Loop(Expression body, LabelTarget @break) { throw null; }

        public static LoopExpression Loop(Expression body) { throw null; }

        public static BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right, bool liftToNull, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right, bool liftToNull, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right) { throw null; }

        public static CatchBlock MakeCatchBlock(Type type, ParameterExpression variable, Expression body, Expression filter) { throw null; }

        public static GotoExpression MakeGoto(GotoExpressionKind kind, LabelTarget target, Expression value, Type type) { throw null; }

        public static IndexExpression MakeIndex(Expression instance, Reflection.PropertyInfo indexer, Collections.Generic.IEnumerable<Expression> arguments) { throw null; }

        public static MemberExpression MakeMemberAccess(Expression expression, Reflection.MemberInfo member) { throw null; }

        public static TryExpression MakeTry(Type type, Expression body, Expression @finally, Expression fault, Collections.Generic.IEnumerable<CatchBlock> handlers) { throw null; }

        public static UnaryExpression MakeUnary(ExpressionType unaryType, Expression operand, Type type, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression MakeUnary(ExpressionType unaryType, Expression operand, Type type) { throw null; }

        public static MemberMemberBinding MemberBind(Reflection.MemberInfo member, Collections.Generic.IEnumerable<MemberBinding> bindings) { throw null; }

        public static MemberMemberBinding MemberBind(Reflection.MemberInfo member, params MemberBinding[] bindings) { throw null; }

        public static MemberMemberBinding MemberBind(Reflection.MethodInfo propertyAccessor, Collections.Generic.IEnumerable<MemberBinding> bindings) { throw null; }

        public static MemberMemberBinding MemberBind(Reflection.MethodInfo propertyAccessor, params MemberBinding[] bindings) { throw null; }

        public static MemberInitExpression MemberInit(NewExpression newExpression, Collections.Generic.IEnumerable<MemberBinding> bindings) { throw null; }

        public static MemberInitExpression MemberInit(NewExpression newExpression, params MemberBinding[] bindings) { throw null; }

        public static BinaryExpression Modulo(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression Modulo(Expression left, Expression right) { throw null; }

        public static BinaryExpression ModuloAssign(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression ModuloAssign(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression ModuloAssign(Expression left, Expression right) { throw null; }

        public static BinaryExpression Multiply(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression Multiply(Expression left, Expression right) { throw null; }

        public static BinaryExpression MultiplyAssign(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression MultiplyAssign(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression MultiplyAssign(Expression left, Expression right) { throw null; }

        public static BinaryExpression MultiplyAssignChecked(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression MultiplyAssignChecked(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression MultiplyAssignChecked(Expression left, Expression right) { throw null; }

        public static BinaryExpression MultiplyChecked(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression MultiplyChecked(Expression left, Expression right) { throw null; }

        public static UnaryExpression Negate(Expression expression, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression Negate(Expression expression) { throw null; }

        public static UnaryExpression NegateChecked(Expression expression, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression NegateChecked(Expression expression) { throw null; }

        public static NewExpression New(Reflection.ConstructorInfo constructor, Collections.Generic.IEnumerable<Expression> arguments, Collections.Generic.IEnumerable<Reflection.MemberInfo> members) { throw null; }

        public static NewExpression New(Reflection.ConstructorInfo constructor, Collections.Generic.IEnumerable<Expression> arguments, params Reflection.MemberInfo[] members) { throw null; }

        public static NewExpression New(Reflection.ConstructorInfo constructor, Collections.Generic.IEnumerable<Expression> arguments) { throw null; }

        public static NewExpression New(Reflection.ConstructorInfo constructor, params Expression[] arguments) { throw null; }

        public static NewExpression New(Reflection.ConstructorInfo constructor) { throw null; }

        public static NewExpression New(Type type) { throw null; }

        public static NewArrayExpression NewArrayBounds(Type type, Collections.Generic.IEnumerable<Expression> bounds) { throw null; }

        public static NewArrayExpression NewArrayBounds(Type type, params Expression[] bounds) { throw null; }

        public static NewArrayExpression NewArrayInit(Type type, Collections.Generic.IEnumerable<Expression> initializers) { throw null; }

        public static NewArrayExpression NewArrayInit(Type type, params Expression[] initializers) { throw null; }

        public static UnaryExpression Not(Expression expression, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression Not(Expression expression) { throw null; }

        public static BinaryExpression NotEqual(Expression left, Expression right, bool liftToNull, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression NotEqual(Expression left, Expression right) { throw null; }

        public static UnaryExpression OnesComplement(Expression expression, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression OnesComplement(Expression expression) { throw null; }

        public static BinaryExpression Or(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression Or(Expression left, Expression right) { throw null; }

        public static BinaryExpression OrAssign(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression OrAssign(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression OrAssign(Expression left, Expression right) { throw null; }

        public static BinaryExpression OrElse(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression OrElse(Expression left, Expression right) { throw null; }

        public static ParameterExpression Parameter(Type type, string name) { throw null; }

        public static ParameterExpression Parameter(Type type) { throw null; }

        public static UnaryExpression PostDecrementAssign(Expression expression, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression PostDecrementAssign(Expression expression) { throw null; }

        public static UnaryExpression PostIncrementAssign(Expression expression, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression PostIncrementAssign(Expression expression) { throw null; }

        public static BinaryExpression Power(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression Power(Expression left, Expression right) { throw null; }

        public static BinaryExpression PowerAssign(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression PowerAssign(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression PowerAssign(Expression left, Expression right) { throw null; }

        public static UnaryExpression PreDecrementAssign(Expression expression, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression PreDecrementAssign(Expression expression) { throw null; }

        public static UnaryExpression PreIncrementAssign(Expression expression, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression PreIncrementAssign(Expression expression) { throw null; }

        public static MemberExpression Property(Expression expression, Reflection.MethodInfo propertyAccessor) { throw null; }

        public static IndexExpression Property(Expression instance, Reflection.PropertyInfo indexer, Collections.Generic.IEnumerable<Expression> arguments) { throw null; }

        public static IndexExpression Property(Expression instance, Reflection.PropertyInfo indexer, params Expression[] arguments) { throw null; }

        public static MemberExpression Property(Expression expression, Reflection.PropertyInfo property) { throw null; }

        public static IndexExpression Property(Expression instance, string propertyName, params Expression[] arguments) { throw null; }

        public static MemberExpression Property(Expression expression, string propertyName) { throw null; }

        public static MemberExpression Property(Expression expression, Type type, string propertyName) { throw null; }

        public static MemberExpression PropertyOrField(Expression expression, string propertyOrFieldName) { throw null; }

        public static UnaryExpression Quote(Expression expression) { throw null; }

        public virtual Expression Reduce() { throw null; }

        public Expression ReduceAndCheck() { throw null; }

        public Expression ReduceExtensions() { throw null; }

        public static BinaryExpression ReferenceEqual(Expression left, Expression right) { throw null; }

        public static BinaryExpression ReferenceNotEqual(Expression left, Expression right) { throw null; }

        public static UnaryExpression Rethrow() { throw null; }

        public static UnaryExpression Rethrow(Type type) { throw null; }

        public static GotoExpression Return(LabelTarget target, Expression value, Type type) { throw null; }

        public static GotoExpression Return(LabelTarget target, Expression value) { throw null; }

        public static GotoExpression Return(LabelTarget target, Type type) { throw null; }

        public static GotoExpression Return(LabelTarget target) { throw null; }

        public static BinaryExpression RightShift(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression RightShift(Expression left, Expression right) { throw null; }

        public static BinaryExpression RightShiftAssign(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression RightShiftAssign(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression RightShiftAssign(Expression left, Expression right) { throw null; }

        public static RuntimeVariablesExpression RuntimeVariables(Collections.Generic.IEnumerable<ParameterExpression> variables) { throw null; }

        public static RuntimeVariablesExpression RuntimeVariables(params ParameterExpression[] variables) { throw null; }

        public static BinaryExpression Subtract(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression Subtract(Expression left, Expression right) { throw null; }

        public static BinaryExpression SubtractAssign(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression SubtractAssign(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression SubtractAssign(Expression left, Expression right) { throw null; }

        public static BinaryExpression SubtractAssignChecked(Expression left, Expression right, Reflection.MethodInfo method, LambdaExpression conversion) { throw null; }

        public static BinaryExpression SubtractAssignChecked(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression SubtractAssignChecked(Expression left, Expression right) { throw null; }

        public static BinaryExpression SubtractChecked(Expression left, Expression right, Reflection.MethodInfo method) { throw null; }

        public static BinaryExpression SubtractChecked(Expression left, Expression right) { throw null; }

        public static SwitchExpression Switch(Expression switchValue, Expression defaultBody, params SwitchCase[] cases) { throw null; }

        public static SwitchExpression Switch(Expression switchValue, Expression defaultBody, Reflection.MethodInfo comparison, Collections.Generic.IEnumerable<SwitchCase> cases) { throw null; }

        public static SwitchExpression Switch(Expression switchValue, Expression defaultBody, Reflection.MethodInfo comparison, params SwitchCase[] cases) { throw null; }

        public static SwitchExpression Switch(Expression switchValue, params SwitchCase[] cases) { throw null; }

        public static SwitchExpression Switch(Type type, Expression switchValue, Expression defaultBody, Reflection.MethodInfo comparison, Collections.Generic.IEnumerable<SwitchCase> cases) { throw null; }

        public static SwitchExpression Switch(Type type, Expression switchValue, Expression defaultBody, Reflection.MethodInfo comparison, params SwitchCase[] cases) { throw null; }

        public static SwitchCase SwitchCase(Expression body, Collections.Generic.IEnumerable<Expression> testValues) { throw null; }

        public static SwitchCase SwitchCase(Expression body, params Expression[] testValues) { throw null; }

        public static SymbolDocumentInfo SymbolDocument(string fileName, Guid language, Guid languageVendor, Guid documentType) { throw null; }

        public static SymbolDocumentInfo SymbolDocument(string fileName, Guid language, Guid languageVendor) { throw null; }

        public static SymbolDocumentInfo SymbolDocument(string fileName, Guid language) { throw null; }

        public static SymbolDocumentInfo SymbolDocument(string fileName) { throw null; }

        public static UnaryExpression Throw(Expression value, Type type) { throw null; }

        public static UnaryExpression Throw(Expression value) { throw null; }

        public override string ToString() { throw null; }

        public static TryExpression TryCatch(Expression body, params CatchBlock[] handlers) { throw null; }

        public static TryExpression TryCatchFinally(Expression body, Expression @finally, params CatchBlock[] handlers) { throw null; }

        public static TryExpression TryFault(Expression body, Expression fault) { throw null; }

        public static TryExpression TryFinally(Expression body, Expression @finally) { throw null; }

        public static bool TryGetActionType(Type[] typeArgs, out Type actionType) { throw null; }

        public static bool TryGetFuncType(Type[] typeArgs, out Type funcType) { throw null; }

        public static UnaryExpression TypeAs(Expression expression, Type type) { throw null; }

        public static TypeBinaryExpression TypeEqual(Expression expression, Type type) { throw null; }

        public static TypeBinaryExpression TypeIs(Expression expression, Type type) { throw null; }

        public static UnaryExpression UnaryPlus(Expression expression, Reflection.MethodInfo method) { throw null; }

        public static UnaryExpression UnaryPlus(Expression expression) { throw null; }

        public static UnaryExpression Unbox(Expression expression, Type type) { throw null; }

        public static ParameterExpression Variable(Type type, string name) { throw null; }

        public static ParameterExpression Variable(Type type) { throw null; }

        protected internal virtual Expression VisitChildren(ExpressionVisitor visitor) { throw null; }
    }

    public enum ExpressionType
    {
        Add = 0,
        AddChecked = 1,
        And = 2,
        AndAlso = 3,
        ArrayLength = 4,
        ArrayIndex = 5,
        Call = 6,
        Coalesce = 7,
        Conditional = 8,
        Constant = 9,
        Convert = 10,
        ConvertChecked = 11,
        Divide = 12,
        Equal = 13,
        ExclusiveOr = 14,
        GreaterThan = 15,
        GreaterThanOrEqual = 16,
        Invoke = 17,
        Lambda = 18,
        LeftShift = 19,
        LessThan = 20,
        LessThanOrEqual = 21,
        ListInit = 22,
        MemberAccess = 23,
        MemberInit = 24,
        Modulo = 25,
        Multiply = 26,
        MultiplyChecked = 27,
        Negate = 28,
        UnaryPlus = 29,
        NegateChecked = 30,
        New = 31,
        NewArrayInit = 32,
        NewArrayBounds = 33,
        Not = 34,
        NotEqual = 35,
        Or = 36,
        OrElse = 37,
        Parameter = 38,
        Power = 39,
        Quote = 40,
        RightShift = 41,
        Subtract = 42,
        SubtractChecked = 43,
        TypeAs = 44,
        TypeIs = 45,
        Assign = 46,
        Block = 47,
        DebugInfo = 48,
        Decrement = 49,
        Dynamic = 50,
        Default = 51,
        Extension = 52,
        Goto = 53,
        Increment = 54,
        Index = 55,
        Label = 56,
        RuntimeVariables = 57,
        Loop = 58,
        Switch = 59,
        Throw = 60,
        Try = 61,
        Unbox = 62,
        AddAssign = 63,
        AndAssign = 64,
        DivideAssign = 65,
        ExclusiveOrAssign = 66,
        LeftShiftAssign = 67,
        ModuloAssign = 68,
        MultiplyAssign = 69,
        OrAssign = 70,
        PowerAssign = 71,
        RightShiftAssign = 72,
        SubtractAssign = 73,
        AddAssignChecked = 74,
        MultiplyAssignChecked = 75,
        SubtractAssignChecked = 76,
        PreIncrementAssign = 77,
        PreDecrementAssign = 78,
        PostIncrementAssign = 79,
        PostDecrementAssign = 80,
        TypeEqual = 81,
        OnesComplement = 82,
        IsTrue = 83,
        IsFalse = 84
    }

    public abstract partial class ExpressionVisitor
    {
        public Collections.ObjectModel.ReadOnlyCollection<Expression> Visit(Collections.ObjectModel.ReadOnlyCollection<Expression> nodes) { throw null; }

        public virtual Expression Visit(Expression node) { throw null; }

        public static Collections.ObjectModel.ReadOnlyCollection<T> Visit<T>(Collections.ObjectModel.ReadOnlyCollection<T> nodes, Func<T, T> elementVisitor) { throw null; }

        public T VisitAndConvert<T>(T node, string callerName)
            where T : Expression { throw null; }

        public Collections.ObjectModel.ReadOnlyCollection<T> VisitAndConvert<T>(Collections.ObjectModel.ReadOnlyCollection<T> nodes, string callerName)
            where T : Expression { throw null; }

        protected internal virtual Expression VisitBinary(BinaryExpression node) { throw null; }

        protected internal virtual Expression VisitBlock(BlockExpression node) { throw null; }

        protected virtual CatchBlock VisitCatchBlock(CatchBlock node) { throw null; }

        protected internal virtual Expression VisitConditional(ConditionalExpression node) { throw null; }

        protected internal virtual Expression VisitConstant(ConstantExpression node) { throw null; }

        protected internal virtual Expression VisitDebugInfo(DebugInfoExpression node) { throw null; }

        protected internal virtual Expression VisitDefault(DefaultExpression node) { throw null; }

        protected virtual ElementInit VisitElementInit(ElementInit node) { throw null; }

        protected internal virtual Expression VisitExtension(Expression node) { throw null; }

        protected internal virtual Expression VisitGoto(GotoExpression node) { throw null; }

        protected internal virtual Expression VisitIndex(IndexExpression node) { throw null; }

        protected internal virtual Expression VisitInvocation(InvocationExpression node) { throw null; }

        protected internal virtual Expression VisitLabel(LabelExpression node) { throw null; }

        protected virtual LabelTarget VisitLabelTarget(LabelTarget node) { throw null; }

        protected internal virtual Expression VisitLambda<T>(Expression<T> node) { throw null; }

        protected internal virtual Expression VisitListInit(ListInitExpression node) { throw null; }

        protected internal virtual Expression VisitLoop(LoopExpression node) { throw null; }

        protected internal virtual Expression VisitMember(MemberExpression node) { throw null; }

        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment node) { throw null; }

        protected virtual MemberBinding VisitMemberBinding(MemberBinding node) { throw null; }

        protected internal virtual Expression VisitMemberInit(MemberInitExpression node) { throw null; }

        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding node) { throw null; }

        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node) { throw null; }

        protected internal virtual Expression VisitMethodCall(MethodCallExpression node) { throw null; }

        protected internal virtual Expression VisitNew(NewExpression node) { throw null; }

        protected internal virtual Expression VisitNewArray(NewArrayExpression node) { throw null; }

        protected internal virtual Expression VisitParameter(ParameterExpression node) { throw null; }

        protected internal virtual Expression VisitRuntimeVariables(RuntimeVariablesExpression node) { throw null; }

        protected internal virtual Expression VisitSwitch(SwitchExpression node) { throw null; }

        protected virtual SwitchCase VisitSwitchCase(SwitchCase node) { throw null; }

        protected internal virtual Expression VisitTry(TryExpression node) { throw null; }

        protected internal virtual Expression VisitTypeBinary(TypeBinaryExpression node) { throw null; }

        protected internal virtual Expression VisitUnary(UnaryExpression node) { throw null; }
    }

    public sealed partial class Expression<TDelegate> : LambdaExpression
    {
        internal Expression() { }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public new TDelegate Compile() { throw null; }

        public Expression<TDelegate> Update(Expression body, Collections.Generic.IEnumerable<ParameterExpression> parameters) { throw null; }
    }

    public sealed partial class GotoExpression : Expression
    {
        internal GotoExpression() { }

        public GotoExpressionKind Kind { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public LabelTarget Target { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        public Expression Value { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public GotoExpression Update(LabelTarget target, Expression value) { throw null; }
    }

    public enum GotoExpressionKind
    {
        Goto = 0,
        Return = 1,
        Break = 2,
        Continue = 3
    }

    public sealed partial class IndexExpression : Expression
    {
        internal IndexExpression() { }

        public Collections.ObjectModel.ReadOnlyCollection<Expression> Arguments { get { throw null; } }

        public Reflection.PropertyInfo Indexer { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public Expression Object { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public IndexExpression Update(Expression @object, Collections.Generic.IEnumerable<Expression> arguments) { throw null; }
    }

    public sealed partial class InvocationExpression : Expression
    {
        internal InvocationExpression() { }

        public Collections.ObjectModel.ReadOnlyCollection<Expression> Arguments { get { throw null; } }

        public Expression Expression { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public InvocationExpression Update(Expression expression, Collections.Generic.IEnumerable<Expression> arguments) { throw null; }
    }

    public sealed partial class LabelExpression : Expression
    {
        internal LabelExpression() { }

        public Expression DefaultValue { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public LabelTarget Target { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public LabelExpression Update(LabelTarget target, Expression defaultValue) { throw null; }
    }

    public sealed partial class LabelTarget
    {
        internal LabelTarget() { }

        public string Name { get { throw null; } }

        public Type Type { get { throw null; } }

        public override string ToString() { throw null; }
    }

    public abstract partial class LambdaExpression : Expression
    {
        internal LambdaExpression() { }

        public Expression Body { get { throw null; } }

        public string Name { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<ParameterExpression> Parameters { get { throw null; } }

        public Type ReturnType { get { throw null; } }

        public bool TailCall { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        public Delegate Compile() { throw null; }
    }

    public sealed partial class ListInitExpression : Expression
    {
        internal ListInitExpression() { }

        public override bool CanReduce { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<ElementInit> Initializers { get { throw null; } }

        public NewExpression NewExpression { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public override Expression Reduce() { throw null; }

        public ListInitExpression Update(NewExpression newExpression, Collections.Generic.IEnumerable<ElementInit> initializers) { throw null; }
    }

    public sealed partial class LoopExpression : Expression
    {
        internal LoopExpression() { }

        public Expression Body { get { throw null; } }

        public LabelTarget BreakLabel { get { throw null; } }

        public LabelTarget ContinueLabel { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public LoopExpression Update(LabelTarget breakLabel, LabelTarget continueLabel, Expression body) { throw null; }
    }

    public sealed partial class MemberAssignment : MemberBinding
    {
        internal MemberAssignment() { }

        public Expression Expression { get { throw null; } }

        public MemberAssignment Update(Expression expression) { throw null; }
    }

    public abstract partial class MemberBinding
    {
        internal MemberBinding() { }

        public MemberBindingType BindingType { get { throw null; } }

        public Reflection.MemberInfo Member { get { throw null; } }

        public override string ToString() { throw null; }
    }

    public enum MemberBindingType
    {
        Assignment = 0,
        MemberBinding = 1,
        ListBinding = 2
    }

    public partial class MemberExpression : Expression
    {
        internal MemberExpression() { }

        public Expression Expression { get { throw null; } }

        public Reflection.MemberInfo Member { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public MemberExpression Update(Expression expression) { throw null; }
    }

    public sealed partial class MemberInitExpression : Expression
    {
        internal MemberInitExpression() { }

        public Collections.ObjectModel.ReadOnlyCollection<MemberBinding> Bindings { get { throw null; } }

        public override bool CanReduce { get { throw null; } }

        public NewExpression NewExpression { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public override Expression Reduce() { throw null; }

        public MemberInitExpression Update(NewExpression newExpression, Collections.Generic.IEnumerable<MemberBinding> bindings) { throw null; }
    }

    public sealed partial class MemberListBinding : MemberBinding
    {
        internal MemberListBinding() { }

        public Collections.ObjectModel.ReadOnlyCollection<ElementInit> Initializers { get { throw null; } }

        public MemberListBinding Update(Collections.Generic.IEnumerable<ElementInit> initializers) { throw null; }
    }

    public sealed partial class MemberMemberBinding : MemberBinding
    {
        internal MemberMemberBinding() { }

        public Collections.ObjectModel.ReadOnlyCollection<MemberBinding> Bindings { get { throw null; } }

        public MemberMemberBinding Update(Collections.Generic.IEnumerable<MemberBinding> bindings) { throw null; }
    }

    public partial class MethodCallExpression : Expression
    {
        internal MethodCallExpression() { }

        public Collections.ObjectModel.ReadOnlyCollection<Expression> Arguments { get { throw null; } }

        public Reflection.MethodInfo Method { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public Expression Object { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public MethodCallExpression Update(Expression @object, Collections.Generic.IEnumerable<Expression> arguments) { throw null; }
    }

    public partial class NewArrayExpression : Expression
    {
        internal NewArrayExpression() { }

        public Collections.ObjectModel.ReadOnlyCollection<Expression> Expressions { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public NewArrayExpression Update(Collections.Generic.IEnumerable<Expression> expressions) { throw null; }
    }

    public partial class NewExpression : Expression
    {
        internal NewExpression() { }

        public Collections.ObjectModel.ReadOnlyCollection<Expression> Arguments { get { throw null; } }

        public Reflection.ConstructorInfo Constructor { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<Reflection.MemberInfo> Members { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public NewExpression Update(Collections.Generic.IEnumerable<Expression> arguments) { throw null; }
    }

    public partial class ParameterExpression : Expression
    {
        internal ParameterExpression() { }

        public bool IsByRef { get { throw null; } }

        public string Name { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }
    }

    public sealed partial class RuntimeVariablesExpression : Expression
    {
        internal RuntimeVariablesExpression() { }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<ParameterExpression> Variables { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public RuntimeVariablesExpression Update(Collections.Generic.IEnumerable<ParameterExpression> variables) { throw null; }
    }

    public sealed partial class SwitchCase
    {
        internal SwitchCase() { }

        public Expression Body { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<Expression> TestValues { get { throw null; } }

        public override string ToString() { throw null; }

        public SwitchCase Update(Collections.Generic.IEnumerable<Expression> testValues, Expression body) { throw null; }
    }

    public sealed partial class SwitchExpression : Expression
    {
        internal SwitchExpression() { }

        public Collections.ObjectModel.ReadOnlyCollection<SwitchCase> Cases { get { throw null; } }

        public Reflection.MethodInfo Comparison { get { throw null; } }

        public Expression DefaultBody { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public Expression SwitchValue { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public SwitchExpression Update(Expression switchValue, Collections.Generic.IEnumerable<SwitchCase> cases, Expression defaultBody) { throw null; }
    }

    public partial class SymbolDocumentInfo
    {
        internal SymbolDocumentInfo() { }

        public virtual Guid DocumentType { get { throw null; } }

        public string FileName { get { throw null; } }

        public virtual Guid Language { get { throw null; } }

        public virtual Guid LanguageVendor { get { throw null; } }
    }

    public sealed partial class TryExpression : Expression
    {
        internal TryExpression() { }

        public Expression Body { get { throw null; } }

        public Expression Fault { get { throw null; } }

        public Expression Finally { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<CatchBlock> Handlers { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public TryExpression Update(Expression body, Collections.Generic.IEnumerable<CatchBlock> handlers, Expression @finally, Expression fault) { throw null; }
    }

    public sealed partial class TypeBinaryExpression : Expression
    {
        internal TypeBinaryExpression() { }

        public Expression Expression { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        public Type TypeOperand { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public TypeBinaryExpression Update(Expression expression) { throw null; }
    }

    public sealed partial class UnaryExpression : Expression
    {
        internal UnaryExpression() { }

        public override bool CanReduce { get { throw null; } }

        public bool IsLifted { get { throw null; } }

        public bool IsLiftedToNull { get { throw null; } }

        public Reflection.MethodInfo Method { get { throw null; } }

        public sealed override ExpressionType NodeType { get { throw null; } }

        public Expression Operand { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        protected internal override Expression Accept(ExpressionVisitor visitor) { throw null; }

        public override Expression Reduce() { throw null; }

        public UnaryExpression Update(Expression operand) { throw null; }
    }
}