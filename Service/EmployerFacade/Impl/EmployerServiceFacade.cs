namespace AccessManagementService.Service.EmployerFacade.Impl;

public class EmployerServiceFacade : IEmployerServiceFacade
{
    public Task<string?> FindEmployerIdByEmployerName(string employerName)
    {
        return Task.FromResult(Guid.NewGuid().ToString())!;
    }
}