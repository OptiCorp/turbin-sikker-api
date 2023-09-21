using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async void UserService_GetInspectorRoleId_ReturnsString()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("User");
            var userUtilities = new UserUtilities();
            var userService = new UserService(dbContext, userUtilities);

            //Act
            var inspectorId = await userService.GetInspectorRoleIdAsync();

            //Assert
            Assert.IsType<string>(inspectorId);
            Assert.Equal("Inspector", inspectorId);
        }

        [Fact]
        public async void UserService_GetAllUsers_ReturnsUserList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("User");
            var userUtilities = new UserUtilities();
            var userService = new UserService(dbContext, userUtilities);

            //Act
            var users = await userService.GetAllUsersAsync();

            //Assert
            Assert.IsType<List<UserDto>>(users);
            Assert.Equal(8, users.Count());
        }

        [Fact]
        public async void UserService_GetAllUsersAdmin_ReturnsUserList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("User");
            var userUtilities = new UserUtilities();
            var userService = new UserService(dbContext, userUtilities);

            //Act
            var users = await userService.GetAllUsersAdminAsync();

            //Assert
            Assert.IsType<List<UserDto>>(users);
            Assert.Equal(10, users.Count());
        }

        [Fact]
        public async void UserService_GetUserById_ReturnsUser()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("User");
            var userUtilities = new UserUtilities();
            var userService = new UserService(dbContext, userUtilities);

            //Act
            var user = await userService.GetUserByIdAsync("User 1");

            //Assert
            Assert.IsType<UserDto>(user);
            Assert.Equal("Username 1", user.Username);
            Assert.Equal("User 1", user.Id);
        }

        [Fact]
        public async void UserService_GetUserByAzureAdUserId_ReturnsUser()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("User");
            var userUtilities = new UserUtilities();
            var userService = new UserService(dbContext, userUtilities);

            //Act
            var user = await userService.GetUserByAzureAdUserIdAsync("AzureAD1@bouvet.no");

            //Assert
            Assert.IsType<User>(user);
            Assert.Equal("User 1", user.Id);
        }

        [Fact]
        public async void UserService_GetUserByUsername_ReturnsUser()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("User");
            var userUtilities = new UserUtilities();
            var userService = new UserService(dbContext, userUtilities);

            //Act
            var user = await userService.GetUserByUsernameAsync("Username 1");

            //Assert
            Assert.IsType<UserDto>(user);
            Assert.Equal("User 1", user.Id);
        }

        [Fact]
        public async void UserService_CreateUser_ReturnsString()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("User");
            var userUtilities = new UserUtilities();
            var userService = new UserService(dbContext, userUtilities);

            var newUser = new UserCreateDto
            {
                AzureAdUserId = "AzureAD10@bouvet.no",
                UserRoleId = "Inspector",
                FirstName = "name",
                LastName = "nameson",
                Email = "some email",
                Username = "Username 10",
            };

            //Act
            var newUserId = await userService.CreateUserAsync(newUser);
            var users = await userService.GetAllUsersAdminAsync();

            //Assert
            Assert.IsType<string>(newUserId);
            Assert.Equal(11, users.Count());
        }

        [Fact]
        public async void UserService_UpdateUser_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("User");
            var userUtilities = new UserUtilities();
            var userService = new UserService(dbContext, userUtilities);

            var updatedUser = new UserUpdateDto
            {
                Id = "User 1",
                UserRoleId = "Inspector",
                Username = "Username 10",
            };

            //Act
            await userService.UpdateUserAsync(updatedUser);
            var user = await userService.GetUserByIdAsync("User 1");

            //Assert
            Assert.Equal("Inspector", user.UserRole.Id);
            Assert.Equal("Username 10", user.Username);
        }

        [Fact]
        public async void UserService_DeleteUser_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("User");
            var userUtilities = new UserUtilities();
            var userService = new UserService(dbContext, userUtilities);

            var userId = "User 1";

            //Act
            await userService.DeleteUserAsync(userId);
            var users = await userService.GetAllUsersAsync();
            var user = await userService.GetUserByIdAsync(userId);

            //Assert
            Assert.Equal(7, users.Count());
            Assert.Equal("Deleted", user.Status);
        }

        [Fact]
        public async void UserService_HardDeleteUser_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("User");
            var userUtilities = new UserUtilities();
            var userService = new UserService(dbContext, userUtilities);

            var userId = "User 1";

            //Act
            await userService.HardDeleteUserAsync(userId);
            var users = await userService.GetAllUsersAdminAsync();
            var user = await userService.GetUserByIdAsync(userId);

            //Assert
            Assert.Equal(9, users.Count());
            Assert.Equal(null, user);
        }
    }
}