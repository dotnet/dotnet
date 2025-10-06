namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;
    using System.Collections.Generic;

    using TaskEx = System.Threading.Tasks.Task;


    internal class AssertEx
    {
        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            var a = actual.GetEnumerator();

            foreach(var t in expected)
            {
                Assert.IsTrue(a.MoveNext());
                Assert.AreEqual(t, a.Current);
            }
        }

        /// <summary>
        /// Verifies that the exact exception is thrown (and not a derived exception type).
        /// </summary>
        /// <typeparam name="T">The type of the exception expected to be thrown.</typeparam>
        /// <param name="testCode">A delegate to the task to be tested.</param>
        /// <returns>The exception that was thrown, when successful.</returns>
        /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
        public static async Task<T> ThrowsAsync<T>(Func<Task> testCode)
            where T : Exception
        {
            return (T)AssertEx.Throws(typeof(T), await AssertEx.ExceptionAsync(testCode));
        }

        /// <summary>
        /// Verifies that the exact exception is thrown (and not a derived exception type).
        /// </summary>
        /// <typeparam name="T">The type of the exception expected to be thrown.</typeparam>
        /// <param name="testCode">A delegate to the task to be tested.</param>
        /// <returns>The exception that was thrown, when successful.</returns>
        /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
        public static T Throws<T>(Action testCode)
            where T : Exception
        {
            return (T)AssertEx.Throws(typeof(T), AssertEx.ExceptionSync(testCode));
        }

        /// <summary>
        /// Verifies that the object has a specified type.
        /// </summary>
        /// <typeparam name="T">The type of the object to expect.</typeparam>
        /// <param name="actual">An object to be tested.</param>
        public static T IsType<T>(object actual)
        {
            Assert.IsTrue(actual is T, "Object is not assignable from " + typeof(T).ToString() + ". Actual: " + actual.ToString());
            return (T)actual;
        }

        /// <summary>
        /// Verifies that a string starts with a given string, using the given comparison type.
        /// </summary>
        /// <param name="expectedSubString">The string expected to be part of the string.</param>
        /// <param name="actualString">The string to be inspected.</param>
        /// <param name="comparisonType">The type of string comparison to perform.</param>
        /// <exception cref="ContainsException">Thrown when the string does not start with the expected string.</exception>
        public static void Contains(string expectedSubString, string actualString, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (expectedSubString == null || actualString == null || actualString.IndexOf(expectedSubString, comparisonType) == -1)
            {
                Assert.Fail("Expected substring: " + expectedSubString + ". Actual string:" + actualString);
            }
        }

        /// <summary>
        /// Verifies that a string starts with a given string, using the given comparison type.
        /// </summary>
        /// <param name="expectedSubString">The string expected to be part of the string.</param>
        /// <param name="actualString">The string to be inspected.</param>
        /// <param name="comparisonType">The type of string comparison to perform.</param>
        /// <exception cref="ContainsException">Thrown when the string does not start with the expected string.</exception>
        public static void Contains<T>(T expectedElement, IList<T> actualList)
        {
            if (actualList == null || actualList.IndexOf(expectedElement) == -1)
            {
                Assert.Fail("Expected substring: " + expectedElement.ToString() + ". Actual list:" + actualList.ToString());
            }
        }

        /// <summary>
        /// Verifies that a string starts with a given string, using the given comparison type.
        /// </summary>
        /// <param name="expectedSubString">The string expected to not be part of the string.</param>
        /// <param name="actualString">The string to be inspected.</param>
        /// <param name="comparisonType">The type of string comparison to perform.</param>
        /// <exception cref="ContainsException">Thrown when the string does not start with the expected string.</exception>
        public static void DoesNotContain(string notExpectedSubString, string actualString, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (notExpectedSubString == null || actualString == null || actualString.IndexOf(notExpectedSubString, comparisonType) != -1)
            {
                Assert.Fail("Not expected substring: " + notExpectedSubString + ". Actual string:" + actualString);
            }
        }

        /// <summary>
        /// Verifies that a string starts with a given string, using the given comparison type.
        /// </summary>
        /// <param name="expectedSubString">The string expected to be part of the string.</param>
        /// <param name="actualString">The string to be inspected.</param>
        /// <param name="comparisonType">The type of string comparison to perform.</param>
        /// <exception cref="ContainsException">Thrown when the string does not start with the expected string.</exception>
        public static void DoesNotContain<T>(T expectedElement, IList<T> actualList)
        {
            if (actualList == null || actualList.IndexOf(expectedElement) != -1)
            {
                Assert.Fail("Not expected substring: " + expectedElement.ToString() + ". Actual list:" + actualList.ToString());
            }
        }

        /// <summary>
        /// Verifies that a string is empty.
        /// </summary>
        /// <param name="actualString">The string to be inspected.</param>
        public static void IsEmpty(string actualString)
        {
            Assert.AreEqual(string.Empty, actualString);
        }

        /// <summary>
        /// Verifies that a collection is empty.
        /// </summary>
        /// <param name="collection">The collection to be inspected.</param>
        public static void IsEmpty(IEnumerable collection)
        {
            Assert.AreEqual(false, collection.GetEnumerator().MoveNext());
        }

        public static void InRange<T>(T actual, T min, T max) where T: IComparable
        {
            Assert.IsTrue(actual.CompareTo(min) >= 0, "Value " + actual.ToString() + " is not in range " + min.ToString() + " - " + max.ToString());
            Assert.IsTrue(actual.CompareTo(max) <= 0, "Value " + actual.ToString() + " is not in range " + min.ToString() + " - " + max.ToString());
        }

        /// <summary>
        /// Verifies that a string starts with a given string, using the given comparison type.
        /// </summary>
        /// <param name="expectedStartString">The string expected to be at the start of the string.</param>
        /// <param name="actualString">The string to be inspected.</param>
        /// <param name="comparisonType">The type of string comparison to perform.</param>
        /// <exception cref="ContainsException">Thrown when the string does not start with the expected string.</exception>
        public static void StartsWith(string expectedStartString, string actualString, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (expectedStartString == null || actualString == null || !actualString.StartsWith(expectedStartString, comparisonType))
            {
                Assert.Fail("Expected start string: " + expectedStartString + ". Actual string:" + actualString);
            }
        }

        /// <summary>
        /// Verifies that a string ends with a given string, using the current culture.
        /// </summary>
        /// <param name="expectedEndString">The string expected to be at the end of the string.</param>
        /// <param name="actualString">The string to be inspected.</param>
        /// <exception cref="ContainsException">Thrown when the string does not end with the expected string.</exception>
        public static void EndsWith(string expectedEndString, string actualString)
        {
            EndsWith(expectedEndString, actualString, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Verifies that a string ends with a given string, using the given comparison type.
        /// </summary>
        /// <param name="expectedEndString">The string expected to be at the end of the string.</param>
        /// <param name="actualString">The string to be inspected.</param>
        /// <param name="comparisonType">The type of string comparison to perform.</param>
        /// <exception cref="ContainsException">Thrown when the string does not end with the expected string.</exception>
        public static void EndsWith(string expectedEndString, string actualString, StringComparison comparisonType)
        {
            if (expectedEndString == null || actualString == null || !actualString.EndsWith(expectedEndString, comparisonType))
            {
                Assert.Fail("Expected end string: " + expectedEndString + ". Actual string:" + actualString);
            }
        }

        private static Exception Throws(Type exceptionType, Exception exception)
        {
            if (exception == null)
            {
                Assert.Fail("Exception is null " + exceptionType.ToString());
            }

            if (exceptionType != exception.GetType())
            {
                Assert.Fail("Exception do not match type " + exceptionType.ToString() + ". Exception: " + exception.ToString());
            }

            return exception;
        }

        private static Exception ThrowsAny(Type exceptionType, Exception exception)
        {
            if (exception == null)
            {
                Assert.Fail("Exception is null " + exceptionType.ToString());
            }
            if (!exceptionType.GetTypeInfo().IsAssignableFrom(exception.GetType().GetTypeInfo()))
            {
                Assert.Fail("Exception is not assignable from " + exceptionType.ToString() + ". Exception: " + exception.ToString());
            }

            return exception;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "The caught exception is resurfaced to the user.")]
        private static async Task<Exception> ExceptionAsync(Func<Task> testCode)
        {
            try
            {
                await testCode();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "The caught exception is resurfaced to the user.")]
        private static Exception ExceptionSync(Action testCode)
        {
            try
            {
                testCode();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}