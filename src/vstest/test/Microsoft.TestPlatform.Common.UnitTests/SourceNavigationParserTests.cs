// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Common.UnitTests;

[TestClass]
public class SourceNavigationParserTests
{
    [TestMethod]
    public void FindMethodLocations_ReturnsSignatureAndBodyStartLines()
    {
        var lines = new[]
        {
            "public class Foo",
            "{",
            "    [TestMethod]",
            "    public void MyMethod()",
            "    {",
            "        DoStuff();",
            "    }",
            "}",
        };

        var result = SourceNavigationParser.FindMethodLocations(lines, "MyMethod");

        Assert.ContainsSingle(result);
        Assert.AreEqual(4, result[0].SignatureLine); // 1-based: "    public void MyMethod()"
        Assert.AreEqual(5, result[0].BodyStartLine); // 1-based: "    {"
    }

    [TestMethod]
    public void FindMethodLocations_OverloadedMethods()
    {
        var lines = new[]
        {
            "public class Foo",
            "{",
            "    public void OverLoaded()",
            "    {",
            "    }",
            "",
            "    public void OverLoaded(string _)",
            "    {",
            "    }",
            "}",
        };

        var result = SourceNavigationParser.FindMethodLocations(lines, "OverLoaded");

        Assert.HasCount(2, result);
        Assert.AreEqual(3, result[0].SignatureLine);
        Assert.AreEqual(4, result[0].BodyStartLine);
        Assert.AreEqual(7, result[1].SignatureLine);
        Assert.AreEqual(8, result[1].BodyStartLine);
    }

    [TestMethod]
    public void FindMethodBodyStartLines_SimpleMethod()
    {
        var lines = new[]
        {
            "public class Foo",
            "{",
            "    public void MyMethod()",
            "    {",
            "        DoStuff();",
            "    }",
            "}",
        };

        var result = SourceNavigationParser.FindMethodBodyStartLines(lines, "MyMethod");

        Assert.ContainsSingle(result);
        Assert.AreEqual(4, result[0]); // 1-based: line "    {"
    }

    [TestMethod]
    public void FindMethodBodyStartLines_MethodWithAttribute()
    {
        var lines = new[]
        {
            "public class Foo",
            "{",
            "    [Test]",
            "    public void PassTestMethod1()",
            "    {",
            "        Assert.AreEqual(5, 5);",
            "    }",
            "}",
        };

        var result = SourceNavigationParser.FindMethodBodyStartLines(lines, "PassTestMethod1");

        Assert.ContainsSingle(result);
        Assert.AreEqual(5, result[0]); // 1-based: line "    {"
    }

    [TestMethod]
    public void FindMethodBodyStartLines_BraceOnSameLineAsSignature()
    {
        var lines = new[]
        {
            "public class Foo",
            "{",
            "    public void Inline() {",
            "    }",
            "}",
        };

        var result = SourceNavigationParser.FindMethodBodyStartLines(lines, "Inline");

        Assert.ContainsSingle(result);
        Assert.AreEqual(3, result[0]); // 1-based: brace is on same line as signature
    }

    [TestMethod]
    public void FindMethodBodyStartLines_OverloadedMethods()
    {
        var lines = new[]
        {
            "public class Foo",
            "{",
            "    public void OverLoaded()",
            "    {",
            "    }",
            "",
            "    public void OverLoaded(string _)",
            "    {",
            "    }",
            "}",
        };

        var result = SourceNavigationParser.FindMethodBodyStartLines(lines, "OverLoaded");

        Assert.HasCount(2, result);
        Assert.AreEqual(4, result[0]); // first overload brace
        Assert.AreEqual(8, result[1]); // second overload brace
    }

