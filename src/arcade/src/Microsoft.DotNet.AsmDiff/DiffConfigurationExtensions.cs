// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Cci;

namespace Microsoft.DotNet.AsmDiff
{
    public static class DiffConfigurationExtensions
    {
        // Options

        public static DiffConfiguration UpdateOptions(this DiffConfiguration configuration, DiffConfigurationOptions options)
        {
            var left = configuration.Left;
            var right = configuration.Right;
            return new DiffConfiguration(left, right, options);
        }

        public static bool IsOptionSet(this DiffConfiguration configuration, DiffConfigurationOptions option)
        {
            return option == DiffConfigurationOptions.None
                       ? configuration.Options == DiffConfigurationOptions.None
                       : (configuration.Options & option) == option;
        }

        public static DiffConfiguration SetOption(this DiffConfiguration configuration, DiffConfigurationOptions option, bool set)
        {
            var newOptions = set
                                 ? configuration.Options | option
                                 : configuration.Options & ~option;
            return configuration.UpdateOptions(newOptions);
        }

        // Add assemblies

        public static DiffConfiguration AddLeftAssemblies(this DiffConfiguration configuration, IEnumerable<string> addedPaths)
        {
            return configuration.AddAssemblies(true, addedPaths);
        }

        public static DiffConfiguration AddRightAssemblies(this DiffConfiguration configuration, IEnumerable<string> addedPaths)
        {
            return configuration.AddAssemblies(false, addedPaths);
        }

        private static DiffConfiguration AddAssemblies(this DiffConfiguration configuration, bool isLeft, IEnumerable<string> addedPaths)
        {
            var existingSet = isLeft
                                  ? configuration.Left
                                  : configuration.Right;
            var existingAssemblyPaths = from a in existingSet.Assemblies
                                        select a.Location;

            var totalAssemblyPaths = existingAssemblyPaths.Concat(addedPaths).ToArray();
            var newName = existingSet.IsEmpty ? null : existingSet.Name;

            return configuration.UpdateAssemblies(isLeft, totalAssemblyPaths, newName);
        }

        // Remove assemblies

        public static DiffConfiguration RemoveLeftAssemblies(this DiffConfiguration configuration, IEnumerable<IAssembly> removals)
        {
            return configuration.RemoveAssemblies(true, removals);
        }

        public static DiffConfiguration RemoveRightAssemblies(this DiffConfiguration configuration, IEnumerable<IAssembly> removals)
        {
            return configuration.RemoveAssemblies(false, removals);
        }

        private static DiffConfiguration RemoveAssemblies(this DiffConfiguration configuration, bool isLeft, IEnumerable<IAssembly> removals)
        {
            var existingSet = isLeft
                                  ? configuration.Left
                                  : configuration.Right;
            var newSet = existingSet.Remove(removals);
            return configuration.UpdateAssemblies(isLeft, newSet);
        }

        // Update assemblies

        public static DiffConfiguration UpdateLeftAssemblies(this DiffConfiguration configuration, IEnumerable<string> paths)
        {
            return configuration.UpdateLeftAssemblies(paths, null);
        }

        public static DiffConfiguration UpdateLeftAssemblies(this DiffConfiguration configuration, IEnumerable<string> paths, string name)
        {
            return configuration.UpdateAssemblies(true, paths, name);
        }

        public static DiffConfiguration UpdateLeftAssemblies(this DiffConfiguration configuration, AssemblySet assemblySet)
        {
            return configuration.UpdateAssemblies(true, assemblySet);
        }

        public static DiffConfiguration UpdateRightAssemblies(this DiffConfiguration configuration, IEnumerable<string> paths)
        {
            return configuration.UpdateRightAssemblies(paths, null);
        }

        public static DiffConfiguration UpdateRightAssemblies(this DiffConfiguration configuration, IEnumerable<string> paths, string name)
        {
            return configuration.UpdateAssemblies(false, paths, name);
        }

        public static DiffConfiguration UpdateRightAssemblies(this DiffConfiguration configuration, AssemblySet assemblySet)
        {
            return configuration.UpdateAssemblies(false, assemblySet);
        }

        private static DiffConfiguration UpdateAssemblies(this DiffConfiguration configuration, bool isLeft, IEnumerable<string> newPaths, string newName)
        {
            var assemblySet = AssemblySet.FromPaths(newPaths, newName);
            return configuration.UpdateAssemblies(isLeft, assemblySet);
        }

        private static DiffConfiguration UpdateAssemblies(this DiffConfiguration configuration, bool isLeft, AssemblySet assemblySet)
        {
            var isRight = !isLeft;
            var existingSet = isLeft
                                  ? configuration.Left
                                  : configuration.Right;

            // This is the a debatible thing here.
            //
            // Without Dipose(), instances of DiffConfiguration would be fully immutable, which means
            // we could pass it around to different threads and you could be sure that no than is stepping
            // on your toes.
            //
            // However, this also means we would only unlock the files on disk when the GC collects the
            // the underlying metadata reader host. This means, nobody can open the files exlusively or
            // deleting them until they are collected.
            //
            // A workaround for the user is to simply close the app but I feel the workaround feels really
            // bad. Especially because most of the assemblies being added to the tool will come from temp
            // folders on the desktop.
            //
            // Since our apps is blocking adding/removing files when an analysis is running, there are
            // no real advantages of full immutable sharing.

            existingSet.Dispose();

            var newLeft = isLeft ? assemblySet : configuration.Left;
            var newRight = isRight ? assemblySet : configuration.Right;

            return new DiffConfiguration(newLeft, newRight, configuration.Options);
        }
    }
}
