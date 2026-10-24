param(
    [string]$Destino = "./publish"
)

$ErrorActionPreference = "Stop"
$projeto = Join-Path $PSScriptRoot "../src/CTED.CatalogoCursos.Api/CTED.CatalogoCursos.Api.csproj"

Remove-Item $Destino -Recurse -Force -ErrorAction SilentlyContinue
dotnet restore $projeto
dotnet publish $projeto -c Release -o $Destino --self-contained false
Write-Host "Publicação concluída em $Destino"
Write-Host "Execute com: dotnet CTED.CatalogoCursos.Api.dll"
