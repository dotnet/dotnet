namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    [TestClass]
    public class WeakConcurrentRandomTests
    {
        [TestMethod]
        public void GeneratedRandomNumbersDoNotRepeatOverMediumSizedSequence()
        {
            // number of randoms to generate
            const int RandomNumberCount = 100000;
            HashSet<ulong> previouslyGeneratedNumbers = new HashSet<ulong>();

            WeakConcurrentRandom rng = WeakConcurrentRandom.Instance;

            for (int i = 0; i < RandomNumberCount; i++)
            {
                ulong randomNumber = rng.Next();

                Assert.IsFalse(previouslyGeneratedNumbers.Contains(randomNumber));

                previouslyGeneratedNumbers.Add(randomNumber);
            }
        }
    }
}
