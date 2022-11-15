using Microsoft.AspNetCore.Identity;

namespace MathQuizCreatorAPI.Models
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// This model is inherits from the IdentityRole. 
    /// It has been included to override the default data type for
    /// the id of string, and use Guid instead. 
    /// </summary>
    public class ApplicationRole : IdentityRole<Guid>
    {
    }
}
