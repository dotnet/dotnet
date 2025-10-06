// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.TestUtils
{
    public class GetSetContext
    {
        private List<KeyValuePair<string, List<object>>> _propertyNamesAndSetGetValues;

        /// <summary>
        /// Any errors will be put here.
        /// </summary>
        public List<string> Errors { get; } = new List<string>();

        /// <summary>
        /// The 'TKey' in <see cref="KeyValuePair{TKey, TValue}"/> is the name of the Method to call.
        /// The first 'TValue' is the default value, the others will be used to perform a 'set / get' and then check values are equal. This catches the error where an assignment is made to the wrong private variable.
        /// </summary>
        public List<KeyValuePair<string, List<object>>> PropertyNamesAndSetGetValue { get { return _propertyNamesAndSetGetValues; } set { _propertyNamesAndSetGetValues = value; } }

        /// <summary>
        /// This is an instance of the object that is to be tested.
        /// </summary>
        public object Object { get; set; }
    }

    public static class TestUtilities
    {
        /// <summary>
        /// Calls all public instance and static properties on an object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="testcase">contains info about the current test case</param>
        public static void CallAllPublicInstanceAndStaticPropertyGets(object obj, string testcase)
        {
            if (obj == null)
            {
                Console.WriteLine(string.Format("Entering: '{0}', obj is null, have to return.  Is the Testcase: '{1}' right?", "CallAllPublicInstanceAndStaticPropertyGets", testcase ?? "testcase is null"));
                return;
            }

            Type type = obj.GetType();

            // call get all public static properties of MyClass type
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);

            // Touch each public property
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                try
                {
                    if (propertyInfo.GetMethod != null)
                    {
                        object retval = propertyInfo.GetValue(obj, null);
                    }
                }
                catch (Exception ex)
                {
                    throw new TestException(string.Format("Testcase: '{0}', type: '{1}', property: '{2}', exception: '{3}'", type, testcase ?? "testcase is null", propertyInfo.Name, ex));
                }
            }
        }

        /// <summary>
        /// Gets a named field on an object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="field"></param>
        public static object GetField(object obj, string field)
        {
            Type type = obj.GetType();
            FieldInfo fieldInfo = type.GetField(field, BindingFlags.NonPublic | BindingFlags.Instance);
            return fieldInfo.GetValue(obj);
        }

        /// <summary>
        /// Sets a named field on an object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="field"></param>
        public static void SetField(object obj, string field, object fieldValue)
        {
            Type type = obj.GetType();
            FieldInfo fieldInfo = type.GetField(field, BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(obj, fieldValue);
        }

        /// <summary>
        /// Gets a named property on an object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="propertyValue"></param>
        public static object GetProperty(object obj, string property)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(property);
            if (propertyInfo == null)
                throw new TestException("property is not found: " + property + ", type: " + type.ToString());

            return propertyInfo.GetValue(obj);
        }

        /// <summary>
        /// Checks initial value, sets and then checks value. Works with multiple properties.
        /// </summary>
        /// <param name="context"> <see cref="GetSetContext"/>, drives the test.</param>
        public static void GetSet(GetSetContext context)
        {
            Type type = context.Object.GetType();

            foreach (KeyValuePair<string, List<object>> propertyKV in context.PropertyNamesAndSetGetValue)
            {
                PropertyInfo propertyInfo = type.GetProperty(propertyKV.Key);
                try
                {
                    if (propertyInfo.GetMethod != null)
                    {
                        object initialValue = propertyInfo.GetValue(context.Object, null);
                        if ((initialValue == null && propertyKV.Value[0] != null))
                        {
                            context.Errors.Add(propertyKV.Key + ": initial value == null && expected != null, expect initial value: " + propertyKV.Value[0].ToString());
                        }
                        else if (initialValue != null && propertyKV.Value[0] == null)
                        {
                            context.Errors.Add(propertyKV.Key + ": initial value != null && expected == null, initial value: " + initialValue.ToString());
                        }
                        else if (initialValue != null && !IdentityComparer.AreEqual(initialValue, propertyKV.Value[0]))
                        {
                            context.Errors.Add(propertyKV.Key + ", initial value != expected. expected: " + propertyKV.Value[0].ToString() + ", was: " + initialValue.ToString());
                        }
                    }

                    if (propertyInfo.SetMethod != null)
                    {
                        for (int i = 1; i < propertyKV.Value.Count; i++)
                        {
                            propertyInfo.SetValue(context.Object, propertyKV.Value[i]);
                            object getVal = propertyInfo.GetValue(context.Object, null);
                            if ((getVal == null && propertyKV.Value[i] != null))
                            {
                                context.Errors.Add(propertyKV.Key + "( " + i.ToString() + "), Get returned null, set was: " + propertyKV.Value[i].ToString());
                            }
                            else if (getVal != null && propertyKV.Value[i] == null)
                            {
                                context.Errors.Add(propertyKV.Key + "( " + i.ToString() + "), Get not null, set was null, get was: " + getVal);
                            }
                            else if (getVal != null && !getVal.Equals(propertyKV.Value[i]))
                            {
                                context.Errors.Add(propertyKV.Key + "( " + i.ToString() + ") Set did not equal get: " + propertyKV.Value[i].ToString() + ", " + getVal + ".");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    context.Errors.Add(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Gets and sets a named property on an object. Checks: initial value.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="initialPropertyValue"></param>
        /// <param name="setPropertyValue"></param>
        public static void GetSet(object obj, string property, object initialPropertyValue, object[] setPropertyValues, List<string> errors)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(property);

            if (propertyInfo == null)
            {
                errors.Add("property get is not found: " + property + ", type: " + type.ToString());
                return;
            }

            object retval = propertyInfo.GetValue(obj);
            if (initialPropertyValue != retval)
            {
                errors.Add("initialPropertyValue != retval: " + initialPropertyValue + " , " + retval);
                return;
            }

            if (propertyInfo.CanWrite)
            {
                foreach (object propertyValue in setPropertyValues)
                {
                    propertyInfo.SetValue(obj, propertyValue);
                    retval = propertyInfo.GetValue(obj);
                    if (propertyValue != retval)
                    {
                        errors.Add("propertyValue != retval: " + propertyValue + " , " + retval);
                    }
                }
            }
        }

        public static string SerializeAsSingleCommaDelimitedString(IEnumerable<string> strings)
        {
            if (strings == null)
            {
                return "null";
            }

            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (string str in strings)
            {
                if (first)
                {
                    sb.AppendFormat("{0}", str ?? "null");
                    first = false;
                }
                else
                {
                    sb.AppendFormat(", {0}", str ?? "null");
                }
            }

            if (first)
            {
                return "empty";
            }

            return sb.ToString();
        }

        /// <summary>
        /// Sets a property, then checks it, checking for an expected exception.
        /// </summary>
        /// <param name="obj">object that has a 'setter'.</param>
        /// <param name="property">the name of the property.</param>
        /// <param name="propertyValue">value to set on the property.</param>
        /// <param name="expectedException">checks that exception is correct.</param>
        public static void SetGet(object obj, string property, object propertyValue, ExpectedException expectedException, GetSetContext context)
        {
            if (obj == null)
                throw new TestException("obj == null");

            if (string.IsNullOrWhiteSpace(property))
                throw new TestException("string.IsNullOrWhiteSpace(property)");

            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(property);

            if (propertyInfo == null)
            {
                context.Errors.Add("'get is not found for property: '" + property + "', type: '" + type.ToString() + "'");
                return;
            }

            if (!propertyInfo.CanWrite)
            {
                context.Errors.Add("can not write to property: '" + property + "', type: '" + type.ToString() + "'");
                return;
            }

            var compareContext = new CompareContext();

            try
            {
                propertyInfo.SetValue(obj, propertyValue);
                object retval = propertyInfo.GetValue(obj);
                IdentityComparer.AreEqual(propertyValue, retval, compareContext);
                expectedException.ProcessNoException(compareContext);
            }
            catch (Exception exception)
            {
                // look for InnerException as exception is a wrapped exception.
                expectedException.ProcessException(exception.InnerException, compareContext);
            }

            context.Errors.AddRange(compareContext.Diffs);
        }

        /// <summary>
        /// Set a named property on an object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="propertyValue"></param>
        public static void SetProperty(object obj, string property, object propertyValue)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(property);
            if (propertyInfo == null)
                throw new TestException("property is not found: " + property + ", type: " + type.ToString());

            object retval = propertyInfo.GetValue(obj);
            if (propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(obj, propertyValue);
            }
            else
            {
                throw new TestException("property 'set' is not found: " + property + ", type: " + type.ToString());
            }
        }

        public static ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, ISecurityTokenValidator tokenValidator, ExpectedException expectedException)
        {
            ClaimsPrincipal retVal = null;
            SecurityToken validatedToken = null;
            try
            {
                retVal = tokenValidator.ValidateToken(securityToken, validationParameters, out validatedToken);
                Assert.True(validatedToken != null);
                expectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                Assert.True(validatedToken == null);
                expectedException.ProcessException(ex);
            }

            return retVal;
        }

        public static void ValidateTokenReplay(string securityToken, ISecurityTokenValidator tokenValidator, TokenValidationParameters validationParameters)
        {
            if (!validationParameters.ValidateTokenReplay)
                return;

            TokenValidationParameters tvp = validationParameters.Clone();
            TokenReplayCache replayCache =
               new TokenReplayCache()
               {
                   OnAddReturnValue = true,
                   OnFindReturnValue = false,
               };

            tvp.TokenReplayCache = replayCache;
            TestUtilities.ValidateToken(securityToken, tvp, tokenValidator, ExpectedException.NoExceptionExpected);

            replayCache.OnFindReturnValue = true;
            TestUtilities.ValidateToken(securityToken, tvp, tokenValidator, ExpectedException.SecurityTokenReplayDetected());

            replayCache.OnFindReturnValue = false;
            replayCache.OnAddReturnValue = false;
            TestUtilities.ValidateToken(securityToken, tvp, tokenValidator, ExpectedException.SecurityTokenReplayAddFailed());
        }

        public static void AssertFailIfErrors(List<string> errors)
        {
            AssertFailIfErrors(null, errors);
        }

        public static void AssertFailIfErrors(CompareContext context)
        {
            AssertFailIfErrors(context.Title, context.Diffs);
        }

        public static void AssertFailIfErrors(string testId, List<string> errors)
        {
            if (errors.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(testId))
                {
                    sb.AppendLine(testId);
                    sb.AppendLine(Environment.NewLine);
                }

                foreach (string str in errors)
                    sb.AppendLine(str);

                throw new TestException(sb.ToString());
            }
        }

        public static void CheckForArgumentNull(CompareContext context, string name, Exception ex)
        {
            if (ex == null)
                context.Diffs.Add($"expecting ArgumentNullException for parameter {name}. Exception is null.");
            else if (!(ex is ArgumentNullException) || !ex.Message.Contains(name))
                context.Diffs.Add($"!(ex is ArgumentNullException) || !ex.Message.Contains({name})");
        }

        public static void CheckForArgumentException(CompareContext context, string name, Exception ex)
        {
            if (ex == null)
                context.Diffs.Add($"expecting ArgumentException for parameter {name}. Exception is null.");
            else if (!(ex is ArgumentException) || !ex.Message.Contains(name))
                context.Diffs.Add($"!(ex is ArgumentException) || !ex.Message.Contains({name})");
        }

        public static byte[] HexToByteArray(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i += 2)
            {
                string s = hexString.Substring(i, 2);
                bytes[i / 2] = byte.Parse(s, NumberStyles.HexNumber, null);
            }

            return bytes;
        }

        public static void XORBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length - 1; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ bytes[i + 1]);
            }
        }

        public static void TestHeader(string testcase, string variation, ref bool first)
        {
            TestHeader($"{testcase} : {variation}", ref first);
        }

        public static void TestHeader(string testcase, ref bool first)
        {
            if (first)
                Console.WriteLine("====================================");

            first = false;
            Console.WriteLine(">>>> " + testcase);
        }

        public static CompareContext WriteHeader(string testcase, TheoryDataBase theoryData)
        {
            WriteHeader($"{testcase} : {theoryData.TestId}", theoryData.First);

            return new CompareContext($"{testcase} : {theoryData.TestId}", theoryData);
        }

        public static CompareContext WriteHeader(string testcase, string testId, bool first)
        {

            if (first)
                Console.WriteLine("====================================");

            Console.WriteLine($">>>> {testcase}, Id: {testId}.");
            return new CompareContext
            {
                Title = $"{testcase} : {testId}",
                TestId = testId
            };
        }

        public static void WriteHeader(string testcase)
        {
            WriteHeader(testcase, true);
        }

        public static void WriteHeader(string testcase, bool first)
        {
            if (first)
                Console.WriteLine("====================================");

            Console.WriteLine(">>>> " + testcase);
        }
    }

    public class TokenReplayCache : ITokenReplayCache
    {
        public bool OnAddReturnValue { get; set; }

        public bool OnFindReturnValue { get; set; }

        public bool TryAdd(string nonce, DateTime expiresAt)
        {
            return OnAddReturnValue;
        }

        public bool TryFind(string nonce)
        {
            return OnFindReturnValue;
        }
    }

    public static class TupleListExtensions
    {
        public static void Add<T1, T2>(this IList<Tuple<T1, T2>> list,
                T1 item1, T2 item2)
        {
            list.Add(Tuple.Create(item1, item2));
        }

        public static void Add<T1, T2, T3>(this IList<Tuple<T1, T2, T3>> list,
                T1 item1, T2 item2, T3 item3)
        {
            list.Add(Tuple.Create(item1, item2, item3));
        }
    }
}
