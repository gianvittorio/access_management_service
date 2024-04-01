using AccessManagementService.Service.UserFacade.Dtos;

namespace AccessManagementService.Service.UserFacade;

public interface IUserServiceFacade
{
    Task<UserResponseDto?> FindUserByEmailAsync(string email);
    
    Task<List<UserResponseDto>> FindUsersByEmployerIdAsync(string employerId);

    Task<UserResponseDto> SaveUserAsync(UserRequestDto userRequestDto);
    
    Task RemoveUserAsync(string userEmail);
}