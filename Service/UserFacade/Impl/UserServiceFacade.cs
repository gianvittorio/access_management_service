using AccessManagementService.Service.UserFacade.Dtos;
using AutoFixture;

namespace AccessManagementService.Service.UserFacade.Impl;

public class UserServiceFacade : IUserServiceFacade
{
    private static readonly Fixture AutoFixture = new();
    
    public Task<UserResponseDto?> FindUserByUserId(string userId)
    {
        var userResponseDto = AutoFixture.Build<UserResponseDto>()
            .With(user => user.UserId, userId)
            .Create();

        return Task.FromResult(userResponseDto)!;
    }

    public Task<UserResponseDto?> FindUserByEmail(string email)
    {
        // var userResponseDto = AutoFixture.Build<UserResponseDto>()
        //     .With(user => user.Email, email)
        //     .Create();
        //
        // return Task.FromResult(userResponseDto)!;

        return Task.FromResult<UserResponseDto?>(null);
    }

    public Task<UserResponseDto> CreateUser(UserRequestDto userRequestDto)
    {
        var userResponseDto = new UserResponseDto
        {
            UserId = Guid.NewGuid().ToString(),
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

    public Task<UserResponseDto> UpdateUser(string userId, UserRequestDto userRequestDto)
    {
        var userResponseDto = new UserResponseDto
        {
            UserId = userRequestDto.UserId ?? string.Empty,
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

    public Task<bool> RemoveUser(string userId)
    {
        return Task.FromResult(true);
    }
}