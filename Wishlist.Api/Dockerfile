FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Wishlist.Api/Wishlist.Api.csproj", "Wishlist.Api/"]
COPY ["Wishlist.DAL/Wishlist.DAL.csproj", "Wishlist.DAL/"]
RUN dotnet restore "Wishlist.Api/Wishlist.Api.csproj"
COPY . .
WORKDIR "/src/Wishlist.Api"
RUN dotnet build "Wishlist.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wishlist.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wishlist.Api.dll"]
