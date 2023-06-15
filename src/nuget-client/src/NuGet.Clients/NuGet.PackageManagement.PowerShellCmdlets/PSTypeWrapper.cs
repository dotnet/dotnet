// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Management.Automation;
using System.Reflection;

namespace NuGetConsole.Host.PowerShell.Implementation
{
    /// <summary>
    /// This class helps provide Get-Interface for PowerShell host.
    /// PowerShell has an object adapter layer. Its Com object adapter works well with IDispatch
    /// members, but disables accessing other interfaces. This class and "Add-WrapperMembers.ps1"
    /// help work around this issue by simulating QueryInterface (Get-Interface).
    /// </summary>
    /// <remarks>
    /// Note that the full class name is reference in Add-WrapperMembers.ps1 (Resources.resx) and Profile.ps1.
    /// Be sure to update all the above 3 places if full class name is changed.
    /// </remarks>
    public class PSTypeWrapper : TypeWrapper<PSObject>
    {
        private PSTypeWrapper(object wrappedObject)
            : base(wrappedObject)
        {
        }

        internal override MethodBinder Binder
        {
            get { return PSMethodBinder.Instance; }
        }

        protected override PSObject CreateInterfaceWrapper(TypeWrapper<PSObject> wrapper, Type interfaceType)
        {
            PSObject psObject = new PSObject(wrapper);
            AddWrapperMembersScript.Invoke(psObject, wrapper.WrappedObject, interfaceType);
            return psObject;
        }

        private static ScriptBlock _addWrapperMembersScript;

        private static ScriptBlock AddWrapperMembersScript
        {
            get
            {
                if (_addWrapperMembersScript == null)
                {
                    string extensionRoot = Path.GetDirectoryName(typeof(PSTypeWrapper).Assembly.Location);
                    string scriptPath = Path.Combine(extensionRoot, "Modules", "NuGet", "Add-WrapperMembers.ps1");
                    string scriptContents = File.ReadAllText(scriptPath);
                    _addWrapperMembersScript = ScriptBlock.Create(scriptContents);
                }
                return _addWrapperMembersScript;
            }
        }

        /// <summary>
        /// GetInterface simulates COM QueryInterface.
        /// </summary>
        /// <param name="scriptValue">A PowerShell BaseObject.</param>
        /// <param name="interfaceType">An interface type expected.</param>
        /// <returns>A PSObject which exposes properties/methods to call the interface members.</returns>
        public static PSObject GetInterface(object scriptValue, Type interfaceType)
        {
            return GetInterface(scriptValue, interfaceType,
                obj => obj as PSTypeWrapper ?? new PSTypeWrapper(obj));
        }

        /// <summary>
        /// Inovke method helper. PowerShell has .NET object adapter and COM object adapters. These adapters
        /// have different behavior. When calling some COM interface members, PowerShell does not unwrap the
        /// parameters, resulting in PSObject being passed to COM methods. This helper method ensures the
        /// parameters are unwrapped, and also handles [ref] parameters.
        /// </summary>
        /// <param name="target">The target object to invoke a methed.</param>
        /// <param name="method">The method info.</param>
        /// <param name="parameters">
        /// Raw arguments. If it contains [ref] args, this method will set their result
        /// Values.
        /// </param>
        /// <returns>Invoke method result.</returns>
        public static object InvokeMethod(object target, MethodInfo method, PSObject[] parameters)
        {
            return PSMethodBinder.Instance.Invoke(method, target, parameters);
        }

        /// <summary>
        /// A PS MethodBinder to support PSTypeWrapper.
        /// </summary>
        private class PSMethodBinder : MethodBinder
        {
            /// <summary>
            /// The singleton instance of PSMethodBinder.
            /// </summary>
            public static readonly PSMethodBinder Instance = new PSMethodBinder();

            /// <summary>
            /// Private constructor.
            /// </summary>
            private PSMethodBinder()
            {
            }

            /// <summary>
            /// Get the BaseObject of a PSObject.
            /// </summary>
            /// <param name="arg">The PSObject.</param>
            /// <returns>The BaseObject of arg, or null if arg is null.</returns>
            private static object GetBaseObject(PSObject arg)
            {
                return arg != null ? arg.BaseObject : null;
            }

            /// <summary>
            /// Override to always unwrap args.
            /// </summary>
            protected override bool IsUnwrapArgsNeeded(ParameterInfo[] paramInfos)
            {
                // Unwrap PSObject parameters to BaseObjects are always needed, plus we need ChangeType
                return true;
            }

            protected override bool TryConvertArg(ParameterInfo paramInfo, object arg, out object argValue)
            {
                object argBaseObject = GetBaseObject(arg as PSObject);

                // normal byval parameter
                if (!paramInfo.IsOut)
                {
                    argValue = ChangeType(paramInfo, argBaseObject);
                    return true; // matched
                }

                var psReference = argBaseObject as PSReference;
                if (psReference != null)
                {
                    argValue = paramInfo.IsIn ? ChangeType(paramInfo, psReference.Value) : null;
                    return true; // matched
                }

                argValue = null;
                return false; // not matched arg
            }

            protected override bool TryReturnArg(ParameterInfo paramInfo, object arg, object argValue)
            {
                // normal byval parameter
                if (!paramInfo.IsOut)
                {
                    return true; // matched
                }

                object argBaseObject = GetBaseObject(arg as PSObject);
                var psReference = argBaseObject as PSReference;
                if (psReference != null)
                {
                    psReference.Value = argValue;
                    return true; // matched
                }

                return false; // not matched arg
            }
        }
    }
}
