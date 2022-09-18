using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace MathQuizCreatorAPI.Models
{
    [JsonObject]
    public class Parameter
    {
        [Key]
        public Guid ParameterId { get; set; }

        private string? _name;

        [BackingField(nameof(_name))]
        [Required]
        public string? Name
        {
            get => _name;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Name can't be null");
                }

                _name = value;
            }
        }

        private string? _value;

        [BackingField(nameof(_value))]
        [Required]
        public string? Value
        {
            get => _value;
            set
            {
                if(value == null)
                {
                    throw new ArgumentException("Value can't be null");
                }

                _value = value;
            }
        }

        private int _order;

        [BackingField(nameof(_order))]
        [Required]
        public int Order
        {
            get => _order;
            set
            {
                if(value < 0)
                {
                    throw new ArgumentException("Order can't be negative");
                }

                _order = value;
            }
        }



        [Required]
        public Guid QuestionId { get; set; }

        private Question? _question;

        [BackingField(nameof(_question))]
        [Required]
        public Question? Question { 
            get => _question;
            set
            {
                if(value == null)
                {
                    throw new ArgumentException("Question can't be null");
                }

                _question = value;
            }
        }



        public Parameter(string name, string value, int order, Question question)
        {
            ParameterId = Guid.NewGuid();

            Name = name;
            Value = value;

            Order = order;

            Question = question;
            QuestionId = question.QuestionId;
        }
    }
}
