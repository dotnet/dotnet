#if !NET
using System;

namespace System.Diagnostics.CodeAnalysis {

	[AttributeUsage (AttributeTargets.Field | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
	sealed class DynamicallyAccessedMembersAttribute : Attribute {

		public DynamicallyAccessedMembersAttribute (DynamicallyAccessedMemberTypes memberTypes)
		{
			MemberTypes = memberTypes;
		}

		public DynamicallyAccessedMemberTypes MemberTypes { get; }
	}

	[Flags]
	enum DynamicallyAccessedMemberTypes {
		None = 0,
		PublicParameterlessConstructor = 0x0001,
		PublicConstructors = 0x0002 | PublicParameterlessConstructor,
		NonPublicConstructors = 0x0004,
		PublicMethods = 0x0008,
		NonPublicMethods = 0x0010,
		PublicFields = 0x0020,
		NonPublicFields = 0x0040,
		PublicNestedTypes = 0x0080,
		NonPublicNestedTypes = 0x0100,
		PublicProperties = 0x0200,
		NonPublicProperties = 0x0400,
		PublicEvents = 0x0800,
		NonPublicEvents = 0x1000,
		Interfaces = 0x2000,
		All = ~None,
	}

	[AttributeUsage (AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
	sealed class RequiresUnreferencedCodeAttribute : Attribute {

		public RequiresUnreferencedCodeAttribute (string message)
		{
			Message = message;
		}

		public string Message { get; }

		public string Url { get; set; }
	}
}
#endif
