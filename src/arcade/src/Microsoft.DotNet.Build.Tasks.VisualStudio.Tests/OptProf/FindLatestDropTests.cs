// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio.UnitTests
{
    public class FindLatestDropTests
    {
        [Fact]
        public void GetLatestDropName()
        {
            Assert.Equal("OptimizationData/dotnet/roslyn/master-vs-deps/41416da1e8531ab0f4e5e7dc67318237323acf23/202326/813349/1", 
                FindLatestDrop.GetLatestDropName(@"
[
  {
    ""CreatedDateUtc"": ""2018-11-27T08:09:10.9866839Z"",
    ""DeletePending"": false,
    ""Name"": ""OptimizationData/dotnet/roslyn/master-vs-deps/af42e741da717a6c0bf9877ae61f8f955f9917cc/201492/808117/1"",
    ""UploadComplete"": true
  },
  {
    ""CreatedDateUtc"": ""2018-11-28T14:54:59.5832452Z"",
    ""DeletePending"": false,
    ""Name"": ""OptimizationData/dotnet/roslyn/master-vs-deps/41416da1e8531ab0f4e5e7dc67318237323acf23/202326/813349/1"",
    ""UploadComplete"": true
  },
  {
    ""CreatedDateUtc"": ""2018-11-27T23:58:42.9833879Z"",
    ""DeletePending"": false,
    ""Name"": ""OptimizationData/dotnet/roslyn/master-vs-deps/11593212665e99186ec0c6c157018f5733925b8e/202008/811476/1"",
    ""UploadComplete"": true
  }
]
"));
        }

        [Theory]
        [InlineData(@"[]")]
        [InlineData(@"[ { } ]")]
        [InlineData(@"[ { ""CreatedDateUtc"" : 1 } ]")]
        [InlineData(@"[ { ""CreatedDateUtc"" : ""2018-11-28T14:54:59.5832452Z"" } ]")]
        public void GetLatestDropName_Error(string json)
        {
            Assert.Throws<ApplicationException>(() => FindLatestDrop.GetLatestDropName(json));
        }
    }
}
