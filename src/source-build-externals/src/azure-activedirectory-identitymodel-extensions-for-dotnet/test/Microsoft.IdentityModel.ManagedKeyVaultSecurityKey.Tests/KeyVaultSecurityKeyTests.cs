// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.KeyVaultExtensions;
using Microsoft.IdentityModel.KeyVaultExtensions.Tests;
using Microsoft.IdentityModel.TestUtils;
using System;
using System.Reflection;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.ManagedKeyVaultSecurityKey.Tests
{
    public class KeyVaultSecurityKeyTests
    {
        private static ExpectedException AdalServiceExceptionExpected = new ExpectedException(typeExpected: typeof(TargetInvocationException), substringExpected: "Exception has been thrown by the target of an invocation.", innerTypeExpected: typeof(AdalServiceException));
        private static ExpectedException ArgumentNullExceptionExpected = new ExpectedException(typeExpected: typeof(TargetInvocationException), substringExpected: "Exception has been thrown by the target of an invocation.", innerTypeExpected: typeof(ArgumentNullException));
        private static ExpectedException KeyVaultErrorExceptionExpected = new ExpectedException(typeExpected: typeof(TargetInvocationException), substringExpected: "Exception has been thrown by the target of an invocation.", innerTypeExpected: typeof(KeyVaultErrorException));

        [Theory, MemberData(nameof(KeyVaultSecurityKeyConfidentialClientTheoryData))]
        public void ConfidentialClientConstructorParams(KeyVaultSecurityKeyConfidentialClientTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ConfidentialClientConstructorParams", theoryData);

            try
            {
                _ = Activator.CreateInstance(theoryData.Type, new object[] { theoryData.KeyIdentifier, theoryData.ClientId, theoryData.ClientSecret });
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<KeyVaultSecurityKeyConfidentialClientTheoryData> KeyVaultSecurityKeyConfidentialClientTheoryData
        {
            get => new TheoryData<KeyVaultSecurityKeyConfidentialClientTheoryData>
            {
                new KeyVaultSecurityKeyConfidentialClientTheoryData
                {
                    // ClientId = default,
                    // ClientSecret = default,
                    ExpectedException = ArgumentNullExceptionExpected,
                    First = true,
                    KeyIdentifier = null,
                    TestId = "Test1",
                    Type = typeof(ManagedKeyVaultSecurityKey),
                },
                new KeyVaultSecurityKeyConfidentialClientTheoryData
                {
                    // ClientId = default,
                    // ClientSecret = default,
                    ExpectedException = ArgumentNullExceptionExpected,
                    KeyIdentifier = string.Empty,
                    TestId = "Test2",
                    Type = typeof(ManagedKeyVaultSecurityKey),
                },
                new KeyVaultSecurityKeyConfidentialClientTheoryData
                {
                    ClientId = null,
                    // ClientSecret = default,
                    ExpectedException = ArgumentNullExceptionExpected,
                    // KeyIdentifier = default,
                    TestId = "Test3",
                    Type = typeof(ManagedKeyVaultSecurityKey),
                },
                new KeyVaultSecurityKeyConfidentialClientTheoryData
                {
                    ClientId = string.Empty,
                    /*
                    ClientSecret = default,
                    */
                    ExpectedException = ArgumentNullExceptionExpected,
                    // KeyIdentifier = default,
                    TestId = "Test4",
                    Type = typeof(ManagedKeyVaultSecurityKey),
                },
                new KeyVaultSecurityKeyConfidentialClientTheoryData
                {
                    // ClientId = default,
                    ClientSecret = null,
                    ExpectedException = ArgumentNullExceptionExpected,
                    // KeyIdentifier = default,
                    TestId = "Test5",
                    Type = typeof(ManagedKeyVaultSecurityKey),
                },
                new KeyVaultSecurityKeyConfidentialClientTheoryData
                {
                    // ClientId = default,
                    ClientSecret = string.Empty,
                    ExpectedException = ArgumentNullExceptionExpected,
                    // KeyIdentifier = default,
                    TestId = "Test6",
                    Type = typeof(ManagedKeyVaultSecurityKey),
                },
                new KeyVaultSecurityKeyConfidentialClientTheoryData
                {
                    // ClientId = default,
                    // ClientSecret = default,
                    // KeyIdentifier = default,
                    TestId = "Test7",
                    Type = typeof(ManagedKeyVaultSecurityKey),
                }
            };
        }

        [Theory, MemberData(nameof(KeyVaultSecurityKeyManagedServiceIdentityTheoryData))]
        public void ManagedServiceIdentityConstructorParams(KeyVaultSecurityKeyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ManagedServiceIdentityConstructorParams", theoryData);

            try
            {
                _ = Activator.CreateInstance(theoryData.Type, new object[] { theoryData.KeyIdentifier });
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
        }

        public static TheoryData<KeyVaultSecurityKeyManagedServiceIdentityTheoryData> KeyVaultSecurityKeyManagedServiceIdentityTheoryData
        {
            get => new TheoryData<KeyVaultSecurityKeyManagedServiceIdentityTheoryData>
            {
                new KeyVaultSecurityKeyManagedServiceIdentityTheoryData
                {
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    First = true,
                    KeyIdentifier = null,
                    TestId = "Test1",
                    Type = typeof(ManagedKeyVaultSecurityKey),
                },
                new KeyVaultSecurityKeyManagedServiceIdentityTheoryData
                {
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    KeyIdentifier = string.Empty,
                    TestId = "Test2",
                    Type = typeof(ManagedKeyVaultSecurityKey),
                },
                new KeyVaultSecurityKeyManagedServiceIdentityTheoryData
                {
                    ExpectedException = KeyVaultErrorExceptionExpected,
                    // KeyIdentifier = default,
                    TestId = "Test3",
                    Type = typeof(ManagedKeyVaultSecurityKey),
                },
            };
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
