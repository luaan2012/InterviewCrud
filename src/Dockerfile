#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS build
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["InterviewCrud.Api.Identity/InterviewCrud.Api.Identity.csproj", "InterviewCrud.Api.Identity/"]
RUN dotnet restore "InterviewCrud.Api.Identity/InterviewCrud.Api.Identity.csproj"
COPY . .
WORKDIR "/src/InterviewCrud.Api.Identity"
RUN dotnet build "InterviewCrud.Api.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InterviewCrud.Api.Identity.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InterviewCrud.Api.Identity.dll"]
EXPOSE 80