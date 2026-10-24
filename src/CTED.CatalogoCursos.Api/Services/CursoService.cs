using CTED.CatalogoCursos.Api.Data;
using CTED.CatalogoCursos.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CTED.CatalogoCursos.Api.Services;

public sealed class CursoService(CatalogoDbContext db)
{
    public async Task<CursoConsultaDto> ConsultarAsync(string? busca, string? categoria, string? modalidade, string? status, CancellationToken cancellationToken)
    {
        var query = db.Cursos.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(busca))
        {
            var termo = busca.Trim().ToLower();
            query = query.Where(x => x.NomeCompleto.ToLower().Contains(termo) || x.NomeBreve.ToLower().Contains(termo));
        }
        if (!string.IsNullOrWhiteSpace(categoria)) query = query.Where(x => x.Categoria == categoria);
        if (!string.IsNullOrWhiteSpace(modalidade)) query = query.Where(x => x.Modalidade == modalidade);
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(x => x.Status == status);

        var cursos = await query.OrderBy(x => x.NomeCompleto).ToListAsync(cancellationToken);
        var resultados = cursos.Select(x => new CursoResumoDto(x.Id, x.NomeCompleto, x.NomeBreve, x.Descricao, x.Categoria, x.Modalidade, x.CargaHoraria, x.Status, x.ImagemUrl, x.DataDisponibilizacao)).ToArray();
        return new CursoConsultaDto(resultados.Length, busca, categoria, modalidade, status, resultados);
    }

    public async Task<CursoDetalheDto?> ObterDetalheAsync(int id, CancellationToken cancellationToken)
    {
        var curso = await db.Cursos.AsNoTracking().Include(x => x.Modulos).ThenInclude(x => x.Aulas).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (curso is null) return null;
        return new CursoDetalheDto(curso.Id, curso.NomeCompleto, curso.NomeBreve, curso.Descricao, curso.Categoria, curso.Modalidade, curso.CargaHoraria, curso.PublicoAlvo, curso.Status, curso.ImagemUrl, curso.DataDisponibilizacao,
            curso.Modulos.OrderBy(x => x.Ordem).Select(m => new ModuloDto(m.Nome, m.Ordem, m.Aulas.OrderBy(a => a.Ordem).Select(a => new AulaDto(a.Titulo, a.Descricao, a.Ordem)).ToArray())).ToArray());
    }

    public async Task<OpcoesFiltroDto> ObterOpcoesAsync(CancellationToken cancellationToken)
    {
        var categorias = await db.Cursos.AsNoTracking().Select(x => x.Categoria).Distinct().OrderBy(x => x).ToArrayAsync(cancellationToken);
        var modalidades = await db.Cursos.AsNoTracking().Select(x => x.Modalidade).Distinct().OrderBy(x => x).ToArrayAsync(cancellationToken);
        var status = await db.Cursos.AsNoTracking().Select(x => x.Status).Distinct().OrderBy(x => x).ToArrayAsync(cancellationToken);
        return new OpcoesFiltroDto(categorias, modalidades, status);
    }
}
