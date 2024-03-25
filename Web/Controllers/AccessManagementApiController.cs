using AccessManagementService.Web.Dtos.EmployerSignup;
using AccessManagementService.Web.Dtos.SelfSignup;
using Microsoft.AspNetCore.Mvc;

namespace AccessManagementService.Web.Controllers;

[ApiController]
[Route("/api/access_management/v1")]
public class AccessManagementApiController : Controller
{
    [HttpPost("pub/self_signup")]
    public ActionResult SelfSignup(SelfSignupRequestDto selfSignupRequestDto)
    {
        return Ok();
    }
    
    [HttpPost("pub/employer_signup")]
    public ActionResult EmployerSignup(EmployerSignupRequestDto employerSignupRequestDto)
    {
        return Ok();
    }
}