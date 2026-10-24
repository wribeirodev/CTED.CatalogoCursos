namespace CTED.CatalogoCursos.Api.Domain;

public sealed class Curso
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string NomeBreve { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Modalidade { get; set; } = string.Empty;
    public int CargaHoraria { get; set; }
    public string PublicoAlvo { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ImagemUrl { get; set; } = string.Empty;
    public DateTime? DataDisponibilizacao { get; set; }
    public List<Modulo> Modulos { get; set; } = [];
}

public sealed class Modulo
{
    public int Id { get; set; }
    public int CursoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public Curso Curso { get; set; } = null!;
    public List<Aula> Aulas { get; set; } = [];
}

public sealed class Aula
{
    public int Id { get; set; }
    public int ModuloId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public Modulo Modulo { get; set; } = null!;
}
