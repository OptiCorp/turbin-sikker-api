using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests.Services
{
    public class UserRoleServiceTests
    {
        [Fact]
        public async void UserRoleService_IsUserRoleInUse_ReturnsBool()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("UserRole");
            var userRoleUtilities = new UserRoleUtilities();
            var userRoleService = new UserRoleService(dbContext, userRoleUtilities);

            var notInUseUserRole = new UserRole
            {
                Id = "UserRole 10",
                Name = "UserRole 10"
            };

            var inUseUserRole = await userRoleService.GetUserRoleById("UserRole 1");

            //Act
            var notInUse = await userRoleService.IsUserRoleInUse(notInUseUserRole);
            var inUse = await userRoleService.IsUserRoleInUse(inUseUserRole);

            //Assert
            Assert.IsType<bool>(notInUse);
            Assert.IsType<bool>(inUse);
            Assert.Equal(inUse, true);
            Assert.Equal(notInUse, false);
        }

        [Fact]
        public async void UserRoleService_GetUserRoles_ReturnsUserRoleList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("UserRole");
            var userRoleUtilities = new UserRoleUtilities();
            var userRoleService = new UserRoleService(dbContext, userRoleUtilities);

            //Act
            var userRoles = await userRoleService.GetUserRoles();

            //Assert
            Assert.IsType<List<UserRole>>(userRoles);
            Assert.Equal(userRoles.Count(), 10);
        }

        [Fact]
        public async void UserRoleService_GetUserRoleById_ReturnsUserRole()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("UserRole");
            var userRoleUtilities = new UserRoleUtilities();
            var userRoleService = new UserRoleService(dbContext, userRoleUtilities);

            var id = "UserRole 1";

            //Act
            var userRole = await userRoleService.GetUserRoleById(id);

            //Assert
            Assert.IsType<UserRole>(userRole);
            Assert.Equal(userRole.Id, "UserRole 1");
            Assert.Equal(userRole.Name, "UserRole 1");
        }

        [Fact]
        public async void UserRoleService_GetUserRoleByName_ReturnsRole()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("UserRole");
            var userRoleUtilities = new UserRoleUtilities();
            var userRoleService = new UserRoleService(dbContext, userRoleUtilities);

            var roleName = "UserRole 1";

            //Act
            var userRole = await userRoleService.GetUserRoleByUserRoleName(roleName);

            //Assert
            Assert.IsType<UserRole>(userRole);
            Assert.Equal(userRole.Id, "UserRole 1");
            Assert.Equal(userRole.Name, "UserRole 1");
        }

        [Fact]
        public async void UserRoleService_CreateUserRole_ReturnsString()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("UserRole");
            var userRoleUtilities = new UserRoleUtilities();
            var userRoleService = new UserRoleService(dbContext, userRoleUtilities);

            var newUserRoleCreateDto = new UserRoleCreateDto
            {
                Name = "UserRole 10"
            };

            //Act
            var newUserRoleId = await userRoleService.CreateUserRole(newUserRoleCreateDto);
            var newUserRole = await userRoleService.GetUserRoleById(newUserRoleId);
            var userRoles = await userRoleService.GetUserRoles();

            //Assert
            Assert.IsType<string>(newUserRoleId);
            Assert.Equal(newUserRole.Name, "UserRole 10");
            Assert.Equal(userRoles.Count(), 11);
        }

        [Fact]
        public async void UserRoleService_UpdateUserRole_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("UserRole");
            var userRoleUtilities = new UserRoleUtilities();
            var userRoleService = new UserRoleService(dbContext, userRoleUtilities);

            var updateUserRoleDto = new UserRoleUpdateDto
            {
                Name = "UserRole 10",
                Id = "UserRole 1"
            };

            //Act
            await userRoleService.UpdateUserRole(updateUserRoleDto);
            var updatedUserRole = await userRoleService.GetUserRoleById("UserRole 1");

            //Assert
            Assert.Equal(updatedUserRole.Name, "UserRole 10");
        }
    }
}