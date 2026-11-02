---
layout: default
title: AntiSSRFHandler
parent: .NET API Reference
nav_order: 2
description: "HttpMessageHandler that enforces AntiSSRF policies"
---

# AntiSSRFHandler

`AntiSSRFHandler` is the type of the object returned from `AntiSSRFPolicy.GetHandler`. The `AntiSSRFHandler` wraps `HttpClientHandler` or `SocketsHttpHandler` and implements `HttpMessageHandler` while exposing some properties on the inner handler. This handler performs DNS resolution validation, scheme enforcement, header checks, and redirect following according to the configured policy.

### .NET Core

In .NET Core, the inner handler is a `SocketsHttpHandler`. The exposed properties below can be used exactly as they are used in the original `SocketsHttpHandler` type. Please see [SocketsHttpHandler](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.socketshandler) for details.

```csharp
-  public bool AllowAutoRedirect { get; set; }
-  public int MaxAutomaticRedirections { get; set; }
-  public CookieContainer CookieContainer { get; set; }
-  public ICredentials? Credentials { get; set; }
-  public int MaxConnectionsPerServer { get; set; }
-  public int MaxResponseHeadersLength { get; set; }
-  public bool UseCookies { get; set; }
-  public TimeSpan ConnectTimeout { get; set; }
-  public TimeSpan ResponseDrainTimeout { get; set; }
-  public TimeSpan PooledConnectionLifetime { get; set; }
-  public SslClientAuthenticationOptions SslOptions { get; set; }
```

### .NET Standard 2.0 / .NET Framework

In .NET Standard 2.0 / .NET Framework, the inner handler is an `HttpClientHandler`. The exposed properties below can be used exactly as they are used in the original `HttpClientHandler` type. Please see [HttpClientHandler](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclienthandler) for details.

```csharp
-  public bool AllowAutoRedirect { get; set; }
-  public int MaxAutomaticRedirections { get; set; }
-  public ICredentials? Credentials { get; set; }
-  public int MaxConnectionsPerServer { get; set; }
-  public int MaxResponseHeadersLength { get; set; }
-  public bool CheckCertificateRevocationList { get; set; }
-  public Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>? ServerCertificateCustomValidationCallback { get; set; }
-  public SslProtocols SslProtocols { get; set; }
```