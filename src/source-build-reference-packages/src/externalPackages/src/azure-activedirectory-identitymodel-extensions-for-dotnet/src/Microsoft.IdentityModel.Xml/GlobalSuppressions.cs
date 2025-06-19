// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Doesn't own object", Scope = "member", Target = "~M:Microsoft.IdentityModel.Xml.EnvelopedSignatureWriter.OnEndRootElement")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Ownership is shared between multiple objects and stream is only in memory", Scope = "member", Target = "~M:Microsoft.IdentityModel.Xml.DSigSerializer.ReadSignedInfo(System.Xml.XmlReader)~Microsoft.IdentityModel.Xml.SignedInfo")]
[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "Breaking change", Scope = "member", Target = "~P:Microsoft.IdentityModel.Xml.KeyInfo.RetrievalMethodUri")]
[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "Breaking change", Scope = "member", Target = "~P:Microsoft.IdentityModel.Xml.Reference.Uri")]
[assembly: SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Breaking change", Scope = "type", Target = "~T:Microsoft.IdentityModel.Xml.XmlSignatureConstants")]
[assembly: SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Current design", Scope = "member", Target = "~P:Microsoft.IdentityModel.Xml.DelegatingXmlDictionaryReader.UseInnerReader")]
[assembly: SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Current design", Scope = "member", Target = "~P:Microsoft.IdentityModel.Xml.DelegatingXmlDictionaryWriter.UseInnerWriter")]
[assembly: SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Breaking change", Scope = "member", Target = "~F:Microsoft.IdentityModel.Xml.XmlSignatureConstants.Elements.Object")]
[assembly: SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Breaking change", Scope = "member", Target = "~M:Microsoft.IdentityModel.Xml.XmlUtil.NormalizeEmptyString(System.String)~System.String")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "Vendored component", Scope = "module")]
