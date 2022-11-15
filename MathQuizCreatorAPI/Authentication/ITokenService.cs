using MathQuizCreatorAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace MathQuizCreatorAPI.Authentication
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Token Service Interface. Used so the Token Service can implement it. 
    /// It is used so that the service can be added through the Program.cs, so
    /// the authentication controller can use it. 
    /// </summary>
    public interface ITokenService
    {
        Task<AuthenticationResponse> GenerateJwtTokenAndRefreshToken(ApplicationUser user);

        Task<AuthenticationResult> ValidateTokensAndGenerateJwtAndRefreshToken(TokenRequest tokenRequest);
    }
}
