using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MathQuizCreatorAPI.Models
{
    [JsonObject]
    public class Entity
    {


        [JsonIgnore]
        [Required]
        public DateTimeOffset LastModifiedTime { get; set; }

        [JsonProperty("lastModifiedTime")]
        [NotMapped]
        public string LastModifiedTimeSerializable
        {
            get
            {
                //return LastModifiedTime.ToString("o");
                return LastModifiedTime.Date.ToShortDateString();
            }
            set
            {
                LastModifiedTime = DateTimeOffset.Parse(value);
            }
        }

        [JsonIgnore]
        [Required]
        public DateTimeOffset CreationTime { get; set; }

        [JsonProperty("creationTime")]
        [NotMapped]
        public string CreationTimeSerializable
        {
            get
            {
                //return CreationTime.ToString("o");
                return CreationTime.Date.ToShortDateString();
            }
            set
            {
                CreationTime = DateTimeOffset.Parse(value);
            }
        }

        public Entity()
        {
            LastModifiedTime = DateTimeOffset.Now;
            CreationTime = DateTimeOffset.Now;
        }
    }
}
