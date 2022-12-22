// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class CredentialsItemTests
    {
        private static readonly string _moreThanOneUsername = Resources.Error_MoreThanOneUsername;
        private static readonly string _moreThanOnePassword = Resources.Error_MoreThanOnePassword;
        private static readonly string _moreThanOneValidAuthenticationTypes = Resources.Error_MoreThanOneValidAuthenticationTypes;

        [Theory]
        [InlineData(null, "user", "pass")]
        [InlineData("", "user", "pass")]
        [InlineData("name", null, "pass")]
        [InlineData("name", "", "pass")]
        [InlineData("name", "user", null)]
        [InlineData("name", "user", "")]
        public void CredentialsItem_Constructor_WithEmptyOrNullParameters_Throws(string name, string username, string password)
        {
            var ex = Record.Exception(() => new CredentialsItem(name, username, password, isPasswordClearText: true, validAuthenticationTypes: null));
            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentException>();
        }

        [Fact]
        public void CredentialsItem_Constructor_WithSpaceOnName_EncodesItCorrectly()
        {
            var item = new CredentialsItem("credentials name", "username", "password", isPasswordClearText: true, validAuthenticationTypes: null);

            item.ElementName.Should().Be("credentials name");
        }

        [Fact]
        public void CredentialsItem_Parsing_WithoutUsername_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <packageSourceCredentials>
        <NuGet.Org meta1='data1'>
            <add key='Password' value='password' />
        </NuGet.Org>
    </packageSourceCredentials>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));

                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("Unable to parse config file because: Credentials item must have username and password. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void CredentialsItem_Parsing_WithoutPasswordOrClearTextPassword_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <packageSourceCredentials>
        <NuGet.Org meta1='data1'>
            <add key='Username' value='username' />
        </NuGet.Org>
    </packageSourceCredentials>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));

                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("Unable to parse config file because: Credentials item must have username and password. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void CredentialsItem_Parsing_WithUsernamePasswordAndClearTextPassword_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <packageSourceCredentials>
        <NuGet.Org meta1='data1'>
            <add key='Username' value='username' />
            <add key='Password' value='password' />
            <add key='ClearTextPassword' value='clearTextPassword' />
        </NuGet.Org>
    </packageSourceCredentials>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("Unable to parse config file because: {0} Path: '{1}'.", _moreThanOnePassword, Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void CredentialsItem_Parsing_WithMultiplePassword_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <packageSourceCredentials>
        <NuGet.Org meta1='data1'>
            <add key='Username' value='username' />
            <add key='Password' value='password' />
            <add key='Password' value='password' />
        </NuGet.Org>
    </packageSourceCredentials>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("Unable to parse config file because: {0} Path: '{1}'.", _moreThanOnePassword, Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void CredentialsItem_Parsing_WithMultipleClearTextPassword_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <packageSourceCredentials>
        <NuGet.Org meta1='data1'>
            <add key='Username' value='username' />
            <add key='ClearTextPassword' value='clearTextPassword' />
            <add key='ClearTextPassword' value='clearTextPassword' />
        </NuGet.Org>
    </packageSourceCredentials>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("Unable to parse config file because: {0} Path: '{1}'.", _moreThanOnePassword, Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void CredentialsItem_Parsing_WithMultipleUsernames_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <packageSourceCredentials>
        <NuGet.Org meta1='data1'>
            <add key='Username' value='username' />
            <add key='Username' value='username2' />
            <add key='Password' value='password' />
        </NuGet.Org>
    </packageSourceCredentials>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("Unable to parse config file because: {0} Path: '{1}'.", _moreThanOneUsername, Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void CredentialsItem_Parsing_WithMultipleValidAuthenticationTypes_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <packageSourceCredentials>
        <NuGet.Org meta1='data1'>
            <add key='Username' value='username' />
            <add key='Password' value='password' />
            <add key='ValidAuthenticationTypes' value='one,two,three' />
            <add key='ValidAuthenticationTypes' value='four,five,six' />
        </NuGet.Org>
    </packageSourceCredentials>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("Unable to parse config file because: {0} Path: '{1}'.", _moreThanOneValidAuthenticationTypes, Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void CredentialsItem_Update_WhenItemIsNotCredentialsItem_Throws()
        {
            // Arrange
            var credentials = new CredentialsItem("name", "user", "pass", isPasswordClearText: true, validAuthenticationTypes: null);

            // Act
            var ex = Record.Exception(() => credentials.Update(new AddItem("key", "value")));

            // Assert
            ex.Should().NotBeNull();
            ex.Should().BeOfType<InvalidOperationException>();
            ex.Message.Should().Be("The item passed to the Update method cannot refer to a different item than the one being updated.");

            credentials.ElementName.Should().Be("name");
            credentials.Username.Should().Be("user");
            credentials.Password.Should().Be("pass");
        }

        [Fact]
        public void CredentialsItem_Update_WhenOriginIsMachineWide_Throws()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var origin = new SettingsFile(mockBaseDirectory, fileName: Settings.DefaultSettingsFileName, isMachineWide: true, isReadOnly: false);

                var xelement = new XElement("name",
                                new XElement("add", new XAttribute("key", "Username"), new XAttribute("value", "user")),
                                new XElement("add", new XAttribute("key", "Password"), new XAttribute("value", "pass")));

                var credentials = new CredentialsItem(xelement, origin);

                // Act
                var ex = Record.Exception(() =>
                    credentials.Update(new CredentialsItem("name", "user", "notpass", isPasswordClearText: true, validAuthenticationTypes: null)));

                // Assert
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be("Unable to update setting since it is in a machine-wide NuGet.Config.");

                credentials.Password.Should().Be("pass");
            }
        }

        [Fact]
        public void CredentialsItem_Update_WhenOriginIsReadOnly_Throws()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var origin = new SettingsFile(mockBaseDirectory, fileName: Settings.DefaultSettingsFileName, isMachineWide: false, isReadOnly: true);

                var xelement = new XElement("name",
                                new XElement("add", new XAttribute("key", "Username"), new XAttribute("value", "user")),
                                new XElement("add", new XAttribute("key", "Password"), new XAttribute("value", "pass")));

                var credentials = new CredentialsItem(xelement, origin);

                // Act
                var ex = Record.Exception(() =>
                    credentials.Update(new CredentialsItem("name", "user", "notpass", isPasswordClearText: true, validAuthenticationTypes: null)));

                // Assert
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be(Resources.CannotUpdateReadOnlyConfig);

                credentials.Password.Should().Be("pass");
            }
        }

        [Fact]
        public void CredentialsItem_Update_ChangeUsername_UpdatesObjectAndXNode()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var origin = new SettingsFile(mockBaseDirectory);

                var xelement = new XElement("name",
                                new XElement("add", new XAttribute("key", "Username"), new XAttribute("value", "user")),
                                new XElement("add", new XAttribute("key", "Password"), new XAttribute("value", "pass")));

                var credentials = new CredentialsItem(xelement, origin);

                // Act
                credentials.Update(new CredentialsItem("name", "newuser", "pass", isPasswordClearText: false, validAuthenticationTypes: null));

                // Assert
                credentials.Username.Should().Be("newuser");

                var credentialElement = credentials.AsXNode() as XElement;
                var childElements = credentialElement.Elements().ToList();

                childElements.Count.Should().Be(2);
                childElements[0].Name.LocalName.Should().Be("add");
                var elattr = childElements[0].Attributes().ToList();
                elattr.Count.Should().Be(2);
                elattr[0].Value.Should().Be("Username");
                elattr[1].Value.Should().Be("newuser");
            }
        }

        [Fact]
        public void CredentialsItem_Update_ChangePassword_UpdatesObjectAndXNode()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var origin = new SettingsFile(mockBaseDirectory);

                var xelement = new XElement("name",
                                new XElement("add", new XAttribute("key", "Username"), new XAttribute("value", "user")),
                                new XElement("add", new XAttribute("key", "Password"), new XAttribute("value", "pass")));

                var credentials = new CredentialsItem(xelement, origin);

                // Act
                credentials.Update(new CredentialsItem("name", "user", "newpass", isPasswordClearText: false, validAuthenticationTypes: null));

                // Assert
                credentials.Password.Should().Be("newpass");

                var credentialElement = credentials.AsXNode() as XElement;
                var childElements = credentialElement.Elements().ToList();

                childElements.Count.Should().Be(2);
                childElements[1].Name.LocalName.Should().Be("add");
                var elattr = childElements[1].Attributes().ToList();
                elattr.Count.Should().Be(2);
                elattr[0].Value.Should().Be("Password");
                elattr[1].Value.Should().Be("newpass");
            }
        }

        [Fact]
        public void CredentialsItem_Update_ChangeClearTextPassword_UpdatesObjectAndXNode()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var origin = new SettingsFile(mockBaseDirectory);

                var xelement = new XElement("name",
                                    new XElement("add", new XAttribute("key", "Username"), new XAttribute("value", "user")),
                                    new XElement("add", new XAttribute("key", "ClearTextPassword"), new XAttribute("value", "pass")));

                var credentials = new CredentialsItem(xelement, origin);

                // Act
                credentials.Update(new CredentialsItem("name", "user", "newpass", isPasswordClearText: true, validAuthenticationTypes: null));

                // Assert
                credentials.Password.Should().Be("newpass");

                var credentialElement = credentials.AsXNode() as XElement;
                var childElements = credentialElement.Elements().ToList();

                childElements.Count.Should().Be(2);
                childElements[1].Name.LocalName.Should().Be("add");
                var elattr = childElements[1].Attributes().ToList();
                elattr.Count.Should().Be(2);
                elattr[0].Value.Should().Be("ClearTextPassword");
                elattr[1].Value.Should().Be("newpass");
            }
        }

        [Fact]
        public void CredentialsItem_Update_MakingPasswordClearText_UpdatesObjectAndXNode()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var origin = new SettingsFile(mockBaseDirectory);

                var xelement = new XElement("name",
                                    new XElement("add", new XAttribute("key", "Username"), new XAttribute("value", "user")),
                                    new XElement("add", new XAttribute("key", "Password"), new XAttribute("value", "pass")));

                var credentials = new CredentialsItem(xelement, origin);

                // Act
                credentials.Update(new CredentialsItem("name", "user", "newpass", isPasswordClearText: true, validAuthenticationTypes: null));

                // Assert
                credentials.Password.Should().Be("newpass");

                var credentialElement = credentials.AsXNode() as XElement;
                var childElements = credentialElement.Elements().ToList();

                childElements.Count.Should().Be(2);
                childElements[1].Name.LocalName.Should().Be("add");
                var elattr = childElements[1].Attributes().ToList();
                elattr.Count.Should().Be(2);
                elattr[0].Value.Should().Be("ClearTextPassword");
                elattr[1].Value.Should().Be("newpass");
            }
        }

        [Fact]
        public void CredentialsItem_Update_MakingPasswordEncrypted_UpdatesObjectAndXNode()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var origin = new SettingsFile(mockBaseDirectory);

                var xelement = new XElement("name",
                                new XElement("add", new XAttribute("key", "Username"), new XAttribute("value", "user")),
                                new XElement("add", new XAttribute("key", "ClearTextPassword"), new XAttribute("value", "pass")));

                var credentials = new CredentialsItem(xelement, origin);

                // Act
                credentials.Update(new CredentialsItem("name", "user", "newpass", isPasswordClearText: false, validAuthenticationTypes: null));

                // Assert
                credentials.Password.Should().Be("newpass");

                var credentialElement = credentials.AsXNode() as XElement;
                var childElements = credentialElement.Elements().ToList();

                childElements.Count.Should().Be(2);
                childElements[1].Name.LocalName.Should().Be("add");
                var elattr = childElements[1].Attributes().ToList();
                elattr.Count.Should().Be(2);
                elattr[0].Value.Should().Be("Password");
                elattr[1].Value.Should().Be("newpass");
            }
        }


        [Fact]
        public void CredentialsItem_Update_RemovingValidAuthenticationTypes_UpdatesObjectAndFile()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var origin = new SettingsFile(mockBaseDirectory);

                var xelement = new XElement("name",
                                new XElement("add", new XAttribute("key", "Username"), new XAttribute("value", "user")),
                                new XElement("add", new XAttribute("key", "ClearTextPassword"), new XAttribute("value", "pass")),
                                new XElement("add", new XAttribute("key", "ValidAuthenticationTypes"), new XAttribute("value", "one, two, three")));

                var credentials = new CredentialsItem(xelement, origin);

                // Act
                credentials.Update(new CredentialsItem("name", "user", "pass", isPasswordClearText: true, validAuthenticationTypes: null));

                // Assert
                credentials.ValidAuthenticationTypes.Should().BeNull();
                origin.IsDirty.Should().BeTrue();
                origin.SaveToDisk();

                var credentialElement = credentials.AsXNode() as XElement;
                var childElements = credentialElement.Elements().ToList();

                childElements.Count.Should().Be(2);

                childElements[1].Name.LocalName.Should().Be("add");
                var elattr = childElements[0].Attributes().ToList();
                elattr.Count.Should().Be(2);
                elattr[0].Value.Should().Be("Username");

                childElements[1].Name.LocalName.Should().Be("add");
                elattr = childElements[1].Attributes().ToList();
                elattr.Count.Should().Be(2);
                elattr[0].Value.Should().Be("ClearTextPassword");
            }
        }

        [Fact]
        public void CredentialsItem_AsXNode_WithSpaceInName_ReturnsCorrectElement()
        {
            // Arrange
            var credentialsItem = new CredentialsItem("credentials name", "username", "password", isPasswordClearText: false, validAuthenticationTypes: null);

            // Act
            var xnode = credentialsItem.AsXNode();

            // Assert
            xnode.Should().BeOfType<XElement>();
            var xelement = xnode as XElement;

            xelement.Name.LocalName.Should().Be("credentials_x0020_name");
            var elements = xelement.Elements().ToList();
            elements.Count.Should().Be(2);
            elements[0].Name.LocalName.Should().Be("add");
            var elattr = elements[0].Attributes().ToList();
            elattr.Count.Should().Be(2);
            elattr[0].Value.Should().Be("Username");
            elattr[1].Value.Should().Be("username");

            elements[1].Name.LocalName.Should().Be("add");
            elattr = elements[1].Attributes().ToList();
            elattr.Count.Should().Be(2);
            elattr[0].Value.Should().Be("Password");
            elattr[1].Value.Should().Be("password");
        }

        [Fact]
        public void CredentialsItem_AsXNode_WithUrlInName_ReturnsCorrectElementName()
        {
            // Arrange
            var credentialsItem = new CredentialsItem("https://nuget.contoso.com/v3/index.json", "username", "password", isPasswordClearText: false, validAuthenticationTypes: null);

            // Act
            var xnode = credentialsItem.AsXNode();

            // Assert
            xnode.Should().BeOfType<XElement>();
            var xelement = xnode as XElement;

            xelement.Name.LocalName.Should().Be("https_x003A__x002F__x002F_nuget.contoso.com_x002F_v3_x002F_index.json");
        }

        [Fact]
        public void CredentialsItem_AsXNode_WithUsernameAndPassword_ReturnsCorrectElement()
        {
            // Arrange
            var credentialsItem = new CredentialsItem("name", "username", "password", isPasswordClearText: false, validAuthenticationTypes: null);

            // Act
            var xnode = credentialsItem.AsXNode();

            // Assert
            xnode.Should().BeOfType<XElement>();
            var xelement = xnode as XElement;

            xelement.Name.LocalName.Should().Be("name");
            var elements = xelement.Elements().ToList();
            elements.Count.Should().Be(2);
            elements[0].Name.LocalName.Should().Be("add");
            var elattr = elements[0].Attributes().ToList();
            elattr.Count.Should().Be(2);
            elattr[0].Value.Should().Be("Username");
            elattr[1].Value.Should().Be("username");

            elements[1].Name.LocalName.Should().Be("add");
            elattr = elements[1].Attributes().ToList();
            elattr.Count.Should().Be(2);
            elattr[0].Value.Should().Be("Password");
            elattr[1].Value.Should().Be("password");
        }

        [Fact]
        public void CredentialsItem_AsXNode_WithUsernameAndClearTextPassword_ReturnsCorrectElement()
        {
            // Arrange
            var credentialsItem = new CredentialsItem("name", "username", "password", isPasswordClearText: true, validAuthenticationTypes: null);

            // Act
            var xnode = credentialsItem.AsXNode();

            // Assert
            xnode.Should().BeOfType<XElement>();
            var xelement = xnode as XElement;

            xelement.Name.LocalName.Should().Be("name");
            var elements = xelement.Elements().ToList();
            elements.Count.Should().Be(2);
            elements[0].Name.LocalName.Should().Be("add");
            var elattr = elements[0].Attributes().ToList();
            elattr.Count.Should().Be(2);
            elattr[0].Value.Should().Be("Username");
            elattr[1].Value.Should().Be("username");

            elements[1].Name.LocalName.Should().Be("add");
            elattr = elements[1].Attributes().ToList();
            elattr.Count.Should().Be(2);
            elattr[0].Value.Should().Be("ClearTextPassword");
            elattr[1].Value.Should().Be("password");
        }

        [Fact]
        public void CredentialsItem_Equals_WithSameElementName_ReturnsTrue()
        {
            var credentials1 = new CredentialsItem("source", "user", "pass", isPasswordClearText: true, validAuthenticationTypes: "one,two,three");
            var credentials2 = new CredentialsItem("source", "user2", "pass", isPasswordClearText: false, validAuthenticationTypes: null);

            credentials1.Equals(credentials2).Should().BeTrue();
        }

        [Fact]
        public void CredentialsItem_Equals_WithDifferentElemenName_ReturnsFalse()
        {
            var credentials1 = new CredentialsItem("source1", "user", "pass", isPasswordClearText: true, validAuthenticationTypes: "one,two,three");
            var credentials2 = new CredentialsItem("source2", "user", "pass", isPasswordClearText: true, validAuthenticationTypes: "one,two,three");

            credentials1.Equals(credentials2).Should().BeFalse();
        }

        [Fact]
        public void CredentialsItem_ElementName_IsCorrect()
        {
            var credentialsItem = new CredentialsItem("source", "user", "pass", isPasswordClearText: false, validAuthenticationTypes: null);

            credentialsItem.ElementName.Should().Be("source");
        }

        [Fact]
        public void CredentialsItem_Clone_ReturnsItemClone()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSourceCredentials>
        <NuGet.Org meta1='data1'>
            <add key='Username' value='username' />
            <add key='Password' value='password' />
            <add key='ValidAuthenticationTypes' value='one,two,three' />
        </NuGet.Org>
    </packageSourceCredentials>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.TryGetSection("packageSourceCredentials", out var section).Should().BeTrue();
                section.Should().NotBeNull();

                section.Items.Count.Should().Be(1);
                var item = section.Items.First();
                item.IsCopy().Should().BeFalse();
                item.Origin.Should().NotBeNull();

                var clone = item.Clone() as CredentialsItem;
                clone.IsCopy().Should().BeTrue();
                clone.Origin.Should().NotBeNull();
                SettingsTestUtils.DeepEquals(clone, item).Should().BeTrue();
            }
        }

        [Fact]
        public void CredentialsItem_Clone_WithSpaceOnName_ReturnsItemClone()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSourceCredentials>
        <nuget_x0020_org>
            <add key='Username' value='username' />
            <add key='Password' value='password' />
        </nuget_x0020_org>
    </packageSourceCredentials>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.TryGetSection("packageSourceCredentials", out var section).Should().BeTrue();
                section.Should().NotBeNull();

                section.Items.Count.Should().Be(1);
                var item = section.Items.First();
                item.IsCopy().Should().BeFalse();
                item.Origin.Should().NotBeNull();

                var clone = item.Clone() as CredentialsItem;
                clone.IsCopy().Should().BeTrue();
                clone.Origin.Should().NotBeNull();
                SettingsTestUtils.DeepEquals(clone, item).Should().BeTrue();
            }
        }
    }
}
