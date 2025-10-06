#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    internal class TestUtils
    {
        public static void ValidateEventLogMessage(TestEventListener listener, string expectedMessage, EventLevel level)
        {
            bool messageFound = false;

            foreach (var actualEvent in listener.Messages.Where((arg) => { return arg.Level == level; }))
            {
                string actualMessage = string.Format(CultureInfo.InvariantCulture, actualEvent.Message, actualEvent.Payload.ToArray());
                messageFound = messageFound || actualMessage.Contains(expectedMessage);
            }

            Assert.IsTrue(messageFound);
        }

        /// <summary>
        /// Generates a new <see cref="System.Data.SqlClient.SqlException"/> with the specified exception number using reflection. 
        /// This is necessary because the constructors for <see cref="System.Data.SqlClient.SqlException"/> are internal to the .NET framework.
        /// </summary>
        /// <param name="exceptionNumber">Exception number of the <see cref="System.Data.SqlClient.SqlException"/>.</param>
        /// <returns>A new instance of <see cref="System.Data.SqlClient.SqlException"/>.</returns>
        public static SqlException GenerateSqlException(int exceptionNumber)
        {
            var ex = (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

            var errorCollection = (SqlErrorCollection)FormatterServices.GetUninitializedObject(typeof(SqlErrorCollection));

            var error = (SqlError)FormatterServices.GetUninitializedObject(typeof(SqlError));

            SetPrivateField(error, "number", exceptionNumber);

            SetPrivateField(errorCollection, "errors", new ArrayList { error });

            SetPrivateField(ex, "_errors", errorCollection);

            return ex;
        }

        /// <summary>
        /// Generates an HttpWebResponse that has the specified status code using reflection. 
        /// This is necessary because the constructors for HttpWebResponse are internal to the .NET framework.
        /// </summary>
        /// <param name="statusCode">Http status code of the response.</param>
        /// <returns>A new instance of <see cref="System.Net.HttpWebResponse"/></returns>
        public static HttpWebResponse GenerateHttpWebResponse(HttpStatusCode statusCode)
        {
            var response = (HttpWebResponse)FormatterServices.GetUninitializedObject(typeof(HttpWebResponse));

            SetPrivateField(response, "m_StatusCode", statusCode);
            return response;
        }

        /// <summary>
        /// Generates an HttpWebResponse that has the specified status code using reflection. 
        /// This is necessary because the constructors for HttpWebResponse are internal to the .NET framework.
        /// </summary>
        /// <param name="statusCode">Http status code of the response.</param>
        /// <param name="headers">Headers to be set on the response.</param>
        /// <returns>A new instance of <see cref="System.Net.HttpWebResponse"/></returns>
        public static HttpWebResponse GenerateHttpWebResponse(HttpStatusCode statusCode, Dictionary<string, string> headers)
        {
            var response = GenerateHttpWebResponse(statusCode);

            var headerCollection = new WebHeaderCollection();
            foreach (var item in headers)
            {
                headerCollection.Add(item.Key, item.Value);
            }

            SetPrivateField(response, "m_HttpResponseHeaders", headerCollection);
            return response;
        }

        public static HttpWebResponse GenerateDisposedHttpWebResponse(HttpStatusCode statusCode)
        {
            var response = (HttpWebResponse)FormatterServices.GetUninitializedObject(typeof(HttpWebResponse));

            SetPrivateField(response, "m_StatusCode", statusCode);
            SetPrivateField(response, "m_propertiesDisposed", true);
            return response;
        }

        private static void SetPrivateField(object obj, string field, object value)
        {
            var member = obj.GetType().GetField(field, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            member.SetValue(obj, value);
        }
    }
}
#endif