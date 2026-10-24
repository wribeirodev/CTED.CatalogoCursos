using CTED.CatalogoCursos.Api.DTOs;
using CTED.CatalogoCursos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CTED.CatalogoCursos.Api.Controllers;

[ApiController]
[Route("api/cursos")]
[Produces("application/json")]
public sealed class CursosController(CursoService service) : ControllerBase
{
    /// <summary>Lista, pesquisa e filtra o catálogo. Todos os parâmetros podem ser combinados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(CursoConsultaDto), StatusCodes.Status200OK)]
    public Task<CursoConsultaDto> ListarCursosPorFiltro(
        [FromQuery] string? nomeCurso, 
        [FromQuery] string? categoria, 
        [FromQuery] string? modalidade, 
        [FromQuery] string? status, 
        CancellationToken cancellationToken)
            => service.ObterCursosPorFiltro(nomeCurso, categoria, modalidade, status, cancellationToken);

    /// <summary>Retorna os filtros disponíveis no catálogo.</summary>
    [HttpGet("opcoes")]
    [ProducesResponseType(typeof(OpcoesFiltroDto), StatusCodes.Status200OK)]
    public Task<OpcoesFiltroDto> ObterOpcoes(CancellationToken cancellationToken) 
        => service.ObterOpcoesAsync(cancellationToken);

    /// <summary>Retorna um curso com módulos e aulas.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CursoDetalheDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CursoDetalheDto>> ObterCursoDetalhadoPorId(
        int id, 
        CancellationToken cancellationToken)
    {
        var curso = await service.ObterDetalheAsync(id, cancellationToken);
        return curso is null ? NotFound(new { mensagem = "Curso não encontrado." }) : Ok(curso);
    }
}
