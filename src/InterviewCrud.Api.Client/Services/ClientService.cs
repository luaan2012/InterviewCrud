
using InterviewCrud.Api.Client.InputModels;
using InterviewCrud.Api.Client.Models;
using InterviewCrud.Api.Client.OutputModels;
using InterviewCrud.Api.Client.Repository;
using MassTransit;
using System.Net;

namespace InterviewCrud.Api.Client.Services
{
    public interface IClientService
    {
        Task<Result<IEnumerable<Models.Client>, string>> ListAll();
        Task<Result<Models.Client, string>> ListById(Guid id);
        Task<Result<bool, string>> Add(RequestClient requestClient);
        Task<bool> Delete(Guid guid);
        Task<bool> Edit(Guid guid, Models.Client requestClient);
        Task<bool> Inactive(Guid guid);
        Task<bool> Active(Guid guid);
    }

    public class ClientService(IClientRepository clientRepository) : IClientService
    {
        private readonly IClientRepository _clientRepository = clientRepository;

        public async Task<Result<IEnumerable<Models.Client>, string>> ListAll()
        {
            var clients = await _clientRepository.ListAll();

            if(clients is not null && clients.Count() > 0)
            {
                return clients.ToArray();
            }

            return "Nenhum cliente encontrado";
        }

        public async Task<Result<Models.Client, string>> ListById(Guid id)
        {
            var clients = await _clientRepository.ListId(id);

            if (clients is not null)
            {
                return clients;
            }

            return "Nenhum cliente encontrado";
        }

        public async Task<Result<bool, string>> Add(RequestClient requestClient)
        {
            var client = RequestClientToClient(requestClient);

            var result = await _clientRepository
                .Add(client,
                MapRequestContactToClient(requestClient.Contacts, client.Id),
                MapRequestAddressesToClient(requestClient.Addresses, client.Id));

            return result ? result : "Aconteceu um erro ao tentar registrar um cliente;";
        }

        public async Task<bool> Delete(Guid guid)
        {
            return await _clientRepository.Delete(guid);
        }

        public async Task<bool> Edit(Guid guid, Models.Client client)
        {
            return await _clientRepository.Update(guid, client);
        }

        public async Task<bool> Inactive(Guid guid)
        {
            return await _clientRepository.Inactive(guid);
        }

        public async Task<bool> Active(Guid guid)
        {
            return await _clientRepository.Active(guid);
        }

        private Models.Client RequestClientToClient(RequestClient requestClient)
        {
            return new Models.Client
            {
                Active = true,
                Name = requestClient.Name,
                LastName = requestClient.LastName,
                CPF = requestClient.CPF,
                RG = requestClient.RG,
                Email = requestClient.LastName,
                EmailUser = requestClient.EmailUser,
                UserId = requestClient.UserId,
                DateBirthday = requestClient.DateBirthday,                
            };
        }
        private IEnumerable<Address> MapRequestAddressesToClient(IEnumerable<RequestAddress> addresses, Guid clientId)
        {
            List<Address> mappedAddresses = [];

            foreach (var address in addresses)
            {
                Address mappedAddress = new()
                {
                    ClientId = clientId,
                    Number = address.Number,
                    Complement = address.Complement,
                    Cep = address.Cep,
                    City = address.City,
                    State = address.State,
                    PublicPlace = address.PublicPlace,
                };

                mappedAddresses.Add(mappedAddress);
            }

            return mappedAddresses;
        }

        private IEnumerable<Contact> MapRequestContactToClient(IEnumerable<RequestContact> requestContact, Guid clientID)
        {
            List<Contact> mappedContacts = [];

            foreach (var contact in requestContact)
            {
                Contact mappedContact = new()
                {
                    ClientId = clientID,
                    NameContact = contact?.NameContact,
                    ContactNumber = contact?.ContactNumber,
                    TypeContact = new TypeContact
                    {
                        ClientId = clientID,
                        Contact = contact?.TypeContact.Contact,
                        TypeContactEnum = contact?.TypeContact.TypeContactEnum
                    },
                };

                mappedContacts.Add(mappedContact);
            }

            return mappedContacts;
        }
    }
}
