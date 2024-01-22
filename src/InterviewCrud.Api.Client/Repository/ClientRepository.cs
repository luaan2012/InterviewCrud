using InterviewCrud.Api.Client.Data;
using InterviewCrud.Api.Client.InputModels;
using InterviewCrud.Api.Client.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewCrud.Api.Client.Repository
{
    public interface IClientRepository
    {
        Task<bool> Add(Models.Client client, Contact contact, Address address);
        Task<bool> AddContact(Contact contact);
        Task<bool> AddAddress(Address address);
        Task<bool> Delete(Guid id);
        Task<bool> Update(Guid id, RequestClient client);
        Task<IEnumerable<Models.Client>> ListAll();
        Task<Models.Client> ListId(Guid id);
        Task<bool> Inactive(Guid id);
        Task<bool> Active(Guid id);
    }

    public class ClientRepository (ApplicationContext context) : IClientRepository
    {
        private readonly ApplicationContext _context = context;
        
        public async Task<bool> Add(Models.Client client, Contact contact, Address address)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            await _context.Client.AddAsync(client);
            await _context.Contacts.AddAsync(contact);
            await _context.TypeContacts.AddAsync(contact.TypeContact);
            await _context.Address.AddAsync(address);

            int result = await _context.SaveChangesAsync();

            if (result != 4)
            {
                await transaction.RollbackAsync();
                return false;
            }

            await transaction.CommitAsync();

            return true;
        }

        public async Task<bool> AddContact(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);

            await _context.TypeContacts.AddAsync(contact.TypeContact);

            return (await _context.SaveChangesAsync()) == 2;
        }

        public async Task<bool> AddAddress(Address address)
        {
            await _context.Address.AddAsync(address);

            return (await _context.SaveChangesAsync()) == 1;
        }

        public async Task<bool> Delete(Guid id)
        {
            var client = _context.Client.FirstOrDefault(x => x.Id == id);

            if(client is null)
            {
                return false;
            }

            _context.Client.Remove(client);

            return (await _context.SaveChangesAsync()) == 1;
        }

        public async Task<bool> Inactive(Guid id)
        {
            var client = _context.Client.FirstOrDefault(x => x.Id == id);

            if (client is null)
            {
                return false;
            }

            client.DateDelete = DateTime.Now;
            client.Active = false;

            _context.Client.Update(client);

            return (await _context.SaveChangesAsync()) == 1;
        }

        public async Task<bool> Active(Guid id)
        {
            var client = _context.Client.FirstOrDefault(x => x.Id == id);

            if (client is null)
            {
                return false;
            }

            client.DateDelete = null;
            client.Active = true;

            _context.Client.Update(client);

            return (await _context.SaveChangesAsync()) == 1;
        }

        public async Task<IEnumerable<Models.Client>> ListAll()
        {
            return await _context.Client.ToListAsync();
        }

        public async Task<Models.Client> ListId(Guid id)
        {
            return _context.Client.FirstOrDefault(x => x.Id == id);
        }

        public async Task<bool> Update(Guid id, RequestClient client)
        {
            var findClient = _context.Client.FirstOrDefault(x => x.Id == id);

            if (findClient is null)
            {
                return false;
            }

            findClient.Name = client.Name;
            findClient.LastName = client.LastName;
            findClient.DateModificated = DateTime.Now;

            _context.Client.Update(findClient);

            return (await _context.SaveChangesAsync()) == 1;
        }
    }
}
