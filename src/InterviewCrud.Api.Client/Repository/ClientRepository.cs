using InterviewCrud.Api.Client.Data;
using InterviewCrud.Api.Client.InputModels;
using InterviewCrud.Api.Client.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace InterviewCrud.Api.Client.Repository
{
    public interface IClientRepository
    {
        Task<bool> Add(Models.Client client, IEnumerable<Contact> contact, IEnumerable<Address> address);
        Task<bool> AddContact(IEnumerable<Contact> contact);
        Task<bool> AddAddress(IEnumerable<Address> address);
        Task<bool> Delete(Guid id);
        Task<bool> Update(Guid id, Models.Client client);
        Task<IEnumerable<Models.Client>> ListAll();
        Task<Models.Client> ListId(Guid id);
        Task<bool> Inactive(Guid id);
        Task<bool> Active(Guid id);
    }

    public class ClientRepository(ApplicationContext context) : IClientRepository
    {
        private readonly ApplicationContext _context = context;

        public async Task<bool> Add(Models.Client client, IEnumerable<Contact> contacts, IEnumerable<Address> addresses)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            await _context.Client.AddAsync(client);

            foreach (var contact in contacts)
            {
                await _context.Contacts.AddAsync(contact);
                await _context.TypeContacts.AddAsync(contact.TypeContact);
            }

            await _context.Address.AddRangeAsync(addresses);

            int result = await _context.SaveChangesAsync();
            int numberSaves = 1 + addresses.Count() + (contacts.Count() + contacts.Count());

            if (result != numberSaves)
            {
                await transaction.RollbackAsync();
                return false;
            }

            await transaction.CommitAsync();

            return true;
        }

        public async Task<bool> AddContact(IEnumerable<Contact> contacts)
        {
            foreach (var contact in contacts)
            {
                await _context.Contacts.AddAsync(contact);
                await _context.TypeContacts.AddAsync(contact.TypeContact);
            }

            return (await _context.SaveChangesAsync()) == (contacts.Count() * 2);
        }

        public async Task<bool> AddAddress(IEnumerable<Address> addresses)
        {
            await _context.Address.AddRangeAsync(addresses);

            return (await _context.SaveChangesAsync()) == addresses.Count();
        }

        public async Task<bool> Delete(Guid id)
        {
            var client = _context.Client.AsNoTracking().FirstOrDefault(x => x.Id == id);

            if (client is null)
            {
                return false;
            }

            _context.Client.Remove(client);

            return (await _context.SaveChangesAsync()) == 1;
        }

        public async Task<bool> Inactive(Guid id)
        {
            var client = _context.Client.AsNoTracking().FirstOrDefault(x => x.Id == id);

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
            return await _context.Client.AsNoTracking().Include(c => c.Contacts).Include(a => a.Addresses).ToListAsync(); ;
        }

        public async Task<Models.Client> ListId(Guid id)
        {
            return await _context.Client
                        .AsTracking()
                        .Where(x => x.Id == id)
                        .Include(x => x.Addresses)
                        .Include(x => x.Contacts)
                            .ThenInclude(c => c.TypeContact)
                        .FirstOrDefaultAsync();
        }

        public async Task<bool> Update(Guid id, Models.Client client)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var findClient = await _context.Client
                    .Include(d => d.Addresses)
                    .Include(d => d.Contacts)
                    .ThenInclude(d => d.TypeContact)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (findClient is null)
                {
                    return false;
                }

                _context.Entry(findClient).CurrentValues.SetValues(client);


                List<Address> addressesToAdd = new List<Address>(findClient.Addresses);

                foreach (var address in client.Addresses)
                {
                    var existingAddress = addressesToAdd.FirstOrDefault(a => a.Id == address.Id);

                    if (existingAddress != null)
                    {
                        _context.Entry(existingAddress).CurrentValues.SetValues(address);
                    }
                    else
                    {
                        addressesToAdd.Add(address);
                    }
                }

                List<Contact> contactsToAdd = new List<Contact>(findClient.Contacts);
                List<TypeContact> typeContactsToAdd = new List<TypeContact>();

                findClient.Addresses = addressesToAdd;

                foreach (var contact in client.Contacts)
                {
                    var existingContact = contactsToAdd.FirstOrDefault(c => c.Id == contact.Id);

                    if (existingContact != null)
                    {
                        _context.Entry(existingContact).CurrentValues.SetValues(contact);

                        var existingTypeContact = _context.TypeContacts.FirstOrDefault(tc => tc.Id == contact.TypeContactId);

                        if (existingTypeContact != null)
                        {
                            _context.Entry(existingTypeContact).CurrentValues.SetValues(contact.TypeContact);
                        }
                        else
                        {
                            typeContactsToAdd.Add(contact.TypeContact);
                        }
                    }
                    else
                    {
                        contactsToAdd.Add(contact);
                        typeContactsToAdd.Add(contact.TypeContact);
                    }
                }

                findClient.Contacts = contactsToAdd;

                int result = await _context.SaveChangesAsync();
                int numberSaves = 1 + findClient.Addresses.Count() + (findClient.Contacts.Count() + findClient.Contacts.Count());

                if (result != numberSaves)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}
