using AccessManagementService.Domain.Core.Lib.EligibilityFileProcessing;
using CsvHelper.Configuration;

namespace AccessManagementService.Service.AccessManagement.Model;

public class UserMap : ClassMap<User>
{
    public UserMap()
    {
        Map(m => m.Email).Name("email");
        Map(m => m.FullName).Name("full_name");
        Map(m => m.Country).Name("country");
        Map(m => m.BirthDate).Name("birth_date");
        Map(m => m.Salary).Name("salary");
    }
}