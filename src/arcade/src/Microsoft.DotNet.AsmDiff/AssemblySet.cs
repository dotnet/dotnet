// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Cci;
using Microsoft.Cci.Extensions;

namespace Microsoft.DotNet.AsmDiff
{
    public sealed class AssemblySet : IEnumerable<IAssembly>, IDisposable
    {
        private AssemblySet(IMetadataHost metadataHost, IEnumerable<IAssembly> includedAssemblies, string name)
        {
            // We want to ensure a few things here:
            //
            // (1) We want the same assembly only once
            // (2) We want the assemblies sorted by identity
            // (3) We want a snapshot
            // (4) We want all assembly references in the host to be resolved.
            //     We don't care whether they can be resolved or not -- we only
            //     care that resolution has been finished so that LoadedUnits
            //     in the host contains all assemblies.

            var includedAssemblySet = new HashSet<IAssembly>(includedAssemblies);
            var includedAssembliesSorted = includedAssemblySet.OrderByIdentity()
                                                              .ToArray();

            // Ensure all assembly references are resolved

            if (metadataHost != null)
                ResolveAllAssemblies(metadataHost);

            var dependencies =
                ClassifyReferences(metadataHost, includedAssemblySet)
                    .Where(a => a.Classification == AssemblyClassification.Dependency ||
                                a.Classification == AssemblyClassification.NonRequiredDependency)
                    .Select(a => a.Reference.ResolvedAssembly)
                    .OrderByIdentity()
                    .ToArray();

            IsEmpty = includedAssembliesSorted.Length == 0 && dependencies.Length == 0;
            Host = metadataHost;
            Assemblies = new ReadOnlyCollection<IAssembly>(includedAssembliesSorted);
            Dependencies = new ReadOnlyCollection<IAssembly>(dependencies);
            Name = name;
        }

        private static void ResolveAllAssemblies(IMetadataHost metadataHost)
        {
            var processedSet = new HashSet<IAssemblyReference>();
            var queue = new Queue<IAssemblyReference>(metadataHost.LoadedUnits.OfType<IAssemblyReference>());

            while (queue.Count > 0)
            {
                var assembly = queue.Dequeue();
                if (!processedSet.Add(assembly))
                    continue;

                var resolved = assembly.ResolvedAssembly;
                foreach (var reference in resolved.AssemblyReferences)
                    queue.Enqueue(reference);
            }
        }

        private static AssemblySet FromAssemblies(IMetadataHost metadataHost, IAssembly[] assemblies, string name)
        {
            // We shouldn't return Empty if an explicit name was provided. Otherwise the name is lost.
            if (string.IsNullOrEmpty(name) && (assemblies == null || assemblies.Length == 0))
                return Empty;

            if (name == null)
            {
                var firstLocation = assemblies.Select(a => a.Location).First();
                name = assemblies.Length == 1
                           ? firstLocation
                           : Path.GetDirectoryName(firstLocation);
            }

            return new AssemblySet(metadataHost, assemblies, name);
        }

        public static AssemblySet FromPaths(string name, params string[] paths)
        {
            return FromPaths(paths.AsEnumerable(), name);
        }

        public static AssemblySet FromPaths(IEnumerable<string> paths, string name)
        {
            if (paths == null)
                return Empty;

            var environment = new HostEnvironment();

            // TODO: That's not necessarily great. We may want to expose a setting for that.
            environment.UnifyToLibPath = true;

            // We want to support path separators here, such as D:\foo.dll;D:\bar.dll
            var allPaths = paths.SelectMany(HostEnvironment.SplitPaths).ToArray();

            var assemblyArray = environment.LoadAssemblies(allPaths).ToArray();

            // We want to support cases where the paths don't point to valid assemblies.
            // In that case, we still want to use the supplied paths to generate a
            // meaningful name.

            if (string.IsNullOrEmpty(name) && assemblyArray.Length == 0 && allPaths.Any())
            {
                var firstPath = allPaths.First();
                name = allPaths.Length == 1
                           ? firstPath
                           : Path.GetDirectoryName(firstPath);
            }

            return FromAssemblies(environment, assemblyArray, name);
        }

        public AssemblySet WithAssemblies(IEnumerable<IAssembly> assemblies)
        {
            var assembliesSnapshot = GetSnapshotAndVerifyAssembliesAreInTheSameHost(assemblies);
            return new AssemblySet(Host, assembliesSnapshot, Name);
        }

