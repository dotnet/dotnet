# Microsoft AntiSSRF

The Microsoft AntiSSRF library is a security-developed, exhaustively-tested secure code library that provides robust URL validation to mitigate the risk of Server-Side Request Forgery (SSRF) vulnerabilities. It is an easy-to-use drop-in library with minimal adoption effort for developers, available for both .NET and Node.js applications.

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

## How the Microsoft AntiSSRF Library Helps

A common scenario in many online services is handling requests from customers containing customer-supplied strings that are, or are used to construct a URL. These strings are often not validated properly, leading to vulnerabilities such as Server-Side Request Forgery which can result in token theft.

AntiSSRF helps mitigate these risks by:
- Automatically validating URLs and network connections and rejecting/refusing unsafe input
- Providing an agent that ensures HTTP requests cannot reach internal or sensitive IP addresses

## Getting Started

### C# (.NET Framework and .NET Core)

- **NuGet Package**: [Microsoft.Security.AntiSSRF](https://www.nuget.org/packages/Microsoft.Security.AntiSSRF/)
- **Documentation**: [AntiSSRF .NET API Documentation](https://microsoft.github.io/AntiSSRF/dotnet-api/)
- **Quick Start**: [Getting Started Guide](https://microsoft.github.io/AntiSSRF/getting-started)
- **Library README**: [.NET README](dotnet/README.md)

### JavaScript/TypeScript (Node.js)

- **npm Package**: [@microsoft/antissrf](https://www.npmjs.com/package/@microsoft/antissrf)
- **Documentation**: [AntiSSRF Node.js API Documentation](https://microsoft.github.io/AntiSSRF/nodejs-api/)
- **Quick Start**: [Getting Started Guide](https://microsoft.github.io/AntiSSRF/getting-started)
- **Library README**: [Node.js README](nodejs/README.md)

## Contributing

We welcome contributions! Please see our contribution resources:

- **Contributing Guide**: [CONTRIBUTING.md](CONTRIBUTING.md)
- **Report Issues**: [GitHub Issues](https://github.com/Microsoft/AntiSSRF/issues)
- **License**: [LICENSE](LICENSE)
- **Security Policy**: [SECURITY.md](SECURITY.md)
- **Code of Conduct**: [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md)
- **Support**: [SUPPORT.md](SUPPORT.md)

## More Resources

### Learning About SSRF

- **AntiSSRF Documentation**: [Microsoft AntiSSRF Documentation](https://microsoft.github.io/AntiSSRF/)
- **OWASP SSRF Guide**: [Server-Side Request Forgery Prevention Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Server_Side_Request_Forgery_Prevention_Cheat_Sheet.html)
- **PortSwigger Web Security Academy**: [Server-side request forgery (SSRF)](https://portswigger.net/web-security/ssrf)
- **CWE-918**: [Server-Side Request Forgery (SSRF)](https://cwe.mitre.org/data/definitions/918.html)

### Testing Tools

- **Dusseldorf**: [Dynamic SSRF Testing Tool](https://github.com/Microsoft/Dusseldorf) - Microsoft's open-source tool for dynamic SSRF testing and validation