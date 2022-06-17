#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebNote.Api/WebNote.Api.csproj", "WebNote.Api/"]
RUN dotnet restore "WebNote.Api/WebNote.Api.csproj"
COPY . .
WORKDIR "/src/WebNote.Api"
RUN dotnet build "WebNote.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebNote.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebNote.Api.dll"]