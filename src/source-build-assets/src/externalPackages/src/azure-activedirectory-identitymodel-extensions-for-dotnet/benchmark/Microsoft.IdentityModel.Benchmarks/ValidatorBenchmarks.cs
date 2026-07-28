// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.IdentityModel.Benchmarks
{
    /// <summary>
    /// Benchmarks for validator methods to measure performance of different implementations
    /// </summary>
    public class ValidatorBenchmarks
    {
        private List<string> _audiences;
        private TokenValidationParameters _parametersWithList;
        private TokenValidationParameters _parametersWithEnumerable;

        [GlobalSetup]
        public void Setup()
        {
            _audiences = ["audience1"];
            var validAudiences = new List<string> { "invalid1", "invalid2", "audience1" }; // Make sure audience is last to maximize iteration

            _parametersWithList = new TokenValidationParameters
            {
                ValidAudiences = validAudiences
            };

            _parametersWithEnumerable = new TokenValidationParameters
            {
                ValidAudiences = validAudiences.Select(x => x) // Force enumerable by using LINQ
            };
        }

        [Benchmark(Baseline = true)]
        public void ValidateAudience_WithList()
        {
            Validators.ValidateAudience(_audiences, null, _parametersWithList);
        }

        [Benchmark]
        public void ValidateAudience_WithEnumerable()
        {
            Validators.ValidateAudience(_audiences, null, _parametersWithEnumerable);
        }
    }
}
