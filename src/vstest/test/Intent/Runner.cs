// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Reflection;

namespace Intent;

public class Runner
{
    public static void Run(IEnumerable<string> paths, IRunLogger logger)
    {
        var assemblies = new List<Assembly>();
        foreach (var path in paths)
        {
            try
            {
                var assembly = Assembly.LoadFrom(path);
                if (assembly.IsExcluded())
                    continue;

                assemblies.Add(assembly);
            }
            catch (Exception ex)
            {
                logger.WriteFrameworkError(ex);
            }
        }

        var types = assemblies.SelectMany(assembly => assembly.GetTypes().SkipNonPublic().SkipExcluded()).ToList();
        var typesWithOnly = types.Where(type => type.GetCustomAttribute<OnlyAttribute>() != null).ToList();

        var methods = types.SelectMany(type => type.GetMethods().SkipExcluded()).ToList();
        var methodsWithOnly = methods.Where(m => m.GetCustomAttribute<OnlyAttribute>() != null).ToList();

        List<MethodInfo> methodsToRun;
        if (typesWithOnly.Count > 0 || methodsWithOnly.Count > 0)
        {
            // Some types or methods are decorated with Only. Putting Only on a type should run all methods in
            // that type. Putting Only on a method should run that method.
            //
            // So we need a list of all distinct methods that match that rule.

            var onlyMethodsFromTypes = typesWithOnly.SelectMany(type => type.GetMethods().SkipExcluded()).ToList();
            methodsToRun = onlyMethodsFromTypes.Concat(methodsWithOnly).Distinct().ToList();
        }
        else
        {
            methodsToRun = methods;
        }

        var failures = new List<(MethodInfo method, Exception exception, TimeSpan duration)>();
        var passed = 0;
        var runStopwatch = Stopwatch.StartNew();
        foreach (var method in methodsToRun)
        {
            try
            {
                var testStopwatch = Stopwatch.StartNew();
                try
                {
                    // Declaring type cannot be really null for types you define in C#
                    // without doing any reflection magic.
                    var instance = Activator.CreateInstance(method.DeclaringType!);
                    var testResult = method.Invoke(instance, []);
                    if (testResult is Task task)
                    {
                        // When the result is a task we need to await it.
                        // TODO: this can be improved with await, imho
                        task.GetAwaiter().GetResult();
                    }

                    passed++;
                    logger.WriteTestPassed(method, testStopwatch.Elapsed);
                }
                catch (Exception ex)
                {
                    if (ex is TargetInvocationException tex && tex.InnerException != null)
                    {
                        failures.Add((method, tex.InnerException, testStopwatch.Elapsed));
                        logger.WriteTestFailure(method, tex.InnerException, testStopwatch.Elapsed);
                    }
                    else
                    {
                        failures.Add((method, ex, testStopwatch.Elapsed));
                        logger.WriteTestFailure(method, ex, testStopwatch.Elapsed);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteFrameworkError(ex);
            }
        }

        logger.WriteSummary(passed, failures, runStopwatch.Elapsed);
    }
}
