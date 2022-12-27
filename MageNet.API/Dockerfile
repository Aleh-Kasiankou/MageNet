FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MageNet/MageNet.csproj", "MageNet/"]
RUN dotnet restore "MageNet/MageNet.csproj"
COPY . .
WORKDIR "/src/MageNet"
RUN dotnet build "MageNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MageNet.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MageNet.dll"]
