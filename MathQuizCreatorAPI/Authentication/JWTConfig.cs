namespace MathQuizCreatorAPI.Authentication
{
    public class JWTConfig
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan ExpiryTimeFrame { get; set; }
    }
}
