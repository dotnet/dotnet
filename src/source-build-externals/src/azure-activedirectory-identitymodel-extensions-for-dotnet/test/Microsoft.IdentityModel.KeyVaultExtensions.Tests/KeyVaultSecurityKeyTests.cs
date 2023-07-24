// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Reflection;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.KeyVaultExtensions.Tests
{
    public class KeyVaultSecurityKeyTests
    {
        private static ExpectedException ArgumentNullExceptionExpected = new ExpectedException(typeExpected: typeof(TargetInvocationException), substringExpected: "Exception has been thrown by the target of an invocation.", innerTypeExpected: typeof(ArgumentNullException));
        private static ExpectedException KeyVaultErrorExceptionExpected = new ExpectedException(typeExpected: typeof(TargetInvocationException), substringExpected: "Exception has been thrown by the target of an invocation.", innerTypeExpected: typeof(KeyVaultErrorException));

        [Theory, MemberData(nameof(KeyVaultSecurityKeyAuthenticationCallbackTheoryData))]
        public void AuthenticationCallbackConstructorParams(KeyVaultSecurityKeyAuthenticationCallbackTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.AuthenticationCallbackConstructorParams", theoryData);

            try
            {
                _ = Activator.CreateInstance(theoryData.Type, new object[] { theoryData.KeyIdentifier, theoryData.Callback });
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
        }

        public static TheoryData<KeyVaultSecurityKeyAuthenticationCallbackTheoryData> KeyVaultSecurityKeyAuthenticationCallbackTheoryData
        {
            get => new TheoryData<KeyVaultSecurityKeyAuthenticationCallbackTheoryData>
            {
                new KeyVaultSecurityKeyAuthenticationCallbackTheoryData
                {
                    // Callback = default,
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    First = true,
                    KeyIdentifier = null,
                    TestId = typeof(KeyVaultSecurityKey).FullName,
                    Type = typeof(KeyVaultSecurityKey),
                },
                new KeyVaultSecurityKeyAuthenticationCallbackTheoryData
                {
                    // Callback = default,
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    KeyIdentifier = string.Empty,
                    TestId = typeof(KeyVaultSecurityKey).FullName,
                    Type = typeof(KeyVaultSecurityKey),
                },
                new KeyVaultSecurityKeyAuthenticationCallbackTheoryData
                {
                    Callback = null,
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    // KeyIdentifier = default,
                    TestId = typeof(KeyVaultSecurityKey).FullName,
                    Type = typeof(KeyVaultSecurityKey),
                },
                new KeyVaultSecurityKeyAuthenticationCallbackTheoryData
                {
                    // Callback = default,
                    ExpectedException = KeyVaultErrorExceptionExpected,
                    // KeyIdentifier = default,
                    TestId = typeof(KeyVaultSecurityKey).FullName,
                    Type = typeof(KeyVaultSecurityKey),
                },
            };
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