    [TestMethod]
    public void FindMethodBodyStartLines_MethodNotFound()
    {
        var lines = new[]
        {
            "public class Foo",
            "{",
            "    public void Other()",
            "    {",
            "    }",
            "}",
        };

        var result = SourceNavigationParser.FindMethodBodyStartLines(lines, "NotExist");

        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void FindMethodBodyStartLines_DoesNotMatchPropertyOrField()
    {
        var lines = new[]
        {
            "public class Foo",
            "{",
            "    public string MyMethod = \"hello\";",
            "    public string MyMethodProp { get; set; }",
            "}",
        };

        // "MyMethod" followed by ' =' should not match (no '(' after name).
        var result = SourceNavigationParser.FindMethodBodyStartLines(lines, "MyMethod");

        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void FindMethodBodyStartLines_AsyncMethod()
    {
        var lines = new[]
        {
            "public class Foo",
            "{",
            "    public async Task AsyncTestMethod()",
            "    {",
            "        await Task.Delay(0);",
            "    }",
            "}",
        };

        var result = SourceNavigationParser.FindMethodBodyStartLines(lines, "AsyncTestMethod");

        Assert.ContainsSingle(result);
        Assert.AreEqual(4, result[0]);
    }

    [TestMethod]
    public void FindMethodBodyStartLines_RealSimpleClassLibrary()
    {
        // Mimics test/TestAssets/SimpleClassLibrary/Class1.cs
        var lines = new[]
        {
            "// Copyright header",
            "",
            "using System.Threading.Tasks;",
            "",
            "namespace SimpleClassLibrary",
            "{",
            "    public class Class1",
            "    {",
            "        public void PassingTest()",
            "        {",                                // line 10
            "            if (new System.Random().Next() == 20) { throw new System.NotImplementedException(); }",
            "        }",
            "",
            "        public async Task AsyncTestMethod()",
            "        {",                                // line 15
            "            await Task.Delay(0);",
            "        }",
            "",
            "        public void OverLoadedMethod()",
            "        {",                                // line 20
            "        }",
            "",
            "        public void OverLoadedMethod(string _)",
            "        {",                                // line 24
            "        }",
            "    }",
            "}",
        };

        Assert.AreEqual(10, SourceNavigationParser.FindMethodBodyStartLines(lines, "PassingTest")[0]);
        Assert.AreEqual(15, SourceNavigationParser.FindMethodBodyStartLines(lines, "AsyncTestMethod")[0]);

        var overloads = SourceNavigationParser.FindMethodBodyStartLines(lines, "OverLoadedMethod");
        Assert.HasCount(2, overloads);
        Assert.AreEqual(20, overloads[0]);
        Assert.AreEqual(24, overloads[1]);
    }

    [TestMethod]
    public void ContainsMethodSignature_MatchesMethodFollowedByParen()
    {
        Assert.IsTrue(SourceNavigationParser.ContainsMethodSignature("    public void MyMethod()", "MyMethod"));
    }

    [TestMethod]
    public void ContainsMethodSignature_MatchesWithWhitespaceBeforeParen()
    {
        Assert.IsTrue(SourceNavigationParser.ContainsMethodSignature("    public void MyMethod ()", "MyMethod"));
    }

    [TestMethod]
    public void ContainsMethodSignature_DoesNotMatchFieldAssignment()
    {
        Assert.IsFalse(SourceNavigationParser.ContainsMethodSignature("    public string MyMethod = \"hello\";", "MyMethod"));
    }

    [TestMethod]
    public void ContainsMethodSignature_DoesNotMatchSubstring()
    {
        // "MyMethodExtended(" should not match "MyMethod" because the next char after "MyMethod" is 'E', not '(' or whitespace.
        Assert.IsFalse(SourceNavigationParser.ContainsMethodSignature("    public void MyMethodExtended()", "MyMethod"));
    }

    [TestMethod]
    public void ContainsMethodSignature_MatchesWhenNameAppearsMultipleTimes()
    {
        // First occurrence is a field, second is the method.
        Assert.IsTrue(SourceNavigationParser.ContainsMethodSignature("    // calls MyMethod then MyMethod()", "MyMethod"));
    }
}
