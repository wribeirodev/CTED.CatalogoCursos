using CTED.CatalogoCursos.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace CTED.CatalogoCursos.Api.Data;

public sealed class CatalogoDbContext(DbContextOptions<CatalogoDbContext> options) : DbContext(options)
{
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Modulo> Modulos => Set<Modulo>();
    public DbSet<Aula> Aulas => Set<Aula>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.NomeCompleto).HasMaxLength(160).IsRequired();
            entity.Property(x => x.NomeBreve).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Categoria).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Modalidade).HasMaxLength(40).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(40).IsRequired();
            entity.HasIndex(x => new { x.Categoria, x.Modalidade, x.Status });
        });

        modelBuilder.Entity<Modulo>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nome).HasMaxLength(120).IsRequired();
            entity.HasOne(x => x.Curso).WithMany(x => x.Modulos).HasForeignKey(x => x.CursoId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Aula>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Titulo).HasMaxLength(160).IsRequired();
            entity.HasOne(x => x.Modulo).WithMany(x => x.Aulas).HasForeignKey(x => x.ModuloId).OnDelete(DeleteBehavior.Cascade);
        });

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        var cursos = new[]
        {
            new Curso { Id = 1, NomeCompleto = "Fundamentos de Pensamento Computacional", NomeBreve = "Pensamento Computacional", Descricao = "Desenvolva raciocínio lógico para resolver problemas do cotidiano e da tecnologia.", Categoria = "Tecnologia", Modalidade = "Online", CargaHoraria = 20, PublicoAlvo = "Iniciantes e estudantes do ensino médio", Status = "Disponível", ImagemUrl = "https://images.unsplash.com/photo-1516321318423-f06f85e504b3?auto=format&fit=crop&w=900&q=80", DataDisponibilizacao = new DateTime(2026, 1, 15) },
            new Curso { Id = 2, NomeCompleto = "Comunicação Profissional no Ambiente Digital", NomeBreve = "Comunicação Digital", Descricao = "Práticas para escrever, apresentar ideias e colaborar em ambientes digitais.", Categoria = "Comunicação", Modalidade = "Online", CargaHoraria = 12, PublicoAlvo = "Profissionais e pessoas em transição de carreira", Status = "Disponível", ImagemUrl = "https://images.unsplash.com/photo-1553877522-43269d4ea984?auto=format&fit=crop&w=900&q=80", DataDisponibilizacao = new DateTime(2026, 2, 10) },
            new Curso { Id = 3, NomeCompleto = "Introdução à Gestão de Projetos", NomeBreve = "Gestão de Projetos", Descricao = "Conheça conceitos, papéis e ferramentas para organizar projetos de forma prática.", Categoria = "Gestão", Modalidade = "Híbrido", CargaHoraria = 30, PublicoAlvo = "Líderes, analistas e equipes multidisciplinares", Status = "Em breve", ImagemUrl = "https://images.unsplash.com/photo-1454165804606-c3d57bc86b40?auto=format&fit=crop&w=900&q=80", DataDisponibilizacao = new DateTime(2026, 11, 5) },
            new Curso { Id = 4, NomeCompleto = "Segurança e Cidadania na Internet", NomeBreve = "Cidadania Digital", Descricao = "Aprenda a proteger dados pessoais e a agir com responsabilidade na internet.", Categoria = "Cidadania", Modalidade = "Presencial", CargaHoraria = 8, PublicoAlvo = "Comunidade escolar e público geral", Status = "Disponível", ImagemUrl = "https://images.unsplash.com/photo-1563013544-824ae1b704d3?auto=format&fit=crop&w=900&q=80", DataDisponibilizacao = new DateTime(2026, 3, 20) },
            new Curso { Id = 5, NomeCompleto = "Análise de Dados com Planilhas", NomeBreve = "Dados com Planilhas", Descricao = "Organize, explore e comunique informações usando planilhas e indicadores.", Categoria = "Tecnologia", Modalidade = "Híbrido", CargaHoraria = 24, PublicoAlvo = "Profissionais administrativos e empreendedores", Status = "Encerrado", ImagemUrl = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?auto=format&fit=crop&w=900&q=80", DataDisponibilizacao = new DateTime(2025, 9, 1) },
            new Curso { Id = 6, NomeCompleto = "Práticas de Colaboração e Trabalho em Equipe", NomeBreve = "Trabalho em Equipe", Descricao = "Fortaleça colaboração, escuta ativa e tomada de decisão em equipe.", Categoria = "Gestão", Modalidade = "Presencial", CargaHoraria = 16, PublicoAlvo = "Educadores, gestores e equipes de atendimento", Status = "Disponível", ImagemUrl = "https://images.unsplash.com/photo-1521737711867-e3b97375f902?auto=format&fit=crop&w=900&q=80", DataDisponibilizacao = new DateTime(2026, 4, 12) }
        };
        modelBuilder.Entity<Curso>().HasData(cursos);

        var modulos = new List<Modulo>();
        var aulas = new List<Aula>();
        var moduloId = 1;
        var aulaId = 1;
        foreach (var curso in cursos)
        {
            for (var m = 1; m <= 2; m++)
            {
                var currentModuloId = moduloId++;
                modulos.Add(new Modulo { Id = currentModuloId, CursoId = curso.Id, Nome = $"Módulo {m} — {ModuloNome(curso.Categoria, m)}", Ordem = m });
                aulas.Add(new Aula { Id = aulaId++, ModuloId = currentModuloId, Titulo = $"Aula {m}.1 — Conceitos essenciais", Descricao = "Conteúdo introdutório com exemplos e atividade guiada.", Ordem = 1 });
                aulas.Add(new Aula { Id = aulaId++, ModuloId = currentModuloId, Titulo = $"Aula {m}.2 — Aplicação prática", Descricao = "Exercícios para aplicar os conhecimentos em uma situação fictícia.", Ordem = 2 });
            }
        }
        modelBuilder.Entity<Modulo>().HasData(modulos);
        modelBuilder.Entity<Aula>().HasData(aulas);
    }

    private static string ModuloNome(string categoria, int ordem) => categoria switch
    {
        "Tecnologia" => ordem == 1 ? "Base e ferramentas" : "Prática orientada",
        "Comunicação" => ordem == 1 ? "Clareza e contexto" : "Colaboração digital",
        "Gestão" => ordem == 1 ? "Planejamento" : "Acompanhamento",
        _ => ordem == 1 ? "Fundamentos" : "Atividade prática"
    };
}
