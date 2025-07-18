﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using Microsoft.CodeAnalysis.PooledObjects;
using Microsoft.VisualStudio.Debugger;
using Microsoft.VisualStudio.Debugger.Clr;
using Microsoft.VisualStudio.Debugger.Evaluation;
using Microsoft.VisualStudio.Debugger.Evaluation.ClrCompilation;
using Xunit;

namespace Microsoft.CodeAnalysis.ExpressionEvaluator
{
    public abstract class ResultProviderTestBase
    {
        internal static string GetDynamicDebugViewEmptyMessage()
        {
            // Value should not be cached since it depends on the current CultureInfo.
            var exceptionType = typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException).Assembly.GetType(
                "Microsoft.CSharp.RuntimeBinder.DynamicMetaObjectProviderDebugView+DynamicDebugViewEmptyException");
            var emptyProperty = exceptionType.GetProperty("Empty");
            return (string)emptyProperty.GetValue(exceptionType.Instantiate());
        }

        internal static DkmClrCustomTypeInfo MakeCustomTypeInfo(params bool[] dynamicFlags)
        {
            if (dynamicFlags == null || dynamicFlags.Length == 0)
            {
                return null;
            }

            var builder = ArrayBuilder<bool>.GetInstance(dynamicFlags.Length);
            builder.AddRange(dynamicFlags);
            var result = CustomTypeInfo.Create(DynamicFlagsCustomTypeInfo.ToBytes(builder), tupleElementNames: null);
            builder.Free();
            return result;
        }

        private readonly DkmInspectionSession _inspectionSession;
        internal readonly DkmInspectionContext DefaultInspectionContext;

        internal ResultProviderTestBase(DkmInspectionSession inspectionSession, DkmInspectionContext defaultInspectionContext)
        {
            _inspectionSession = inspectionSession;
            DefaultInspectionContext = defaultInspectionContext;

            // We never want to swallow Exceptions (generate a non-fatal Watson) when running tests.
            ExpressionEvaluatorFatalError.IsFailFastEnabled = true;
        }

        internal DkmClrValue CreateDkmClrValue(
            object value,
            Type type = null,
            string alias = null,
            DkmEvaluationResultFlags evalFlags = DkmEvaluationResultFlags.None,
            DkmClrValueFlags valueFlags = DkmClrValueFlags.None)
        {
            if (type == null)
            {
                type = value.GetType();
            }
            return new DkmClrValue(
                value,
                DkmClrValue.GetHostObjectValue((TypeImpl)type, value),
                new DkmClrType((TypeImpl)type),
                alias,
                evalFlags,
                valueFlags);
        }

        internal DkmClrValue CreateDkmClrValue(
            object value,
            DkmClrType type,
            string alias = null,
            DkmEvaluationResultFlags evalFlags = DkmEvaluationResultFlags.None,
            DkmClrValueFlags valueFlags = DkmClrValueFlags.None,
            ulong nativeComPointer = 0)
        {
            return new DkmClrValue(
                value,
                DkmClrValue.GetHostObjectValue(type.GetLmrType(), value),
                type,
                alias,
                evalFlags,
                valueFlags,
                nativeComPointer: nativeComPointer);
        }

        internal DkmClrValue CreateErrorValue(
            DkmClrType type,
            string message)
        {
            return new DkmClrValue(
                value: null,
                hostObjectValue: message,
                type: type,
                alias: null,
                evalFlags: DkmEvaluationResultFlags.None,
                valueFlags: DkmClrValueFlags.Error);
        }

        #region Formatter Tests

        internal string FormatNull<T>(bool useHexadecimal = false)
        {
            return FormatValue(null, typeof(T), useHexadecimal);
        }

        internal string FormatValue(object value, bool useHexadecimal = false)
        {
            return FormatValue(value, value.GetType(), useHexadecimal);
        }

        internal string FormatValue(object value, Type type, bool useHexadecimal = false)
        {
            var clrValue = CreateDkmClrValue(value, type);
            return FormatValue(clrValue, useHexadecimal);
        }

