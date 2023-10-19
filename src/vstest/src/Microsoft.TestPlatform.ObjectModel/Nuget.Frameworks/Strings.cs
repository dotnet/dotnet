
namespace NuGetClone.Frameworks {    
    
    internal class Strings {
        
        internal static string ArgumentCannotBeNullOrEmpty {
            get {
                return "The argument cannot be null or empty.";
            }
        }
        
        internal static string FrameworkDoesNotSupportProfiles {
            get {
                return ".NET 5.0 and above does not support profiles.";
            }
        }

        internal static string FrameworkMismatch {
            get {
                return "Frameworks must have the same identifier, profile, and platform.";
            }
        }
        
        internal static string InvalidFrameworkIdentifier {
            get {
                return "{0} is the invalid framework identifier.";
            }
        }
        
        internal static string InvalidFrameworkVersion {
            get {
                return "{0} is the invalid framework version.";
            }
        }
        
        internal static string InvalidPlatformVersion {
            get {
                return "{0} is the invalid platform version.";
            }
        }
        
        internal static string InvalidPortableFrameworksDueToHyphen {
            get {
                return "{0} is the invalid portable framework string.";
            }
        }
        
        internal static string MissingPortableFrameworks {
            get {
                return "{0} is the invalid portable framework string.";
            }
        }
    }
}
