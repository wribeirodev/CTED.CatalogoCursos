namespace CTED.CatalogoCursos.Api.DTOs;

public sealed record CursoResumoDto(
    int Id,
    string NomeCompleto,
    string NomeBreve,
    string Descricao,
    string Categoria,
    string Modalidade,
    int CargaHoraria,
    string Status,
    string ImagemUrl,
    DateTime? DataDisponibilizacao);

public sealed record AulaDto(string Titulo, string Descricao, int Ordem);
public sealed record ModuloDto(string Nome, int Ordem, IReadOnlyCollection<AulaDto> Aulas);

public sealed record CursoDetalheDto(
    int Id,
    string NomeCompleto,
    string NomeBreve,
    string Descricao,
    string Categoria,
    string Modalidade,
    int CargaHoraria,
    string PublicoAlvo,
    string Status,
    string ImagemUrl,
    DateTime? DataDisponibilizacao,
    IReadOnlyCollection<ModuloDto> Modulos);

public sealed record CursoConsultaDto(
    int Total,
    string? Busca,
    string? Categoria,
    string? Modalidade,
    string? Status,
    IReadOnlyCollection<CursoResumoDto> Resultados);

public sealed record OpcoesFiltroDto(
    IReadOnlyCollection<string> Categorias,
    IReadOnlyCollection<string> Modalidades,
    IReadOnlyCollection<string> Status);
