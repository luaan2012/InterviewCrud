FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./InterviewCrud.Api.Client/InterviewCrud.Api.Client.csproj", "InterviewCrud.Api.Client.csproj/"]
COPY . .
WORKDIR "InterviewCrud.Api.Client.csproj"
RUN dotnet build "InterviewCrud.Api.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InterviewCrud.Api.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InterviewCrud.Api.Client.dll"]