        internal string FormatValue(DkmClrValue clrValue, bool useHexadecimal = false)
        {
            var inspectionContext = CreateDkmInspectionContext(_inspectionSession, DkmEvaluationFlags.None, radix: useHexadecimal ? 16u : 10u);
            return clrValue.GetValueString(inspectionContext, Formatter.NoFormatSpecifiers);
        }

        internal bool HasUnderlyingString(object value)
        {
            return HasUnderlyingString(value, value.GetType());
        }

        internal bool HasUnderlyingString(object value, Type type)
        {
            var clrValue = GetValueForUnderlyingString(value, type);
            return clrValue.HasUnderlyingString(DefaultInspectionContext);
        }

        internal string GetUnderlyingString(object value)
        {
            var clrValue = GetValueForUnderlyingString(value, value.GetType());
            return clrValue.GetUnderlyingString(DefaultInspectionContext);
        }

        internal DkmClrValue GetValueForUnderlyingString(object value, Type type)
        {
            return CreateDkmClrValue(
                value,
                type,
                evalFlags: DkmEvaluationResultFlags.RawString);
        }

        #endregion

        #region ResultProvider Tests

        internal DkmInspectionContext CreateDkmInspectionContext(
            DkmEvaluationFlags flags = DkmEvaluationFlags.None,
            uint radix = 10,
            DkmRuntimeInstance runtimeInstance = null)
        {
            return CreateDkmInspectionContext(_inspectionSession, flags, radix, runtimeInstance);
        }

        internal static DkmInspectionContext CreateDkmInspectionContext(
            DkmInspectionSession inspectionSession,
            DkmEvaluationFlags flags,
            uint radix,
            DkmRuntimeInstance runtimeInstance = null)
        {
            return new DkmInspectionContext(inspectionSession, flags, radix, runtimeInstance);
        }

        internal DkmEvaluationResult FormatResult(string name, DkmClrValue value, DkmClrType declaredType = null, DkmInspectionContext inspectionContext = null)
        {
            return FormatResult(name, name, value, declaredType, inspectionContext: inspectionContext);
        }

