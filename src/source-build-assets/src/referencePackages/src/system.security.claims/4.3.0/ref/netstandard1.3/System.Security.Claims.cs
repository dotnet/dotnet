// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Security.Claims")]
[assembly: System.Reflection.AssemblyDescription("System.Security.Claims")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Claims")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.1.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Security.Claims
{
    public partial class Claim
    {
        public Claim(IO.BinaryReader reader, ClaimsIdentity subject) { }

        public Claim(IO.BinaryReader reader) { }

        protected Claim(Claim other, ClaimsIdentity subject) { }

        protected Claim(Claim other) { }

        public Claim(string type, string value, string valueType, string issuer, string originalIssuer, ClaimsIdentity subject) { }

        public Claim(string type, string value, string valueType, string issuer, string originalIssuer) { }

        public Claim(string type, string value, string valueType, string issuer) { }

        public Claim(string type, string value, string valueType) { }

        public Claim(string type, string value) { }

        protected virtual byte[] CustomSerializationData { get { throw null; } }

        public string Issuer { get { throw null; } }

        public string OriginalIssuer { get { throw null; } }

        public Collections.Generic.IDictionary<string, string> Properties { get { throw null; } }

        public ClaimsIdentity Subject { get { throw null; } }

        public string Type { get { throw null; } }

        public string Value { get { throw null; } }

        public string ValueType { get { throw null; } }

        public virtual Claim Clone() { throw null; }

        public virtual Claim Clone(ClaimsIdentity identity) { throw null; }

        public override string ToString() { throw null; }

        protected virtual void WriteTo(IO.BinaryWriter writer, byte[] userData) { }

        public virtual void WriteTo(IO.BinaryWriter writer) { }
    }

    public partial class ClaimsIdentity : Principal.IIdentity
    {
        public const string DefaultIssuer = "LOCAL AUTHORITY";
        public const string DefaultNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public const string DefaultRoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public ClaimsIdentity() { }

        public ClaimsIdentity(Collections.Generic.IEnumerable<Claim> claims, string authenticationType, string nameType, string roleType) { }

        public ClaimsIdentity(Collections.Generic.IEnumerable<Claim> claims, string authenticationType) { }

        public ClaimsIdentity(Collections.Generic.IEnumerable<Claim> claims) { }

        public ClaimsIdentity(IO.BinaryReader reader) { }

        protected ClaimsIdentity(ClaimsIdentity other) { }

        public ClaimsIdentity(Principal.IIdentity identity, Collections.Generic.IEnumerable<Claim> claims, string authenticationType, string nameType, string roleType) { }

        public ClaimsIdentity(Principal.IIdentity identity, Collections.Generic.IEnumerable<Claim> claims) { }

        public ClaimsIdentity(Principal.IIdentity identity) { }

        public ClaimsIdentity(string authenticationType, string nameType, string roleType) { }

        public ClaimsIdentity(string authenticationType) { }

        public ClaimsIdentity Actor { get { throw null; } set { } }

        public virtual string AuthenticationType { get { throw null; } }

        public object BootstrapContext { get { throw null; } set { } }

        public virtual Collections.Generic.IEnumerable<Claim> Claims { get { throw null; } }

        protected virtual byte[] CustomSerializationData { get { throw null; } }

        public virtual bool IsAuthenticated { get { throw null; } }

        public string Label { get { throw null; } set { } }

        public virtual string Name { get { throw null; } }

        public string NameClaimType { get { throw null; } }

        public string RoleClaimType { get { throw null; } }

        public virtual void AddClaim(Claim claim) { }

        public virtual void AddClaims(Collections.Generic.IEnumerable<Claim> claims) { }

        public virtual ClaimsIdentity Clone() { throw null; }

        protected virtual Claim CreateClaim(IO.BinaryReader reader) { throw null; }

        public virtual Collections.Generic.IEnumerable<Claim> FindAll(Predicate<Claim> match) { throw null; }

        public virtual Collections.Generic.IEnumerable<Claim> FindAll(string type) { throw null; }

        public virtual Claim FindFirst(Predicate<Claim> match) { throw null; }

        public virtual Claim FindFirst(string type) { throw null; }

        public virtual bool HasClaim(Predicate<Claim> match) { throw null; }

        public virtual bool HasClaim(string type, string value) { throw null; }

        public virtual void RemoveClaim(Claim claim) { }

        public virtual bool TryRemoveClaim(Claim claim) { throw null; }

        protected virtual void WriteTo(IO.BinaryWriter writer, byte[] userData) { }

        public virtual void WriteTo(IO.BinaryWriter writer) { }
    }

    public partial class ClaimsPrincipal : Principal.IPrincipal
    {
        public ClaimsPrincipal() { }

        public ClaimsPrincipal(Collections.Generic.IEnumerable<ClaimsIdentity> identities) { }

        public ClaimsPrincipal(IO.BinaryReader reader) { }

        public ClaimsPrincipal(Principal.IIdentity identity) { }

        public ClaimsPrincipal(Principal.IPrincipal principal) { }

        public virtual Collections.Generic.IEnumerable<Claim> Claims { get { throw null; } }

        public static Func<ClaimsPrincipal> ClaimsPrincipalSelector { get { throw null; } set { } }

        public static ClaimsPrincipal Current { get { throw null; } }

        protected virtual byte[] CustomSerializationData { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<ClaimsIdentity> Identities { get { throw null; } }

        public virtual Principal.IIdentity Identity { get { throw null; } }

        public static Func<Collections.Generic.IEnumerable<ClaimsIdentity>, ClaimsIdentity> PrimaryIdentitySelector { get { throw null; } set { } }

        public virtual void AddIdentities(Collections.Generic.IEnumerable<ClaimsIdentity> identities) { }

        public virtual void AddIdentity(ClaimsIdentity identity) { }

        public virtual ClaimsPrincipal Clone() { throw null; }

        protected virtual ClaimsIdentity CreateClaimsIdentity(IO.BinaryReader reader) { throw null; }

        public virtual Collections.Generic.IEnumerable<Claim> FindAll(Predicate<Claim> match) { throw null; }

        public virtual Collections.Generic.IEnumerable<Claim> FindAll(string type) { throw null; }

        public virtual Claim FindFirst(Predicate<Claim> match) { throw null; }

        public virtual Claim FindFirst(string type) { throw null; }

        public virtual bool HasClaim(Predicate<Claim> match) { throw null; }

        public virtual bool HasClaim(string type, string value) { throw null; }

        public virtual bool IsInRole(string role) { throw null; }

        protected virtual void WriteTo(IO.BinaryWriter writer, byte[] userData) { }

        public virtual void WriteTo(IO.BinaryWriter writer) { }
    }

    public static partial class ClaimTypes
    {
        public const string Actor = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor";
        public const string Anonymous = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/anonymous";
        public const string Authentication = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication";
        public const string AuthenticationInstant = "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant";
        public const string AuthenticationMethod = "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod";
        public const string AuthorizationDecision = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authorizationdecision";
        public const string CookiePath = "http://schemas.microsoft.com/ws/2008/06/identity/claims/cookiepath";
        public const string Country = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country";
        public const string DateOfBirth = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth";
        public const string DenyOnlyPrimaryGroupSid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarygroupsid";
        public const string DenyOnlyPrimarySid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarysid";
        public const string DenyOnlySid = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/denyonlysid";
        public const string DenyOnlyWindowsDeviceGroup = "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlywindowsdevicegroup";
        public const string Dns = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dns";
        public const string Dsa = "http://schemas.microsoft.com/ws/2008/06/identity/claims/dsa";
        public const string Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        public const string Expiration = "http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration";
        public const string Expired = "http://schemas.microsoft.com/ws/2008/06/identity/claims/expired";
        public const string Gender = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender";
        public const string GivenName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
        public const string GroupSid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid";
        public const string Hash = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/hash";
        public const string HomePhone = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/homephone";
        public const string IsPersistent = "http://schemas.microsoft.com/ws/2008/06/identity/claims/ispersistent";
        public const string Locality = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality";
        public const string MobilePhone = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone";
        public const string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public const string OtherPhone = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/otherphone";
        public const string PostalCode = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/postalcode";
        public const string PrimaryGroupSid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid";
        public const string PrimarySid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid";
        public const string Role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public const string Rsa = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rsa";
        public const string SerialNumber = "http://schemas.microsoft.com/ws/2008/06/identity/claims/serialnumber";
        public const string Sid = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid";
        public const string Spn = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/spn";
        public const string StateOrProvince = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/stateorprovince";
        public const string StreetAddress = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/streetaddress";
        public const string Surname = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
        public const string System = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/system";
        public const string Thumbprint = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumbprint";
        public const string Upn = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn";
        public const string Uri = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri";
        public const string UserData = "http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata";
        public const string Version = "http://schemas.microsoft.com/ws/2008/06/identity/claims/version";
        public const string Webpage = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/webpage";
        public const string WindowsAccountName = "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsaccountname";
        public const string WindowsDeviceClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsdeviceclaim";
        public const string WindowsDeviceGroup = "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsdevicegroup";
        public const string WindowsFqbnVersion = "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsfqbnversion";
        public const string WindowsSubAuthority = "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowssubauthority";
        public const string WindowsUserClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsuserclaim";
        public const string X500DistinguishedName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/x500distinguishedname";
    }

    public static partial class ClaimValueTypes
    {
        public const string Base64Binary = "http://www.w3.org/2001/XMLSchema#base64Binary";
        public const string Base64Octet = "http://www.w3.org/2001/XMLSchema#base64Octet";
        public const string Boolean = "http://www.w3.org/2001/XMLSchema#boolean";
        public const string Date = "http://www.w3.org/2001/XMLSchema#date";
        public const string DateTime = "http://www.w3.org/2001/XMLSchema#dateTime";
        public const string DaytimeDuration = "http://www.w3.org/TR/2002/WD-xquery-operators-20020816#dayTimeDuration";
        public const string DnsName = "http://schemas.xmlsoap.org/claims/dns";
        public const string Double = "http://www.w3.org/2001/XMLSchema#double";
        public const string DsaKeyValue = "http://www.w3.org/2000/09/xmldsig#DSAKeyValue";
        public const string Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        public const string Fqbn = "http://www.w3.org/2001/XMLSchema#fqbn";
        public const string HexBinary = "http://www.w3.org/2001/XMLSchema#hexBinary";
        public const string Integer = "http://www.w3.org/2001/XMLSchema#integer";
        public const string Integer32 = "http://www.w3.org/2001/XMLSchema#integer32";
        public const string Integer64 = "http://www.w3.org/2001/XMLSchema#integer64";
        public const string KeyInfo = "http://www.w3.org/2000/09/xmldsig#KeyInfo";
        public const string Rfc822Name = "urn:oasis:names:tc:xacml:1.0:data-type:rfc822Name";
        public const string Rsa = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rsa";
        public const string RsaKeyValue = "http://www.w3.org/2000/09/xmldsig#RSAKeyValue";
        public const string Sid = "http://www.w3.org/2001/XMLSchema#sid";
        public const string String = "http://www.w3.org/2001/XMLSchema#string";
        public const string Time = "http://www.w3.org/2001/XMLSchema#time";
        public const string UInteger32 = "http://www.w3.org/2001/XMLSchema#uinteger32";
        public const string UInteger64 = "http://www.w3.org/2001/XMLSchema#uinteger64";
        public const string UpnName = "http://schemas.xmlsoap.org/claims/UPN";
        public const string X500Name = "urn:oasis:names:tc:xacml:1.0:data-type:x500Name";
        public const string YearMonthDuration = "http://www.w3.org/TR/2002/WD-xquery-operators-20020816#yearMonthDuration";
    }
}

namespace System.Security.Principal
{
    public partial class GenericIdentity : Claims.ClaimsIdentity
    {
        protected GenericIdentity(GenericIdentity identity) { }

        public GenericIdentity(string name, string type) { }

        public GenericIdentity(string name) { }

        public override string AuthenticationType { get { throw null; } }

        public override Collections.Generic.IEnumerable<Claims.Claim> Claims { get { throw null; } }

        public override bool IsAuthenticated { get { throw null; } }

        public override string Name { get { throw null; } }

        public override Claims.ClaimsIdentity Clone() { throw null; }
    }

    public partial class GenericPrincipal : Claims.ClaimsPrincipal
    {
        public GenericPrincipal(IIdentity identity, string[] roles) { }

        public override IIdentity Identity { get { throw null; } }

        public override bool IsInRole(string role) { throw null; }
    }
}