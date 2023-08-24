// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Exception is returned", Scope = "member", Target = "~M:Microsoft.IdentityModel.Protocols.SignedHttpRequest.SignedHttpRequestHandler.ValidateSignedHttpRequestAsync(Microsoft.IdentityModel.Protocols.SignedHttpRequest.SignedHttpRequestValidationContext,System.Threading.CancellationToken)~System.Threading.Tasks.Task{Microsoft.IdentityModel.Protocols.SignedHttpRequest.SignedHttpRequestValidationResult}")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Previously released as read / write", Scope = "member", Target = "~P:Microsoft.IdentityModel.Protocols.SignedHttpRequest.SignedHttpRequestDescriptor.AdditionalHeaderClaims")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Previously released as read / write", Scope = "member", Target = "~P:Microsoft.IdentityModel.Protocols.SignedHttpRequest.SignedHttpRequestDescriptor.AdditionalPayloadClaims")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Headers need to be lowercase to calcuate appropriate hash", Scope = "type", Target = "~T:Microsoft.IdentityModel.Protocols.SignedHttpRequest.SignedHttpRequestHandler")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "Vendored component", Scope = "module")]
[assembly: SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Breaking change", Scope = "type", Target = "~T:Microsoft.IdentityModel.Protocols.SignedHttpRequest.SignedHttpRequestHandler")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Previously released as read / write", Scope = "member", Target = "~P:Microsoft.IdentityModel.Protocols.SignedHttpRequest.SignedHttpRequestInvalidNonceClaimException.PropertyBag")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Breaking Change", Scope = "member", Target = "~T:Microsoft.IdentityModel.Protocols.SignedHttpRequest.SignedHttpRequestHandler)")]
