// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

extern alias LegacyClasp;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using Microsoft.WebTools.Languages.Shared.Editor.Composition;
using Microsoft.WebTools.Languages.Shared.Editor.Text;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

/// <summary>
///  Provides reflection-based access to the Web Tools LSP infrastructure needed for tests.
/// </summary>
internal static class WebTools
{
    private const string ServerAssemblyName = "Microsoft.WebTools.Languages.LanguageServer.Server, Version=17.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
    private const string EditorAssemblyName = "Microsoft.WebTools.Languages.Shared.Editor, Version=17.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
    private const string ApplyFormatEditsHandlerTypeName = "Microsoft.WebTools.Languages.LanguageServer.Server.Html.OperationHandlers.ApplyFormatEditsHandler";
    private const string BufferManagerTypeName = "Microsoft.WebTools.Languages.LanguageServer.Server.Shared.Buffer.BufferManager";
    private const string RequestContextTypeName = "Microsoft.WebTools.Languages.LanguageServer.Server.Shared.Clasp.RequestContext";
    private const string ApplyFormatEditsParamTypeName = "Microsoft.WebTools.Languages.Shared.Editor.LanguageServer.ContainedLanguage.ApplyFormatEditsParam";
    private const string ApplyFormatEditsResponseTypeName = "Microsoft.WebTools.Languages.Shared.Editor.LanguageServer.ContainedLanguage.ApplyFormatEditsResponse";
    private const string TextChangeTypeName = "Microsoft.WebTools.Languages.Shared.Editor.EditorHelpers.TextChange";

    private static Assembly? s_serverAssembly;
    private static Assembly? s_editorAssembly;

    private static Type GetType(Assembly assembly, string name)
        => assembly.GetType(name, throwOnError: true).AssumeNotNull();

    private static object CreateInstance(Type type, params object?[]? args)
        => Activator.CreateInstance(type, args).AssumeNotNull();

    private static MethodInfo GetMethod(Type type, string name)
        => type.GetMethod(name).AssumeNotNull();

    private static MethodInfo GetMethod(Type type, string name, Type[] parameterTypes)
        => type.GetMethod(name, parameterTypes).AssumeNotNull();

    private static PropertyInfo GetProperty(Type type, string name)
        => type.GetProperty(name).AssumeNotNull();

    private static Assembly ServerAssembly
        => s_serverAssembly ?? InterlockedOperations.Initialize(ref s_serverAssembly,
            Assembly.Load(ServerAssemblyName));

    private static Assembly EditorAssembly
        => s_editorAssembly ?? InterlockedOperations.Initialize(ref s_editorAssembly,
            Assembly.Load(EditorAssemblyName));

    public abstract class ReflectedObject(object instance)
    {
        public object Instance => instance;
    }

    public sealed class BufferManager(object instance) : ReflectedObject(instance)
    {
        private static Type? s_type;
        private static MethodInfo? s_createBufferMethod;

        public static Type Type
            => s_type ?? InterlockedOperations.Initialize(ref s_type,
                WebTools.GetType(ServerAssembly, BufferManagerTypeName));

        private static MethodInfo CreateBufferMethod
            => s_createBufferMethod ?? InterlockedOperations.Initialize(ref s_createBufferMethod,
                GetMethod(Type, nameof(CreateBuffer)));

        public ITextSnapshot CreateBuffer(
            Uri documentUri,
            string contentTypeName,
            string initialContent,
            int snapshotVersionFromLSP)
        {
            return (ITextSnapshot)CreateBufferMethod
                .Invoke(Instance, [documentUri, contentTypeName, initialContent, snapshotVersionFromLSP])
                .AssumeNotNull();
        }

        public static BufferManager New(
            IContentTypeRegistryService contentTypeService,
            ITextBufferFactoryService textBufferFactoryService,
            IEnumerable<Lazy<IWebTextBufferListener, IOrderedComponentContentTypes>> textBufferListeners)
        {
            var instance = CreateInstance(Type, contentTypeService, textBufferFactoryService, textBufferListeners);
            return new(instance);
        }
    }

    public sealed class RequestContext(object instance) : ReflectedObject(instance)
    {
        private static Type? s_type;

        public static Type Type
            => s_type ?? InterlockedOperations.Initialize(ref s_type,
                WebTools.GetType(ServerAssembly, RequestContextTypeName));

        public static RequestContext New(ITextSnapshot textSnapshot)
        {
            var instance = CreateInstance(Type, textSnapshot);
            return new(instance);
        }
    }

    public sealed class ApplyFormatEditsParam(object instance) : ReflectedObject(instance)
    {
        private static Type? s_type;

        public static Type Type
            => s_type ?? InterlockedOperations.Initialize(ref s_type,
                WebTools.GetType(EditorAssembly, ApplyFormatEditsParamTypeName));

        public static ApplyFormatEditsParam DeserializeFrom(string jsonText)
        {
            var instance = JsonConvert.DeserializeObject(jsonText, Type).AssumeNotNull();
            return new(instance);
        }
    }

    public sealed class ApplyFormatEditsResponse(object instance) : ReflectedObject(instance)
    {
        private static Type? s_type;
        private static PropertyInfo? s_textChangesProperty;

