// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root flicense information.

using System;
using System.Globalization;
using Xunit;

namespace NuGet.Packaging.Licenses.Test
{
    public class NuGetLicenseExpressionParserTests
    {
        [Theory]
        [InlineData("MIT OR LPL-1.0", "OR", true)]
        [InlineData("MIT AND LPL-1.0", "AND", true)]
        [InlineData("MIT OR (AFL-1.1 WITH 389-exception)", "OR", true)]
        [InlineData("(AFL-1.1 WITH 389-exception) OR MIT", "OR", true)]
        [InlineData("MIT OR AFL-1.1 WITH 389-exception AND Apache-2.0", "OR", true)]
        [InlineData("MIT OR AFL-1.1 WITH 389-exception", "OR", true)]
        [InlineData("MIT OR (LPL-1.0 OR AFL-1.1 WITH 389-exception)", "OR", true)]
        [InlineData("MIT OR (AFL-1.1 WITH 389-exception OR LPL-1.0)", "OR", true)]
        [InlineData("MIT WITH 389-exception AND AFL-1.1 OR LPL-1.0", "OR", true)]
        [InlineData("MIT OR AFL-1.1 AND LPL-1.0 WITH 389-exception", "OR", true)]
        [InlineData("MIT OR ((AFL-1.1 WITH 389-exception) AND Apache-2.0)", "OR", true)]
        [InlineData("((MIT) AND (LPL-1.0))", "AND", true)]
        [InlineData("MIT OR (LPL-1.0 OR (AFL-1.1 WITH 389-exception))", "OR", true)]
        [InlineData("MIT OR (LPL-1.0 AND (AFL-1.1 WITH 389-exception) AND AMDPLPA)", "OR", true)]
        [InlineData("MIT OR ((AFL-1.1 WITH 389-exception) AND LPL-1.0)", "OR", true)]
        [InlineData("MIT AND (AFL-1.1 WITH 389-exception OR Apache-2.0)", "AND", true)] // here. This expr is a weird legal expression, but a good test for the exp parser.
        [InlineData("(AFL-1.1+ WITH 389-exception) OR MIT", "OR", true)]
        [InlineData("MIT OR LPL-1.0+", "OR", true)]
        [InlineData("(AMDPLPA AND BSD-2-Clause)", "AND", true)]
        [InlineData("((( (AMDPLPA) AND BSD-2-Clause)))", "AND", true)]
        [InlineData("(And+) AND or", "AND", false)]
        [InlineData("(AMDPLPA AND BSD-2-Clause) AND (MIT WITH 389-exception)", "AND", true)]
        [InlineData("(AMDPLPA AND BSD-2-Clause) AND (MIT OR Apache-2.0)", "AND", true)]
        [InlineData("   MIT OR LPL-1.0       ", "OR", true)]
        [InlineData("\r\n MIT OR LPL-1.0       ", "OR", true)]
        [InlineData("\n\n\n\n\n\n\nMIT OR LPL-1.0       ", "OR", true)]
        public void LicenseExpressionParser_ParsesComplexExpression(string expression, string rootOperator, bool hasStandardIdentifiers)
        {
            var licenseExpression = NuGetLicenseExpression.Parse(expression);
            Assert.Equal(expression.Replace("(", "").Replace(")", "").Trim(), licenseExpression.ToString());

            Assert.Equal(licenseExpression.HasOnlyStandardIdentifiers(), hasStandardIdentifiers);

            if (Enum.TryParse<LogicalOperatorType>(rootOperator, true, out var logicalOperator))
            {
                var exp = licenseExpression as LogicalOperator;
                Assert.Equal(exp.LogicalOperatorType, logicalOperator);
            }
            else if (rootOperator.Equals("WITH", StringComparison.OrdinalIgnoreCase))
            {
                var exp = licenseExpression as WithOperator;
                Assert.NotNull(expression);
            }
            else
            {
                Assert.Equal(rootOperator, licenseExpression.ToString());
            }
        }

