using System.Globalization;
using System.Net;
using AccessManagementService.Domain.Core.Lib.EligibilityFileProcessing;
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
            var userCredentials = new UserCredentials
            {
                Email = selfSignupRequestDto.Email,
                Password = selfSignupRequestDto.Password,
                EmployerName = selfSignupRequestDto.EmployerName,
                FullName = selfSignupRequestDto.FullName ?? string.Empty,
                Country = selfSignupRequestDto.Country ?? string.Empty,
                BirthDate = selfSignupRequestDto.BirthDate is null ? DateTime.MinValue.ToString(CultureInfo.InvariantCulture) : selfSignupRequestDto.BirthDate.Value.ToString(CultureInfo.InvariantCulture),
                Salary = selfSignupRequestDto.Salary ?? Decimal.Zero
            };
            var selfSignUpResult = await 
                _accessManagementService.SelfSignUpAsync(userCredentials);
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