using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathQuizCreatorAPI.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // Creator Id when logged in
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; } // so it is only used once.
        public bool IsRevoked { get; set; } // to make sure they are valid
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}
