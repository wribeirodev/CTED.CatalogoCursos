# CTED Catálogo de Cursos — API

Backend da atividade prática do processo seletivo de Desenvolvedor de Sistemas I. Esta entrega contém somente a API e o banco de dados, sem front-end, conforme solicitado. Todos os dados são fictícios e não há integração com Moodle.

## Objetivo

Disponibilizar um catálogo educacional consultável por uma interface cliente. A API permite listar cursos, pesquisar por nome, combinar filtros de categoria, modalidade e status, obter a quantidade de resultados e abrir o detalhe de um curso com seus módulos e aulas.

## Tecnologias

- C# 12 e ASP.NET Core Web API (.NET 8);
- Entity Framework Core 8;
- SQLite, banco de código aberto e sem servidor para execução local;
- Swagger/OpenAPI para exploração dos endpoints;
- Visual Studio 2022 17.8 ou superior, com o workload “ASP.NET and web development”.

## Arquitetura

A solução usa uma arquitetura simples em camadas:

- `Controllers`: contrato HTTP e códigos de resposta;
- `Services`: regras de consulta e montagem dos DTOs;
- `Domain`: entidades persistidas;
- `Data`: DbContext, mapeamentos e carga inicial;
- `DTOs`: modelos públicos da API;
- `Middleware`: tratamento uniforme de erros.

O banco SQLite é criado automaticamente na primeira execução em `src/CTED.CatalogoCursos.Api/Data/cted-cursos.db`. O `EnsureCreated` cria as tabelas e insere seis cursos fictícios. Para um cenário produtivo com alteração frequente do modelo, recomenda-se substituir por migrations do EF Core.

## Estrutura

```text
CTED.CatalogoCursos/
├── CTED.CatalogoCursos.sln
├── README.md
├── USO_DE_IA.md
├── scripts/
│   ├── executar-local.ps1
│   └── publicar.ps1
└── src/CTED.CatalogoCursos.Api/
    ├── Controllers/
    ├── Data/
    ├── Domain/
    ├── DTOs/
    ├── Middleware/
    ├── Services/
    ├── Program.cs
    └── appsettings*.json
```

## Executar no Visual Studio 2022

1. Instale o Visual Studio 2022 17.8+ com o workload **ASP.NET and web development**.
2. Abra `CTED.CatalogoCursos.sln`.
3. Selecione o projeto `CTED.CatalogoCursos.Api` como projeto de inicialização.
4. Execute com `Ctrl+F5` ou `F5`. O perfil abre o Swagger em `https://localhost:7043/swagger`.
5. O certificado HTTPS de desenvolvimento pode solicitar confiança na primeira execução.

Também é possível executar no terminal do desenvolvedor:

```powershell
dotnet restore
dotnet run --project .\src\CTED.CatalogoCursos.Api --launch-profile CTED.CatalogoCursos.Api
```

O SDK não está incluído no repositório; no Visual Studio ele é instalado pelo workload ou pode ser obtido pelo instalador oficial do .NET 8 SDK.

## Banco de dados

A conexão padrão fica em `appsettings.json`:

```json
"DefaultConnection": "Data Source=Data/cted-cursos.db"
```

Para apagar e recriar o catálogo local, pare a API, apague `src/CTED.CatalogoCursos.Api/Data/cted-cursos.db` e execute novamente. O banco será recriado com os seis registros e seus módulos/aulas.

## Endpoints

| Método | Endpoint | Finalidade |
|---|---|---|
| GET | `/api/cursos` | Lista cursos e aplica busca/filtros |
| GET | `/api/cursos/opcoes` | Retorna categorias, modalidades e status disponíveis |
| GET | `/api/cursos/{id}` | Retorna detalhe, módulos e aulas |
| GET | `/health` | Verifica se a API e o banco respondem |
| GET | `/swagger` | Documentação interativa, disponível no ambiente Development |

Parâmetros opcionais de `/api/cursos`: `busca`, `categoria`, `modalidade` e `status`. Eles podem ser combinados:

```text
GET /api/cursos?busca=dados&categoria=Tecnologia&status=Encerrado
```

A resposta de listagem contém `total`, os critérios recebidos e `resultados`. Quando nenhum curso for encontrado, a API retorna HTTP 200 com `total: 0` e `resultados: []`; cabe ao front-end apresentar a mensagem adequada. Curso inexistente retorna HTTP 404.

## CORS

As origens locais `http://localhost:3000` e `http://localhost:5173` já estão liberadas. Em homologação/produção, configure `Cors:AllowedOrigins` via variável de ambiente ou arquivo de configuração. Não use `AllowAnyOrigin` em produção.

## Publicação

### Publicação em pasta

No PowerShell, na raiz do repositório:

```powershell
.\scripts\publicar.ps1 -Destino .\publish
cd .\publish
dotnet CTED.CatalogoCursos.Api.dll
```

O script publica em modo Release, restaura os pacotes e gera uma pasta independente do Visual Studio. Para publicar em um servidor Windows com .NET 8 instalado, copie a pasta `publish`, defina `ASPNETCORE_ENVIRONMENT=Production` e execute o DLL. A porta pode ser definida com `ASPNETCORE_URLS=http://0.0.0.0:8080`.

### IIS (resumo)

1. Instale o Hosting Bundle do ASP.NET Core 8 no servidor.
2. Publique com o script acima e copie o conteúdo para uma pasta do IIS.
3. Crie um site/aplicação apontando para essa pasta e configure um Application Pool “No Managed Code”.
4. Garanta permissão de escrita na subpasta `Data` para a identidade do Application Pool, pois o SQLite cria/atualiza o arquivo local.
5. Configure a connection string e as origens CORS por ambiente.

### Linux/Nginx (resumo)

Instale o runtime ASP.NET Core 8, copie a pasta publicada, configure `ASPNETCORE_URLS=http://127.0.0.1:5080`, crie um serviço systemd para `dotnet CTED.CatalogoCursos.Api.dll` e use Nginx como proxy reverso com HTTPS. Dê permissão de escrita na pasta `Data` ao usuário do serviço.

## Responsividade, acessibilidade e integração

Responsividade e acessibilidade são responsabilidades do front-end, que consumirá os contratos JSON desta API. A API já fornece dados semânticos completos, mensagens de erro, quantidade de resultados e estados que permitem ao front-end exibir carregamento, erro e estado vazio. Os campos de imagem são URLs públicas fictícias apenas para composição do catálogo.

## Limitações conhecidas

- Não há autenticação, autorização ou painel de administração, pois o escopo exige apenas consulta.
- O SQLite é adequado para demonstração e pequeno volume; para produção com concorrência elevada, use PostgreSQL ou SQL Server e migrations.
- O catálogo é carregado por seed; não existem endpoints de cadastro, edição ou exclusão.
- A integração com Moodle e o front-end/Figma não fazem parte desta entrega backend.
- As imagens apontam para URLs externas de composição; em ambiente isolado, substitua por arquivos ou CDN próprios.

## Validação manual sugerida

Após iniciar a API, teste `/health`, `/api/cursos`, a combinação de filtros, `/api/cursos/1` e `/api/cursos/999`. No Swagger, confirme que a listagem apresenta seis cursos, pelo menos três categorias, duas modalidades e três status.
