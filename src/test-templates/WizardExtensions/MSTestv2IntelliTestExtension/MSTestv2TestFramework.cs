// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MSTestv2IntelliTestExtension
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ExtendedReflection.Asserts;
    using Microsoft.ExtendedReflection.Collections;
    using Microsoft.ExtendedReflection.Metadata;
    using Microsoft.ExtendedReflection.Metadata.Names;
    using Microsoft.ExtendedReflection.Monitoring;
    using Microsoft.ExtendedReflection.Utilities.Safe.Diagnostics;
    using Microsoft.Pex.Engine.ComponentModel;
    using Microsoft.Pex.Engine.TestFrameworks;

    /// <summary>
    /// MSTestv2 test framework
    /// </summary>
    [Serializable]
    public class MSTestv2TestFramework : AttributeBasedTestFrameworkBase
    {
        private static readonly bool Net2process = typeof(object).Assembly.GetName().Version.Major == 2;

        [NonSerialized]
        private TypeName assemblySetUpAttribute;

        /// <summary>
        /// The expected exception attribute.
        /// </summary>
        [NonSerialized]
        private TypeName expectedExceptionAttribute;

        [NonSerialized]
        private TypeName assemblyTearDownAttribute;

        [NonSerialized]
        private TypeName fixtureAttribute;

        [NonSerialized]
        private TypeName fixtureSetUpAttribute;

        [NonSerialized]
        private TypeName fixtureTearDownAttribute;

        [NonSerialized]
        private TypeName setUpAttribute;

        [NonSerialized]
        private TypeName testAttribute;

        [NonSerialized]
        private TypeName tearDownAttribute;

        [NonSerialized]
        private TypeName ignoreAttribute;

        [NonSerialized]
        private TypeName assertionExceptionType;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSTestv2TestFramework"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public MSTestv2TestFramework(IPexComponent host)
            : base(host)
        {
        }

        /// <summary>
        /// Gets identify of the test framework
        /// </summary>
        public override string Name
        {
            get { return "MSTestv2"; }
        }

        /// <summary>
        /// Gets the assembly name of the framework main's assembly. This name is used
        /// to automatically discover test frameworks, based the assembly references
        /// </summary>
        public override ShortAssemblyName AssemblyName
        {
            get { return MSTestv2TestFrameworkMetadata.AssemblyName; }
        }

        /// <summary>
        /// Gets the root namespace.
        /// </summary>
        /// <value>The root namespace.</value>
        public override string RootNamespace
        {
            get { return MSTestv2TestFrameworkMetadata.RootNamespace; }
        }

        /// <summary>
        /// Gets the adapter and test framework references.
        /// </summary>
        public override ICountable<ShortReferenceAssemblyName> References
        {
            get
            {
                return Indexable.Two(
                    new ShortReferenceAssemblyName(ShortAssemblyName.FromName("MSTest.TestAdapter"), "2.2.10", AssemblyReferenceType.NugetReference),
                    new ShortReferenceAssemblyName(ShortAssemblyName.FromName("MSTest.TestFramework"), "2.2.10", AssemblyReferenceType.NugetReference));
            }
        }

        /// <summary>
        /// Gets a value indicating whether
        /// partial test classes
        /// </summary>
        public override bool SupportsPartialClasses
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the ExpectedException attribute.
        /// </summary>
        /// <value>The expected exception attribute.</value>
        public override TypeName ExpectedExceptionAttribute
        {
            get
            {
                if (this.expectedExceptionAttribute == null)
                {
                    this.expectedExceptionAttribute = MSTestv2TestFrameworkMetadata.AttributeName("ExpectedException");
                }

                return this.expectedExceptionAttribute;
            }
        }

        /// <summary>
        /// Gets the assembly set up attribute.
        /// </summary>
        /// <value>The assembly set up attribute.</value>
        public TypeName AssemblySetUpAttribute
        {
            get
            {
                if (this.assemblySetUpAttribute == null)
                {
                    this.assemblySetUpAttribute = MSTestv2TestFrameworkMetadata.AttributeName("AssemblyInitialize");
                }

                return this.assemblySetUpAttribute;
            }
        }

        /// <summary>
        /// Gets the assembly tear down attribute.
        /// </summary>
        /// <value>The assembly tear down attribute.</value>
        public TypeName AssemblyTearDownAttribute
        {
            get
            {
                if (this.assemblyTearDownAttribute == null)
                {
                    this.assemblyTearDownAttribute = MSTestv2TestFrameworkMetadata.AttributeName("AssemblyCleanup");
                }

                return this.assemblyTearDownAttribute;
            }
        }

        /// <summary>
        /// Gets a value indicating whether fixture set up tear down are instance methods.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [fixture set up tear down instance]; otherwise, <c>false</c>.
        /// </value>
        public override bool FixtureSetupTeardownInstance
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the name of the fixture attribute.
        /// </summary>
        /// <value>The fixture attribute.</value>
        public override TypeName FixtureAttribute
        {
            get
            {
                if (this.fixtureAttribute == null)
                {
                    this.fixtureAttribute = MSTestv2TestFrameworkMetadata.AttributeName("TestClass");
                }

                return this.fixtureAttribute;
            }
        }

        /// <summary>
        /// Gets the name of the fixture setup attribute
        /// </summary>
        /// <value>The fixture set up attribute.</value>
        public override TypeName FixtureSetupAttribute
        {
            get
            {
                if (this.fixtureSetUpAttribute == null)
                {
                    this.fixtureSetUpAttribute = MSTestv2TestFrameworkMetadata.AttributeName("ClassInitialize");
                }

                return this.fixtureSetUpAttribute;
            }
        }

        /// <summary>
        /// Gets the name of the fixture teardown attribute
        /// </summary>
        /// <value>The fixture tear down attribute.</value>
        public override TypeName FixtureTeardownAttribute
        {
            get
            {
                if (this.fixtureTearDownAttribute == null)
                {
                    this.fixtureTearDownAttribute = MSTestv2TestFrameworkMetadata.AttributeName("ClassCleanup");
                }

                return this.fixtureTearDownAttribute;
            }
        }

        /// <summary>
        /// Gets the name of the test setup attribute.
        /// </summary>
        /// <value>The set up attribute.</value>
        public override TypeName SetupAttribute
        {
            get
            {
                if (this.setUpAttribute == null)
                {
                    this.setUpAttribute = MSTestv2TestFrameworkMetadata.AttributeName("TestInitialize");
                }

                return this.setUpAttribute;
            }
        }

        /// <summary>
        /// Gets the name of the test attribute.
        /// </summary>
        /// <value>The set up attribute.</value>
        public override TypeName TestAttribute
        {
            get
            {
                if (this.testAttribute == null)
                {
                    this.testAttribute = MSTestv2TestFrameworkMetadata.AttributeName("TestMethod");
                }

                return this.testAttribute;
            }
        }

        /// <summary>
        /// Gets the name of the test teardown attribute.
        /// </summary>
        /// <value>The tear down attribute.</value>
        public override TypeName TeardownAttribute
        {
            get
            {
                if (this.tearDownAttribute == null)
                {
                    this.tearDownAttribute = MSTestv2TestFrameworkMetadata.AttributeName("TestCleanup");
                }

                return this.tearDownAttribute;
            }
        }

        /// <summary>
        /// Gets the ignore attribute.
        /// </summary>
        /// <value>The ignore attribute.</value>
        public override TypeName IgnoreAttribute
        {
            get
            {
                if (this.ignoreAttribute == null)
                {
                    this.ignoreAttribute = MSTestv2TestFrameworkMetadata.AttributeName("Ignore");
                }

                return this.ignoreAttribute;
            }
        }

        /// <summary>
        /// Gets the type of the assertion exception.
        /// </summary>
        /// <value>The type of the assertion exception.</value>
        public override TypeName AssertionExceptionType
        {
            get
            {
                if (this.assertionExceptionType == null)
                {
                    this.assertionExceptionType = MSTestv2TestFrameworkMetadata.AttributeName("AssertFailedException");
                }

                return this.assertionExceptionType;
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets a value that indicates if this framework support static unit tests
        /// </summary>
        public override bool SupportsStaticTestMethods
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the assert method filters.
        /// </summary>
        public override IIndexable<IAssertMethodFilter> AssertMethodFilters
        {
            get
            {
                return Indexable.One<IAssertMethodFilter>(MSTestv2AssertMethodFilter.Instance);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the ignore attribute constructor takes a message as its first argument.
        /// </summary>
        protected override bool HasIgnoreAttributeMessage
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the ignore message property.
        /// </summary>
        /// <value>The ignore message property.</value>
        protected override string IgnoreMessageProperty
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the expected exception property name.
        /// </summary>
        /// <value>The expected exception property.</value>
        protected override string ExpectedExceptionProperty
        {
            get { return "ExceptionType"; }
        }

        /// <summary>
        /// Gets a value indicating if the bitness is supported
        /// </summary>
        /// <param name="bitness">The bitness.</param>
        /// <returns>True if supported.</returns>
        public override bool SupportsProjectBitness(Bitness bitness)
        {
            SafeDebug.Assume(bitness != Bitness.Unsupported, "bitness != Bitness.Unsupported");
            if (Net2process)
            {
                return bitness == Bitness.AnyCpu || bitness == Bitness.x86;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Tries to get the assembly set up tear down attribute.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="setUp">The set up.</param>
        /// <param name="tearDown">The tear down.</param>
        /// <returns>True if found.</returns>
        public override bool TryGetAssemblySetupTeardownMethods(
            AssemblyEx assembly,
            out Method setUp,
            out Method tearDown)
        {
            // <preconditions>
            SafeDebug.AssumeNotNull((object)assembly, "assembly");

            // </preconditions>
            setUp = tearDown = null;

            // look for [TestClass] types
            foreach (var typeDefinition in assembly.TypeDefinitions)
            {
                if (typeDefinition.IsVisible(VisibilityContext.Exported))
                {
                    if (typeDefinition.IsEnumType ||
                        typeDefinition.GenericTypeParameters.Length > 0)
                    {
                        continue;
                    }

                    if (!this.IsFixture(typeDefinition))
                    {
                        continue;
                    }

                    // looking for assembly setup/teardown methods
                    foreach (var methodDefinition in typeDefinition.DeclaredStaticMethods)
                    {
                        if (methodDefinition.IsVisible(VisibilityContext.Exported) &&
                            methodDefinition.GenericMethodParameters.Length == 0)
                        {
                            if (AttributeHelper.IsDefined(methodDefinition, this.AssemblySetUpAttribute, true))
                            {
                                setUp = methodDefinition.Instantiate(TypeEx.NoTypes, TypeEx.NoTypes);
                            }

                            if (AttributeHelper.IsDefined(methodDefinition, this.AssemblyTearDownAttribute, true))
                            {
                                tearDown = methodDefinition.Instantiate(TypeEx.NoTypes, TypeEx.NoTypes);
                            }

                            // nothing else to look for
                            if (setUp != null && tearDown != null)
                            {
                                break;
                            }
                        }
                    }

                    // nothing else to look for
                    if (setUp != null && tearDown != null)
                    {
                        break;
                    }
                }
            }

            return setUp != null || tearDown != null;
        }

        /// <summary>
        /// Gets a list of attribute that should be duplicated from the
        /// pex test to the parameterized test
        /// </summary>
        /// <returns>Attribute types.</returns>
        protected override IEnumerable<TypeName> GetSatelliteAttributeTypes()
        {
            return Indexable.Array(
                    MSTestv2TestFrameworkMetadata.AttributeName("DeploymentItem"),
                    MSTestv2TestFrameworkMetadata.AttributeName("Owner"),
                    MSTestv2TestFrameworkMetadata.AttributeName("Priority"),
                    MSTestv2TestFrameworkMetadata.AttributeName("TestProperty"),
                    MSTestv2TestFrameworkMetadata.AttributeName("Timeout"),
                    MSTestv2TestFrameworkMetadata.AttributeName("WorkItem"),
                    MSTestv2TestFrameworkMetadata.AttributeName("CssIteration"),
                    MSTestv2TestFrameworkMetadata.AttributeName("CssProjectStructureAttributeCtor"));
        }

        /// <summary>
        /// Tries to query the categories.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="names">The names.</param>
        /// <returns>True if found.</returns>
        protected override bool TryGetCategories(
            ICustomAttributeProviderEx element,
            out IEnumerable<string> names)
        {
            names = null;
            return false;
        }
    }
}
