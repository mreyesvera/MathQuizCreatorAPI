using MathQuizCreatorAPI.Authentication;
using MathQuizCreatorAPI.DTOs;
using MathQuizCreatorAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MathQuizCreatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _config;
        private ITokenService _tokenService;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, 
            IConfiguration config, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginData)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(loginData.Email);
                
                var invalidCredentialsResult = new AuthenticationResult()
                {
                    Errors = new List<string>()
                        {
                            "Invalid credentials."
                        },
                    Success = false
                };

                if (existingUser == null)
                {
                    //return Unauthorized();
                    return BadRequest(invalidCredentialsResult);
                }

                var validPassword = await _userManager.CheckPasswordAsync(existingUser, loginData.Password);

                if (!validPassword)
                {
                    return BadRequest(invalidCredentialsResult);
                }

                var authenticationResponse = await _tokenService.GenerateJwtTokenAndRefreshToken(existingUser);

                var userRoles = await _userManager.GetRolesAsync(existingUser);

                if (userRoles == null || userRoles.Count != 1)
                {
                    return BadRequest(new AuthenticationResult()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid user."
                        },
                        Success = false
                    });
                }

                var userRole = userRoles.ElementAt(0);

                return Ok(new AuthenticationResult()
                {
                    AccessToken = authenticationResponse.AccessToken,
                    RefreshToken = authenticationResponse.RefreshToken,
                    Success = true,
                    User = new UserSimplifiedDto()
                    {
                        UserId = existingUser.Id,
                        UserName = existingUser.UserName,
                        Email = existingUser.Email,
                        Role = userRole,
                    }
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerData)
        {
            try
            {
                // Validate
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if email exists
                var userExists = await _userManager.FindByEmailAsync(registerData.Email);

                if(userExists != null)
                {
                    return BadRequest(new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Email already exists."
                        }
                    });
                }

                var newUser = new ApplicationUser()
                {
                    Email = registerData.Email,
                    UserName = registerData.Username,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                var isCreated = await _userManager.CreateAsync(newUser, registerData.Password);

                if (!isCreated.Succeeded)
                {
                    return BadRequest(new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Could not create user."
                        }
                    });
                }

                bool doesRoleExist = await _roleManager.RoleExistsAsync(registerData.Role);
                if (!doesRoleExist)
                {
                    return BadRequest(new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                            {
                                "Invalid Role"
                            }
                    });
                }

                await _userManager.AddToRoleAsync(newUser, registerData.Role);

                return Created("", registerData);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            try
            {
                var result = await _tokenService.ValidateTokensAndGenerateJwtAndRefreshToken(tokenRequest);

                if(result == null)
                {
                    return BadRequest(new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token validation failed."
                        }
                    });
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }
    }
}
