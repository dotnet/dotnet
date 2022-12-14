// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.DirectoryServices.Tests;
using System.Linq;
using Xunit;
using System.Collections.Generic;

namespace System.DirectoryServices.AccountManagement.Tests
{
    public class AccountManagementTests
    {
        internal static bool IsLdapConfigurationExist => LdapConfiguration.Configuration != null;
        internal static bool IsActiveDirectoryServer => IsLdapConfigurationExist && LdapConfiguration.Configuration.IsActiveDirectoryServer;
        internal static bool IsDomainJoinedClient => !Environment.MachineName.Equals(Environment.UserDomainName, StringComparison.OrdinalIgnoreCase);

        [Fact]
        public void TestConstructors()
        {
            using var context = new PrincipalContext(ContextType.Machine);

            using (var principal = new ComputerPrincipal(context))
            {
                Assert.Same(context, principal.Context);
                Assert.Empty(principal.ServicePrincipalNames);
                Assert.Equal(ContextType.Machine, principal.ContextType);
            }
            Assert.Throws<ArgumentException>(() => new ComputerPrincipal(null));
            Assert.Throws<ArgumentException>(() => new ComputerPrincipal(null, "samAccountName", "password", true));
            Assert.Throws<ArgumentException>(() => new ComputerPrincipal(context, null, "password", true));
            Assert.Throws<ArgumentException>(() => new ComputerPrincipal(context, "samAccountName", null, true));

            using (var principal = new UserPrincipal(context))
            {
                Assert.Same(context, principal.Context);
                Assert.Equal(ContextType.Machine, principal.ContextType);
            }
            Assert.Throws<ArgumentException>(() => new UserPrincipal(null));
            Assert.Throws<ArgumentException>(() => new UserPrincipal(null, "samAccountName", "password", true));
            Assert.Throws<ArgumentException>(() => new UserPrincipal(context, null, "password", true));
            Assert.Throws<ArgumentException>(() => new UserPrincipal(context, "samAccountName", null, true));

            using (var principal = new GroupPrincipal(context))
            {
                Assert.Same(context, principal.Context);
                Assert.Equal(ContextType.Machine, principal.ContextType);
            }
            Assert.Throws<ArgumentException>(() => new GroupPrincipal(null));
            Assert.Throws<ArgumentException>(() => new GroupPrincipal(null, "samAccountName"));
            Assert.Throws<ArgumentException>(() => new GroupPrincipal(context, null));
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestCurrentUser()
        {
            using (PrincipalContext context = DomainContext)
            using (UserPrincipal p = FindUser(LdapConfiguration.Configuration.UserNameWithNoDomain, context))
            {
                Assert.NotNull(p);
                Assert.Equal(LdapConfiguration.Configuration.UserNameWithNoDomain, p.SamAccountName);
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer), nameof(IsDomainJoinedClient))]
        public void TestCurrentUserContext()
        {
            using (PrincipalContext context = DomainContext)
            using (UserPrincipal p = FindUser(LdapConfiguration.Configuration.UserNameWithNoDomain, context))
            using (UserPrincipal cu = UserPrincipal.Current)
            {
                Assert.NotNull(cu);
                Assert.NotEqual(cu.Context.Name, p.Context.Name);
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestCurrentUserUsingSearchFilter()
        {
            using (PrincipalContext context = DomainContext)
            using (UserPrincipal p = FindUserUsingFilter(LdapConfiguration.Configuration.UserNameWithNoDomain, context))
            {
                Assert.NotNull(p);
                Assert.Equal(LdapConfiguration.Configuration.UserNameWithNoDomain, p.SamAccountName);
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestGuestsGroup()
        {
            using (PrincipalContext context = DomainContext)
            using (GroupPrincipal p = FindGroup("Guests", context))
            {
                Assert.NotNull(p);
                Assert.Equal("Guests", p.Name);
                Assert.Equal("Guests", p.SamAccountName);
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestAddingUser()
        {
            UserData u1 = UserData.GenerateUserData("CoreFxUser1");
            UserData u2 = UserData.GenerateUserData("CoreFxUser2");
            UserData u3 = UserData.GenerateUserData("CoreFxUser3");

            DeleteUser(u1.Name);
            DeleteUser(u2.Name);
            DeleteUser(u3.Name);

            try
            {
                using (PrincipalContext context = DomainContext)
                using (UserPrincipal p1 = CreateUser(context, u1))
                using (UserPrincipal p2 = CreateUser(context, u2))
                using (UserPrincipal p3 = CreateUser(context, u3))
                {
                    Assert.NotNull(p1);
                    Assert.NotNull(p2);
                    Assert.NotNull(p3);

                    ValidateRecentAddedUser(context, u1);
                    ValidateRecentAddedUser(context, u2);
                    ValidateRecentAddedUser(context, u3);

                    ValidateUserUsingPrincipal(context, p1);
                    ValidateUserUsingPrincipal(context, p2);
                    ValidateUserUsingPrincipal(context, p3);
                }
            }
            finally
            {
                DeleteUser(u1.Name);
                DeleteUser(u2.Name);
                DeleteUser(u3.Name);
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestAddingGroup()
        {
            GroupData gd1 = GroupData.GenerateGroupData("CoreFXGroup1");
            GroupData gd2 = GroupData.GenerateGroupData("CoreFXGroup2");
            GroupData gd3 = GroupData.GenerateGroupData("CoreFXGroup3");

            DeleteGroup(gd1.Name);
            DeleteGroup(gd2.Name);
            DeleteGroup(gd3.Name);

            try
            {
                using (PrincipalContext context = DomainContext)
                using (GroupPrincipal p1 = CreateGroup(context, gd1))
                using (GroupPrincipal p2 = CreateGroup(context, gd2))
                using (GroupPrincipal p3 = CreateGroup(context, gd3))
                {
                    Assert.NotNull(p1);
                    Assert.NotNull(p2);
                    Assert.NotNull(p3);

                    ValidateRecentAddedGroup(context, gd1);
                    ValidateRecentAddedGroup(context, gd2);
                    ValidateRecentAddedGroup(context, gd3);

                    ValidateGroupUsingPrincipal(context, p1);
                    ValidateGroupUsingPrincipal(context, p2);
                    ValidateGroupUsingPrincipal(context, p3);
                }
            }
            finally
            {
                DeleteGroup(gd1.Name);
                DeleteGroup(gd2.Name);
                DeleteGroup(gd3.Name);
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestAddingUserToAGroup()
        {
            UserData u1 = UserData.GenerateUserData("CoreFxUser4");
            GroupData g1 = GroupData.GenerateGroupData("CoreFXGroup4");

            DeleteUser(u1.Name);
            DeleteGroup(g1.Name);

            try
            {
                using (PrincipalContext context = DomainContext)
                using (UserPrincipal user = CreateUser(context, u1))
                using (GroupPrincipal group = CreateGroup(context, g1))
                {
                    Assert.Equal(u1.Name, user.Name);
                    Assert.Equal(g1.Name, group.Name);

                    // First, check the user is not in the group

                    Assert.DoesNotContain(group, user.GetGroups());
                    Assert.False(user.IsMemberOf(group));
                    Assert.False(group.Members.Contains(user));

                    // second, add user and validate it is member of the group

                    group.Members.Add(context, IdentityType.Name, user.Name);
                    group.Save();

                    Assert.Contains(group, user.GetGroups());
                    Assert.True(user.IsMemberOf(group));
                    Assert.True(group.Members.Contains(user));

                    // Third, remove the user from the group and check again
                    group.Members.Remove(context, IdentityType.Name, user.Name);
                    group.Save();
                    Assert.DoesNotContain(group, user.GetGroups());
                    Assert.False(user.IsMemberOf(group));
                    Assert.False(group.Members.Contains(user));
                }
            }
            finally
            {
                DeleteUser(u1.Name);
                DeleteGroup(g1.Name);
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestDeleteUserAndGroup()
        {
            UserData u1 = UserData.GenerateUserData("CoreFxUser5");
            GroupData g1 = GroupData.GenerateGroupData("CoreFXGroup5");

            DeleteUser(u1.Name);
            DeleteGroup(g1.Name);

            try
            {
                using (PrincipalContext context = DomainContext)
                {
                    using (UserPrincipal up = FindUser(u1.Name, context)) { Assert.Null(up); }
                    using (GroupPrincipal gp = FindGroup(g1.Name, context)) { Assert.Null(gp); }

                    using (UserPrincipal user = CreateUser(context, u1))
                    using (GroupPrincipal group = CreateGroup(context, g1))
                    {
                        using (UserPrincipal up = FindUser(u1.Name, context))
                        {
                            Assert.NotNull(up);
                            up.Delete();
                        }
                        using (GroupPrincipal gp = FindGroup(g1.Name, context))
                        {
                            Assert.NotNull(gp);
                            gp.Delete();
                        }
                    }

                    using (UserPrincipal up = FindUser(u1.Name, context)) { Assert.Null(up); }
                    using (GroupPrincipal gp = FindGroup(g1.Name, context)) { Assert.Null(gp); }
                }
            }
            finally
            {
                DeleteUser(u1.Name);
                DeleteGroup(g1.Name);
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestNegativeCases()
        {
            UserData u1 = UserData.GenerateUserData("CoreFxUser6");
            GroupData g1 = GroupData.GenerateGroupData("CoreFXGroup6");

            DeleteUser(u1.Name);
            DeleteGroup(g1.Name);

            try
            {
                Assert.Throws<InvalidEnumArgumentException>(() => new PrincipalContext((ContextType) 768, null, null, null));
                Assert.Throws<PrincipalServerDownException>(() => new PrincipalContext(ContextType.Domain, "InvalidDomainName", null, null));
                Assert.Throws<ArgumentException>(() => new PrincipalContext(ContextType.Domain, LdapConfiguration.Configuration.ServerName, "InvalidTestUserName", null));
                Assert.Throws<ArgumentException>(() => new PrincipalContext(ContextType.Domain, LdapConfiguration.Configuration.ServerName, LdapConfiguration.Configuration.UserName, null));
                Assert.Throws<ArgumentException>(() => new UserPrincipal(null));
                Assert.Throws<ArgumentException>(() => new GroupPrincipal(null));

                using (PrincipalContext context = DomainContext)
                {
                    using (UserPrincipal user = CreateUser(context, u1))
                    using (GroupPrincipal group = CreateGroup(context, g1))
                    {
                        Assert.Throws<PrincipalExistsException>(() => CreateUser(context, u1));
                        Assert.Throws<PrincipalExistsException>(() => CreateGroup(context, g1));

                        group.Members.Add(context, IdentityType.Name, user.Name);
                        group.Save();
                        Assert.Throws<PrincipalExistsException>(() => group.Members.Add(context, IdentityType.Name, user.Name));
                        group.Members.Remove(context, IdentityType.Name, user.Name);
                        group.Save();

                        user.Delete();
                        Assert.Throws<InvalidOperationException>(() => user.Delete());
                        Assert.Throws<InvalidOperationException>(() => user.Save());

                        group.Delete();
                        Assert.Throws<InvalidOperationException>(() => group.Delete());
                        Assert.Throws<InvalidOperationException>(() => group.Save());
                    }
                }
            }
            finally
            {
                DeleteUser(u1.Name);
                DeleteGroup(g1.Name);
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestInvalidSaves()
        {
            UserData u1 = UserData.GenerateUserData("CoreFxUser9");

            DeleteUser(u1.Name);

            try
            {
                using var context = DomainContext;
                using var user = new UserPrincipal(context, u1.Name, u1.Password, true);
                
                Assert.Throws<InvalidOperationException>(() => user.Save(null));
                Assert.Throws<InvalidOperationException>(() => user.Save(new PrincipalContext(ContextType.Machine)));
            }
            finally
            {
                DeleteUser(u1.Name);
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestComputerContext()
        {
            using (PrincipalContext context = DomainContext)
            {
                using (ComputerPrincipal cp = new ComputerPrincipal(context))
                {
                    cp.Name = "*";
                    PrincipalSearcher ps = new PrincipalSearcher();
                    ps.QueryFilter = cp;
                    using (ComputerPrincipal r1 = ps.FindOne() as ComputerPrincipal)
                    using (ComputerPrincipal r2 = ComputerPrincipal.FindByIdentity(context, r1.Name))
                    {
                        Assert.Equal(r2.AccountExpirationDate, r1.AccountExpirationDate);
                        Assert.Equal(r2.Description, r1.Description);
                        Assert.Equal(r2.DisplayName, r1.DisplayName);
                        Assert.Equal(r2.DistinguishedName, r1.DistinguishedName);
                        Assert.Equal(r2.Guid, r1.Guid);
                        Assert.Equal(r2.HomeDirectory, r1.HomeDirectory);
                        Assert.Equal(r2.HomeDrive, r1.HomeDrive);
                        Assert.Equal(r2.SamAccountName, r1.SamAccountName);
                        Assert.Equal(r2.Sid, r1.Sid);
                        Assert.Equal(r2.UserPrincipalName, r1.UserPrincipalName);
                    }
                }
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestComputerNegativeCases()
        {
            using var context = DomainContext;

        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestUpdateUserAndGroupData()
        {
            UserData u1 = UserData.GenerateUserData("CoreFxUser7");
            GroupData g1 = GroupData.GenerateGroupData("CoreFXGroup7");

            DeleteUser(u1.Name);
            DeleteGroup(g1.Name);

            try
            {
                using (PrincipalContext context = DomainContext)
                using (UserPrincipal user = CreateUser(context, u1))
                using (GroupPrincipal group = CreateGroup(context, g1))
                {
                    using (UserPrincipal up = FindUser(u1.Name, context)) { Assert.Equal(user.DisplayName, up.DisplayName); }
                    using (GroupPrincipal gp = FindGroup(g1.Name, context)) { Assert.Equal(group.DisplayName, gp.DisplayName); }

                    user.DisplayName = "Updated CoreFx Test Child User 4";
                    
                    user.Save();
                    group.DisplayName = "Updated CoreFX Test Group Container 4";
                    group.Save();

                    using (UserPrincipal up = FindUser(u1.Name, context)) { Assert.Equal("Updated CoreFx Test Child User 4", up.DisplayName); }
                    using (GroupPrincipal gp = FindGroup(g1.Name, context)) { Assert.Equal("Updated CoreFX Test Group Container 4", gp.DisplayName); }
                }
            }
            finally
            {
                DeleteUser(u1.Name);
                DeleteGroup(g1.Name);
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestCredentials()
        {
            UserData u1 = UserData.GenerateUserData("CoreFxUser8");

            DeleteUser(u1.Name);

            try
            {
                using (PrincipalContext context = DomainContext)
                using (UserPrincipal p1 = CreateUser(context, u1))
                {
                    Assert.True(context.ValidateCredentials(u1.Name, u1.Password));
                    Assert.True(context.ValidateCredentials(u1.Name, u1.Password, ContextOptions.ServerBind));

                    Assert.Throws<System.DirectoryServices.Protocols.LdapException>(() => context.ValidateCredentials(u1.Name, "WrongPassword"));
                    Assert.Throws<System.DirectoryServices.Protocols.LdapException>(() => context.ValidateCredentials("WrongUser", u1.Password));
                }
            }
            finally
            {
                DeleteUser(u1.Name);
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestCustomUserAttributes()
        {
            var userData = CustomUserData.GenerateUserData("CustomCoreFxUser1");

            DeleteUser(userData.Name);

            try
            {
                using var context = DomainContext;
                using (var principal = CreateCustomUser(context, userData))
                {
                    Assert.NotNull(principal);
                    ValidateRecentAddedUser(context, userData);
                    ValidateUserUsingPrincipal(context, principal);

                    using var foundPrincipal = FindCustomUser(userData.Name, context);
                    Assert.NotNull(foundPrincipal);

                    Assert.Equal(userData.PostalCode, foundPrincipal.PostalCode);
                    Assert.Equal(principal.PostalCode, foundPrincipal.PostalCode);

                    Assert.Equal(userData.PostalAddress, foundPrincipal.PostalAddress);
                    Assert.Equal(principal.PostalAddress, foundPrincipal.PostalAddress);
                }
            }
            finally
            {
                DeleteUser(userData.Name);
            }
        }

        [ConditionalFact(nameof(IsActiveDirectoryServer))]
        public void TestMultiValueCustomAttributes()
        {
            var userData = CustomUserData.GenerateUserData("CustomCoreFxUser2");
            userData.PostalAddress.Add("Second address");

            DeleteUser(userData.Name);

            // Check whether directory-data is equivalent to expected data
            void CheckAddressWithDirectory(PrincipalContext context, List<string> address)
            {
                using var foundPrincipal = FindCustomUser(userData.Name, context);
                Assert.NotNull(foundPrincipal);
                Assert.Equal(address.ToHashSet(), foundPrincipal.PostalAddress.ToHashSet());
            };

            // Helper to update list
            void UpdateAddressList(CustomUserPrincipal principal, Action<List<string>> update)
            {
                var localCopy = principal.PostalAddress;
                update(localCopy);
                principal.PostalAddress = localCopy;
                principal.Save();
            }

            try
            {
                // Initial setup
                using var context = DomainContext;
                using var principal = CreateCustomUser(context, userData);
                Assert.NotNull(principal);
                Assert.Equal(userData.PostalAddress, principal.PostalAddress);

                CheckAddressWithDirectory(context, principal.PostalAddress);

                // Add address
                UpdateAddressList(principal, addresses => addresses.Add("Third address"));
                CheckAddressWithDirectory(context, principal.PostalAddress);

                // Remove address
                UpdateAddressList(principal, addresses => addresses.Remove("Second address"));
                CheckAddressWithDirectory(context, principal.PostalAddress);

                // Remove address so we have one remaining
                UpdateAddressList(principal, addresses => addresses.Remove("Third address"));
                CheckAddressWithDirectory(context, principal.PostalAddress);
            }
            finally
            {
                DeleteUser(userData.Name);
            }
        }

        private void ValidateRecentAddedUser(PrincipalContext context, UserData userData)
        {
            using (UserPrincipal p = FindUser(userData.Name, context))
            {
                Assert.NotNull(p);
                Assert.Equal(userData.Name, p.Name);
                Assert.Equal(userData.FirstName, p.GivenName);
                Assert.Equal(userData.LastName, p.Surname);
                Assert.Equal(userData.DisplayName, p.DisplayName);
                Assert.True(p.DistinguishedName.IndexOf(userData.Name, StringComparison.OrdinalIgnoreCase) >= 0);
                Assert.Equal(userData.Name, p.SamAccountName);
            }
        }

        private void ValidateRecentAddedGroup(PrincipalContext context, GroupData groupData)
        {
            using (GroupPrincipal p = FindGroup(groupData.Name, context))
            {
                Assert.NotNull(p);
                Assert.Equal(groupData.Name, p.Name);
                Assert.Equal(groupData.Description, p.Description);
                Assert.Equal(groupData.DisplayName, p.DisplayName);
                Assert.Equal(groupData.Name, p.SamAccountName);
                Assert.True(p.DistinguishedName.IndexOf(groupData.Name, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void ValidateUserUsingPrincipal(PrincipalContext context, UserPrincipal user)
        {
            using (UserPrincipal p = FindUser(user.Name, context))
            {
                Assert.NotNull(p);
                Assert.Equal(user.Name, p.Name);
                Assert.Equal(user.GivenName, p.GivenName);
                Assert.Equal(user.Surname, p.Surname);
                Assert.Equal(user.DisplayName, p.DisplayName);
                Assert.Equal(user.SamAccountName, p.SamAccountName);

                Assert.Equal(user.Guid, p.Guid);
                Assert.Equal(user.Sid, p.Sid);
                Assert.Equal(user.UserPrincipalName, p.UserPrincipalName);
                Assert.Equal(user.UserCannotChangePassword, p.UserCannotChangePassword);
                Assert.Equal(user.Enabled, p.Enabled);
                Assert.Equal(user.AccountExpirationDate, p.AccountExpirationDate);
            }
        }

        private void ValidateGroupUsingPrincipal(PrincipalContext context, GroupPrincipal group)
        {
            using (GroupPrincipal p = FindGroup(group.Name, context))
            {
                Assert.NotNull(p);
                Assert.Equal(group.Name, p.Name);
                Assert.Equal(group.DisplayName, p.DisplayName);
                Assert.Equal(group.SamAccountName, p.SamAccountName);

                Assert.Equal(group.Guid, p.Guid);
                Assert.Equal(group.Sid, p.Sid);
                Assert.Equal(group.UserPrincipalName, p.UserPrincipalName);
                Assert.Equal(group.DistinguishedName, p.DistinguishedName);
            }
        }

        private UserPrincipal CreateUser(PrincipalContext context, UserData userData)
        {
            UserPrincipal user = new UserPrincipal(context, userData.Name, userData.Password, true);

            // assign some properties to the user principal
            user.GivenName = userData.FirstName;
            user.Surname = userData.LastName;
            user.DisplayName = userData.DisplayName;
            user.Save();
            return user;
        }

        private CustomUserPrincipal CreateCustomUser(PrincipalContext context, CustomUserData userData)
        {
            CustomUserPrincipal user = new CustomUserPrincipal(context, userData.Name, userData.Password, true);

            // assign some properties to the custom user principal
            user.GivenName = userData.FirstName;
            user.Surname = userData.LastName;
            user.DisplayName = userData.DisplayName;
            user.PostalCode = userData.PostalCode;
            user.PostalAddress = userData.PostalAddress;
            user.Save();
            return user;
        }

        private GroupPrincipal CreateGroup(PrincipalContext context, GroupData groupData)
        {
            GroupPrincipal group = new GroupPrincipal(context, groupData.Name);
            group.Description = groupData.Description;
            group.DisplayName = groupData.DisplayName;
            group.Save();
            return group;
        }

        private void DeleteGroup(string groupName)
        {
            try
            {
                using (PrincipalContext context = DomainContext)
                using (GroupPrincipal p = FindGroup(groupName, context))
                {
                    if (p != null)
                        p.Delete();
                }
            }
            catch
            {
                // ignore the failure as we use this method to ensure clean up even if the group not exist
            }
        }

        private void DeleteUser(string userName)
        {
            try
            {
                using (PrincipalContext context = DomainContext)
                using (UserPrincipal p = FindUser(userName, context))
                {
                    if (p != null)
                        p.Delete();
                }
            }
            catch
            {
                // ignore the failure as we use this method to ensure clean up even if the user not exist
            }
        }

        private GroupPrincipal FindGroup(string groupName, PrincipalContext context)
        {
            return GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupName);
        }

        private UserPrincipal FindUser(string userName, PrincipalContext context)
        {
            return UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName);
        }

        private CustomUserPrincipal FindCustomUser(string userName, PrincipalContext context)
        {
            return CustomUserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName);
        }

        private UserPrincipal FindUserUsingFilter(string userName, PrincipalContext context)
        {
            CustomUserPrincipal userPrincipal = new CustomUserPrincipal(context);
            userPrincipal.SetUserNameFilter(userName);
            PrincipalSearcher searcher = new PrincipalSearcher(userPrincipal);
            return searcher.FindOne() as UserPrincipal;
        }

        private PrincipalContext DomainContext => new PrincipalContext(
                                                        ContextType.Domain,
                                                        LdapConfiguration.Configuration.ServerName,
                                                        LdapConfiguration.Configuration.UserName,
                                                        LdapConfiguration.Configuration.Password);
    }

    internal class UserData
    {
        internal static UserData GenerateUserData(string name)
        {
            UserData ud = new UserData();
            ud.Name = name;
            ud.Password = Guid.NewGuid().ToString() + "#1aZ";
            ud.FirstName = "First " + name;
            ud.LastName = "Last " + name;
            ud.DisplayName = "Display " + name;
            return ud;
        }

        internal string Name        { get; set; }
        internal string Password    { get; set; }
        internal string FirstName   { get; set; }
        internal string LastName    { get; set; }
        internal string DisplayName { get; set; }
    }

    internal class CustomUserData : UserData
    {
        internal static new CustomUserData GenerateUserData(string name) => new CustomUserData
        {
            Name = name,
            Password = Guid.NewGuid().ToString() + "#1aZ",
            FirstName = "First " + name,
            LastName = "Last " + name,
            DisplayName = "Display " + name,
            PostalAddress = new List<string> { "Postal Address " + name },
            PostalCode = "Code " + name
        };

        internal string PostalCode { get; set; }
        internal List<string> PostalAddress { get; set; }
    }

    internal class GroupData
    {
        internal static GroupData GenerateGroupData(string name)
        {
            GroupData gd = new GroupData();
            gd.Name = name;
            gd.Description = "Description " + name;
            gd.DisplayName = "Display " + name;
            return gd;
        }

        internal string Name        { get; set; }
        internal string Description { get; set; }
        internal string DisplayName { get; set; }
    }

    [DirectoryObjectClass("user")]
    [DirectoryRdnPrefix("CN")]
    public class CustomUserPrincipal : UserPrincipal
    {
        private CustomFilter _customFilter;

        public CustomUserPrincipal(PrincipalContext context) : base(context) { }
        public CustomUserPrincipal(PrincipalContext context, string samAccountName, string password, bool enabled)
            : base(context, samAccountName, password, enabled) { }

        public void SetUserNameFilter(string name)
        {
            ((CustomFilter) AdvancedSearchFilter).SetFilter(name);
        }

        public override AdvancedFilters AdvancedSearchFilter => _customFilter ??= new CustomFilter(this);

        // Custom properties
        [DirectoryProperty("postalCode")]
        public string PostalCode
        {
            get => ExtensionGet("postalCode").FirstOrDefault() as string;
            set => ExtensionSet("postalCode", value);
        }

        [DirectoryProperty("postalAddress")]
        public List<string> PostalAddress
        {
            get => ExtensionGet("postalAddress").OfType<string>().ToList();
            set => ExtensionSet("postalAddress", value == null || value?.Count == 0 ? null : value.ToArray());
        }

        // Method overrides
        public new static CustomUserPrincipal FindByIdentity(PrincipalContext context, IdentityType identityType, string identityValue)
        {
            return FindByIdentityWithType(context, typeof(CustomUserPrincipal), identityType, identityValue) as CustomUserPrincipal;
        }
    }

    public class CustomFilter : AdvancedFilters
    {
        public CustomFilter(Principal p) : base(p) { }

        public void SetFilter(string userName)
        {
            this.AdvancedFilterSet("samAccountName", userName, typeof(string), MatchType.Equals);
        }
    }
}
