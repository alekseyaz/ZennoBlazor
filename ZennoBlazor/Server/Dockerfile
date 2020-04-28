#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ZennoBlazor/Server/ZennoBlazor.Server.csproj", "ZennoBlazor/Server/"]
COPY ["ZennoBlazor/Client/ZennoBlazor.Client.csproj", "ZennoBlazor/Client/"]
RUN dotnet restore "ZennoBlazor/Server/ZennoBlazor.Server.csproj"
COPY . .
WORKDIR "/src/ZennoBlazor/Server"
RUN dotnet build "ZennoBlazor.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZennoBlazor.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZennoBlazor.Server.dll"]