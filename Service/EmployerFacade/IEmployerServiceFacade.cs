using AccessManagementService.Service.EmployerFacade.Dtos;

namespace AccessManagementService.Service.EmployerFacade;

public interface IEmployerServiceFacade
{
    Task<string> FindEmployerIdByEmployerName(string employerName);
}