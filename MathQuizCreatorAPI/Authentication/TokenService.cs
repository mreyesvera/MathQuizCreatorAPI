using MathQuizCreatorAPI.Data;
using MathQuizCreatorAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MathQuizCreatorAPI.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(AppDbContext context, IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
        }

        public async Task<AuthenticationResponse> GenerateJwtTokenAndRefreshToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            //var secretKey = Encoding.UTF8.GetBytes(_config.GetSection("JWT:SecretKey").Value);
            var secretKey = Encoding.UTF8.GetBytes(_config.GetSection("JWT:SecretKey").Value);
            var issuer = _config.GetSection("JWT:Issuer").Value;
            var audience = _config.GetSection("JWT:Audience").Value;

            // Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Claims Identity
            var claimsIdentity = new ClaimsIdentity(claims, "serverAuth");

            // Token valid within ExpiryTimeFrame
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config.GetSection("JWT:ExpiryTimeFrame").Value)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer,
                Audience = audience,
            };

            // Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Convert write object token into string
            var jwtToken = tokenHandler.WriteToken(token);

            // Refresh Token
            var refreshToken = new RefreshToken
            {
                AddedDate = DateTime.Now,
                Token = $"{RandomStringGenerator(25)}_{Guid.NewGuid()}",
                UserId = user.Id,
                IsRevoked = false,
                IsUsed = false,
                JwtId = token.Id,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            _context.RefreshToken.Add(refreshToken);
            await _context.SaveChangesAsync();

            var tokenResponse = new AuthenticationResponse
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token
            };

            // Return tokens
            return tokenResponse;
        }

        public async Task<AuthenticationResult> ValidateTokensAndGenerateJwtAndRefreshToken(TokenRequest tokenRequest)
        {
            //var secretKey = Encoding.UTF8.GetBytes(_config.GetSection("JWT:SecretKey").Value);
            var secretKey = Encoding.UTF8.GetBytes(_config.GetSection("JWT:SecretKey").Value);
            var issuer = _config.GetSection("JWT:Issuer").Value;
            var audience = _config.GetSection("JWT:Audience").Value;

            try
            {
                // Validation Parameters
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateLifetime = false,
                    RequireExpirationTime = true
                };

                // Token Handler
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;

                // Validate token
                var principle = tokenHandler.ValidateToken(tokenRequest.AccessToken, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = (JwtSecurityToken)securityToken;

                var result = jwtSecurityToken != null 
                    && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if (!result)
                {
                    return null;
                }

                // Check if jwt token is expired, no need to regenerate if it's not expired
                var utcExpiryDate = long.Parse(principle.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

                // Convert date for comparison
                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if(expiryDate > DateTime.UtcNow.AddSeconds(1))
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "JWT token has not expired."
                        }
                    };
                }

                // Check if refresh token exists
                var refreshTokenExists = await _context.RefreshToken
                    .Where(rf => rf.Token == tokenRequest.RefreshToken)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if(refreshTokenExists == null)
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Invalid refresh token."
                        }
                    };
                }

                // Check if the refresh token has expired
                if(refreshTokenExists.ExpiryDate < DateTime.UtcNow)
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Refresh token has expired, please login again."
                        }
                    };
                }

                // Check if the refresh token has been used or not
                if (refreshTokenExists.IsUsed)
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Refresh token has been used, it cannot be used again."
                        }
                    };
                }

                // Check if refresh token has been revoked
                if (refreshTokenExists.IsRevoked)
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Refresh token has been revoked, it cannot be used."
                        }
                    };
                }

                // Check reference of the token
                var jti = principle.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
                if(refreshTokenExists.JwtId != jti)
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Refresh token referance does not match the jwt token."
                        }
                    };
                }

                // Start new token
                refreshTokenExists.IsUsed = true;
                _context.Update(refreshTokenExists);
                await _context.SaveChangesAsync();

                // Retrieve user
                var user = await _context.Users
                    .Where(u => u.Id == refreshTokenExists.UserId)
                    .FirstOrDefaultAsync();

                if(user == null)
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "User could not be found."
                        }
                    };
                }

                // Generate new JWT token
                var tokens = await GenerateJwtTokenAndRefreshToken(user);

                if(tokens == null)
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                            {
                                "Could not create tokens."
                            }
                    };
                }

                // Return new tokens
                return new AuthenticationResult()
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken,
                    Success = true,
                };

            }
            catch (Exception ex)
            { // Console log any unexpected error

                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }

        }

        private string RandomStringGenerator(int length)
        {
            var random = new Random();
            const string allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(allChars, length)
                .Select(ac => ac[random.Next(ac.Length)]).ToArray()
            );
        }

        private DateTime UnixTimeStampToDateTime(long unixDate)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixDate).ToUniversalTime();
            return dateTime;
        }
    }
}
