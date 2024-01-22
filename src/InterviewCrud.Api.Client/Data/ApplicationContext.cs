using InterviewCrud.Api.Client.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewCrud.Api.Client.Data
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        public DbSet<Models.Client> Client { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<TypeContact> TypeContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.TypeContact)
                .WithMany()
                .HasForeignKey(c => c.TypeContactId);
        }
    }
}
