namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System.Reflection;
    using Microsoft.ApplicationInsights.TestFramework;
    using System.Diagnostics.Tracing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public class CoreEventSourceTest
    {
        [TestMethod]
        public void MethodsAreImplementedConsistentlyWithTheirAttributes()
        {
            EventSourceTest.MethodsAreImplementedConsistentlyWithTheirAttributes(CoreEventSource.Log);
        }

        [TestMethod]
        public void LogErrorIsDoesNotHaveUserActionableKeywordToBeClearlyIndicatedInPortalUx()
        {
            Assert.AreNotEqual(CoreEventSource.Keywords.UserActionable, GetEventAttribute("LogError").Keywords & CoreEventSource.Keywords.UserActionable);
        }

        private static EventAttribute GetEventAttribute(string methodName)
        {
            MethodInfo method = typeof(CoreEventSource).GetMethod(methodName);
            return method.GetCustomAttribute<EventAttribute>();
        }

#if REDFIELD
        /// <summary>
        /// This is a sanitiy check.
        /// The 'Redfield' compilation flag should switch the name of EventSource class.
        /// Devs can review the test log and confirm that this test runs and passes.
        /// This will serve as a verification that the Redfield compilation flag worked as expected.
        /// 
        /// To run this test:
        /// dotnet build /p:Redfield=True ".\dotnet\BASE\Microsoft.ApplicationInsights.sln"
        /// dotnet test ".\bin\Debug\test\Microsoft.ApplicationInsights.Tests\net5.0\Microsoft.ApplicationInsights.Tests.dll" --filter Name~VerifyRedfieldEventSourceName
        /// </summary>
        [TestMethod]
        public void VerifyRedfieldEventSourceName()
        {
            var expectedName = "Redfield-Microsoft-ApplicationInsights-Core";

            var eventSourceAttribute = typeof(CoreEventSource)
                .GetCustomAttributes(typeof(EventSourceAttribute))
                .Single() as EventSourceAttribute;

            Assert.IsNotNull(eventSourceAttribute);
            Assert.AreEqual(expectedName, eventSourceAttribute.Name);
        }
#endif
    }
}