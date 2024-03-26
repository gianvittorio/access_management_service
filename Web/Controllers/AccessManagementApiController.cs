using System.Net;
using AccessManagementService.Service.AccessManagement;
using AccessManagementService.Web.Dtos.EmployerSignup;
using AccessManagementService.Web.Dtos.SelfSignup;
using Microsoft.AspNetCore.Mvc;

namespace AccessManagementService.Web.Controllers;

[ApiController]
[Route("/api/access_management/v1")]
public class AccessManagementApiController : Controller
{
    private readonly IAccessManagementService _accessManagementService;

    public AccessManagementApiController(IAccessManagementService accessManagementService)
    {
        _accessManagementService = accessManagementService;
    }

    [HttpPost("pub/self_signup")]
    public async Task<ActionResult> SelfSignup(SelfSignupRequestDto selfSignupRequestDto)
    {
        try
        {
            var selfSignUpResult = await 
                _accessManagementService.SelfSignUp(selfSignupRequestDto.Email, selfSignupRequestDto.Password, selfSignupRequestDto.Country);
            var selfSignupResponseDto = new SelfSignupResponseDto
            {
                SignedIn = selfSignUpResult.SignedIn,
                UserId = selfSignUpResult.UserId,
                EmployerId = selfSignUpResult.EmployerId
            };

            return Ok(selfSignupResponseDto);
        }
        catch (ArgumentException argumentException)
        {
            return BadRequest(argumentException.Message);
        }
        catch (Exception exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
        }
    }
    
    [HttpPost("pub/employer_signup")]
    public ActionResult EmployerSignup(EmployerSignupRequestDto employerSignupRequestDto)
    {
        return Ok();
    }
}