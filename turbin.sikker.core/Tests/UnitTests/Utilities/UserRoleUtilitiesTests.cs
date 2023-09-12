using turbin.sikker.core.Model;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests
{
    public class UserRoleTests
    {
        private readonly IUserRoleUtilities _userRoleUtilities;

        public UserRoleTests()
        {
            _userRoleUtilities = new UserRoleUtilities();
        }

        [Fact]
        public void IsUserRoleNameTaken_sameName_True()
        {
            List<UserRole> roles = new List<UserRole>();
            var role1 = new UserRole {
                Id = "1",
                Name = "Role 1"
            };
            var role2 = new UserRole {
                Id = "2",
                Name = "Role 2"
            };
            var role3 = new UserRole {
                Id = "3",
                Name = "Role 3"
            };

            roles.Add(role1);
            roles.Add(role2);
            roles.Add(role3);

           var role4 = new UserRole {
                Id = "4",
                Name = "Role 1"
            };

            Assert.True(_userRoleUtilities.IsUserRoleNameTaken(roles, role4.Name));
        }

        [Fact]
        public void IsUserRoleNameTaken_differentName_False()
        {
            List<UserRole> roles = new List<UserRole>();
            var role1 = new UserRole {
                Id = "1",
                Name = "Role 1"
            };
            var role2 = new UserRole {
                Id = "2",
                Name = "Role 2"
            };
            var role3 = new UserRole {
                Id = "3",
                Name = "Role 3"
            };

            roles.Add(role1);
            roles.Add(role2);
            roles.Add(role3);

           var role4 = new UserRole {
                Id = "4",
                Name = "Role 4"
            };

            Assert.False(_userRoleUtilities.IsUserRoleNameTaken(roles, role4.Name));
        }

        [Fact]
        public void IsValidUserRole_exists_True()
        {
            List<UserRole> roles = new List<UserRole>();
            var role1 = new UserRole {
                Id = "1",
                Name = "Role 1"
            };
            var role2 = new UserRole {
                Id = "2",
                Name = "Role 2"
            };
            var role3 = new UserRole {
                Id = "3",
                Name = "Role 3"
            };

            roles.Add(role1);
            roles.Add(role2);
            roles.Add(role3);

            Assert.True(_userRoleUtilities.IsValidUserRole(roles, role1.Id));
        }

        [Fact]
        public void IsValidUserRole_notExists_False()
        {
            List<UserRole> roles = new List<UserRole>();
            var role1 = new UserRole {
                Id = "1",
                Name = "Role 1"
            };
            var role2 = new UserRole {
                Id = "2",
                Name = "Role 2"
            };
            var role3 = new UserRole {
                Id = "3",
                Name = "Role 3"
            };

            roles.Add(role1);
            roles.Add(role2);
            roles.Add(role3);

           var role4 = new UserRole {
                Id = "4",
                Name = "Role 4"
            };

            Assert.False(_userRoleUtilities.IsValidUserRole(roles, role4.Id));
        }

    }
}