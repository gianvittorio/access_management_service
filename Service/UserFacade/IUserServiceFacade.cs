using AccessManagementService.Service.UserFacade.Dtos;

namespace AccessManagementService.Service.UserFacade;

public interface IUserServiceFacade
{
    Task<UserResponseDto?> FindUserByUserId(string userId);
    
    Task<UserResponseDto?> FindUserByEmail(string email);

    Task<UserResponseDto> CreateUser(UserRequestDto userRequestDto);

    Task<UserResponseDto> UpdateUser(string userId, UserRequestDto userRequestDto);

    Task<bool> RemoveUser(string userId);
}