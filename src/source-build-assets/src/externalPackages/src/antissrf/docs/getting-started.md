---
layout: default
title: Getting Started
nav_order: 2
description: "Learn how to install and configure AntiSSRF in your application"
---

# Getting Started with Microsoft AntiSSRF

The Microsoft AntiSSRF library helps protect your applications from Server-Side Request Forgery (SSRF) vulnerabilities by providing robust URL validation and secure HTTP client configurations.

## Installation

### .NET Framework and .NET Core (C#)

Install the NuGet package:

```bash
dotnet add package Microsoft.Security.AntiSSRF
```

### Node.js (JavaScript/TypeScript)

Install the npm package:

```bash
npm install @microsoft/antissrf
```

## Quick Start Examples

### .NET Usage

```csharp
using Microsoft.Security.AntiSSRF;
using System.Net.Http;

// Create a policy for external-only requests
var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);

// Get a secure HttpMessageHandler
using var handler = policy.GetHandler();

// Use with HttpClient
using var client = new HttpClient(handler);

// Make secure requests - internal IPs will be blocked
var response = await client.GetAsync("https://api.example.com/data");
```

### Node.js Usage

```javascript
const { AntiSSRFPolicy, PolicyConfigOptions } = require('@microsoft/antissrf');
const https = require('https');

// Create a policy for external-only requests
const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);

// Get a secure HTTPS agent
const httpsAgent = policy.getHttpsAgent({ keepAlive: true });

// Use with standard Node.js requests
const options = {
  hostname: 'api.example.com',
  path: '/data',
  agent: httpsAgent
};

https.get(options, (res) => {
  // Handle response
});
```

## How to Use

The AntiSSRF library provides validation for different scenarios based on your trust requirements:

| Use Case | Description | Documentation Link |
|----------|-------------|-------------------|
| **General Case** | The untrusted URL can belong to **any domain** or an **untrusted domain**. | [.NET](../dotnet-api/antissrfpolicy) \| [Node.js](../nodejs-api/antissrfpolicy) |
| **Azure Key Vault Domain** | The untrusted URL must be an **Azure Key Vault endpoint**. | [.NET](../dotnet-api/urivalidator/inazurekeyvaultdomain) \| [Node.js](../nodejs-api/urivalidator/inazurekeyvaultdomain) |
| **Azure Storage Domain** | The untrusted URL must be an **Azure Storage endpoint**. | [.NET](../dotnet-api/urivalidator/inazurestoragedomain/) \| [Node.js](../nodejs-api/urivalidator/inazurestoragedomain/) |
| **Allowlist of Trusted Domains** | The untrusted URL must belong to a **specific, trusted domain**. | [.NET](../dotnet-api/urivalidator/indomain/) \| [Node.js](../nodejs-api/urivalidator/indomain/) |

## Best Practices

1. **Use Separate Handlers for External vs. Internal Requests**

    Always create separate HTTP clients for external and internal requests so that you can use the strictest security policy possible on each.
    This approach ensures that external API calls cannot accidentally reach internal services, and internal calls are restricted to only the networks you explicitly trust. When using the `AntiSSRFPolicy`, you can choose different built-in configuration options intended for each use-case.

2. **Only Use `InDomain` for Owned and Trusted Domains**

    A domain should only be considered trusted if you fully control both the domain itself and all subdomains. You should trust the DNS responses for these domains and should be sure that no subdomain is configurable by a third party.

3. **Add X-Forwarded-For Header whenever possible**

    The `X-Forwarded-For` header can be an important defense-in-depth strategy against SSRF vulnerabilities. Some services, including IMDS, will drop all incoming requests with the `X-Forwarded-For` present. By ensuring that the header is added to all outgoing requests, your service can be sure that it will never have an SSRF vulnerability that leaks data from IMDS.

4. **Stay up-to-date**

    Keep the library updated to receive the latest security changes. Instead of using `PolicyConfigOptions.ExternalOnlyV1`, consider using `PolicyConfigOptions.ExternalOnlyLatest`.

## Next Steps

### Learn More
- 📖 **API Documentation**: [.NET API](../dotnet-api) \| [Node.js API](../nodejs-api)
- ❓ **Common Questions**: [FAQ](../faq)

### Get Support

- 🐛 **Report Issues**: [GitHub Issues](https://github.com/Microsoft/AntiSSRF/issues)
- 📧 **Contact**: [antissrf-oss@microsoft.com](mailto:antissrf-oss@microsoft.com)
