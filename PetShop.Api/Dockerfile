FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["PetShop.Api/PetShop.Api.csproj", "PetShop.Api/"]
RUN dotnet restore "PetShop.Api/PetShop.Api.csproj"
COPY . .
WORKDIR "/src/PetShop.Api"
RUN dotnet build "PetShop.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PetShop.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetShop.Api.dll"]