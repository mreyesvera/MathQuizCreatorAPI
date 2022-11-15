using MathQuizCreatorAPI.DTOs.Authentication;

namespace MathQuizCreatorAPI.Authentication
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Authentication Result Dto. Used to send back result data to the user
    /// when trying to authenticate.
    /// The way this model is created and used is based on learning resources 
    /// and experience from my previous co-op at Medic.
    /// </summary>
    public class AuthenticationResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public UserSimplifiedDto? User { get; set; }
    }
}
