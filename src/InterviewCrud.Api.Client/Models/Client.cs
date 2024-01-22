namespace InterviewCrud.Api.Client.Models
{
    public class Client
    {
        public Client()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? EmailUser { get; set; }
        public string? Email{ get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? CPF { get; set; }
        public string? RG { get; set; }
        public bool? Active { get; set; }
        public IEnumerable<Address>? Addresses { get; set; }
        public IEnumerable<Contact>? Contacts{ get; set; }
        public DateTime? DateBirthday { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateDelete { get; set; }
        public DateTime? DateModificated { get; set; }
    }
}
