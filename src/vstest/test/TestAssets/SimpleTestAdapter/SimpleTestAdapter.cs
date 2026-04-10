// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace SimpleTestAdapter;

/// <summary>
/// A minimal test adapter with no external dependencies beyond Microsoft.TestPlatform.ObjectModel.
/// Test methods are discovered by scanning for the [SimpleTest] attribute. This avoids MSTest
/// version compatibility issues when testing with older vstest.console versions.
/// </summary>
[FileExtension(".dll")]
[DefaultExecutorUri(ExecutorUri)]
[ExtensionUri(ExecutorUri)]
public class SimpleTestAdapter : ITestExecutor, ITestDiscoverer
{
    public const string ExecutorUri = "executor://simple.testadapter";

    public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext,
        IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
    {
        foreach (var source in sources)
        {
            foreach (var testCase in GetTestCases(source))
            {
                discoverySink.SendTestCase(testCase);
            }
        }
    }

    public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
    {
        foreach (var test in tests)
        {
            var result = RunTest(test);
            frameworkHandle.RecordResult(result);
        }
    }

    public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
    {
        foreach (var source in sources)
        {
            foreach (var testCase in GetTestCases(source))
            {
                var result = RunTest(testCase);
                frameworkHandle.RecordResult(result);
            }
        }
    }

    public void Cancel()
    {
    }

    private static IEnumerable<TestCase> GetTestCases(string source)
    {
        Assembly assembly;
        try
        {
            assembly = Assembly.LoadFrom(source);
        }
        catch
        {
            yield break;
        }

        foreach (var type in assembly.GetTypes())
        {
            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                var attr = method.GetCustomAttributes().FirstOrDefault(a => a.GetType().Name == nameof(SimpleTestAttribute));
                if (attr is null)
                {
                    continue;
                }

                yield return new TestCase($"{type.FullName}.{method.Name}", new Uri(ExecutorUri), source)
                {
                    DisplayName = method.Name,
                    CodeFilePath = source,
                };
            }
        }
    }

    private static TestResult RunTest(TestCase testCase)
    {
        try
        {
            var source = testCase.Source;
            var assembly = Assembly.LoadFrom(source);
            var fqn = testCase.FullyQualifiedName;
            var lastDot = fqn.LastIndexOf('.');
            var typeName = fqn.Substring(0, lastDot);
            var methodName = fqn.Substring(lastDot + 1);

            var type = assembly.GetType(typeName);
            if (type is null)
            {
                return new TestResult(testCase) { Outcome = TestOutcome.NotFound };
            }

            var method = type.GetMethod(methodName);
            if (method is null)
            {
                return new TestResult(testCase) { Outcome = TestOutcome.NotFound };
            }

            var instance = Activator.CreateInstance(type);
            method.Invoke(instance, null);
            return new TestResult(testCase) { Outcome = TestOutcome.Passed };
        }
        catch (TargetInvocationException ex) when (ex.InnerException is SimpleTestFailException failEx)
        {
            return new TestResult(testCase)
            {
                Outcome = TestOutcome.Failed,
                ErrorMessage = failEx.Message,
                ErrorStackTrace = failEx.StackTrace,
            };
        }
        catch (Exception ex)
        {
            return new TestResult(testCase)
            {
                Outcome = TestOutcome.Failed,
                ErrorMessage = ex.Message,
                ErrorStackTrace = ex.StackTrace,
            };
        }
    }
}

/// <summary>
/// Marks a method as a test method for <see cref="SimpleTestAdapter"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class SimpleTestAttribute : Attribute
{
}

/// <summary>
/// Marks a test as expected to fail.
/// </summary>
public static class SimpleAssert
{
    public static void Fail(string message = "Test failed.")
        => throw new SimpleTestFailException(message);
}

/// <summary>
/// Exception thrown by <see cref="SimpleAssert.Fail"/> to indicate an intentional test failure.
/// </summary>
public class SimpleTestFailException : Exception
{
    public SimpleTestFailException(string message) : base(message) { }
}
