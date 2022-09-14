#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/User/Blessings.User.Api/Blessings.User.Api.csproj", "Services/User/Blessings.User.Api/"]
RUN dotnet restore "Services/User/Blessings.User.Api/Blessings.User.Api.csproj"
COPY . .
WORKDIR "/src/Services/User/Blessings.User.Api"
RUN dotnet build "Blessings.User.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blessings.User.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blessings.User.Api.dll"]