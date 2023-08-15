using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace turbin.sikker.core.Services
{
    public class UserService : IUserService
    {
        private readonly TurbinSikkerDbContext _context;
        private readonly IChecklistWorkflowService _checklistWorkflowService; // Add this field


        public UserService(TurbinSikkerDbContext context, IChecklistWorkflowService checklistWorkflowService)
        {
            _context = context;
            _checklistWorkflowService = checklistWorkflowService;
        }


        public bool IsUsernameTaken(IEnumerable<UserDto> users, string username)
        {
            return users.Any(u => u.Username == username);
        }

        public bool IsEmailTaken(IEnumerable<UserDto> users, string userEmail)
        {
            return users.Any(u => u.Email == userEmail);
        }

        public bool IsValidStatus(string value)
        {
            string lowerCaseValue = value.ToLower();
            return lowerCaseValue == "active" || lowerCaseValue == "disabled" || lowerCaseValue == "deleted";
        }

        private static string GetUserStatus(UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Active:
                    return "Active";
                case UserStatus.Disabled:
                    return "Disabled";
                case UserStatus.Deleted:
                    return "Deleted";
                default:
                    return "Active";
            }
        }

        public string GetInspectorRoleId()
        {
            var inspectorRole = _context.UserRole.FirstOrDefault(role => role.Name == "Inspector");
            return inspectorRole?.Id;

        }

        public IEnumerable<UserDto> GetUsers()
        {
            using (var dbContext = _context)
            {
                var users = dbContext.User
                    .Include(u => u.UserRole)
                    .Where(u => u.Status == UserStatus.Active)
                    .ToList();

                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    var userDto = new UserDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Username = user.Username,
                        UserRole = user.UserRole,
                        Status = GetUserStatus(user.Status),
                        CreatedDate = user.CreatedDate,
                        UpdatedDate = user.UpdatedDate,
                        AzureAdUserId = user.AzureAdUserId
                    };

                    if (!string.IsNullOrEmpty(user.Id))
                    {
                        userDto.ChecklistWorkflows = _checklistWorkflowService.GetAllChecklistWorkflowsByUserId(user.Id).ToList();
                    }
                    else
                    {
                        userDto.ChecklistWorkflows = new List<ChecklistWorkflow>();
                    }

                    userDtos.Add(userDto);
                }

                return userDtos;
            }
        }


        public IEnumerable<UserDto> GetAllUsers()
        {
            using (var dbContext = _context)
            {
                var users = dbContext.User
                    .Include(u => u.UserRole)
                    .ToList();
                var userDtos = new List<UserDto>();
                foreach (var user in users)
                {
                    var userDto = new UserDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Username = user.Username,
                        UserRole = user.UserRole,
                        Status = GetUserStatus(user.Status),
                        CreatedDate = user.CreatedDate,
                        UpdatedDate = user.UpdatedDate,
                        AzureAdUserId = user.AzureAdUserId
                    };
                    if (!string.IsNullOrEmpty(user.Id))
                    {
                        userDto.ChecklistWorkflows = _checklistWorkflowService.GetAllChecklistWorkflowsByUserId(user.Id).ToList();
                    }
                    else
                    {
                        userDto.ChecklistWorkflows = new List<ChecklistWorkflow>();
                    }
                    userDtos.Add(userDto);
                }
                return userDtos;
            }
        }

        public User GetUserById(string id)
        {
            return _context.User.Include(u => u.UserRole).FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByAzureAdUserId(string azureAdUserId)
        {
            return _context.User.Include(u => u.UserRole).FirstOrDefault(u => u.AzureAdUserId == azureAdUserId);
        }

        public User GetUserByUsername(string username)
        {
            return _context.User.Include(u => u.UserRole).FirstOrDefault(u => u.Username == username);
        }


        public void CreateUser(UserCreateDto userDto)
        {
            var user = new User
            {
                AzureAdUserId = userDto.AzureAdUserId,
                Username = userDto.Username,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                UserRoleId = userDto.UserRoleId,
                CreatedDate = DateTime.Now,
            };
            _context.User.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(string userId, UserUpdateDto updatedUserDto)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                if (updatedUserDto.Username != null)
                    user.Username = updatedUserDto.Username;

                if (updatedUserDto.FirstName != null)
                    user.FirstName = updatedUserDto.FirstName;

                if (updatedUserDto.LastName != null)
                    user.LastName = updatedUserDto.LastName;

                if (updatedUserDto.Email != null)
                    user.Email = updatedUserDto.Email;

                if (updatedUserDto.UserRoleId != null)
                    user.UserRoleId = updatedUserDto.UserRoleId;

                if (updatedUserDto.Status != null)
                {
                    string status = updatedUserDto.Status.ToLower();

                    if (status == "active")
                    {
                        user.Status = UserStatus.Active;
                    }
                    else if (status == "disabled")
                    {
                        user.Status = UserStatus.Disabled;
                    }
                    else if (status == "deleted")
                    {
                        user.Status = UserStatus.Deleted;
                    }
                }

                user.UpdatedDate = DateTime.Now;

                _context.SaveChanges();

            }
        }

        public void DeleteUser(string id)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                user.Status = UserStatus.Deleted;
                _context.SaveChanges();
            }
        }

        public void HardDeleteUser(string id)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }
        }

    }

}