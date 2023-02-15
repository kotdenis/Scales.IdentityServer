FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY Scales.IdentityServer.sln Scales.IdentityServer.sln
COPY ./Scales.IdentityServer/Scales.IdentityServer.csproj ./
RUN dotnet restore Scales.IdentityServer.csproj
COPY . .

RUN dotnet build ./Scales.IdentityServer/Scales.IdentityServer.csproj -c Release -o /app/build 

FROM build AS publish
RUN dotnet publish ./Scales.IdentityServer/Scales.IdentityServer.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Scales.IdentityServer.dll"]