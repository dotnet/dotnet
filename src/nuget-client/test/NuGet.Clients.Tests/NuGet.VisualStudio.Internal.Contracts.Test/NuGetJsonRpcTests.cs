// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Moq;
using StreamJsonRpc;
using StreamJsonRpc.Protocol;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public class NuGetJsonRpcTests
    {
        [Fact]
        public void GetErrorDetailsDataType_WhenErrorIsNull_CallsBase()
        {
            using (var rpc = new TestJsonRpc())
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                Type? type = rpc.GetErrorDetailsDataTypeHelper(error: null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                Assert.Equal(typeof(CommonErrorData), type);
            }
        }

        [Fact]
        public void GetErrorDetailsDataType_WhenErrorPropertyIsNull_CallsBase()
        {
            using (var rpc = new TestJsonRpc())
            {
                Type? type = rpc.GetErrorDetailsDataTypeHelper(
                    new JsonRpcError()
                    {
                        Error = null
                    });

                Assert.Equal(typeof(CommonErrorData), type);
            }
        }

        [Fact]
        public void GetErrorDetailsDataType_WhenErrorCodePropertyIsNotAMatch_CallsBase()
        {
            using (var rpc = new TestJsonRpc())
            {
                Type? type = rpc.GetErrorDetailsDataTypeHelper(
                    new JsonRpcError()
                    {
                        Error = new JsonRpcError.ErrorDetail()
                        {
                            Code = (JsonRpcErrorCode)int.MinValue
                        }
                    });

                Assert.Equal(typeof(CommonErrorData), type);
            }
        }

        [Fact]
        public void GetErrorDetailsDataType_WhenErrorCodePropertyIsAMatch_ReturnsRemoteErrorType()
        {
            using (var rpc = new TestJsonRpc())
            {
                Type? type = rpc.GetErrorDetailsDataTypeHelper(
                    new JsonRpcError()
                    {
                        Error = new JsonRpcError.ErrorDetail()
                        {
                            Code = (JsonRpcErrorCode)(int)RemoteErrorCode.RemoteError
                        }
                    });

                Assert.Equal(typeof(RemoteError), type);
            }
        }

        private sealed class TestJsonRpc : NuGetJsonRpc
        {
            internal TestJsonRpc()
                : base(Mock.Of<IJsonRpcMessageHandler>())
            {
            }

            internal Type? GetErrorDetailsDataTypeHelper(JsonRpcError error)
            {
                return GetErrorDetailsDataType(error);
            }
        }
    }
}
