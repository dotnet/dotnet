namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    internal class ResponseStatusCodes
    {
        public const int Success = 200;
        public const int PartialSuccess = 206;
        public const int RequestTimeout = 408;
        public const int UnknownNetworkError = 999;
        public const int ResponseCodeTooManyRequests = 429;
        public const int ResponseCodeTooManyRequestsOverExtendedTime = 439;
        public const int InternalServerError = 500;
        public const int BadGateway = 502;
        public const int ServiceUnavailable = 503;
        public const int GatewayTimeout = 504;
        public const int BadRequest = 400; // AAD: AI resource was configured for AAD, but SDK is using older api. (v2 and v2.1).
        public const int Unauthorized = 401; // AAD: token is either absent, invalid, or expired.
        public const int Forbidden = 403; // AAD: Provided credentials do not grant access to ingest telemetry.
    }
}
