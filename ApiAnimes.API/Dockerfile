# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar a solução inteira para o contêiner
COPY ["ApiAnimes.sln", "./"]

# Copiar todos os projetos para o contêiner
COPY ["ApiAnimes.API/ApiAnimes.API.csproj", "ApiAnimes.API/"]
COPY ["ApiAnimes.Application/ApiAnimes.Application.csproj", "ApiAnimes.Application/"]
COPY ["ApiAnimes.Domain/ApiAnimes.Domain.csproj", "ApiAnimes.Domain/"]
COPY ["ApiAnimes.Infra/ApiAnimes.Infra.csproj", "ApiAnimes.Infra/"]
COPY ["ApiAnimes.Tests/ApiAnimes.Tests.csproj", "ApiAnimes.Tests/"]

# Restaurar dependências de todos os projetos
RUN dotnet restore "ApiAnimes.sln"

# Copiar o restante dos arquivos da solução
COPY . .

WORKDIR "/src/ApiAnimes.API"

# Compilar o projeto
RUN dotnet build "ApiAnimes.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ApiAnimes.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiAnimes.API.dll"]
