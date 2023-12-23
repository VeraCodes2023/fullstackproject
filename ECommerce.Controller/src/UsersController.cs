using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core;
using ECommerceBusiness;
using ECommerceCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;


namespace ECommerce.Controller.UserController
{
   
    [ApiController]
    [Route("api/v1/[controller]")]
    [EnableCors("AllowAny")]
    public class UsersController : ControllerBase
    {
        private  IUserService _userService;
        private IAuthService _authService;
         private ITokenService _tokenService;

        public UsersController(IUserService userService,IAuthService authService, ITokenService tokenService)
        {
            _userService = userService;
            _authService = authService;
            _tokenService= tokenService;
        }

        [HttpGet(), Authorize(Roles = "Admin")]
        public ActionResult<ServiceResponse<IEnumerable<UserReadDTO>>> GetAllUsers([FromQuery] UserQueryParameters options)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<UserReadDTO>>();
            try
            {
                var users = _userService.GetAllUsers(options);
                serviceResponse.Success = true;
                serviceResponse.Data = users;
                serviceResponse.Message = "All users retrieved successfully!";
                serviceResponse.StatusCode = 200; 
                return Ok(serviceResponse);
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500; 
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        }
        [HttpPost()]
        public ActionResult<ServiceResponse<UserReadDTO>> Register([FromBody] UserCreateDTO userCreateDTO)
        {
            var serviceResponse = new ServiceResponse<UserReadDTO>();
            try
            {
                var registeredUser = _userService.Register(userCreateDTO);
                serviceResponse.Success = true;
                serviceResponse.Data = registeredUser;
                serviceResponse.Message = "User registered successfully!";
                serviceResponse.StatusCode = 201; 
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500;
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        }
        
        [HttpPost("login")]
        public ActionResult<ServiceResponse<string>>Login([FromBody] Credential credential)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                var loginResult = _userService.Login(credential);
                serviceResponse.Success = true;
                serviceResponse.Data = loginResult;
                serviceResponse.Message = "Login successful!";
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500;
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        }

        [HttpGet("{id}"),Authorize(Roles = "Admin")]
        public ActionResult<ServiceResponse<UserReadDTO>>  GetSingle(Guid id)
        {
            var serviceResponse = new ServiceResponse<UserReadDTO>();
            try
            {
                var user = _userService.GetUserById(id);
                serviceResponse.Success = true;
                serviceResponse.Data = user;
                serviceResponse.Message = "User is retreived Successfully!";
                serviceResponse.StatusCode =200;
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode=500;
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
           
        }

        [HttpDelete("{id}"),Authorize(Roles = "Admin")]
        public ActionResult< ServiceResponse <bool>> DeleteUser(Guid id)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var deletionResult = _userService.DeleteUser(id);
                serviceResponse.Success = true;
                serviceResponse.Data = deletionResult;
                serviceResponse.StatusCode = 200;
                serviceResponse.Message = "User deleted successfully!";
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500;
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        }

        [HttpPut("{id}"),Authorize(Roles = "Admin")]
        public ActionResult<ServiceResponse<UserReadDTO>> UpdateUser(Guid id,[FromBody] UserUpdateDTO user)
        {
            var serviceResponse = new ServiceResponse<UserReadDTO>();
            try
            {
                var updatedUser = _userService.UpdateUser(id, user);
                serviceResponse.Success = true;
                serviceResponse.Data = updatedUser;
                serviceResponse.Message = "User updated successfully!";
                serviceResponse.StatusCode = 200;
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500;
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        }
     
        [HttpGet("profile/{id}"),Authorize()]
        public ActionResult<ServiceResponse<UserReadDTO>>GetUserProfile(Guid id)
        {
            var serviceResponse = new ServiceResponse<UserReadDTO>();
            try
            {
                var userProfile = _userService.GetUserProfile(id);
                serviceResponse.Success = true;
                serviceResponse.Data = userProfile;
                serviceResponse.StatusCode = 200;
                serviceResponse.Message = "User profile retrieved successfully!";
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500;
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        }

        [HttpPut("profile/{id}"),Authorize()]
        public ActionResult< ServiceResponse<UserReadDTO>> UpdateUserProfile(Guid id,[FromBody] UserUpdateDTO updatedProfile)
        {
            var serviceResponse = new ServiceResponse<UserReadDTO>();
            try
            {
                var userProfile = _userService.UpdateUserProfile(id, updatedProfile);
                serviceResponse.Success = true;
                serviceResponse.Data = userProfile;
                serviceResponse.StatusCode = 200;
                serviceResponse.Message = "User profile updated successfully!";
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500;
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        }

        [HttpGet("getbyemail", Name = "GetUserByEmail"),Authorize(Roles = "Admin")]
        public ActionResult<ServiceResponse<UserReadDTO>> GetUserByEmail([FromQuery(Name = "email")] string email)
        {
            var serviceResponse = new ServiceResponse<UserReadDTO>();
            try
            {
                var user = _userService.GetUserByEmail(email);
                serviceResponse.Success = true;
                serviceResponse.Data = user;
                serviceResponse.Message = "User retrieved by email successfully!";
                serviceResponse.StatusCode = 200;
                return Ok(serviceResponse);
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
}
