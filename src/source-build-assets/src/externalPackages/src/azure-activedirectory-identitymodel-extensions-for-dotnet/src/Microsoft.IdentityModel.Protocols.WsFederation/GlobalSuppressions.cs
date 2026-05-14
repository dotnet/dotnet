// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Globalization is not used")]

[assembly: SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Breaking change", Scope = "type", Target = "~T:Microsoft.IdentityModel.Xml.WsAddressing")]
[assembly: SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Breaking change", Scope = "type", Target = "~T:Microsoft.IdentityModel.Xml.WsPolicy")]
[assembly: SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Breaking change", Scope = "type", Target = "~T:Microsoft.IdentityModel.Xml.WsTrustConstants")]
[assembly: SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Breaking change", Scope = "type", Target = "~T:Microsoft.IdentityModel.Xml.WsTrustConstants_1_3")]
[assembly: SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Breaking change", Scope = "type", Target = "~T:Microsoft.IdentityModel.Xml.WsTrustConstants_1_4")]
[assembly: SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Breaking change", Scope = "type", Target = "~T:Microsoft.IdentityModel.Xml.WsTrustConstants_2005")]
[assembly: SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Breaking change", Scope = "type", Target = "~T:Microsoft.IdentityModel.Xml.WsUtility.Elements")]
[assembly: SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Breaking change", Scope = "type", Target = "~T:Microsoft.IdentityModel.Protocols.WsFederation.WsFederationConstants")]

[assembly: SuppressMessage("Design", "CA1055:Uri return values should not be strings", Justification = "Breaking change", Scope = "member", Target = "~M:Microsoft.IdentityModel.Protocols.WsFederation.WsFederationMessage.CreateSignInUrl~System.String")]
[assembly: SuppressMessage("Design", "CA1055:Uri return values should not be strings", Justification = "Breaking change", Scope = "member", Target = "~M:Microsoft.IdentityModel.Protocols.WsFederation.WsFederationMessage.CreateSignOutUrl~System.String")]

[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Checked in utility method", Scope = "member", Target = "~T:Microsoft.IdentityModel.Protocols.WsFederation.WsFederationMetadataSerializer")]

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Breaking change", Scope = "namespaceanddescendants", Target = "~T:Microsoft.IdentityModel.Xml.WsTrustConstants")]
[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Breaking change", Scope = "type", Target = "~T:Microsoft.IdentityModel.Xml.WsTrustConstants_1_3")]
[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Breaking change", Scope = "type", Target = "~T:Microsoft.IdentityModel.Xml.WsTrustConstants_1_4")]
[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Breaking change", Scope = "type", Target = "~T:Microsoft.IdentityModel.Xml.WsTrustConstants_2005")]

[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Doesn't own object", Scope = "member", Target = "~M:Microsoft.IdentityModel.Protocols.WsFederation.WsFederationMetadataSerializer.WriteMetadata(System.Xml.XmlWriter,Microsoft.IdentityModel.Protocols.WsFederation.WsFederationConfiguration)")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Doesn't own object", Scope = "member", Target = "~M:Microsoft.IdentityModel.Protocols.WsFederation.WsFederationMetadataSerializer.ReadMetadata(System.Xml.XmlReader)~Microsoft.IdentityModel.Protocols.WsFederation.WsFederationConfiguration")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Doesn't own object", Scope = "member", Target = "~M:Microsoft.IdentityModel.Protocols.WsFederation.WsFederationMetadataSerializer.ReadEntityDescriptor(System.Xml.XmlReader)~Microsoft.IdentityModel.Protocols.WsFederation.WsFederationConfiguration")]

[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Breaking change", Scope = "member", Target = "~P:Microsoft.IdentityModel.Protocols.WsFederation.SecurityTokenServiceTypeRoleDescriptor.KeyInfos")]
#if NET6_0_OR_GREATER
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "Adding StringComparison.Ordinal adds a performance penalty.", Scope = "member", Target = "~M:Microsoft.IdentityModel.Protocols.WsFederation.QueryHelper.ParseNullableQuery(System.String)~System.Collections.Generic.IDictionary{System.String,System.Collections.Generic.IList{System.String}}")]
#endif

