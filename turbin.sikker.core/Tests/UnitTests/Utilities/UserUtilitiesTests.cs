
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests
{
    public class UserTests
    {
        private readonly IUserUtilities _userUtilities;

        public UserTests()
        {
            _userUtilities = new UserUtilities();
        }


        [Fact]
        public void IsValidStatus_pending_true()
        {
            Assert.True(_userUtilities.IsValidStatus("aCtIVe"));
        }

        [Fact]
        public void IsValidStatus_approved_true()
        {
            Assert.True(_userUtilities.IsValidStatus("DIsABLEd"));
        }

        [Fact]
        public void IsValidStatus_rejected_true()
        {
            Assert.True(_userUtilities.IsValidStatus("deletED"));
        }

        [Fact]
        public void IsValidStatus_completed_false()
        {
            Assert.False(_userUtilities.IsValidStatus("DeACTIvated"));
        }

        [Fact]
        public void IsUsernameTaken_sameName_True()
        {
            List<UserDto> users = new List<UserDto>();
            var user1 = new UserDto {
                Id = "1",
                AzureAdUserId = "azure1",
                FirstName = "firstname1",
                LastName = "lastname1",
                Email = "email1",
                Username = "username1",
                Status = "active",
                CreatedDate = DateTime.Now
            };
            var user2 = new UserDto {
                Id = "2",
                AzureAdUserId = "azure2",
                FirstName = "firstname2",
                LastName = "lastname2",
                Email = "email2",
                Username = "username2",
                Status = "active",
                CreatedDate = DateTime.Now
            };
            var user3 = new UserDto {
                Id = "3",
                AzureAdUserId = "azure3",
                FirstName = "firstname3",
                LastName = "lastname3",
                Email = "email3",
                Username = "username3",
                Status = "active",
                CreatedDate = DateTime.Now
            };

            users.Add(user1);
            users.Add(user2);
            users.Add(user3);

           var user4 = new UserDto {
                Id = "4",
                AzureAdUserId = "azure4",
                FirstName = "firstname4",
                LastName = "lastname4",
                Email = "email4",
                Username = "username1",
                Status = "active",
                CreatedDate = DateTime.Now
            };

            Assert.True(_userUtilities.IsUsernameTaken(users, user4.Username));
        }

        [Fact]
        public void IsUsernameTaken_differentName_False()
        {
            List<UserDto> users = new List<UserDto>();
            var user1 = new UserDto {
                Id = "1",
                AzureAdUserId = "azure1",
                FirstName = "firstname1",
                LastName = "lastname1",
                Email = "email1",
                Username = "username1",
                Status = "active",
                CreatedDate = DateTime.Now
            };
            var user2 = new UserDto {
                Id = "2",
                AzureAdUserId = "azure2",
                FirstName = "firstname2",
                LastName = "lastname2",
                Email = "email2",
                Username = "username2",
                Status = "active",
                CreatedDate = DateTime.Now
            };
            var user3 = new UserDto {
                Id = "3",
                AzureAdUserId = "azure3",
                FirstName = "firstname3",
                LastName = "lastname3",
                Email = "email3",
                Username = "username3",
                Status = "active",
                CreatedDate = DateTime.Now
            };

            users.Add(user1);
            users.Add(user2);
            users.Add(user3);

           var user4 = new UserDto {
                Id = "4",
                AzureAdUserId = "azure4",
                FirstName = "firstname4",
                LastName = "lastname4",
                Email = "email4",
                Username = "username4",
                Status = "active",
                CreatedDate = DateTime.Now
            };

            Assert.False(_userUtilities.IsUsernameTaken(users, user4.Username));
        }

        [Fact]
        public void IsEmailTaken_sameEmail_True()
        {
            List<UserDto> users = new List<UserDto>();
            var user1 = new UserDto {
                Id = "1",
                AzureAdUserId = "azure1",
                FirstName = "firstname1",
                LastName = "lastname1",
                Email = "email1",
                Username = "username1",
                Status = "active",
                CreatedDate = DateTime.Now
            };
            var user2 = new UserDto {
                Id = "2",
                AzureAdUserId = "azure2",
                FirstName = "firstname2",
                LastName = "lastname2",
                Email = "email2",
                Username = "username2",
                Status = "active",
                CreatedDate = DateTime.Now
            };
            var user3 = new UserDto {
                Id = "3",
                AzureAdUserId = "azure3",
                FirstName = "firstname3",
                LastName = "lastname3",
                Email = "email3",
                Username = "username3",
                Status = "active",
                CreatedDate = DateTime.Now
            };

            users.Add(user1);
            users.Add(user2);
            users.Add(user3);

           var user4 = new UserDto {
                Id = "4",
                AzureAdUserId = "azure4",
                FirstName = "firstname4",
                LastName = "lastname4",
                Email = "email1",
                Username = "username4",
                Status = "active",
                CreatedDate = DateTime.Now
            };

            Assert.True(_userUtilities.IsEmailTaken(users, user4.Email));
        }

        [Fact]
        public void IsEmailTaken_differentEmail_False()
        {
            List<UserDto> users = new List<UserDto>();
            var user1 = new UserDto {
                Id = "1",
                AzureAdUserId = "azure1",
                FirstName = "firstname1",
                LastName = "lastname1",
                Email = "email1",
                Username = "username1",
                Status = "active",
                CreatedDate = DateTime.Now
            };
            var user2 = new UserDto {
                Id = "2",
                AzureAdUserId = "azure2",
                FirstName = "firstname2",
                LastName = "lastname2",
                Email = "email2",
                Username = "username2",
                Status = "active",
                CreatedDate = DateTime.Now
            };
            var user3 = new UserDto {
                Id = "3",
                AzureAdUserId = "azure3",
                FirstName = "firstname3",
                LastName = "lastname3",
                Email = "email3",
                Username = "username3",
                Status = "active",
                CreatedDate = DateTime.Now
            };

            users.Add(user1);
            users.Add(user2);
            users.Add(user3);

           var user4 = new UserDto {
                Id = "4",
                AzureAdUserId = "azure4",
                FirstName = "firstname4",
                LastName = "lastname4",
                Email = "email4",
                Username = "username4",
                Status = "active",
                CreatedDate = DateTime.Now
            };

            Assert.False(_userUtilities.IsEmailTaken(users, user4.Email));
        }
    }
}