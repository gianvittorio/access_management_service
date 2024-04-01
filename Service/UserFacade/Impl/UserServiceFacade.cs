using AccessManagementService.Service.UserFacade.Dtos;
using AutoFixture;

namespace AccessManagementService.Service.UserFacade.Impl;

public class UserServiceFacade : IUserServiceFacade
{
    private static readonly Fixture AutoFixture = new();
    
    public Task<UserResponseDto?> FindUserByUserIdAsync(string userId)
    {
        var userResponseDto = AutoFixture.Build<UserResponseDto>()
            .With(user => user.Id, userId)
            .Create();

        return Task.FromResult(userResponseDto)!;
    }

    public Task<UserResponseDto?> FindUserByEmailAsync(string email)
    {
        var userResponseDto = AutoFixture.Build<UserResponseDto>()
            .With(user => user.Email, email)
            .Create();
        
        return Task.FromResult(userResponseDto)!;

        //return Task.FromResult<UserResponseDto?>(null);
    }
    
    public Task<UserResponseDto> SaveUserAsync(UserRequestDto userRequestDto)
    {
        var userResponseDto = new UserResponseDto
        {
            Id = Guid.NewGuid().ToString(),
            Email = userRequestDto.Email,
            AccessType = userRequestDto.AccessType,
            BirthDate = userRequestDto.BirthDate,
            Country = userRequestDto.Country,
            EmployerId = userRequestDto.EmployerId,
            FullName = userRequestDto.FullName,
            Password = userRequestDto.Password,
            Salary = userRequestDto.Salary
        };

        return Task.FromResult(userResponseDto);
    }
    
    public Task RemoveUserAsync(string userEmail)
    {
        return Task.CompletedTask;
    }
}