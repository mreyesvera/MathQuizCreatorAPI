using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MathQuizCreatorAPI.Models
{

    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// This model is used so other models can inherit from it
    /// and have a last modified time and creation time. 
    /// </summary>
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
                //return LastModifiedTime.Date.ToShortDateString();
                return LastModifiedTime.Date.ToString("yyyy-MM-dd");
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
                //return CreationTime.Date.ToShortDateString();
                return LastModifiedTime.Date.ToString("yyyy-MM-dd");
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
