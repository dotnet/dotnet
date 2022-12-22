// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml.Linq;
using XliffTasks.Model;
using Xunit;
using static XliffTasks.Model.XlfNames;

namespace XliffTasks.Tests
{
    public class XElementExtensionsTests
    {
        [Fact]
        public void GetTargetValueOrDefault_ReturnsTargetWhenTargetIsPresent()
        {
            XElement transUnitElement =
                new(TransUnit,
                    new XElement(Source, "source text"),
                    new XElement(Target, "target text"));

            string targetValue = transUnitElement.GetTargetValue();

            Assert.Equal(expected: "target text", actual: targetValue);
        }

        [Fact]
        public void GetTargetValueOrDefault_ReturnsEmptyStringWhenTargetIsEmpty()
        {
            XElement transUnitElement =
                new(TransUnit,
                    new XElement(Source, "source text"),
                    new XElement(Target, string.Empty));

            string targetValue = transUnitElement.GetTargetValue();

            Assert.Equal(expected: string.Empty, actual: targetValue);
        }

        [Fact]
        public void GetTargetValueOrDefault_ReturnsSourceWhenTargetIsMissing()
        {
            XElement transUnitElement =
                new(TransUnit,
                    new XElement(Source, "source text"));

            string targetValue = transUnitElement.GetTargetValue();

            Assert.Equal(expected: "source text", actual: targetValue);
        }

        [Fact]
        public void SetTargetValue_SetsValueIfTargetIsPresent()
        {
            XElement transUnitElement =
                new(TransUnit,
                    new XElement(Source, "source text"),
                    new XElement(Target, "original target text"),
                    new XElement(Note));

            transUnitElement.SetTargetValue("new target text");

            Assert.Equal(expected: "new target text", actual: transUnitElement.Element(Target).Value);
        }

        [Fact]
        public void SetTargetValue_TargetIsCreatedIfNotPresent()
        {
            XElement transUnitElement =
                new(TransUnit,
                    new XElement(Source, "source text"),
                    new XElement(Note));

            transUnitElement.SetTargetValue("new target text");

            Assert.Equal(expected: "new target text", actual: transUnitElement.Element(Target).Value);
            Assert.Equal(expected: transUnitElement.Element(Source), actual: transUnitElement.Element(Target).PreviousNode);
        }

        [Fact]
        public void GetTargetState_ReturnsStateWhenStateIsPresent()
        {
            XElement transUnitElement =
                new(TransUnit,
                    new XElement(Target,
                        new XAttribute("state", "original state value")));

            string stateValue = transUnitElement.GetTargetState();

            Assert.Equal(expected: "original state value", actual: stateValue);
        }

        [Fact]
        public void GetTargetState_ReturnsDefaultWhenTargetIsPresentButStateIsNot()
        {
            XElement transUnitElement =
                new(TransUnit,
                    new XElement(Target));

            string stateValue = transUnitElement.GetTargetState();

            Assert.Equal(expected: "new", actual: stateValue);
        }

        [Fact]
        public void GetTargetState_ReturnsDefaultWhenTargetIsNotPresent()
        {
            XElement transUnitElement = new(TransUnit);

            string stateValue = transUnitElement.GetTargetState();

            Assert.Equal(expected: "new", actual: stateValue);
        }

        [Fact]
        public void SetTargetState_SetsStateWhenAlreadyPresent()
        {
            XElement transUnitElement =
                new(TransUnit,
                    new XElement(Source, "soruce text"),
                    new XElement(Target,
                        new XAttribute("state", "new"),
                        "target text"));

            transUnitElement.SetTargetState("translated");

            Assert.Equal(expected: "translated", actual: transUnitElement.Element(Target).Attribute("state").Value);
        }

        [Fact]
        public void SetTargetState_AddsStateAttributeIfNotPresent()
        {
            XElement transUnitElement =
                new(TransUnit,
                    new XElement(Source, "soruce text"),
                    new XElement(Target, "target text"));

            transUnitElement.SetTargetState("translated");

            Assert.Equal(expected: "translated", actual: transUnitElement.Element(Target).Attribute("state").Value);
        }

        [Fact]
        public void SetTargetState_AddsTargetElementIfNotPresent()
        {
            XElement transUnitElement =
                new(TransUnit,
                    new XElement(Source, "soruce text"));

            transUnitElement.SetTargetState("translated");

            Assert.Equal(expected: "translated", actual: transUnitElement.Element(Target).Attribute("state").Value);
        }
    }
}