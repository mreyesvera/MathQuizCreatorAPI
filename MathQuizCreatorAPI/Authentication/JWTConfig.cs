namespace MathQuizCreatorAPI.Authentication
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// JWT Config. Used in the process of authentication for the JWT tokens configuration.
    /// The way this model is created and used is based on learning resources 
    /// and experience from my previous co-op at Medic.
    /// </summary>
    public class JWTConfig
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan ExpiryTimeFrame { get; set; }
    }
}
