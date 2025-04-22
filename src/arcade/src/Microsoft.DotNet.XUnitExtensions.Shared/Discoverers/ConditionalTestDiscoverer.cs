// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if !USES_XUNIT_3
using Xunit.Abstractions;
#endif
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions
{
    // Internal helper class for code common to conditional test discovery through
    // [ConditionalFact] and [ConditionalTheory]
    internal static class ConditionalTestDiscoverer
    {
        // This helper method evaluates the given condition member names for a given set of test cases.
        // If any condition member evaluates to 'false', the test cases are marked to be skipped.
        // The skip reason is the collection of all the condition members that evaluated to 'false'.
        internal static string EvaluateSkipConditions(
#if USES_XUNIT_3
            IXunitTestMethod testMethod,
#else
            ITestMethod testMethod,
#endif
            object[] conditionArguments)
        {
            Type calleeType = null;
            string[] conditionMemberNames = null;

            if (CheckInputToSkipExecution(conditionArguments, ref calleeType, ref conditionMemberNames, testMethod)) return null;

#if USES_XUNIT_3
            MethodInfo testMethodInfo = testMethod.Method;
#else
            MethodInfo testMethodInfo = testMethod.Method.ToRuntimeMethod();
#endif
            Type testMethodDeclaringType = testMethodInfo.DeclaringType;
            List<string> falseConditions = new List<string>(conditionMemberNames.Count());

            foreach (string entry in conditionMemberNames)
            {
                string conditionMemberName = entry;

                // Null condition member names are silently tolerated
                if (string.IsNullOrWhiteSpace(conditionMemberName))
                {
                    continue;
                }

                Type declaringType;

                if (calleeType != null)
                {
                    declaringType = calleeType;
                }
                else
                {
                    declaringType = testMethodDeclaringType;

                    string[] symbols = conditionMemberName.Split('.');
                    if (symbols.Length == 2)
                    {
                        conditionMemberName = symbols[1];
#if USES_XUNIT_3
                        declaringType = testMethod.TestClass.Class.Assembly.ExportedTypes.Where(t => t.Name.Contains(symbols[0])).FirstOrDefault();
#else
                        ITypeInfo type = testMethod.TestClass.Class.Assembly.GetTypes(false).Where(t => t.Name.Contains(symbols[0])).FirstOrDefault();
                        if (type != null)
                        {
                            declaringType = type.ToRuntimeType();
                        }
#endif
                    }
                }

                Func<bool> conditionFunc;
                if ((conditionFunc = LookupConditionalMember(declaringType, conditionMemberName)) == null)
                {
                    throw new ConditionalDiscovererException(GetFailedLookupString(conditionMemberName, declaringType));
                }

                // In the case of multiple conditions, collect the results of all
                // of them to produce a summary skip reason.
                try
                {
                    if (!conditionFunc())
                    {
                        falseConditions.Add(conditionMemberName);
                    }
                }
                catch (Exception exc)
                {
                    falseConditions.Add($"{conditionMemberName} ({exc.GetType().Name})");
                }
            }

            // Compose a summary of all conditions that returned false.
            if (falseConditions.Count > 0)
            {
                return string.Format("Condition(s) not met: \"{0}\"", string.Join("\", \"", falseConditions));
            }

            // No conditions returned false (including the absence of any conditions).
            return null;
        }

#if USES_XUNIT_3
        internal static bool TryEvaluateSkipConditions(ITestFrameworkDiscoveryOptions discoveryOptions, IXunitTestMethod testMethod, object[] conditionArguments, out string skipReason, out ExecutionErrorTestCase errorTestCase)
#else
        internal static bool TryEvaluateSkipConditions(ITestFrameworkDiscoveryOptions discoveryOptions, IMessageSink diagnosticMessageSink, ITestMethod testMethod, object[] conditionArguments, out string skipReason, out ExecutionErrorTestCase errorTestCase)
#endif
        {
            skipReason = null;
            errorTestCase = null;
            try
            {
                skipReason = EvaluateSkipConditions(testMethod, conditionArguments);
                return true;
            }
            catch (ConditionalDiscovererException e)
            {
#if USES_XUNIT_3
                var details = TestIntrospectionHelper.GetTestCaseDetails(discoveryOptions, testMethod, new Xunit.FactAttribute());
                errorTestCase = new ExecutionErrorTestCase(testMethod, details.TestCaseDisplayName, details.UniqueID, e.Message);
#else
                errorTestCase = new ExecutionErrorTestCase(
                    diagnosticMessageSink,
                    discoveryOptions.MethodDisplayOrDefault(),
                    discoveryOptions.MethodDisplayOptionsOrDefault(),
                    testMethod,
                    e.Message);
#endif
                return false;
            }
        }

        internal static string GetFailedLookupString(string name, Type type)
        {
            return
                $"An appropriate member '{name}' could not be found. " +
                $"The conditional method needs to be a static method, property, or field on the type {type} or any ancestor, " +
                "of any visibility, accepting zero arguments, and having a return type of Boolean.";
        }
        
        internal static Func<bool> LookupConditionalMember(Type t, string name)
        {
            if (t == null || name == null)
                return null;

            TypeInfo ti = t.GetTypeInfo();

            MethodInfo mi = ti.GetDeclaredMethod(name);
            if (mi != null && mi.IsStatic && mi.GetParameters().Length == 0 && mi.ReturnType == typeof(bool))
                return () => (bool)mi.Invoke(null, null);

            PropertyInfo pi = ti.GetDeclaredProperty(name);
            if (pi != null && pi.PropertyType == typeof(bool) && pi.GetMethod != null && pi.GetMethod.IsStatic && pi.GetMethod.GetParameters().Length == 0)
                return () => (bool)pi.GetValue(null);

            FieldInfo fi = ti.GetDeclaredField(name);
            if (fi != null && fi.FieldType == typeof(bool) && fi.IsStatic)
                return () => (bool)fi.GetValue(null);

            return LookupConditionalMember(ti.BaseType, name);
        }

        internal static bool CheckInputToSkipExecution(object[] conditionArguments, ref Type calleeType, ref string[] conditionMemberNames, ITestMethod testMethod = null)
        {
            // A null or empty list of conditionArguments is treated as "no conditions".
            // and the test cases will be executed.
            // Example: [ConditionalClass()]
            if (conditionArguments == null || conditionArguments.Length == 0) return true;

            calleeType = conditionArguments[0] as Type;
            if (calleeType != null)
            {
                if (conditionArguments.Length < 2)
                {
                    // [ConditionalFact(typeof(x))] no provided methods.
                    return true;
                }

                // [ConditionalFact(typeof(x), "MethodName")]
                conditionMemberNames = conditionArguments[1] as string[];
            }
            else
            {
                // For [ConditionalClass], unable to get the Type info. All test cases will be executed.
                if (testMethod == null) return true;

                // [ConditionalFact("MethodName")]
                conditionMemberNames = conditionArguments[0] as string[];
            }

            // [ConditionalFact((string[]) null)]
            if (conditionMemberNames == null || conditionMemberNames.Count() == 0) return true;

            return false;
        }
    }
}
