namespace InterviewCrud.Api.Client.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid TypeContactId { get; set; }
        public string Name { get; set; }
        public TypeContact TypeContact { get; set; }
        public string ContactNumber { get; set; }

    }

    public class TypeContact
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string? Contact { get; set; }
        public TypeContactEnum? TypeContactEnum { get; set; }
    }

    public enum TypeContactEnum
    {
        Email,
        Phone,
        CellPhone,
        WhatsApp
    }
}
