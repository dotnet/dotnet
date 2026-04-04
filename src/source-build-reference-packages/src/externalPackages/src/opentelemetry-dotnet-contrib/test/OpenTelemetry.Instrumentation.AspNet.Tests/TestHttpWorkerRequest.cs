// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Web;

namespace OpenTelemetry.Instrumentation.AspNet.Tests;

internal class TestHttpWorkerRequest : HttpWorkerRequest
{
    public override string GetKnownRequestHeader(int index)
    {
        if (index == 39)
        {
            return "Custom User Agent v1.2.3";
        }

        return base.GetKnownRequestHeader(index);
    }

    public override void EndOfRequest()
    {
        throw new NotImplementedException();
    }

    public override void FlushResponse(bool finalFlush)
    {
        throw new NotImplementedException();
    }

    public override string GetHttpVerbName()
    {
        throw new NotImplementedException();
    }

    public override string GetHttpVersion()
    {
        return "FakeHTTP/123";
    }

    public override string GetLocalAddress()
    {
        return "fake-local-address"; // avoid throwing exception
    }

    public override int GetLocalPort()
    {
        return 1234; // avoid throwing exception
    }

    public override string GetQueryString()
    {
        throw new NotImplementedException();
    }

    public override string GetRawUrl()
    {
        throw new NotImplementedException();
    }

    public override string GetRemoteAddress()
    {
        return "fake-remote-address"; // avoid throwing exception
    }

    public override int GetRemotePort()
    {
        throw new NotImplementedException();
    }

    public override string GetUriPath()
    {
        throw new NotImplementedException();
    }

    public override void SendKnownResponseHeader(int index, string value)
    {
        throw new NotImplementedException();
    }

    public override void SendResponseFromFile(string filename, long offset, long length)
    {
        throw new NotImplementedException();
    }

    public override void SendResponseFromFile(IntPtr handle, long offset, long length)
    {
        throw new NotImplementedException();
    }

    public override void SendResponseFromMemory(byte[] data, int length)
    {
        throw new NotImplementedException();
    }

    public override void SendStatus(int statusCode, string statusDescription)
    {
        throw new NotImplementedException();
    }

    public override void SendUnknownResponseHeader(string name, string value)
    {
        throw new NotImplementedException();
    }
}
