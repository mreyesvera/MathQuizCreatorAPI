using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace MathQuizCreatorAPI.Models
{
    [JsonObject]
    public class User : Entity
    {
        [Key]
        public Guid UserId { get; set; }

        private string? _email;

        [BackingField(nameof(_email))]
        [Required]
        [EmailAddress]
        public string? Email {
            get => _email;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Email can't be null");
                }

                _email = value;
            }
        }

        private string? _username;

        [BackingField(nameof(_username))]
        [Required]
        public string? Username {
            get => _username;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Username can't be null");
                }

                _username = value;
            }
        }

        private string? _password;

        [BackingField(nameof(_password))]
        [Required]
        public string? Password {
            get => _password;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Password can't be null");
                }

                _password = value;
            }
        }

        [Required]
        public Guid RoleId { get; set; }

        private Role? _role;

        [BackingField(nameof(_role))]
        [Required]
        public Role? Role {
            get => _role;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Role can't be null");
                }

                _role = value;
            }
        }


        public User(string email, string username, string password, Role role) : base()
        {
            UserId = Guid.NewGuid();
            Email = email;
            Username = username;
            Password = password;
            Role = role;
            RoleId = role.RoleId;
        }

    }
}
