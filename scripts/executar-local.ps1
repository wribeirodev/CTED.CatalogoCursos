$ErrorActionPreference = "Stop"
$projeto = Join-Path $PSScriptRoot "../src/CTED.CatalogoCursos.Api/CTED.CatalogoCursos.Api.csproj"
dotnet run --project $projeto --launch-profile CTED.CatalogoCursos.Api
