using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace MathQuizCreatorAPI.Models
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// This model is represents a Parameter set by a user when
    /// creating a question. It holds the parameter name, the value
    /// and the order it is in. 
    /// 
    /// For the future, it would be better to split it into another
    /// model that holds the name of the Parameter. That way there
    /// would be less repetition in the database and it would be
    /// easier to handle the structure. 
    /// </summary>
    [JsonObject]
    public class Parameter
    {
        [Key]
        public Guid ParameterId { get; set; }

        private string? _name;

        [BackingField(nameof(_name))]
        [MaxLength(200)]
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
        [MaxLength(200)]
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



        public Guid? QuestionId { get; set; }

        private Question? _question;

        [BackingField(nameof(_question))]
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


        public Parameter()
        {

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
