// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Css.Parser.Classify
{
    internal static class ClassificationTypes
    {
        internal const string Comment = "CssComment";
        internal const string String = "CssString";
        internal const string Number = "CssNumber";
        internal const string Units = "CssUnits";
        internal const string Important = "CssImportant";
        internal const string ItemName = "CssItemName";
        internal const string ItemNamespace = "CssItemNamespace";
        internal const string CurlyBrace = "CssCurlyBrace";
        internal const string FunctionName = "CssFunctionName";
        internal const string FunctionBrace = "CssFunctionBrace";
        internal const string FunctionArgument = "CssFunctionArgument";
        internal const string AtDirectiveName = "CssAtDirectiveName";
        internal const string AtDirectiveKeyword = "CssAtDirectiveKeyword";
        internal const string CharsetName = "CssCharsetName";
        internal const string ImportUrl = "CssImportUrl";
        internal const string UrlFunction = "CssUrlFunction";
        internal const string UrlString = "CssUrlString";
        internal const string HexColor = "CssHexColor";
        internal const string FunctionColor = "CssFunctionColor";
        internal const string ElementTagName = "CssElementTagName";
        internal const string ElementAttribute = "CssElementAttribute";
        internal const string ElementAttributeValue = "CssElementAttributeValue";
        internal const string LengthUnits = "CssLengthUnits";
        internal const string TimeUnits = "CssTimeUnits";
        internal const string ViewUnits = "CssViewUnits";
        internal const string GridUnits = "CssGridUnits";
        internal const string PercentUnits = "CssPercentUnits";
        internal const string FrequencyUnits = "CssFrequencyUnits";
        internal const string ResolutionUnits = "CssResolutionUnits";
        internal const string AngleUnits = "CssAngleUnits";
        internal const string PropertyDeclaration = "CssPropertyDeclaration";
        internal const string PropertyName = "CssPropertyName";
        internal const string PropertyValue = "CssPropertyValue";
        internal const string PseudoClass = "CssPseudoClass";
        internal const string PseudoElement = "CssPseudoElement";
        internal const string PseudoPageType = "CssPseudoPageType";
        internal const string CalcExpression = "CssCalcExpression";
        internal const string MediaQuery = "CssMediaQuery";
        internal const string MediaQueryOperation = "CssMediaQueryOperation";
        internal const string MediaType = "CssMediaType";
        internal const string MediaFeatureName = "CssMediaFeatureName";
        internal const string MediaFeatureValue = "CssMediaFeatureValue";
        internal const string MediaCombineOperator = "CssMediaCombineOperator";
        internal const string Selector = "CssSelector";
        internal const string SelectorCombineOperator = "CssSelectorCombineOperator";
        internal const string SelectorOperator = "CssSelectorOperator";
        internal const string SquareBracket = "CssSquareBracket";
        internal const string ClassSelector = "CssClassSelector";
        internal const string IdSelector = "CssIdSelector";
        internal const string UnicodeRange = "CssUnicodeRange";
        internal const string CustomPropertyName = "CssCustomPropertyName";
    }
}
