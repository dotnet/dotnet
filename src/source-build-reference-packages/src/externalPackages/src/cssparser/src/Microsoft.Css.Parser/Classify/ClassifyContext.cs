// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Microsoft.Css.Parser.Classify
{
    // Context for colorization. Items may declare their own context or have context
    // provided by parent element. For example, token items in a property value may
    // all be colorized as parent (property value item).

    internal enum CssClassifierContextType
    {
        Default,
        Comment,
        String,
        Number,
        Units,
        Important,
        ItemName,
        ItemNamespace,
        CurlyBrace,
        FunctionName,
        FunctionBrace,
        FunctionArgument,
        FunctionColor,
        AtDirectiveName,
        AtDirectiveKeyword,
        CharsetName,
        ImportUrl,
        UrlFunction,
        UrlString,
        HexColor,
        ElementTagName,
        ElementAttribute,
        ElementAttributeValue,
        LengthUnits,
        TimeUnits,
        ViewUnits,
        GridUnits,
        PercentUnits,
        FrequencyUnits,
        ResolutionUnits,
        AngleUnits,
        PropertyDeclaration,
        PropertyName,
        PropertyValue,
        PseudoClass,
        PseudoElement,
        PseudoPageType,
        Punctuation, // comma, semicolon, colon...
        CalcExpression,
        MediaQuery,
        MediaQueryOperation,
        MediaType,
        MediaFeatureName,
        MediaFeatureValue,
        MediaCombineOperator,
        Selector,
        SelectorCombineOperator, // >, +
        SelectorOperator, // ~=, $=
        SquareBracket,
        ClassSelector,
        IdSelector,
        UnicodeRange,
        CustomPropertyName,
        NamespaceUrl,

        // Keep this as the final entry in the array
        //  This depends on none of the entries above specifying a value
        CssClassifierContextTypeCount
    }

    /// <summary>
    /// Classification context helps with colorization performance
    /// since it can be filled during parsing rather than via second
    /// tree walk. Since CSS parser is oriented towards editing this
    /// actually makes sense. Context is an interface rather than enum
    /// since enums are not extensible while we want to allow
    /// derived parsers (such as LESS parser) to provide and attach
    /// new type of classification contexts.
    /// </summary>
    public interface IClassifierContext
    {
        bool IsDefault();
        Type ContextType { get; }
        int ContextValue { get; }
        string ClassificationName { get; }
        bool IsEqualTo(int contextTypeValue, Type contextObjectType);
    }

    internal sealed class CssClassifierContext : IClassifierContext
    {
        private readonly CssClassifierContextType _contextType;

        public CssClassifierContext(CssClassifierContextType contextType)
        {
            _contextType = contextType;
            ClassificationName = InternalGetClassificationName(contextType);
        }

        public bool IsDefault()
        {
            return _contextType == CssClassifierContextType.Default;
        }

        public Type ContextType
        {
            get { return typeof(CssClassifierContextType); }
        }

        public int ContextValue
        {
            get { return (int)_contextType; }
        }

        public bool IsEqualTo(int contextTypeValue, Type contextObjectType)
        {
            return (_contextType == (CssClassifierContextType)contextTypeValue) && (ContextType == contextObjectType);
        }

        public string ClassificationName { get; }

        private static string InternalGetClassificationName(CssClassifierContextType contextType)
        {
            switch (contextType)
            {
                default: return "Default";
                case CssClassifierContextType.Comment: return ClassificationTypes.Comment;
                case CssClassifierContextType.String: return ClassificationTypes.String;
                case CssClassifierContextType.Number: return ClassificationTypes.Number;
                case CssClassifierContextType.Units: return ClassificationTypes.Units;
                case CssClassifierContextType.Important: return ClassificationTypes.Important;
                case CssClassifierContextType.ItemName: return ClassificationTypes.ItemName;
                case CssClassifierContextType.ItemNamespace: return ClassificationTypes.ItemNamespace;
                case CssClassifierContextType.CurlyBrace: return ClassificationTypes.CurlyBrace;
                case CssClassifierContextType.FunctionName: return ClassificationTypes.FunctionName;
                case CssClassifierContextType.FunctionBrace: return ClassificationTypes.FunctionBrace;
                case CssClassifierContextType.FunctionArgument: return ClassificationTypes.FunctionArgument;
                case CssClassifierContextType.AtDirectiveName: return ClassificationTypes.AtDirectiveName;
                case CssClassifierContextType.AtDirectiveKeyword: return ClassificationTypes.AtDirectiveKeyword;
                case CssClassifierContextType.CharsetName: return ClassificationTypes.CharsetName;
                case CssClassifierContextType.ImportUrl: return ClassificationTypes.ImportUrl;
                case CssClassifierContextType.UrlFunction: return ClassificationTypes.UrlFunction;
                case CssClassifierContextType.UrlString: return ClassificationTypes.UrlString;
                case CssClassifierContextType.HexColor: return ClassificationTypes.HexColor;
                case CssClassifierContextType.FunctionColor: return ClassificationTypes.FunctionColor;
                case CssClassifierContextType.ElementTagName: return ClassificationTypes.ElementTagName;
                case CssClassifierContextType.ElementAttribute: return ClassificationTypes.ElementAttribute;
                case CssClassifierContextType.ElementAttributeValue: return ClassificationTypes.ElementAttributeValue;
                case CssClassifierContextType.LengthUnits: return ClassificationTypes.LengthUnits;
                case CssClassifierContextType.TimeUnits: return ClassificationTypes.TimeUnits;
                case CssClassifierContextType.ViewUnits: return ClassificationTypes.ViewUnits;
                case CssClassifierContextType.GridUnits: return ClassificationTypes.GridUnits;
                case CssClassifierContextType.PercentUnits: return ClassificationTypes.PercentUnits;
                case CssClassifierContextType.FrequencyUnits: return ClassificationTypes.FrequencyUnits;
                case CssClassifierContextType.ResolutionUnits: return ClassificationTypes.ResolutionUnits;
                case CssClassifierContextType.AngleUnits: return ClassificationTypes.AngleUnits;
                case CssClassifierContextType.PropertyDeclaration: return ClassificationTypes.PropertyDeclaration;
                case CssClassifierContextType.PropertyName: return ClassificationTypes.PropertyName;
                case CssClassifierContextType.PropertyValue: return ClassificationTypes.PropertyValue;
                case CssClassifierContextType.PseudoClass: return ClassificationTypes.PseudoClass;
                case CssClassifierContextType.PseudoElement: return ClassificationTypes.PseudoElement;
                case CssClassifierContextType.PseudoPageType: return ClassificationTypes.PseudoPageType;
                case CssClassifierContextType.CalcExpression: return ClassificationTypes.CalcExpression;
                case CssClassifierContextType.MediaQuery: return ClassificationTypes.MediaQuery;
                case CssClassifierContextType.MediaQueryOperation: return ClassificationTypes.MediaQueryOperation;
                case CssClassifierContextType.MediaType: return ClassificationTypes.MediaType;
                case CssClassifierContextType.MediaFeatureName: return ClassificationTypes.MediaFeatureName;
                case CssClassifierContextType.MediaFeatureValue: return ClassificationTypes.MediaFeatureValue;
                case CssClassifierContextType.MediaCombineOperator: return ClassificationTypes.MediaCombineOperator;
                case CssClassifierContextType.Selector: return ClassificationTypes.Selector;
                case CssClassifierContextType.SelectorCombineOperator: return ClassificationTypes.SelectorCombineOperator;
                case CssClassifierContextType.SelectorOperator: return ClassificationTypes.SelectorOperator;
                case CssClassifierContextType.SquareBracket: return ClassificationTypes.SquareBracket;
                case CssClassifierContextType.ClassSelector: return ClassificationTypes.ClassSelector;
                case CssClassifierContextType.IdSelector: return ClassificationTypes.IdSelector;
                case CssClassifierContextType.UnicodeRange: return ClassificationTypes.UnicodeRange;
                case CssClassifierContextType.CustomPropertyName: return ClassificationTypes.CustomPropertyName;
            }
        }
    }

    internal static class CssClassifierContextCache
    {
        private static readonly IClassifierContext[] s_cachedContexts = new IClassifierContext[(int)CssClassifierContextType.CssClassifierContextTypeCount];

        internal static IClassifierContext FromTypeEnum(CssClassifierContextType contextType)
        {
            int index = (int)contextType;

            if (s_cachedContexts[index] == null)
            {
                s_cachedContexts[index] = new CssClassifierContext(contextType);
            }

            return s_cachedContexts[index];
        }
    }
}
