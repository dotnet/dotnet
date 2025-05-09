// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Css.Parser.Parser
{
    internal enum ParseErrorType
    {
        // Missing item errors

        AtDirectiveNameMissing, // @
        AtDirectiveSemicolonMissing, // @import
        AttributeSelectorCloseBracketMissing, // body[foo { }
        AttributeSelectorElementMissing, // body[=bar]
        AttributeSelectorOperationMissing, // body[item1 item2]
        AttributeSelectorValueMissing, // body[foo=]
        ClassNameMissing, // body.
        CloseBraceMismatch, // (]
        CloseCommentMissing, // /*
        CloseCurlyBraceMissing, // {
        CloseFunctionBraceMissing, // rbg(0,0,0
        CloseQuoteMissing, // "string
        ColonMissingInDeclaration, // { color red; }
        EncodingMissing, // @charset ;
        FunctionArgumentCommaMissing, // :not(foo bar)
        FunctionArgumentMissing, // rgb(,100)
        IdMissing, // body#
        ImportantMissing, // { color: red !hi; }
        KeyFrameBlockNameMissing, // @keyframes { }
        MediaTypeMissing, // @media screen, { }
        OpenCurlyBraceMissing, // @media screen }
        OpenCurlyBraceMissingForRule, // .foo }
        PropertyNameMissing, // { : red; }
        PropertyValueMissing, // { color: ; }
        PseudoClassNameMissing, // body:
        PseudoElementNameMissing, // body::
        PseudoPageIdentifierMissing, // @page :
        SelectorAfterCombineOperatorMissing, // body > { }
        SelectorAfterCommaMissing, // body, { }
        SelectorBeforeCombineOperatorMissing, // > body { }
        SelectorBeforeCommaMissing, // , body { }
        SelectorBeforeRuleBlockMissing, // { }
        UrlImportMissing, // @import ;
        UrlNamespaceMissing, // @namespace ;

        // Expected item errors

        DeclarationExpected, // @font-face { { } }
        KeyFramesSelectorExpected, // @keyframes { #foo { } }
        MediaExpressionExpected, // @media screen and foo {}
        PropertyValueExpected, // { color: (); }
        PseudoFunctionSelectorExpected, // :not(&foo)
        SimpleSelectorExpected, // *body { }
        CustomPropertyNameExpected, // var(--foo)

        // Unexpected item errors

        UnexpectedAtDirective, // .foo { @media { } }
        UnexpectedBangInProperty, // { color: red !important !important }
        UnexpectedBangInSelector, // !!ol
        UnexpectedMediaQueryToken, // @media screen foo { }
        UnexpectedParseError, // this error is only used in "unreachable" parts of the parser code
        UnexpectedToken, // { #foo: red; }

        CustomError, // extensions can add errors with custom text
    }

    internal enum ParseErrorLocation
    {
        WholeItem,
        BeforeItem,
        AfterItem,
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    internal struct ParseError
    {
        internal ParseError(ParseErrorType errorType, ParseErrorLocation location)
            : this(errorType, location, null)
        {
        }

        internal ParseError(ParseErrorType errorType, ParseErrorLocation location, string customText)
            : this()
        {
            ErrorType = errorType;
            Location = location;
            Text = customText;
        }

        internal ParseErrorType ErrorType { get; set; }
        internal ParseErrorLocation Location { get; set; }
        internal string Text { get; set; }
    }
}
