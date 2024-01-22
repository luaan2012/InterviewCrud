
using InterviewCrud.Api.Client.InputModels;
using InterviewCrud.Api.Client.Models;
using InterviewCrud.Api.Client.OutputModels;
using InterviewCrud.Api.Client.Repository;
using MassTransit;

namespace InterviewCrud.Api.Client.Services
{
    public interface IClientService
    {
        Task<Result<IEnumerable<Models.Client>, string>> ListAll();
        Task<Result<Models.Client, string>> ListById(Guid id);
        Task<Result<bool, string>> Add(RequestClient requestClient);
        Task<bool> Delete(Guid guid);
        Task<bool> Edit(Guid guid, RequestClient requestClient);
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
                RequestContactToClient(requestClient.Contact, client.Id),
                RequestAddressToClient(requestClient.Address, client.Id));

            return result ? result : "Aconteceu um erro ao tentar registrar um cliente;";
        }

        public async Task<bool> Delete(Guid guid)
        {
            return await _clientRepository.Delete(guid);
        }

        public async Task<bool> Edit(Guid guid, RequestClient requestClient)
        {
            return await _clientRepository.Update(guid, requestClient);
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
        private Address RequestAddressToClient(RequestAddress address, Guid clientID)
        {
            return new Address
            {
                ClientId = clientID,
                Number = address.Number,
                Complement = address.Complement,
                Neighborhood = address.Neighborhood,
                Cep = address.Cep,
                City = address.City,
                State = address.State,
                PublicPlace = address.PublicPlace,
            };
        }

        private Contact RequestContactToClient(RequestContact requestContact, Guid clientID)
        {
            return new Contact
            {
                ClientId = clientID,
                Name = requestContact.Name,
                ContactNumber = requestContact.ContactNumber,
                TypeContact = new TypeContact
                {
                    ClientId = clientID,
                    Contact = requestContact.TypeContact.Contact,
                    TypeContactEnum = requestContact.TypeContact.TypeContactEnum
                },
            };
        }

    }
}
