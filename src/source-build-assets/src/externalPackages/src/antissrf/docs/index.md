---
layout: default
title: Home
nav_order: 1
description: "Microsoft AntiSSRF Documentation - Protect your applications from SSRF attacks"
permalink: /
---

# Microsoft AntiSSRF Libraries

## What is Server-Side Request Forgery (SSRF)?

Server-Side Request Forgery (also known as SSRF) is a critical web security vulnerability in which an attacker can manipulate the server-side application to make network requests to an arbitrary endpoint. Through this vulnerability, the attacker manipulates the target web server to connect to internal, sensitive networks or exfiltrate sensitive data to an untrusted endpoint on the Internet.

SSRF can lead (but is not limited) to:
- Exposure of internal services
- Leakage of sensitive data
- Service disruption
- Remote code execution

### What is "Untrusted" Input?

**All incoming HTTP requests are untrusted.** Any data originating from outside your service's immediate trust boundary must be treated as potentially malicious. This includes:

- User-provided URLs, filenames, or identifiers
- Data from external APIs, webhooks, or partner services
- Configuration values, metadata, or file contents that users can influence
- Requests from your own service's backend applications or other components within the same environment (query parameters, headers, form fields, etc.)

Even data that doesn't initially appear to be a URL should be treated as one. For example, a workspace name or resource identifier that gets concatenated into a URL. All untrusted input used in URL construction MUST be validated.

## What is the Microsoft AntiSSRF Library?

The Microsoft AntiSSRF Library is a security-developed, exhaustively-tested secure code library available for multiple platforms, that provides robust URL validation to mitigate the risk of SSRF vulnerabilities in code where it is integrated. It is an easy-to-use drop-in library with a minimal toil of adoption on developers.

### How the Microsoft AntiSSRF Library Helps

A common scenario in many online services is handling requests from customers containing customer-supplied strings that are, or are used to construct a URL. These strings are often not validated properly, leading to vulnerabilities such as Server-Side Request Forgery which can result in token theft.

AntiSSRF helps mitigate these risks by:

- Automatically validating URLs and network connections and rejecting/refusing unsafe input
- Providing an agent that ensures HTTP requests cannot reach internal or sensitive IP addresses

## Supported Languages and Frameworks

| Language | Documentation | Notes |
| --- | --- | --- |
| C# | [AntiSSRF .NET Library](dotnet-api/) | For web clients using `HttpClient` objects |
| JavaScript/TypeScript | [AntiSSRF Node.js Library](nodejs-api/) | For requests using NodeJS HTTP(S) Agents |

{: .note }
> Broader platform support is under development.

## Next Steps

### Learn More

- 🚀 **Getting Started**: [Installation and Quick Start Guide](getting-started)
- 📖 **API Documentation**: [.NET API](dotnet-api) \| [Node.js API](nodejs-api)
- ❓ **Common Questions**: [FAQ](faq)

### Get Support

- 🐛 **Report Issues**: [GitHub Issues](https://github.com/Microsoft/AntiSSRF/issues)
- 📧 **Contact**: [antissrf-oss@microsoft.com](mailto:antissrf-oss@microsoft.com)

