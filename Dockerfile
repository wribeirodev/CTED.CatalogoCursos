# Etapa 1: restaura, compila e publica a API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# No projeto original, o csproj fica dentro de src/CTED.CatalogoCursos.Api
COPY src/CTED.CatalogoCursos.Api/CTED.CatalogoCursos.Api.csproj src/CTED.CatalogoCursos.Api/
RUN dotnet restore src/CTED.CatalogoCursos.Api/CTED.CatalogoCursos.Api.csproj

# Copia o restante da solução
COPY . .
RUN dotnet publish src/CTED.CatalogoCursos.Api/CTED.CatalogoCursos.Api.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

# Etapa 2: imagem menor somente com o runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app
COPY --from=build /app/publish .

# O Render fornece a variável PORT em tempo de execução.
# O valor 8080 serve como fallback para execução local.
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet CTED.CatalogoCursos.Api.dll --urls http://0.0.0.0:${PORT:-8080}"]
