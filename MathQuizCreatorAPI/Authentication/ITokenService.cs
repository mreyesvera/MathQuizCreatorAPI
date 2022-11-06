using MathQuizCreatorAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace MathQuizCreatorAPI.Authentication
{
    public interface ITokenService
    {
        Task<AuthenticationResponse> GenerateJwtTokenAndRefreshToken(ApplicationUser user);

        Task<AuthenticationResult> ValidateTokensAndGenerateJwtAndRefreshToken(TokenRequest tokenRequest);
    }
}
