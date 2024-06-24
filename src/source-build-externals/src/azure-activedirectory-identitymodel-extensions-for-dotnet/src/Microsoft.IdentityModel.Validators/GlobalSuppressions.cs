// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Needs to be ignored", Scope = "member", Target = "~M:Microsoft.IdentityModel.Validators.AadIssuerValidator.IsValidIssuer(System.String,System.String,System.String)~System.Boolean")]
[assembly: SuppressMessage("Usage", "VSTHRD002:Avoid problematic synchronous waits", Justification = "Cannot change the signature of the public method.", Scope = "member", Target = "~M:Microsoft.IdentityModel.Validators.AadIssuerValidator.Validate(System.String,Microsoft.IdentityModel.Tokens.SecurityToken,Microsoft.IdentityModel.Tokens.TokenValidationParameters)~System.String")]
#if NET6_0_OR_GREATER
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "Adding StringComparison.Ordinal adds a performance penalty.", Scope = "member", Target = "~M:Microsoft.IdentityModel.Validators.AadIssuerValidator.IsValidIssuer(System.String,System.String,System.String)~System.Boolean")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "Adding StringComparison.Ordinal adds a performance penalty.", Scope = "member", Target = "~M:Microsoft.IdentityModel.Validators.AadIssuerValidator.#ctor(System.Net.Http.HttpClient,System.String)")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "Adding StringComparison.Ordinal adds a performance penalty.", Scope = "member", Target = "~M:Microsoft.IdentityModel.Validators.AadIssuerValidator.CreateV1Authority(System.String,System.String)~System.String")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "Adding StringComparison.Ordinal adds a performance penalty.", Scope = "member", Target = "~M:Microsoft.IdentityModel.Validators.AadIssuerValidator.GetProtocolVersion(System.String)~Microsoft.IdentityModel.Validators.AadIssuerValidator.ProtocolVersion")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "Adding StringComparison.Ordinal adds a performance penalty.", Scope = "member", Target = "~M:Microsoft.IdentityModel.Validators.AadIssuerValidator.GetProtocolVersion(System.String)~Microsoft.IdentityModel.Validators.ProtocolVersion")]
#endif

