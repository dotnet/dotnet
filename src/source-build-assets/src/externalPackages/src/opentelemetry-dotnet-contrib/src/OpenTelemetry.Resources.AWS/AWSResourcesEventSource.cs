// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Resources.AWS;

[EventSource(Name = "OpenTelemetry-Resources-AWS")]
internal sealed class AWSResourcesEventSource : EventSource, IServerCertificateValidationEventSource
{
    public static AWSResourcesEventSource Log = new();

    private const int EventIdFailedToExtractAttributes = 1;
    private const int EventIdFailedToValidateCertificate = 2;
    private const int EventIdFailedToCreateHttpHandler = 3;
    private const int EventIdFailedCertificateFileNotExists = 4;
    private const int EventIdFailedToLoadCertificateInStorage = 5;

    [NonEvent]
    public void ResourceAttributesExtractException(string format, Exception ex)
    {
        if (this.IsEnabled(EventLevel.Warning, (EventKeywords)(-1)))
        {
            this.FailedToExtractResourceAttributes(format, ex.ToInvariantString());
        }
    }

    [NonEvent]
    public void FailedToCreateHttpHandler(Exception exception)
    {
        if (this.IsEnabled(EventLevel.Warning, (EventKeywords)(-1)))
        {
            this.FailedToCreateHttpHandler(exception.ToInvariantString());
        }
    }

    [Event(EventIdFailedToExtractAttributes, Message = "Failed to extract resource attributes in '{0}'. Error Message: '{1}'", Level = EventLevel.Warning)]
    public void FailedToExtractResourceAttributes(string format, string exception)
    {
        this.WriteEvent(EventIdFailedToExtractAttributes, format, exception);
    }

    [Event(EventIdFailedToValidateCertificate, Message = "Failed to validate certificate. Details: '{0}'", Level = EventLevel.Warning)]
    public void FailedToValidateCertificate(string error)
    {
        this.WriteEvent(EventIdFailedToValidateCertificate, error);
    }

    [Event(EventIdFailedToCreateHttpHandler, Message = "Failed to create HTTP handler. Exception: '{0}'", Level = EventLevel.Warning)]
    public void FailedToCreateHttpHandler(string error)
    {
        this.WriteEvent(EventIdFailedToCreateHttpHandler, error);
    }

    [Event(EventIdFailedCertificateFileNotExists, Message = "Certificate file does not exist. File: '{0}'", Level = EventLevel.Warning)]
    public void CertificateFileDoesNotExist(string filename)
    {
        this.WriteEvent(EventIdFailedCertificateFileNotExists, filename);
    }

    [Event(EventIdFailedToLoadCertificateInStorage, Message = "Failed to load certificate in trusted storage. File: '{0}'", Level = EventLevel.Warning)]
    public void FailedToLoadCertificateInTrustedStorage(string filename)
    {
        this.WriteEvent(EventIdFailedToLoadCertificateInStorage, filename);
    }
}
