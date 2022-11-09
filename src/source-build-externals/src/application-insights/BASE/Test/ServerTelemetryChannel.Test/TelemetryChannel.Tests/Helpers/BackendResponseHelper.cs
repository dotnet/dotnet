namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers
{
    using System.Globalization;

    public static class BackendResponseHelper
    {
        public static string CreateBackendResponse(int itemsReceived, int itemsAccepted, string[] errorCodes,
            int indexStartWith = 0)
        {
            string singleItem = "{{" +
                                "\"index\": {0}," +
                                "\"statusCode\": {1}," +
                                "\"message\": \"Explanation\"" +
                                "}}";

            string errorList = string.Empty;
            for (int i = 0; i < errorCodes.Length; ++i)
            {
                string errorCode = errorCodes[i];
                if (!string.IsNullOrEmpty(errorList))
                {
                    errorList += ",";
                }

                errorList += string.Format(CultureInfo.InvariantCulture, singleItem, indexStartWith + i, errorCode);
            }

            return
                "{" +
                "\"itemsReceived\": " + itemsReceived + "," +
                "\"itemsAccepted\": " + itemsAccepted + "," +
                "\"errors\": [" + errorList + "]" +
                "}";
        }
    }
}
