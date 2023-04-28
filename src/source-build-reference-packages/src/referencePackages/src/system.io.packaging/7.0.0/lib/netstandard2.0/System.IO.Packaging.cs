// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.IO.Packaging")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides classes that support storage of multiple data objects in a single container.")]
[assembly: System.Reflection.AssemblyFileVersion("7.0.22.51805")]
[assembly: System.Reflection.AssemblyInformationalVersion("7.0.0+d099f075e45d2aa6007a22b71b45a08758559f80")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.IO.Packaging")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("7.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.IO
{
    public partial class FileFormatException : FormatException
    {
        public FileFormatException() { }

        protected FileFormatException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public FileFormatException(string? message, Exception? innerException) { }

        public FileFormatException(string? message) { }

        public FileFormatException(Uri? sourceUri, Exception? innerException) { }

        public FileFormatException(Uri? sourceUri, string? message, Exception? innerException) { }

        public FileFormatException(Uri? sourceUri, string? message) { }

        public FileFormatException(Uri? sourceUri) { }

        public Uri? SourceUri { get { throw null; } }

        public override void GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }
    }
}

namespace System.IO.Packaging
{
    public enum CompressionOption
    {
        NotCompressed = -1,
        Normal = 0,
        Maximum = 1,
        Fast = 2,
        SuperFast = 3
    }

    public enum EncryptionOption
    {
        None = 0,
        RightsManagement = 1
    }

    public abstract partial class Package : IDisposable
    {
        protected Package(FileAccess openFileAccess) { }

        public FileAccess FileOpenAccess { get { throw null; } }

        public PackageProperties PackageProperties { get { throw null; } }

        public void Close() { }

        public PackagePart CreatePart(Uri partUri, string contentType, CompressionOption compressionOption) { throw null; }

        public PackagePart CreatePart(Uri partUri, string contentType) { throw null; }

        protected abstract PackagePart CreatePartCore(Uri partUri, string contentType, CompressionOption compressionOption);
        public PackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string relationshipType, string? id) { throw null; }

