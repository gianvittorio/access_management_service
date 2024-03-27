using AccessManagementService.Service.UserFacade.Dtos;

namespace AccessManagementService.Service.UserFacade;

public interface IUserServiceFacade
{
    Task<UserResponseDto?> FindUserByUserIdAsync(string userId);
    
    Task<UserResponseDto?> FindUserByEmailAsync(string email);

    Task<UserResponseDto> SaveUserAsync(UserRequestDto userRequestDto);
    
    Task RemoveUserAsync(string userEmail);
}