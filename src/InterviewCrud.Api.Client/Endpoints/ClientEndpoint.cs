
using InterviewCrud.Api.Client.InputModels;
using InterviewCrud.Api.Client.Services;

namespace InterviewCrud.Api.Client.Endpoints
{
    public static class ClientEndpoint
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            app.MapGet("ListAll", ListAll).RequireAuthorization();
            app.MapGet("ListById", ListById).RequireAuthorization();
            app.MapPost("AddClient", AddClient).RequireAuthorization();
            app.MapPut("Edit", Edit).RequireAuthorization();
            app.MapDelete("Delete", Delete).RequireAuthorization();
            app.MapPut("Inactive", Inactive).RequireAuthorization();
            app.MapPut("Active", Inactive).RequireAuthorization();
        }

        public static async Task<IResult> ListAll(IClientService clientService)
        {
            var result = await clientService.ListAll();

            return result.Match(
                m => Results.Ok(m),
                err => Results.BadRequest(err)
            );
        }

        public static async Task<IResult> ListById(Guid id, IClientService clientService)
        {
            var result = await clientService.ListById(id);

            return result.Match(
                m => Results.Ok(m),
                err => Results.BadRequest(err)
            );
        }

        public static async Task<IResult> AddClient(RequestClient client, IClientService clientService)
        {
            var result = await clientService.Add(client);

            return result.Match(
                m => Results.Ok(m),
                err => Results.BadRequest(err)
            );
        }

        public static async Task<IResult> Delete(Guid guid, IClientService clientService)
        {
            var result = await clientService.Delete(guid);

            return result ? Results.Ok(result) : Results.BadRequest(result);
        }

        public static async Task<IResult> Edit(Guid guid, Models.Client client, IClientService clientService)
        {
            var result = await clientService.Edit(guid, client);

            return result ? Results.Ok(result) : Results.BadRequest(result);
        }

        public static async Task<IResult> Inactive(Guid guid, IClientService clientService)
        {
            var result = await clientService.Inactive(guid);

            return result ? Results.Ok(result) : Results.BadRequest(result);
        }

        public static async Task<IResult> Active(Guid guid, IClientService clientService)
        {
            var result = await clientService.Active(guid);

            return result ? Results.Ok(result) : Results.BadRequest(result);
        }
    }
}
