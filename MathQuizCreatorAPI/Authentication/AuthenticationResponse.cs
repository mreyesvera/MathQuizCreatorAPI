namespace MathQuizCreatorAPI.Authentication
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Authentication Response Dto. Used to send back token data back to the
    /// user when they are authenticated. 
    /// The way this model is created and used is based on learning resources 
    /// and experience from my previous co-op at Medic.
    /// </summary>
    public class AuthenticationResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
