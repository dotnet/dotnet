// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Base exception for all Rocksteady service exceptions
/// </summary>
#if NETFRAMEWORK
[Serializable]
#endif
public class TestPlatformException : Exception
{
    public TestPlatformException(string? message)
        : base(message)
    {
    }

    public TestPlatformException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}

#if FullCLR
    /// <summary>
    /// This class converts WCF fault exception to a strongly-typed exception
    /// </summary>
    public static class ExceptionConverter
    {
        /// <summary>
        /// This method converts WCF fault exception to a strongly-typed exception
        /// </summary>
        /// <param name="faultEx">FaultException</param>
        /// <returns>strongly typed exception that is wrapped in Fault Exception</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public static Exception ConvertException(FaultException faultEx)
        {
            ValidateArg.NotNull(faultEx, nameof(faultEx));
            if (faultEx.Code == null || faultEx.Code.Name == null)
            {
                return new TestPlatformException(faultEx.Message, faultEx);
            }
            return ConvertException(faultEx.Code.Name, faultEx.Message, faultEx);
        }

        /// <summary>
        /// Creates a strongly-typed exception that is represented by the exception name
        /// passed as parameter
        /// </summary>
        /// <param name="exceptionType">Exception type class name</param>
        /// <param name="message">message of exception</param>
        /// <param name="innerException">actual exception that is to be wrapped</param>
        /// <returns>actual exception that is represented by the exception name</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Instantiating an instance of Exception class")]
        private static Exception ConvertException(string exceptionType, string message, Exception innerException)
        {
            try
            {
                string className = rocksteadyExceptionNameSpace + "." + exceptionType;
                Type t = typeof(Microsoft.VisualStudio.TestPlatform.Common.TestRunner.TestPlatformException);
                string assembly = Assembly.GetAssembly(t).FullName;

                System.Runtime.Remoting.ObjectHandle handle = Activator.CreateInstance(assembly, className, true,
                                0, null, new object[] { message, innerException }, CultureInfo.InvariantCulture, null, null);
                return ((TestPlatformException)handle.Unwrap());
            }
            catch (Exception)
            {
                // Ignore it, and try to get System.Exception
            }

            try
            {
                string className = "System" + "." + exceptionType;
                Type t = typeof(System.Exception);
                string assembly = Assembly.GetAssembly(t).FullName;

                System.Runtime.Remoting.ObjectHandle handle = Activator.CreateInstance(assembly, className, true,
                                0, null, new object[] { message, innerException }, CultureInfo.InvariantCulture, null, null);
                Exception tempEx = (Exception)handle.Unwrap();
                return tempEx;
            }
            catch (Exception)
            {
                // Neither System nor TestPlatformException, but still pass it as TestPlatformException
                return new TestPlatformException(message, innerException);
            }
        }

        private const string rocksteadyExceptionNameSpace = "Microsoft.VisualStudio.TestPlatform.Core.TestRunner";
    }
#endif

#if NETFRAMEWORK
[Serializable]
#endif
public class ProcessExitedException : TestPlatformException
{
    public ProcessExitedException(string? message) : base(message) { }
    public ProcessExitedException(string? message, Exception? inner) : base(message, inner) { }
}