        [Theory]
        [InlineData("MIT", true, false, false)]
        [InlineData("AFL-1.1+", true, true, false)]
        [InlineData("MyFancyLicense", false, false, false)]
        [InlineData("UNLICENSED", true, false, true)]
        public void LicenseExpressionParser_ParsesSimpleExpression(string expression, bool hasStandardIdentifiers, bool hasPlus, bool isUnlicensed)
        {
            var licenseExpression = NuGetLicenseExpression.Parse(expression);
            Assert.Equal(expression, licenseExpression.ToString());
            Assert.Equal(licenseExpression.HasOnlyStandardIdentifiers(), hasStandardIdentifiers);
            Assert.NotNull(licenseExpression as NuGetLicense);
            Assert.Equal(hasPlus, (licenseExpression as NuGetLicense).Plus);
            Assert.Equal(isUnlicensed, licenseExpression.IsUnlicensed());
        }

        [Theory]
        [InlineData("MIT WITH LPL-1.0", "LPL-1.0", false)] // Both identifiers are licenses
        [InlineData("mif-exception WITH Classpath-exception-2.0", "mif-exception", true)] // Both identifiers are exceptions
        public void LicenseExpressionParser_ThrowsForMismatchedArguments(string expression, string badIdentifier, bool IsExceptionAsLicense)
        {
            var ex = Assert.Throws<NuGetLicenseExpressionParsingException>(() => NuGetLicenseExpression.Parse(expression));
            if (IsExceptionAsLicense)
            {
                Assert.Equal(ex.Message, string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_LicenseIdentifierIsException, badIdentifier));
            }
            else
            {
                Assert.Equal(ex.Message, string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_ExceptionIdentifierIsLicense, badIdentifier));
            }
        }

        [Theory]
        [InlineData("(GPL-1.0 WITH 389-exception) OR MIT", "GPL-1.0")]
        [InlineData("MIT OR GPL-1.0 WITH 389-exception AND Apache-2.0", "GPL-1.0")]
        [InlineData("MIT OR GPL-1.0 WITH 389-exception", "GPL-1.0")]
        [InlineData("MIT OR (LPL-1.0 OR GPL-1.0 WITH 389-exception)", "GPL-1.0")]
        [InlineData("MIT OR (GPL-1.0 WITH 389-exception OR LPL-1.0)", "GPL-1.0")]
        [InlineData("(LGPL-2.1 AND BSD-2-Clause)", "LGPL-2.1")]
        [InlineData("((( (LGPL-2.1) AND BSD-2-Clause)))", "LGPL-2.1")]
        public void LicenseExpressionParser_ComplexExpressionWithDeprecatedIdentifiersThrows(string expression, string deprecatedValue)
        {
            var ex = Assert.Throws<NuGetLicenseExpressionParsingException>(() => NuGetLicenseExpression.Parse(expression));
            Assert.Equal(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_DeprecatedIdentifier, deprecatedValue), ex.Message);
        }

        [Theory]
        [InlineData("MIT AND ")]
        [InlineData("(MIT AND BSD-2-Clause")]
        [InlineData("MIT (AND) LPL-1.0")]
        [InlineData("((MIT) (AND) (LPL-1.0))")]
        [InlineData("MIT (AND LPL-1.0)")]
        [InlineData("MIT () OR LPL-1.0")]
        [InlineData("MIT OR GPL-1.0 (WITH 389-exception OR LPL-1.0)")]
        [InlineData("()")]
        [InlineData("MIT WITH OR LPL-1.0")]
        [InlineData("MIT++")]
        [InlineData("UNLICENSED++")]
        [InlineData("MIT+ OR LPL-1.0+++")]
        [InlineData("(MIT OR LPL-1.0)+")]
        public void LicenseExpressionParser_StrictParseThrowsForInvalidExpressions(string expression)
        {
            Assert.Throws<NuGetLicenseExpressionParsingException>(() => NuGetLicenseExpression.Parse(expression));
        }

        [Theory]
        [InlineData("MIT and BSD-2-Clause")]
        [InlineData("MIT or BSD-2-Clause")]
        [InlineData("MIT with Classpath-exception-2.0")]
        public void LicenseExpressionParser_ThrowsForExpressionsWithBadCasing(string expression)
        {
            Assert.Throws<NuGetLicenseExpressionParsingException>(() => NuGetLicenseExpression.Parse(expression));
        }

        [Theory]
        [InlineData("MIT WITH classpath-exception-2.0", "classpath-exception-2.0")]
        public void LicenseExpressionParser_ThrowsForInvalidExceptionDueToBadCasing(string expression, string exception)
        {
            var ex = Assert.Throws<NuGetLicenseExpressionParsingException>(() => NuGetLicenseExpression.Parse(expression));
            Assert.Equal(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_InvalidExceptionIdentifier, exception), ex.Message);
        }

        [Theory]
        [InlineData("MIT AND UNLICENSED", "UNLICENSED")]
        public void LicenseExpressionParser_ThrowsForInvalidLicensedCombinations(string expression, string exception)
        {
            var ex = Assert.Throws<NuGetLicenseExpressionParsingException>(() => NuGetLicenseExpression.Parse(expression));
            Assert.Equal(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_UnexpectedIdentifier, exception), ex.Message);
        }

        [Theory]
        [InlineData("UNLICENSED+")]
        [InlineData("MIT OR UNLICENSED+")]
        public void LicenseExpressionParser_ThrowsForBadUnlicensedExpression(string expression)
        {
            var ex = Assert.Throws<NuGetLicenseExpressionParsingException>(() => NuGetLicenseExpression.Parse(expression));
            Assert.Equal(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_IllegalUnlicensedOperator), ex.Message);
        }

        [Theory]
        [InlineData("MIt WITH Classpath-exception-2.0", false, true)]
        [InlineData("MIt AND BSD-2-Clause", false, true)]
        [InlineData("MIt OR BSD-2-Clause", false, true)]
        [InlineData("MIT OR BSD-2-ClausE", true, false)]
        [InlineData("MIt AND BSD-2-ClausE", false, false)]
        [InlineData("MIt+ OR BSD-2-Clause", false, true)]
        public void LicenseExpressionParser_CreatesNonStandardExpressionsWithBadCasing(string expression, bool isFirstOperatorStandard, bool isSecondOperatorStandard)
        {
            var licenseExpression = NuGetLicenseExpression.Parse(expression);
            var withExpression = licenseExpression as WithOperator;
            var logicalExpression = licenseExpression as LogicalOperator;

            if (withExpression != null)
            {
                Assert.Equal(isFirstOperatorStandard, withExpression.License.IsStandardLicense);
            }
            if (logicalExpression != null)
            {
                Assert.Equal(isFirstOperatorStandard, (logicalExpression.Left as NuGetLicense).IsStandardLicense);
                Assert.Equal(isSecondOperatorStandard, (logicalExpression.Right as NuGetLicense).IsStandardLicense);
            }
            Assert.Equal(expression, licenseExpression.ToString());
        }

        [Theory]
        [InlineData("A AND ")]
        [InlineData("(A AND B")]
        [InlineData("A AND B)")]
        [InlineData("A (AND) B")]
        [InlineData("((A) (AND) (B))")]
        [InlineData("A (AND B)")]
        [InlineData("A () OR B")]
        [InlineData("A OR C (WITH 389-exception OR B)")]
        [InlineData("A WITH B C WITH D")]
        [InlineData(")A( AND B")]
        [InlineData("A( AND )B")]
        public void LicenseExpressionParser_ThrowsForInvalidExpressions(string expression)
        {
            Assert.Throws<NuGetLicenseExpressionParsingException>(() => NuGetLicenseExpression.Parse(expression));
        }

        [Theory]
        [InlineData("(GPL-1.0+ WITH Classpath-exception-2.0) OR MIT!")]
        [InlineData("(GPL-1.0+ WITH Classpath-exception-2.0) OR MIT@")]
        [InlineData("(GPL-1.0+ WITH Classpath-exception-2.0) OR MIT/")]
        [InlineData("(GPL-1.0+ WITH Classpath-exception-2.0) OR MIT[]")]
        [InlineData("      (GPL-1.0+ WITH Classpath-exception-2.0) OR MIT[] ")]
        public void LicenseExpressionParser_ParseThrowsForInvalidCharactersInExpression(string expression)
        {
            var ex = Assert.Throws<NuGetLicenseExpressionParsingException>(() => NuGetLicenseExpression.Parse(expression));
            Assert.Equal(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_InvalidCharacters, expression), ex.Message);
        }

        [Theory]
        [InlineData("LGPL-2.1 AND ")]
        [InlineData("(LGPL-2.1 AND BSD-2-CLAUSE")]
        [InlineData("MIT AND LPL-1.0)")]
        [InlineData("MIT (AND) LPL-1.0")]
        [InlineData("((MIT) (AND) (LPL-1.0))")]
        [InlineData("MIT (AND LPL-1.0)")]
        [InlineData("MIT  () OR LPL-1.0")]
        [InlineData("MIT OR GPL-1.0 (WITH Classpath-exception-2.0 OR LPL-1.0)")]
        [InlineData("A WITH B C WITH D")]
        public void LicenseExpressionParser_EvaluateThrowsWhenBracketsDoNotMatch(string expression)
        {
            Assert.Throws<NuGetLicenseExpressionParsingException>(() => NuGetLicenseExpression.Parse(expression));
        }

        [Theory]
        [InlineData("MIT OR (GPL-1.0 Classpath-exception-2.0 WITH)")]
        [InlineData("MIT LPL-1.0 GPL-1.0 Classpath-exception-2.0 WITH OR OR")]
        [InlineData("OR MIT (GPL-1.0 Classpath-exception-2.0 WITH)")]
        [InlineData("MIT WITH GPL-1.0 WITH Classpath-exception-2.0 WITH LPL-1.0")]
        [InlineData("A WITH B C WITH D")]
        [InlineData("A WITH B C WITH D OR E")]
        public void LicenseExpressionParser_NotStrict_EvaluateThrowsIfExpressionIsInvalid(string expression)
        {
            Assert.Throws<NuGetLicenseExpressionParsingException>(() => NuGetLicenseExpression.Parse(expression));
        }

        [Fact]
        public void LicenseExpressionParser_ComplexExpressionEvaluatesTheCorrectTree()
        {
            var expression = "MIT AND (AFL-1.1 WITH 389-exception OR Apache-2.0)";
            var licenseExpression = NuGetLicenseExpression.Parse(expression);

            // Validate the expression parsing
            Assert.Equal(expression.Replace("(", "").Replace(")", "").Trim(), licenseExpression.ToString());
            Assert.Equal(LicenseExpressionType.Operator, licenseExpression.Type);

            // Validate the logical operator
            var rootLogicalOperator = licenseExpression as LogicalOperator;
            Assert.NotNull(rootLogicalOperator);
            Assert.Equal(LogicalOperatorType.And, rootLogicalOperator.LogicalOperatorType);

            // left should be MIT.
            var MIT = rootLogicalOperator.Left as NuGetLicense;
            Assert.NotNull(MIT);
            Assert.Equal(LicenseExpressionType.License, MIT.Type);
            Assert.Equal(MIT.ToString(), "MIT");

            // right should be AFL-1.1 WITH 389-exception or Apache-2.0

            var logicalOrExpression = rootLogicalOperator.Right as LogicalOperator;
            Assert.NotNull(logicalOrExpression);
            Assert.Equal(LogicalOperatorType.Or, logicalOrExpression.LogicalOperatorType);

            // left is AFL-1.1 with 389-exception

            var withExpression = logicalOrExpression.Left as WithOperator;
            Assert.NotNull(withExpression);
            Assert.Equal(LicenseOperatorType.WithOperator, withExpression.OperatorType);

            // LIcense is AFL-1.1, Exception is 389-exception

            Assert.Equal(withExpression.License.Identifier, "AFL-1.1");
            Assert.Equal(withExpression.Exception.Identifier, "389-exception");

            // right of the logical or is Apache-2.0
            var apache = logicalOrExpression.Right as NuGetLicense;

            Assert.NotNull(apache);
            Assert.Equal(LicenseExpressionType.License, apache.Type);
            Assert.Equal(apache.ToString(), "Apache-2.0");
        }

        [Fact]
        public void LicenseExpressionParser_SimpleExpressionEvaluatesTheCorrectTree()
        {
            var expression = "MIT+";
            var licenseExpression = NuGetLicenseExpression.Parse(expression);

            // Validate the expression parsing
            Assert.Equal(expression.Replace("(", "").Replace(")", "").Trim(), licenseExpression.ToString());
            Assert.Equal(LicenseExpressionType.License, licenseExpression.Type);

            var mit = licenseExpression as NuGetLicense;

            Assert.NotNull(mit);
            Assert.Equal(LicenseExpressionType.License, mit.Type);
            Assert.Equal(mit.ToString(), "MIT+");
            Assert.True(mit.Plus);
            Assert.Equal("MIT", mit.Identifier);
        }
    }
}