        public PackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string relationshipType) { throw null; }

        public void DeletePart(Uri partUri) { }

        protected abstract void DeletePartCore(Uri partUri);
        public void DeleteRelationship(string id) { }

        protected virtual void Dispose(bool disposing) { }

        public void Flush() { }

        protected abstract void FlushCore();
        public PackagePart GetPart(Uri partUri) { throw null; }

        protected abstract PackagePart? GetPartCore(Uri partUri);
        public PackagePartCollection GetParts() { throw null; }

        protected abstract PackagePart[] GetPartsCore();
        public PackageRelationship GetRelationship(string id) { throw null; }

        public PackageRelationshipCollection GetRelationships() { throw null; }

        public PackageRelationshipCollection GetRelationshipsByType(string relationshipType) { throw null; }

        public static Package Open(Stream stream, FileMode packageMode, FileAccess packageAccess) { throw null; }

        public static Package Open(Stream stream, FileMode packageMode) { throw null; }

        public static Package Open(Stream stream) { throw null; }

        public static Package Open(string path, FileMode packageMode, FileAccess packageAccess, FileShare packageShare) { throw null; }

        public static Package Open(string path, FileMode packageMode, FileAccess packageAccess) { throw null; }

        public static Package Open(string path, FileMode packageMode) { throw null; }

        public static Package Open(string path) { throw null; }

        public virtual bool PartExists(Uri partUri) { throw null; }

        public bool RelationshipExists(string id) { throw null; }

        void IDisposable.Dispose() { }
    }

    public abstract partial class PackagePart
    {
        protected PackagePart(Package package, Uri partUri, string? contentType, CompressionOption compressionOption) { }

        protected PackagePart(Package package, Uri partUri, string? contentType) { }

        protected PackagePart(Package package, Uri partUri) { }

        public CompressionOption CompressionOption { get { throw null; } }

        public string ContentType { get { throw null; } }

        public Package Package { get { throw null; } }

        public Uri Uri { get { throw null; } }

        public PackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string relationshipType, string? id) { throw null; }

        public PackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string relationshipType) { throw null; }

        public void DeleteRelationship(string id) { }

        protected virtual string GetContentTypeCore() { throw null; }

        public PackageRelationship GetRelationship(string id) { throw null; }

        public PackageRelationshipCollection GetRelationships() { throw null; }

        public PackageRelationshipCollection GetRelationshipsByType(string relationshipType) { throw null; }

        public Stream GetStream() { throw null; }

        public Stream GetStream(FileMode mode, FileAccess access) { throw null; }

        public Stream GetStream(FileMode mode) { throw null; }

        protected abstract Stream? GetStreamCore(FileMode mode, FileAccess access);
        public bool RelationshipExists(string id) { throw null; }
    }

    public partial class PackagePartCollection : Collections.Generic.IEnumerable<PackagePart>, Collections.IEnumerable
    {
        public Collections.Generic.IEnumerator<PackagePart> GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<PackagePart> Collections.Generic.IEnumerable<PackagePart>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public abstract partial class PackageProperties : IDisposable
    {
        protected PackageProperties() { }

        public abstract string? Category { get; set; }
        public abstract string? ContentStatus { get; set; }
        public abstract string? ContentType { get; set; }
        public abstract DateTime? Created { get; set; }
        public abstract string? Creator { get; set; }
        public abstract string? Description { get; set; }
        public abstract string? Identifier { get; set; }
        public abstract string? Keywords { get; set; }
        public abstract string? Language { get; set; }
        public abstract string? LastModifiedBy { get; set; }
        public abstract DateTime? LastPrinted { get; set; }
        public abstract DateTime? Modified { get; set; }
        public abstract string? Revision { get; set; }
        public abstract string? Subject { get; set; }
        public abstract string? Title { get; set; }
        public abstract string? Version { get; set; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }
    }

    public partial class PackageRelationship
    {
        public string Id { get { throw null; } }

        public Package Package { get { throw null; } }

        public string RelationshipType { get { throw null; } }

        public Uri SourceUri { get { throw null; } }

        public TargetMode TargetMode { get { throw null; } }

        public Uri TargetUri { get { throw null; } }
    }

    public partial class PackageRelationshipCollection : Collections.Generic.IEnumerable<PackageRelationship>, Collections.IEnumerable
    {
        public Collections.Generic.IEnumerator<PackageRelationship> GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class PackageRelationshipSelector
    {
        public PackageRelationshipSelector(Uri sourceUri, PackageRelationshipSelectorType selectorType, string selectionCriteria) { }

        public string SelectionCriteria { get { throw null; } }

        public PackageRelationshipSelectorType SelectorType { get { throw null; } }

        public Uri SourceUri { get { throw null; } }

        public Collections.Generic.List<PackageRelationship> Select(Package package) { throw null; }
    }

    public enum PackageRelationshipSelectorType
    {
        Id = 0,
        Type = 1
    }

    public static partial class PackUriHelper
    {
        public static readonly string UriSchemePack;
        public static int ComparePackUri(Uri? firstPackUri, Uri? secondPackUri) { throw null; }

        public static int ComparePartUri(Uri? firstPartUri, Uri? secondPartUri) { throw null; }

        public static Uri Create(Uri packageUri, Uri? partUri, string? fragment) { throw null; }

        public static Uri Create(Uri packageUri, Uri? partUri) { throw null; }

        public static Uri Create(Uri packageUri) { throw null; }

        public static Uri CreatePartUri(Uri partUri) { throw null; }

        public static Uri GetNormalizedPartUri(Uri partUri) { throw null; }

        public static Uri GetPackageUri(Uri packUri) { throw null; }

        public static Uri? GetPartUri(Uri packUri) { throw null; }

        public static Uri GetRelationshipPartUri(Uri partUri) { throw null; }

        public static Uri GetRelativeUri(Uri sourcePartUri, Uri targetPartUri) { throw null; }

        public static Uri GetSourcePartUriFromRelationshipPartUri(Uri relationshipPartUri) { throw null; }

        public static bool IsRelationshipPartUri(Uri partUri) { throw null; }

        public static Uri ResolvePartUri(Uri sourcePartUri, Uri targetUri) { throw null; }
    }

    public enum TargetMode
    {
        Internal = 0,
        External = 1
    }

    public sealed partial class ZipPackage : Package
    {
        internal ZipPackage() : base (default(FileAccess)) { }

        protected override PackagePart CreatePartCore(Uri partUri, string contentType, CompressionOption compressionOption) { throw null; }

        protected override void DeletePartCore(Uri partUri) { }

        protected override void Dispose(bool disposing) { }

        protected override void FlushCore() { }

        protected override PackagePart? GetPartCore(Uri partUri) { throw null; }

        protected override PackagePart[] GetPartsCore() { throw null; }
    }

    public sealed partial class ZipPackagePart : PackagePart
    {
        internal ZipPackagePart() : base(default(Package), default(Uri)) { }

        protected override Stream? GetStreamCore(FileMode streamFileMode, FileAccess streamFileAccess) { throw null; }
    }
}