        public static Type Type
            => s_type ?? InterlockedOperations.Initialize(ref s_type,
                WebTools.GetType(EditorAssembly, ApplyFormatEditsResponseTypeName));

        private static PropertyInfo TextChangesProperty
            => s_textChangesProperty ?? InterlockedOperations.Initialize(ref s_textChangesProperty,
                GetProperty(Type, nameof(TextChanges)));

        private ImmutableArray<TextChange> _textChanges;

        public ImmutableArray<TextChange> TextChanges
        {
            get
            {
                if (_textChanges.IsDefault)
                {
                    var textChanges = (object[])TextChangesProperty.GetValue(Instance).AssumeNotNull();

                    using var builder = new PooledArrayBuilder<TextChange>(textChanges.Length);

                    foreach (var textChange in textChanges)
                    {
                        builder.Add(new TextChange(textChange));
                    }

                    ImmutableInterlocked.InterlockedInitialize(ref _textChanges, builder.DrainToImmutable());
                }

                return _textChanges;
            }
        }
    }

    public sealed class TextChange(object instance) : ReflectedObject(instance)
    {
        private static Type? s_type;
        private static PropertyInfo? s_positionProperty;
        private static PropertyInfo? s_lengthProperty;
        private static PropertyInfo? s_newTextProperty;

        public static Type Type
            => s_type ?? InterlockedOperations.Initialize(ref s_type,
                WebTools.GetType(EditorAssembly, TextChangeTypeName));

        private static PropertyInfo PositionProperty
            => s_positionProperty ?? InterlockedOperations.Initialize(ref s_positionProperty,
                GetProperty(Type, nameof(Position)));

        private static PropertyInfo LengthProperty
            => s_lengthProperty ?? InterlockedOperations.Initialize(ref s_lengthProperty,
                GetProperty(Type, nameof(Length)));

        private static PropertyInfo NewTextProperty
            => s_newTextProperty ?? InterlockedOperations.Initialize(ref s_newTextProperty,
                GetProperty(Type, nameof(NewText)));

        private int? _position;
        private int? _length;
        private string? _newText;

        public int Position => _position ??= (int)PositionProperty.GetValue(Instance).AssumeNotNull();
        public int Length => _length ??= (int)LengthProperty.GetValue(Instance).AssumeNotNull();
        public string NewText => _newText ??= (string)NewTextProperty.GetValue(Instance).AssumeNotNull();

        public override int GetHashCode()
            => Instance.GetHashCode();
        public override bool Equals(object? obj)
            => Instance.Equals(obj);
        public override string? ToString()
            => Instance.ToString();
    }

    public sealed class ApplyFormatEditsHandler(object instance) : ReflectedObject(instance)
    {
        private static Type? s_type;
        private static MethodInfo? s_handleRequestAsyncMethod;

        public static Type Type
            => s_type ?? InterlockedOperations.Initialize(ref s_type,
                WebTools.GetType(ServerAssembly, ApplyFormatEditsHandlerTypeName));

        private static MethodInfo HandleRequestAsyncMethod
            => s_handleRequestAsyncMethod ?? InterlockedOperations.Initialize(ref s_handleRequestAsyncMethod,
                GetMethod(
                    Type,
                    nameof(HandleRequestAsync),
                    [ApplyFormatEditsParam.Type, RequestContext.Type, typeof(CancellationToken)]));

        public async Task<ApplyFormatEditsResponse> HandleRequestAsync(
            ApplyFormatEditsParam request,
            RequestContext context,
            CancellationToken cancellationToken)
        {
            var task = (Task)HandleRequestAsyncMethod
                .Invoke(Instance, [request.Instance, context.Instance, cancellationToken])
                .AssumeNotNull();

            await task;

            var result = GetProperty(task.GetType(), "Result").GetValue(task).AssumeNotNull();

            return new ApplyFormatEditsResponse(result);
        }

        public static ApplyFormatEditsHandler New(
            ITextBufferFactoryService3 textBufferFactoryService,
            BufferManager bufferManager,
            ILspLogger logger)
        {
            var instance = CreateInstance(Type, textBufferFactoryService, bufferManager.Instance, new LegacyClaspILspLogger(logger));
            return new(instance);
        }

        /// <summary>
        /// Wraps the razor logger (from the clasp source package) into the binary clasp logger that webtools uses.
        /// </summary>
        /// <param name="logger"></param>
        private class LegacyClaspILspLogger(ILspLogger logger) : LegacyClasp.Microsoft.CommonLanguageServerProtocol.Framework.ILspLogger
        {
            public void LogEndContext(string message, params object[] @params) => logger.LogEndContext(message, @params);

            public void LogError(string message, params object[] @params) => logger.LogError(message, @params);

            public void LogException(Exception exception, string? message = null, params object[] @params) => logger.LogException(exception, message, @params);

            public void LogInformation(string message, params object[] @params) => logger.LogInformation(message, @params);

            public void LogStartContext(string message, params object[] @params) => logger.LogStartContext(message, @params);

            public void LogWarning(string message, params object[] @params) => logger.LogWarning(message, @params);
        }
    }
}
