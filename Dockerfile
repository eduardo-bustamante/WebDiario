# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia o .csproj de dentro da subpasta e restaura
COPY ["WebDiario/WebDiario.csproj", "WebDiario/"]
RUN dotnet restore "WebDiario/WebDiario.csproj"

# Copia todo o restante e compila
COPY . .
WORKDIR "/src/WebDiario"
RUN dotnet publish "WebDiario.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio de Execução
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WebDiario.dll"]
