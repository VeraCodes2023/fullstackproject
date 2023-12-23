using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using ECommerceBusiness;
using ECommerceCore;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceController;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private IAuthService _service;
    public AuthController(IAuthService service)
    {
        _service = service;
    }


    [HttpPost("login")]
    public ActionResult<ServiceResponse<string>>Login([FromBody] Credential credential)
    {
        var serviceResponse = new ServiceResponse<string>();
        try
        {
            string token = _service.Login(credential);

            if (!string.IsNullOrEmpty(token))
            {
                serviceResponse.Data = token;
                serviceResponse.Success = true;
                serviceResponse.Message = "Login successful!";
                serviceResponse.StatusCode = 200; 
                return Ok(serviceResponse);
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Invalid credentials.";
                serviceResponse.StatusCode = 401; 
                return Unauthorized(serviceResponse);
            }
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
            serviceResponse.StatusCode = 500; 
            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }
    }
}
