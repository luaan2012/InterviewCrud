using System.ComponentModel.DataAnnotations.Schema;

namespace InterviewCrud.Api.Client.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public int Number { get; set; }
        public string? Complement{ get; set; }
        public string? Cep { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PublicPlace { get; set; }
        [NotMapped]
        public string CompletAddress { get
            {
                return $"{PublicPlace}, {Number}, {Complement} - {City} - {State}";
            }
        }
    }
}
