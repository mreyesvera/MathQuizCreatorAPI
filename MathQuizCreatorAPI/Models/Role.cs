using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MathQuizCreatorAPI.Models
{
    [JsonObject]
    public class Role : Entity
    {
        [Key]
        public Guid RoleId { get; set; }

        private string? _title;

        [BackingField(nameof(_title))]
        [Required]
        public string? Title {
            get => _title;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Title can't be null");
                }

                _title = value;
            }
        }

        public Role()
        {

        }


        public Role(string title) : base()
        {
            RoleId = Guid.NewGuid();
            Title = title;
        }
    }
}
