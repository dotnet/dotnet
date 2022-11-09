using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Helix.Client
{
    partial class HelixApiResponseClassifier
    {
        public override bool IsRetriableException(Exception exception)
        {
            return base.IsRetriableException(exception) ||
                   exception is TaskCanceledException ||
                   exception is OperationCanceledException ||
                   exception is HttpRequestException ||
                   exception is RestApiException raex && raex.Response.Status >= 500 && raex.Response.Status <= 599 ||
                   exception is IOException ||
                   exception is SocketException ||
                   exception is RestApiException jobListEx && jobListEx.Response.Status == 400 && jobListEx.Message.Contains("Provided Job List Uri is not accessible");
        }
    }

    partial class HelixApi
    {
        partial void HandleFailedRequest(RestApiException ex)
        {
            if (ex.Response.Status == (int)HttpStatusCode.BadRequest)
            {
                JObject content;
                try
                {
                    content = JObject.Parse(ex.Response.Content);
                }
                catch (Exception)
                {
                    return;
                }

                if (content["Message"] is JValue value && value.Type == JTokenType.String)
                {
                    string message = (string)value.Value;

                    throw new ArgumentException(message, ex);
                }
            }
        }
    }
}
