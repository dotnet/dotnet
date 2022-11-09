// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Linq.Expressions")]
[assembly: AssemblyDescription("System.Linq.Expressions")]
[assembly: AssemblyDefaultAlias("System.Linq.Expressions")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.0.0")]

[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.BinaryExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.BlockExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.CatchBlock))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.ConditionalExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.ConstantExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.DebugInfoExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.DefaultExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.ElementInit))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.Expression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.Expression<>))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.ExpressionType))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.ExpressionVisitor))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.GotoExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.GotoExpressionKind))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.IArgumentProvider))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.IDynamicExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.IndexExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.InvocationExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.LabelExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.LabelTarget))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.LambdaExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.ListInitExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.LoopExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.MemberAssignment))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.MemberBinding))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.MemberBindingType))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.MemberExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.MemberInitExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.MemberListBinding))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.MemberMemberBinding))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.MethodCallExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.NewArrayExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.NewExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.ParameterExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.RuntimeVariablesExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.SwitchCase))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.SwitchExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.SymbolDocumentInfo))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.TryExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.TypeBinaryExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.Expressions.UnaryExpression))]
[assembly: TypeForwardedTo(typeof(System.Linq.IOrderedQueryable))]
[assembly: TypeForwardedTo(typeof(System.Linq.IOrderedQueryable<>))]
[assembly: TypeForwardedTo(typeof(System.Linq.IQueryable))]
[assembly: TypeForwardedTo(typeof(System.Linq.IQueryable<>))]
[assembly: TypeForwardedTo(typeof(System.Linq.IQueryProvider))]