        public AssemblySet Remove(IEnumerable<IAssembly> assemblies)
        {
            var snapshot = assemblies.ToArray();
            var assemblyPaths = Assemblies.Except(snapshot).Select(a => a.Location).ToArray();
            var dependencyPaths = Dependencies.Except(snapshot).Select(a => a.Location).ToArray();
            var allPaths = assemblyPaths.Union(dependencyPaths);
            var newAssemblySet = FromPaths(allPaths, Name);
            var newAssemblies = assemblyPaths.Select(p => newAssemblySet.Host.LoadUnitFrom(p)).OfType<IAssembly>();
            return newAssemblySet.WithAssemblies(newAssemblies);
        }

        private IEnumerable<IAssembly> GetSnapshotAndVerifyAssembliesAreInTheSameHost(IEnumerable<IAssembly> assemblies)
        {
            var assembliesSnapshot = assemblies.ToArray();
            var loadedAssemblies = Host == null
                                       ? Enumerable.Empty<IAssembly>()
                                       : Host.LoadedUnits.OfType<IAssembly>();
            var loadedAssembliesSet = new HashSet<IAssembly>(loadedAssemblies);

            if (assembliesSnapshot.Any() && loadedAssembliesSet.Any())
            {
                if (assembliesSnapshot.Any(a => !loadedAssembliesSet.Contains(a)))
                    throw new ArgumentException("assemblies", "Assemblies must only contain assemblies loaded in the current host");
            }

            return assembliesSnapshot;
        }

        private static IEnumerable<IAssembly> GetRequiredAssemblies(IEnumerable<IAssembly> assemblies)
        {
            var queue = new Queue<IAssemblyReference>(assemblies);
            var processedSet = new HashSet<IAssembly>();

            while (queue.Count > 0)
            {
                var reference = queue.Dequeue();
                var resolved = reference.ResolvedAssembly;
                if (resolved is Dummy || !processedSet.Add(resolved))
                    continue;

                foreach (var assemblyReference in resolved.AssemblyReferences)
                    queue.Enqueue(assemblyReference);
            }

            return processedSet.OrderByIdentity();
        }

        private static IEnumerable<ClassifiedAssembly> ClassifyReferences(IMetadataHost host, IEnumerable<IAssembly> includedAssemblies)
        {
            if (host == null)
                return Enumerable.Empty<ClassifiedAssembly>();

            var includedAssemblySet = new HashSet<IAssembly>(includedAssemblies);
            var loadedAssemblies = host.LoadedUnits.OfType<IAssembly>()
                                       .Where(a => !(a is Dummy))
                                       .ToArray();

            var requiredAssemblySet = new HashSet<IAssembly>(GetRequiredAssemblies(includedAssemblySet));

            // NOTE: Due to unification within our host, we need to make sure we only consider
            //       the unified reference. We do this by using the identitity of the resovled
            //       assembly, if it exists. For unresolved identities we use the identity of
            //       the reference.
            var allReferences = loadedAssemblies.Concat(loadedAssemblies.SelectMany(a => a.AssemblyReferences))
                                                .Select(r => r.ResolvedAssembly.IsDummy() ? r : r.ResolvedAssembly);

            var processedIdentities = new HashSet<AssemblyIdentity>();

            return from reference in allReferences
                   where processedIdentities.Add(reference.AssemblyIdentity)
                   let resolved = reference.ResolvedAssembly
                   let classification = resolved is Dummy
                                            ? AssemblyClassification.MissingDependency
                                            : includedAssemblySet.Contains(resolved)
                                                  ? AssemblyClassification.Included
                                                  : requiredAssemblySet.Contains(resolved)
                                                        ? AssemblyClassification.Dependency
                                                        : AssemblyClassification.NonRequiredDependency
                   orderby reference.Name.Value,
                           reference.GetPublicKeyToken(),
                           reference.Version
                   select new ClassifiedAssembly(reference, classification);
        }

        public static readonly AssemblySet Empty = new AssemblySet(null, Enumerable.Empty<IAssembly>(), "Empty");

        public bool IsEmpty { get; private set; }

        public bool IsNull
        {
            get { return ReferenceEquals(this, Empty); }
        }

        public IMetadataHost Host { get; private set; }

        public string Name { get; private set; }

        public ReadOnlyCollection<IAssembly> Assemblies { get; private set; }

        public ReadOnlyCollection<IAssembly> Dependencies { get; private set; }

        public void Dispose()
        {
            var disposableHost = Host as IDisposable;
            if (disposableHost != null)
                disposableHost.Dispose();
        }

        public IEnumerator<IAssembly> GetEnumerator()
        {
            return Assemblies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
