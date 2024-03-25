using AccessManagementService.Service.AccessManagement.Model;
using AutoFixture;

namespace AccessManagementService.Service.AccessManagement.Impl;

public class AccessManagementService : IAccessManagementService
{
    private static readonly Fixture AutoFixture = new Fixture();
    
    public Task<User> FindUserByEmail(string email)
    {
        var persistedUser = AutoFixture.Build<User>()
            .With(user => user.Email, email)
            .Create();

        return Task.FromResult(persistedUser);
    }

    public Task<User> CreateUser(User newUser)
    {
        return Task.FromResult(newUser);
    }

    public Task<User> UpdateUser(string userId, User persistedUser)
    {
        var updatedUser = new User
        {
            UserId = userId,
            Email = persistedUser.Email,
            Password = persistedUser.Password,
            Country = persistedUser.Country,
            AccessType = persistedUser.AccessType,
            FullName = persistedUser.FullName,
            EmployerId = persistedUser.EmployerId,
            BirthDate = persistedUser.BirthDate,
            Salary = persistedUser.Salary
        };

        return Task.FromResult(updatedUser);
    }

    public Task<bool> RemoveUser(string userId)
    {
        return Task.FromResult(true);
    }
}