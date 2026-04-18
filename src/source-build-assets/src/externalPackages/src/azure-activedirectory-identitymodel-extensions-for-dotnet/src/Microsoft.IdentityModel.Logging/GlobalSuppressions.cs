// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Previously released as non-static / inheritable", Scope = "type", Target = "~T:Microsoft.IdentityModel.Logging.LogHelper")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Execution should not be altered for exceptions on format", Scope = "member", Target = "~M:Microsoft.IdentityModel.Logging.IdentityModelEventSource.PrepareMessage(System.Diagnostics.Tracing.EventLevel,System.String,System.Object[])~System.String")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Breaking change", Scope = "member", Target = "~P:Microsoft.IdentityModel.Logging.LoggerContext.PropertyBag")]
