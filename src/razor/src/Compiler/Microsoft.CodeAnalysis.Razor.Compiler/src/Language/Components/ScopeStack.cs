// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;

namespace Microsoft.AspNetCore.Razor.Language.Components;

/// <summary>
/// Keeps track of the nesting of elements/containers while writing out the C# source code
/// for a component. This allows us to detect mismatched start/end tags, as well as inject
/// additional C# source to capture component descendants in a lambda.
/// </summary>
internal class ScopeStack
{
    private readonly Stack<ScopeEntry> _stack = new Stack<ScopeEntry>();

    public string BuilderVarName =>
        Current.BuilderVarNumber == 1
            ? ComponentsApi.RenderTreeBuilder.BuilderParameter
            : $"{ComponentsApi.RenderTreeBuilder.BuilderParameter}{Current.BuilderVarNumber}";

    public string RenderModeVarName =>
       Current.BuilderVarNumber == 1 && Current.RenderModeCount == 0
            ? ComponentsApi.RenderTreeBuilder.RenderModeVariableName
            : $"{ComponentsApi.RenderTreeBuilder.RenderModeVariableName}{Current.BuilderVarNumber}_{Current.RenderModeCount}";
   
    public string FormNameVarName =>
       Current.BuilderVarNumber == 1 && Current.FormNameCount == 0
            ? ComponentsApi.RenderTreeBuilder.FormNameVariableName
            : $"{ComponentsApi.RenderTreeBuilder.FormNameVariableName}{Current.BuilderVarNumber}_{Current.FormNameCount}";

    public int Depth => _stack.Count - 1;

    private ScopeEntry Current => _stack.Peek();

    public ScopeStack()
    {
        _stack.Push(new ScopeEntry() { BuilderVarNumber = 1 });
    }

    public void OpenComponentScope(CodeRenderingContext context, string name, string parameterName)
    {
        // Writes code that looks like:
        //
        // ((__builder) => { ... })
        // OR
        // ((context) => (__builder) => { ... })

        if (parameterName != null)
        {
            context.CodeWriter.Write($"({parameterName}) => ");
        }
        OpenScope(context);
    }

    public void OpenTemplateScope(CodeRenderingContext context) => OpenScope(context);

    private void OpenScope(CodeRenderingContext context)
    {
        var scope = new ScopeEntry() { BuilderVarNumber = Current.BuilderVarNumber + 1 };
        _stack.Push(scope);
        scope.LambdaScope = context.CodeWriter.BuildLambda(BuilderVarName);
    }

    public void CloseScope(CodeRenderingContext context)
    {
        var currentScope = _stack.Pop();
        currentScope.LambdaScope.Dispose();
    }

    public void IncrementRenderMode()
    {
        Current.RenderModeCount++;
    }

    public void IncrementFormName()
    { 
        Current.FormNameCount++;
    }

    private class ScopeEntry
    {
        public int RenderModeCount;
        public int FormNameCount;
        public int BuilderVarNumber;
        public IDisposable LambdaScope;
    }
}
