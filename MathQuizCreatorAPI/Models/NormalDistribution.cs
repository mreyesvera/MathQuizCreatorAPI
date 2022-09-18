using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace MathQuizCreatorAPI.Models
{
    [JsonObject]
    public class NormalDistribution
    {
        [Key]
        public Guid NormalDistributionId { get; set; }

        [Required]
        public bool Values { get; set; }

        public enum XorZValues
        {
            X,
            Z
        };

        private XorZValues _xorz;

        [BackingField(nameof(_xorz))]
        [Required]
        public XorZValues XorZ {
            get
            {
                return _xorz;
            }
            set 
            {
                if (!Enum.IsDefined(typeof(XorZValues), value)) // might be unnecessary
                {
                    throw new ArgumentException("xorz should be one of the following values: " + Enum.GetValues(typeof(XorZValues)));
                }

                _xorz = value;
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
                if (XorZ == XorZValues.Z) // double check if this is ok?
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
                if (XorZ == XorZValues.Z) // double check if this is ok?
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




        [Required]
        public Guid QuestionId { get; set; }

        private Question? _question;

        [BackingField(nameof(_question))]
        [Required]
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



        public NormalDistribution(bool values, string xorz, double mean, double stdv, double min, double max, double area, string color, Question question)
        {
            NormalDistributionId = Guid.NewGuid();

            Values = values;

            if(!Enum.TryParse(xorz, out XorZValues xorzenum))
            {
                throw new ArgumentException("xorz should be one of the following values: " + Enum.GetValues(typeof(XorZValues)));
            }

            XorZ = xorzenum;

            Mean = mean;
            StandardDeviation = stdv;

            Min = min;
            Max = max;
            Area = area;

            Color = color;

            Question = question;
            QuestionId = question.QuestionId;
        }
    }
}
