# Registro de uso de Inteligência Artificial

## Ferramenta utilizada

Foi utilizado um assistente de Inteligência Artificial (Manus) para apoiar a interpretação das instruções da atividade, o desenho da arquitetura, a criação do esqueleto da API em C#/.NET, a elaboração do seed de dados fictícios e a revisão da documentação.

## Objetivo do uso

O objetivo foi acelerar a criação da base técnica do backend, mantendo separação clara entre domínio, acesso a dados, serviços e controllers. A IA também foi usada para sugerir contratos JSON, filtros combináveis e instruções de execução/publicação.

## Prompts relevantes e decisão de aproveitamento

1. Solicitação para analisar o PDF e implementar somente API e banco compatíveis com Visual Studio 2022. Foi aproveitada a extração dos requisitos: consulta, busca, filtros combináveis, quantidade, detalhe, dados fictícios e documentação.
2. Solicitação para criar uma Web API .NET organizada, com SQLite e Entity Framework Core. Foram aproveitadas a divisão em camadas, o uso de DTOs, Swagger, health check, CORS e seed.
3. Solicitação para revisar a documentação e as instruções de publicação. Foram aproveitados os exemplos de execução local, publicação em pasta, IIS e Linux/Nginx.

Não foram aproveitadas sugestões de front-end, CRUD de cursos, integração obrigatória com Moodle ou uso de serviços pagos, pois estavam fora do recorte solicitado para esta entrega.

## Como o conteúdo foi validado

A implementação foi comparada item a item com o PDF. O modelo de dados possui os campos obrigatórios: nome completo, nome breve, descrição, categoria, modalidade, carga horária, público-alvo, status, imagem e data. O seed contém seis cursos, três ou mais categorias, duas ou mais modalidades, três status e módulos/aulas variados. Os endpoints foram desenhados para cobrir cada funcionalidade obrigatória do catálogo.

A validação técnica final deve ser feita no ambiente do candidato com Visual Studio 2022 e .NET 8 SDK, executando a solução, abrindo o Swagger e testando os endpoints descritos no README.

## Exemplo de erro ou inadequação identificado

Uma abordagem inicial poderia retornar somente uma lista de cursos, sem o total de resultados e sem um endpoint de detalhe com módulos e aulas. Isso não atenderia integralmente às exigências de “apresentar a quantidade de resultados” e “visualizar informações detalhadas”.

## Correção realizada

Foi criado o DTO `CursoConsultaDto`, que retorna `Total` e `Resultados`, e o endpoint `GET /api/cursos/{id}` foi implementado com `Include`/`ThenInclude` para retornar módulos e descrições de aulas. Também foi documentado que a busca sem resultados retorna HTTP 200 com lista vazia, permitindo ao front-end exibir a mensagem de estado vazio.
