using Microsoft.AspNetCore.Identity;

namespace MathQuizCreatorAPI.Authentication
{
    public interface ITokenService
    {
        Task<AuthenticationResponse> GenerateJwtTokenAndRefreshToken(IdentityUser user);

        Task<AuthenticationResult> ValidateTokensAndGenerateJwtAndRefreshToken(TokenRequest tokenRequest);
    }
}
