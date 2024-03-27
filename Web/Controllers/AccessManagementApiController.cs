using System.Net;
using AccessManagementService.Service.AccessManagement;
using AccessManagementService.Web.Dtos.EmployerSignup;
using AccessManagementService.Web.Dtos.SelfSignup;
using Microsoft.AspNetCore.Mvc;

namespace AccessManagementService.Web.Controllers;

[ApiController]
[Route("/api/access_management/v1")]
public class AccessManagementApiController(IAccessManagementService accessManagementService) : Controller
{
    private readonly IAccessManagementService _accessManagementService = accessManagementService;

    [HttpPost("pub/self_signup")]
    public async Task<ActionResult<SelfSignupResponseDto>> SelfSignup(SelfSignupRequestDto selfSignupRequestDto)
    {
        try
        {
            var selfSignUpResult = await 
                _accessManagementService.SelfSignUpAsync(selfSignupRequestDto.Email, selfSignupRequestDto.Password, selfSignupRequestDto.Country, selfSignupRequestDto.EmployerName);
            var selfSignupResponseDto = new SelfSignupResponseDto
            {
                UserId = selfSignUpResult.UserId,
                UserAccessType = selfSignUpResult.UserAccessType.ToString()
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
    public async Task<ActionResult> EmployerSignup(EmployerSignupRequestDto employerSignupRequestDto)
    {
        try
        {
            var fileProcessingResult =
                await _accessManagementService.SaveEligibilityMetadataAsync(employerSignupRequestDto.FileUrl,
                    employerSignupRequestDto.EmployerName);
            var employerSignupResponseDto = new EmployerSignupResponseDto
            {
                ProcessedLines = fileProcessingResult.ProcessedLines,
                SkippedLines = fileProcessingResult.SkippedLines
            };

            return Ok(employerSignupResponseDto);
        }
        catch (Exception exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
        }
    }
}