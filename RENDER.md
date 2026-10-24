# Publicação no Render com Docker

## Pré-requisito

Suba o conteúdo desta pasta para um repositório GitHub ou GitLab. O arquivo `Dockerfile` deve ficar na mesma pasta do arquivo `CTED.CatalogoCursos.Api.csproj`.

## Criar o serviço

1. No Render, escolha **New > Web Service**.
2. Conecte o repositório.
3. Selecione **Docker** como ambiente.
4. Deixe o caminho do Dockerfile como `./Dockerfile`.
5. Escolha um plano gratuito ou o plano desejado.
6. Faça o deploy.

O `Dockerfile` usa a variável `PORT` fornecida automaticamente pelo Render e inicia a API com:

```text
dotnet CTED.CatalogoCursos.Api.dll --urls http://0.0.0.0:$PORT
```

Depois do deploy, teste:

```text
https://SEU-ENDERECO.onrender.com/health
https://SEU-ENDERECO.onrender.com/swagger
```

## Banco SQLite no Render

O banco é criado no diretório `Data` na primeira inicialização. Como o armazenamento padrão de um serviço Render pode ser efêmero, o arquivo SQLite pode ser perdido quando houver novo deploy, reinício ou alteração de instância.

Para uma demonstração da atividade, isso é suficiente, pois os seis cursos são recriados automaticamente pelo seed. Para produção, use um banco PostgreSQL gerenciado e altere o provider do Entity Framework.

## Testar Docker localmente

Na pasta que contém o Dockerfile:

```powershell
docker build -t cted-catalogo-api .
docker run --rm -p 8080:8080 -e PORT=8080 cted-catalogo-api
```

Depois acesse:

```text
http://localhost:8080/health
http://localhost:8080/swagger
```
