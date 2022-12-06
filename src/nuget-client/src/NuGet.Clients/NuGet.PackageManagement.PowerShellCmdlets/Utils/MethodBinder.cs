// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace NuGetConsole.Host
{
    /// <summary>
    /// A simple method binder to call interface methods.
    /// </summary>
    public abstract class MethodBinder
    {
        /// <summary>
        /// Try to invoke a method.
        /// </summary>
        /// <param name="type">The runtime Type to look up the method.</param>
        /// <param name="name">The method name to invoke.</param>
        /// <param name="target">The target object to invoke the method.</param>
        /// <param name="args">Arguments for the method call.</param>
        /// <param name="result">Result of the method call.</param>
        /// <returns>true if the method call is performed.</returns>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        public bool TryInvoke(Type type, string name, object target, object[] args, out object result)
        {
            MemberInfo[] members = type.GetMember(
                name, MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod);

            // Don't support overload yet
            if (members.Length == 1
                && members[0] is MethodInfo)
            {
                result = Invoke((MethodInfo)members[0], target, args);
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Invoke a method.
        /// </summary>
        /// <param name="method">The method info.</param>
        /// <param name="target">The target object to invoke the method.</param>
        /// <param name="args">Arguments for the method call.</param>
        /// <returns>Result of the method call.</returns>
        public object Invoke(MethodInfo method, object target, object[] args)
        {
            object[] unwrappedArgs = UnwrapArgs(method, args);
            object result = method.Invoke(target, unwrappedArgs);
            return WrapResult(method, args, result, unwrappedArgs);
        }

        /// <summary>
        /// Helper method to Convert an arg to a parameter type. This method in turn calls
        /// System.Convert.ChangeType().
        /// </summary>
        /// <param name="parameterInfo">The expected parameter info.</param>
        /// <param name="arg">The arg value.</param>
        /// <returns>The arg value converted to expected parameter type.</returns>
        protected static object ChangeType(ParameterInfo parameterInfo, object arg)
        {
            Type parameterType = parameterInfo.ParameterType;
            if (parameterType.IsByRef)
            {
                parameterType = parameterType.GetElementType();
            }
            return Convert.ChangeType(arg, parameterType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Try to match an arg with a parameter.
        /// </summary>
        /// <param name="parameterInfo">The current expected parameter info.</param>
        /// <param name="arg">A passed in arg.</param>
        /// <param name="argValue">The actual arg value if arg matches paramInfo.</param>
        /// <returns>true if arg matches paramInfo.</returns>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        protected abstract bool TryConvertArg(ParameterInfo parameterInfo, object arg, out object argValue);

        /// <summary>
        /// Try to rematch an arg with a parameter when returning from a method call.
        /// This is used to return values in [ref] args. This method is expected to do
        /// the same processing as TryConvertArg and return the same value. If a [ref]
        /// parameter is present, return the argValue in the arg.
        /// </summary>
        /// <param name="parameterInfo">The current expected parameter info.</param>
        /// <param name="arg">A passed in arg.</param>
        /// <param name="argValue">The arg value after the method call.</param>
        /// <returns>true if arg matches paramInfo.</returns>
        protected abstract bool TryReturnArg(ParameterInfo parameterInfo, object arg, object argValue);

        /// <summary>
        /// Try to get an optional arg value if a pamameter is optional. This binder allows any [out] parameter
        /// to be optional.
        /// </summary>
        /// <param name="paramInfo">The parameter info.</param>
        /// <param name="argValue">The output arg value</param>
        /// <returns>true if the parameter is considered optional.</returns>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        protected virtual bool TryGetOptionalArg(ParameterInfo paramInfo, out object argValue)
        {
            if (paramInfo.IsOut
                || paramInfo.IsOptional)
            {
                argValue = paramInfo.RawDefaultValue;
                if (argValue == DBNull.Value)
                {
                    // When default parameter is not really specified, use null.
                    // This works with types like "int32&".
                    argValue = null;
                }

                return true;
            }

            argValue = null;
            return false;
        }

        /// <summary>
        /// Create a tuple to return method call results. The results include the method return value
        /// and omitted [out] parameter values.
        /// </summary>
        /// <param name="allResults">The results list.</param>
        /// <returns>A tuple that contains method call results.</returns>
        protected virtual object CreateResultTuple(IList<object> allResults)
        {
            return allResults;
        }

        /// <summary>
        /// Determines if an arg represents a Type. If true, ConvertToType will be called to get the Type.
        /// Used for generic methods.
        /// </summary>
        /// <param name="arg">An arg object.</param>
        /// <returns>true if arg represents a Type.</returns>
        public virtual bool IsType(object arg)
        {
            return arg is Type;
        }

        /// <summary>
        /// Convert an arg to a Type.
        /// </summary>
        /// <param name="arg">An arg object.</param>
        /// <returns>A Type instance represented by arg.</returns>
        public virtual Type ConvertToType(object arg)
        {
            return (Type)arg;
        }

        /// <summary>
        /// Check if unwrap args is needed. If false, UnwrapArgs/WrapResult become NOP.
        /// The default implementation returns true if there is any [out] params.
        /// </summary>
        /// <param name="parameterInfos">All the parameterinfo.</param>
        /// <returns>true if UnwrapArgs/WrapResult should be performed.</returns>
        protected virtual bool IsUnwrapArgsNeeded(ParameterInfo[] parameterInfos)
        {
            return parameterInfos.Any(p => p.IsOut);
        }

        private object[] UnwrapArgs(MethodInfo m, object[] args)
        {
            ParameterInfo[] paramInfos = m.GetParameters();

            // Skip if not needed
            if (!IsUnwrapArgsNeeded(paramInfos))
            {
                return args;
            }

            object[] newArgs = new object[paramInfos.Length];
            int k = 0;
            for (int i = 0; i < paramInfos.Length; i++)
            {
                ParameterInfo paramInfo = paramInfos[i];
                object argValue;
                if (k < args.Length
                    && TryConvertArg(paramInfo, args[k], out argValue)) // If args[k] matches
                {
                    newArgs[i] = argValue;
                    k++;
                    continue;
                }

                if (TryGetOptionalArg(paramInfo, out argValue))
                {
                    newArgs[i] = argValue;
                }
                else
                {
                    throw new MissingMemberException();
                }
            }
            return newArgs;
        }

        private object WrapResult(MethodInfo m, object[] args, object result, object[] unwrappedArgs)
        {
            if (args == unwrappedArgs)
            {
                return result;
            }

            List<object> allResults = new List<object>();
            if (result != null
                && result.GetType() != typeof(void))
            {
                allResults.Add(result);
            }

            ParameterInfo[] paramInfos = m.GetParameters();
            int k = 0;
            for (int i = 0; i < paramInfos.Length; i++)
            {
                ParameterInfo paramInfo = paramInfos[i];
                if (k < args.Length
                    && TryReturnArg(paramInfo, args[k], unwrappedArgs[i])) // If args[k] matches
                {
                    k++;
                    continue;
                }

                if (paramInfo.IsOut) // arg not supplied
                {
                    allResults.Add(unwrappedArgs[i]);
                }
            }

            return allResults.Count > 1 ? CreateResultTuple(allResults) : result;
        }
    }
}