        internal DkmEvaluationResult FormatResult(string name, string fullName, DkmClrValue value, DkmClrType declaredType = null, DkmClrCustomTypeInfo declaredTypeInfo = null, DkmInspectionContext inspectionContext = null)
        {
            var asyncResult = FormatAsyncResult(name, fullName, value, declaredType, declaredTypeInfo, inspectionContext);
            var exception = asyncResult.Exception;
            if (exception != null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
            return asyncResult.Result;
        }

        internal DkmEvaluationAsyncResult FormatAsyncResult(string name, string fullName, DkmClrValue value, DkmClrType declaredType = null, DkmClrCustomTypeInfo declaredTypeInfo = null, DkmInspectionContext inspectionContext = null)
        {
            DkmEvaluationAsyncResult asyncResult = default(DkmEvaluationAsyncResult);
            var workList = new DkmWorkList();
            value.GetResult(
                workList,
                DeclaredType: declaredType ?? value.Type,
                CustomTypeInfo: declaredTypeInfo,
                InspectionContext: inspectionContext ?? DefaultInspectionContext,
                FormatSpecifiers: Formatter.NoFormatSpecifiers,
                ResultName: name,
                ResultFullName: fullName,
                CompletionRoutine: r => asyncResult = r);
            workList.Execute();
            return asyncResult;
        }

        internal DkmEvaluationResult[] GetChildren(DkmEvaluationResult evalResult, DkmInspectionContext inspectionContext = null)
        {
            DkmEvaluationResultEnumContext enumContext;
            var builder = ArrayBuilder<DkmEvaluationResult>.GetInstance();

            // Request 0-3 children.
            int size;
            DkmEvaluationResult[] items;
            for (size = 0; size < 3; size++)
            {
                items = GetChildren(evalResult, size, inspectionContext, out enumContext);
                var totalChildCount = enumContext.Count;
                Assert.InRange(totalChildCount, 0, int.MaxValue);
                var expectedSize = (size < totalChildCount) ? size : totalChildCount;
                Assert.Equal(expectedSize, items.Length);
            }

            // Request items (increasing the size of the request with each iteration).
            size = 1;
            items = GetChildren(evalResult, size, inspectionContext, out enumContext);
            while (items.Length > 0)
            {
                builder.AddRange(items);
                Assert.True(builder.Count <= enumContext.Count);

                int offset = builder.Count;
                // Request 0 items.
                items = GetItems(enumContext, offset, 0);
                Assert.Equal(items.Length, 0);
                // Request >0 items.
                size++;
                items = GetItems(enumContext, offset, size);
            }

            Assert.Equal(builder.Count, enumContext.Count);
            return builder.ToArrayAndFree();
        }

        internal DkmEvaluationResult[] GetChildren(DkmEvaluationResult evalResult, int initialRequestSize, DkmInspectionContext inspectionContext, out DkmEvaluationResultEnumContext enumContext)
        {
            DkmGetChildrenAsyncResult getChildrenResult = default(DkmGetChildrenAsyncResult);
            var workList = new DkmWorkList();
            evalResult.GetChildren(workList, initialRequestSize, inspectionContext ?? DefaultInspectionContext, r => { getChildrenResult = r; });
            workList.Execute();
            var exception = getChildrenResult.Exception;
            if (exception != null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
            enumContext = getChildrenResult.EnumContext;
            return getChildrenResult.InitialChildren;
        }

        internal DkmEvaluationResult[] GetItems(DkmEvaluationResultEnumContext enumContext, int startIndex, int count)
        {
            DkmEvaluationEnumAsyncResult getItemsResult = default(DkmEvaluationEnumAsyncResult);
            var workList = new DkmWorkList();
            enumContext.GetItems(workList, startIndex, count, r => { getItemsResult = r; });
            workList.Execute();
            var exception = getItemsResult.Exception;
            if (exception != null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
            return getItemsResult.Items;
        }

        private const DkmEvaluationResultCategory UnspecifiedCategory = (DkmEvaluationResultCategory)(-1);
        private const DkmEvaluationResultAccessType UnspecifiedAccessType = (DkmEvaluationResultAccessType)(-1);
        public const string UnspecifiedValue = "<<unspecified value>>";

        internal static DkmEvaluationResult EvalResult(
            string name,
            string value,
            string type,
            string fullName,
            DkmEvaluationResultFlags flags = DkmEvaluationResultFlags.None,
            DkmEvaluationResultCategory category = UnspecifiedCategory,
            DkmEvaluationResultAccessType access = UnspecifiedAccessType,
            string editableValue = null,
            DkmCustomUIVisualizerInfo[] customUIVisualizerInfo = null)
        {
            return DkmSuccessEvaluationResult.Create(
                null,
                null,
                name,
                fullName,
                flags,
                value,
                editableValue,
                type,
                category,
                access,
                default(DkmEvaluationResultStorageType),
                default(DkmEvaluationResultTypeModifierFlags),
                null,
                (customUIVisualizerInfo != null) ? new ReadOnlyCollection<DkmCustomUIVisualizerInfo>(customUIVisualizerInfo) : null,
                null,
                null);
        }

        internal static DkmIntermediateEvaluationResult EvalIntermediateResult(
            string name,
            string fullName,
            string expression,
            DkmLanguage language)
        {
            return DkmIntermediateEvaluationResult.Create(
                InspectionContext: null,
                StackFrame: null,
                Name: name,
                FullName: fullName,
                Expression: expression,
                IntermediateLanguage: language,
                TargetRuntime: null,
                DataItem: null);
        }

        internal static DkmEvaluationResult EvalFailedResult(
            string name,
            string message,
            string type = null,
            string fullName = null,
            DkmEvaluationResultFlags flags = DkmEvaluationResultFlags.None)
        {
            return DkmFailedEvaluationResult.Create(
                null,
                null,
                name,
                fullName,
                message,
                flags,
                type,
                null);
        }

        internal static void Verify(IReadOnlyList<DkmEvaluationResult> actual, params DkmEvaluationResult[] expected)
        {
            try
            {
                int n = actual.Count;
                Assert.Equal(expected.Length, n);
                for (int i = 0; i < n; i++)
                {
                    Verify(actual[i], expected[i]);
                }
            }
            catch
            {
                foreach (var result in actual)
                {
                    Console.WriteLine("{0}, ", ToString(result));
                }
                throw;
            }
        }

        private static string ToString(DkmEvaluationResult result)
        {
            if (result is DkmSuccessEvaluationResult success)
                return ToString(success);

            if (result is DkmIntermediateEvaluationResult intermediate)
                return ToString(intermediate);

            return ToString((DkmFailedEvaluationResult)result);
        }

        private static string ToString(DkmSuccessEvaluationResult result)
        {
            var pooledBuilder = PooledStringBuilder.GetInstance();
            var builder = pooledBuilder.Builder;
            builder.Append("EvalResult(");
            builder.Append(Quote(result.Name));
            builder.Append(", ");
            builder.Append((result.Value == null) ? "null" : Quote(Escape(result.Value)));
            builder.Append(", ");
            builder.Append(Quote(result.Type));
            builder.Append(", ");
            builder.Append((result.FullName != null) ? Quote(Escape(result.FullName)) : "null");
            if (result.Flags != DkmEvaluationResultFlags.None)
            {
                builder.Append(", ");
                builder.Append(FormatEnumValue(result.Flags));
            }
            if (result.Category != DkmEvaluationResultCategory.Other)
            {
                builder.Append(", ");
                builder.Append(FormatEnumValue(result.Category));
            }
            if (result.Access != DkmEvaluationResultAccessType.None)
            {
                builder.Append(", ");
                builder.Append(FormatEnumValue(result.Access));
            }
            if (result.EditableValue != null)
            {
                builder.Append(", ");
                builder.Append(Quote(result.EditableValue));
            }
            builder.Append(')');
            return pooledBuilder.ToStringAndFree();
        }

        private static string ToString(DkmIntermediateEvaluationResult result)
        {
            var pooledBuilder = PooledStringBuilder.GetInstance();
            var builder = pooledBuilder.Builder;
            builder.Append("IntermediateEvalResult(");
            builder.Append(Quote(result.Name));
            builder.Append(", ");
            builder.Append(Quote(result.Expression));
            if (result.Type != null)
            {
                builder.Append(", ");
                builder.Append(Quote(result.Type));
            }
            if (result.FullName != null)
            {
                builder.Append(", ");
                builder.Append(Quote(Escape(result.FullName)));
            }
            if (result.Flags != DkmEvaluationResultFlags.None)
            {
                builder.Append(", ");
                builder.Append(FormatEnumValue(result.Flags));
            }
            builder.Append(')');
            return pooledBuilder.ToStringAndFree();
        }

        private static string ToString(DkmFailedEvaluationResult result)
        {
            var pooledBuilder = PooledStringBuilder.GetInstance();
            var builder = pooledBuilder.Builder;
            builder.Append("EvalFailedResult(");
            builder.Append(Quote(result.Name));
            builder.Append(", ");
            builder.Append(Quote(result.ErrorMessage));
            if (result.Type != null)
            {
                builder.Append(", ");
                builder.Append(Quote(result.Type));
            }
            if (result.FullName != null)
            {
                builder.Append(", ");
                builder.Append(Quote(Escape(result.FullName)));
            }
            if (result.Flags != DkmEvaluationResultFlags.None)
            {
                builder.Append(", ");
                builder.Append(FormatEnumValue(result.Flags));
            }
            builder.Append(')');
            return pooledBuilder.ToStringAndFree();
        }

        private static string Escape(string str)
        {
            return str.Replace("\"", "\\\"");
        }

        private static string Quote(string str)
        {
            return '"' + str + '"';
        }

        private static string FormatEnumValue(Enum e)
        {
            var parts = e.ToString().Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            var enumTypeName = e.GetType().Name;
            return string.Join(" | ", parts.Select(p => enumTypeName + "." + p));
        }

        internal static void Verify(DkmEvaluationResult actual, DkmEvaluationResult expected)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.FullName, actual.FullName);
            var expectedSuccess = expected as DkmSuccessEvaluationResult;
            var expectedIntermediate = expected as DkmIntermediateEvaluationResult;
            if (expectedSuccess != null)
            {
                var actualSuccess = (DkmSuccessEvaluationResult)actual;

                Assert.NotEqual(UnspecifiedValue, actualSuccess.Value);
                if (expectedSuccess.Value != UnspecifiedValue)
                {
                    Assert.Equal(expectedSuccess.Value, actualSuccess.Value);
                }

                Assert.Equal(expectedSuccess.Type, actualSuccess.Type);
                Assert.Equal(expectedSuccess.Flags, actualSuccess.Flags);
                if (expectedSuccess.Category != UnspecifiedCategory)
                {
                    Assert.Equal(expectedSuccess.Category, actualSuccess.Category);
                }
                if (expectedSuccess.Access != UnspecifiedAccessType)
                {
                    Assert.Equal(expectedSuccess.Access, actualSuccess.Access);
                }

                Assert.Equal(expectedSuccess.EditableValue, actualSuccess.EditableValue);
                Assert.True(
                    (expectedSuccess.CustomUIVisualizers == actualSuccess.CustomUIVisualizers) ||
                    (expectedSuccess.CustomUIVisualizers != null && actualSuccess.CustomUIVisualizers != null &&
                    expectedSuccess.CustomUIVisualizers.SequenceEqual(actualSuccess.CustomUIVisualizers, CustomUIVisualizerInfoComparer.Instance)));
            }
            else if (expectedIntermediate != null)
            {
                var actualIntermediate = (DkmIntermediateEvaluationResult)actual;
                Assert.Equal(expectedIntermediate.Expression, actualIntermediate.Expression);
                Assert.Equal(expectedIntermediate.IntermediateLanguage.Id.LanguageId, actualIntermediate.IntermediateLanguage.Id.LanguageId);
                Assert.Equal(expectedIntermediate.IntermediateLanguage.Id.VendorId, actualIntermediate.IntermediateLanguage.Id.VendorId);
            }
            else
            {
                var actualFailed = (DkmFailedEvaluationResult)actual;
                var expectedFailed = (DkmFailedEvaluationResult)expected;
                Assert.Equal(expectedFailed.ErrorMessage, actualFailed.ErrorMessage);
                Assert.Equal(expectedFailed.Type, actualFailed.Type);
                Assert.Equal(expectedFailed.Flags, actualFailed.Flags);
            }
        }

        #endregion

        private sealed class CustomUIVisualizerInfoComparer : IEqualityComparer<DkmCustomUIVisualizerInfo>
        {
            internal static readonly CustomUIVisualizerInfoComparer Instance = new CustomUIVisualizerInfoComparer();

            bool IEqualityComparer<DkmCustomUIVisualizerInfo>.Equals(DkmCustomUIVisualizerInfo x, DkmCustomUIVisualizerInfo y)
            {
                return x == y ||
                    (x != null && y != null &&
                    x.Id == y.Id &&
                    x.MenuName == y.MenuName &&
                    x.Description == y.Description &&
                    x.Metric == y.Metric &&
                    x.UISideVisualizerTypeName == y.UISideVisualizerTypeName &&
                    x.UISideVisualizerAssemblyName == y.UISideVisualizerAssemblyName &&
                    x.UISideVisualizerAssemblyLocation == y.UISideVisualizerAssemblyLocation &&
                    x.DebuggeeSideVisualizerTypeName == y.DebuggeeSideVisualizerTypeName &&
                    x.DebuggeeSideVisualizerAssemblyName == y.DebuggeeSideVisualizerAssemblyName &&
                    x.ExtensionPartId == y.ExtensionPartId);
            }

            int IEqualityComparer<DkmCustomUIVisualizerInfo>.GetHashCode(DkmCustomUIVisualizerInfo obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}
