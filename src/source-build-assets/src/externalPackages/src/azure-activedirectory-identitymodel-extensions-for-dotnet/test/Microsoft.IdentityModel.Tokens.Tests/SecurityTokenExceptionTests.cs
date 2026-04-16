// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
#if NET8_0_OR_GREATER
using System.Text.Json;
#else
using System.Runtime.Serialization.Formatters.Binary;
#endif
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class SecurityTokenExceptionTests
    {
        [Theory, MemberData(nameof(ExceptionTestData))]
        public void SecurityTokenExceptionSerializationTests(SecurityTokenExceptionTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.{nameof(SecurityTokenExceptionSerializationTests)}", theoryData);

            try
            {
                var exception = (Exception)Activator.CreateInstance(theoryData.ExceptionType);
                theoryData.ExceptionSetter?.Invoke(exception);

                var memoryStream = new MemoryStream();

#if NET8_0_OR_GREATER
                var serializerOptions = new JsonSerializerOptions();
                serializerOptions.Converters.Add(new SecurityKeyConverterWithTypeDiscriminator());

                JsonSerializer.Serialize(memoryStream, exception, theoryData.ExceptionType, serializerOptions);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var serializedException = JsonSerializer.Deserialize(memoryStream, theoryData.ExceptionType, serializerOptions);
#else
                BinaryFormatter formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                formatter.Serialize(memoryStream, exception);
#pragma warning restore SYSLIB0011 // Type or member is obsolete

                memoryStream.Seek(0, SeekOrigin.Begin);

                formatter.Binder = new ExceptionSerializationBinder();
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var serializedException = formatter.Deserialize(memoryStream);
#pragma warning restore SYSLIB0011 // Type or member is obsolete

#endif
                theoryData.ExpectedException.ProcessNoException(context);

                IdentityComparer.AreEqual(exception, serializedException, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SecurityTokenExceptionTheoryData> ExceptionTestData
        {
            get
            {
                return new TheoryData<SecurityTokenExceptionTheoryData>
                {
                    new SecurityTokenExceptionTheoryData
                    {
                        First = true,
                        TestId = "SecurityTokenInvalidAudienceExceptionSerializesProperties",
                        ExceptionType = typeof(SecurityTokenInvalidAudienceException),
                        ExceptionSetter = (ex) =>
                        {
                            if (!(ex is SecurityTokenInvalidAudienceException securityTokenInvalidAudienceException))
                                throw new ArgumentException($"expected argument of type {nameof(SecurityTokenInvalidAudienceException)} received type {ex.GetType()}");

                            securityTokenInvalidAudienceException.InvalidAudience = Guid.NewGuid().ToString();
                        }
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenInvalidAudienceExceptionSerializesPropertiesDefaultValue",
                        ExceptionType = typeof(SecurityTokenInvalidAudienceException),
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenInvalidIssuerExceptionSerializesProperties",
                        ExceptionType = typeof(SecurityTokenInvalidIssuerException),
                        ExceptionSetter = (ex) =>
                        {
                            if (!(ex is SecurityTokenInvalidIssuerException securityTokenInvalidIssuerException))
                                throw new ArgumentException($"expected argument of type {nameof(SecurityTokenInvalidIssuerException)} received type {ex.GetType()}");

                            securityTokenInvalidIssuerException.InvalidIssuer = Guid.NewGuid().ToString();
                        }
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenInvalidIssuerExceptionSerializesPropertiesDefaultValue",
                        ExceptionType = typeof(SecurityTokenInvalidIssuerException),
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenExpiredExceptionSerializesProperties",
                        ExceptionType = typeof(SecurityTokenExpiredException),
                        ExceptionSetter = (ex) =>
                        {
                            if (!(ex is SecurityTokenExpiredException securityTokenExpiredException))
                                throw new ArgumentException($"expected argument of type {nameof(SecurityTokenExpiredException)} received type {ex.GetType()}");

                            securityTokenExpiredException.Expires = DateTime.Now;
                        }
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenExpiredExceptionSerializesPropertiesDefaultValue",
                        ExceptionType = typeof(SecurityTokenExpiredException),
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenInvalidLifetimeExceptionProperties",
                        ExceptionType = typeof(SecurityTokenInvalidLifetimeException),
                        ExceptionSetter = (ex) =>
                        {
                            if (!(ex is SecurityTokenInvalidLifetimeException securityTokenInvalidLifetimeException))
                                throw new ArgumentException($"expected argument of type {nameof(SecurityTokenInvalidLifetimeException)} received type {ex.GetType()}");

                            securityTokenInvalidLifetimeException.Expires = DateTime.Now;
                            securityTokenInvalidLifetimeException.NotBefore = DateTime.Now;
                        }
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenInvalidLifetimeExceptionPropertiesDefaultValue",
                        ExceptionType = typeof(SecurityTokenInvalidLifetimeException),
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenInvalidTypeExceptionSerializesProperties",
                        ExceptionType = typeof(SecurityTokenInvalidTypeException),
                        ExceptionSetter = (ex) =>
                        {
                            if (!(ex is SecurityTokenInvalidTypeException securityTokenInvalidTypeException))
                                throw new ArgumentException($"expected argument of type {nameof(SecurityTokenInvalidTypeException)} received type {ex.GetType()}");

                            securityTokenInvalidTypeException.InvalidType = Guid.NewGuid().ToString();
                        }
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenInvalidTypeExceptionSerializesPropertiesDefaultValue",
                        ExceptionType = typeof(SecurityTokenInvalidTypeException),
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenNotYetValidExceptionSerializesProperties",
                        ExceptionType = typeof(SecurityTokenNotYetValidException),
                        ExceptionSetter = (ex) =>
                        {
                            if (!(ex is SecurityTokenNotYetValidException securityTokenNotYetValidException))
                                throw new ArgumentException($"expected argument of type {nameof(SecurityTokenNotYetValidException)} received type {ex.GetType()}");

                            securityTokenNotYetValidException.NotBefore = DateTime.Now;
                        }
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenNotYetValidExceptionSerializesPropertiesDefaultValue",
                        ExceptionType = typeof(SecurityTokenNotYetValidException),
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenInvalidSigningKeyExceptionSerializesProperties",
                        ExceptionType = typeof(SecurityTokenInvalidSigningKeyException),
                        ExceptionSetter = (ex) =>
                        {
                            if (!(ex is SecurityTokenInvalidSigningKeyException securityTokenInvalidSigningKeyException))
                                throw new ArgumentException($"expected argument of type {nameof(SecurityTokenInvalidSigningKeyException)} received type {ex.GetType()}");

                            securityTokenInvalidSigningKeyException.SigningKey = new CustomSecurityKey();
                        },
                        PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                        {
                            [typeof(SecurityTokenInvalidSigningKeyException)] = new List<string>{ "SigningKey" }
                        }
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenInvalidSigningKeyExceptionSerializesPropertiesDefaultValue",
                        ExceptionType = typeof(SecurityTokenInvalidSigningKeyException),
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenInvalidAlgorithmSerializesProperties",
                        ExceptionType = typeof(SecurityTokenInvalidAlgorithmException),
                        ExceptionSetter = (ex) =>
                        {
                            if (!(ex is SecurityTokenInvalidAlgorithmException securityTokenInvalidAlgorithm))
                                throw new ArgumentException($"expected argument of type {nameof(SecurityTokenInvalidAlgorithmException)} received type {ex.GetType()}");

                            securityTokenInvalidAlgorithm.InvalidAlgorithm = Guid.NewGuid().ToString();
                        },
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenInvalidAlgorithmSerializesPropertiesDefaultValue",
                        ExceptionType = typeof(SecurityTokenInvalidAlgorithmException),
                    },
#pragma warning disable CS0618 // Type or member is obsolete
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenUnableToValidateExceptionDefaultValue",
                        ExceptionType = typeof(SecurityTokenUnableToValidateException),
                    },
                    new SecurityTokenExceptionTheoryData
                    {
                        TestId = "SecurityTokenUnableToValidateExceptionProperties",
                        ExceptionType = typeof(SecurityTokenUnableToValidateException),
                        ExceptionSetter = (ex) =>
                        {
                            if (!(ex is SecurityTokenUnableToValidateException securityTokenUnableToValidateException))
                                throw new ArgumentException($"expected argument of type {nameof(SecurityTokenUnableToValidateException)} received type {ex.GetType()}");

                            securityTokenUnableToValidateException.ValidationFailure = ValidationFailure.InvalidIssuer;
                            securityTokenUnableToValidateException.ValidationFailure |= ValidationFailure.InvalidLifetime;
                        },
                    },
#pragma warning restore CS0618 // Type or member is obsolete
                };
            }
        }    
    }

    public class SecurityTokenExceptionTheoryData : TheoryDataBase
    {
        public Type ExceptionType { get; set; }

        public Action<Exception> ExceptionSetter { get; set; }
    }

    public class ExceptionSerializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            // One way to discover expected types is through testing deserialization
            // of **valid** data and logging the types used.

            //Console.WriteLine($"BindToType('{assemblyName}', '{typeName}')");

            if (typeName == "Microsoft.IdentityModel.Tokens.SecurityTokenInvalidAudienceException" ||
                typeName == "Microsoft.IdentityModel.Tokens.SecurityTokenInvalidIssuerException" ||
                typeName == "Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException" ||
                typeName == "Microsoft.IdentityModel.Tokens.SecurityTokenInvalidLifetimeException" ||
                typeName == "Microsoft.IdentityModel.Tokens.SecurityTokenInvalidTypeException" ||
                typeName == "Microsoft.IdentityModel.Tokens.SecurityTokenNotYetValidException" ||
                typeName == "Microsoft.IdentityModel.Tokens.SecurityTokenInvalidSigningKeyException" ||
                typeName == "Microsoft.IdentityModel.Tokens.SecurityTokenInvalidAlgorithmException" ||
                typeName == "Microsoft.IdentityModel.Tokens.SecurityTokenUnableToValidateException" ||
                typeName == "Microsoft.IdentityModel.Tokens.ValidationFailure")
            {
                return null;
            }
            else
            {
                throw new ArgumentException("Unexpected type: ", nameof(typeName));
            }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant

