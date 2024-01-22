FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./InterviewCrud.Api.Identity/InterviewCrud.Api.Identity.csproj", "InterviewCrud.Api.Identity.csproj/"]
COPY . .
WORKDIR "/src"
RUN dotnet build "InterviewCrud.Api.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InterviewCrud.Api.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InterviewCrud.Api.Identity.dll"]