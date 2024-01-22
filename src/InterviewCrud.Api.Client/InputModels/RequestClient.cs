using InterviewCrud.Api.Client.Models;

namespace InterviewCrud.Api.Client.InputModels
{
    public class RequestClient
    {
        public Guid? UserId { get; set; }
        public string? EmailUser { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? CPF { get; set; }
        public string? RG { get; set; }
        public IEnumerable<RequestAddress>? Addresses { get; set; }
        public IEnumerable<RequestContact>? Contacts { get; set; }
        public DateTime? DateBirthday { get; set; }
    }
    public class RequestAddress
    {
        public int Number { get; set; }
        public string? Complement { get; set; }
        public string? Neighborhood { get; set; }
        public string? Cep { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PublicPlace { get; set; }
    }

    public class RequestContact
    {
        public string? NameContact { get; set; }
        public RequestTypeContact? TypeContact { get; set; }
        public string? ContactNumber { get; set; }

    }

    public class RequestTypeContact
    {
        public string? Contact { get; set; }
        public TypeContactEnum? TypeContactEnum { get; set; }
    }
}
