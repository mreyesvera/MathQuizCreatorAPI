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
    /// This model represents a Normal Distribution graph's settings. 
    /// At the moment it's unused, but it would have been used for the
    /// functionality of adding a normal distribution to questions. 
    /// </summary>
    [JsonObject]
    public class NormalDistribution
    {
        [Key]
        public Guid NormalDistributionId { get; set; }

        [Required]
        public bool Values { get; set; }

        public enum NormalDistributionType
        {
            X,
            Z
        };

        private NormalDistributionType _distributionType;

        [BackingField(nameof(_distributionType))]
        [Required]
        public NormalDistributionType DistributionType {
            get
            {
                return _distributionType;
            }
            set 
            {
                if (!Enum.IsDefined(typeof(NormalDistributionType), value)) 
                {
                    throw new ArgumentException("xorz should be one of the following values: " + Enum.GetValues(typeof(NormalDistributionType)));
                }

                _distributionType = value;
            } 
        }


        private double _mean;

        [BackingField(nameof(_mean))]
        [Required]
        public double Mean {
            get
            {
                return _mean;
            }
            set
            {
                if (DistributionType == NormalDistributionType.Z) 
                {
                    if (value != 0)
                        throw new ArgumentException("In a z-distribution the mean should be 0");

                }

                _mean = value;
            }
        }

        private double _standardDeviation;

        [BackingField(nameof(_standardDeviation))]
        [Required]
        public double StandardDeviation {
            get
            {
                return _standardDeviation;
            }
            set
            {
                if (DistributionType == NormalDistributionType.Z) 
                {
                    if (value != 0)
                        throw new ArgumentException("In a z-distribution the standard deviation should be 1");

                }

                _standardDeviation = value;
            }
        }

        [Required]
        public double Min { get; set;  }

        [Required]
        public double Max { get; set; }

        [Required]
        public double Area { get; set; }


        private string _color = "#5B9BD5";

        [BackingField(nameof(_color))]
        [MaxLength(200)]
        [Required]
        public string Color { 
            get
            {
                return _color;
            }
            set
            {
                if(value == null)
                {
                    return;
                    
                }

                if(!Regex.IsMatch(value.ToLower(), @"^#[0-9a-f]{6}$"))
                {
                    throw new ArgumentException("Color is not in the expected format. Please write color starting with a # followed by 6 hex digits.");
                }

                _color = value;
            }
        }




        public Guid? QuestionId { get; set; }

        private Question? _question;

        [BackingField(nameof(_question))]
        public Question? Question
        {
            get => _question;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Question can't be null");
                }

                _question = value;
            }
        }

        public NormalDistribution()
        {

        }

        public NormalDistribution(bool values, string distributionType, double mean, double standardDeviation, double min, double max, double area, string color, Question question)
        {
            NormalDistributionId = Guid.NewGuid();

            Values = values;

            if(!Enum.TryParse(distributionType, out NormalDistributionType distributionTypeEnum))
            {
                throw new ArgumentException("xorz should be one of the following values: " + Enum.GetValues(typeof(NormalDistributionType)));
            }

            DistributionType = distributionTypeEnum;

            Mean = mean;
            StandardDeviation = standardDeviation;

            Min = min;
            Max = max;
            Area = area;

            Color = color;

            Question = question;
            QuestionId = question.QuestionId;
        }
    }
}
