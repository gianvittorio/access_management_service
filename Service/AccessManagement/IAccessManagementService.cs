using AccessManagementService.Service.AccessManagement.Model;

namespace AccessManagementService.Service.AccessManagement;

public interface IAccessManagementService
{
    Task<User> FindUserByEmail(string email);

    Task<User> CreateUser(User newUser);

    Task<User> UpdateUser(string userId, User persistedUser);

    Task<bool> RemoveUser(string userId);
}