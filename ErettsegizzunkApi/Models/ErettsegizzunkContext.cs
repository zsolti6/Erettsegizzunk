using Microsoft.EntityFrameworkCore;

namespace ErettsegizzunkApi.Models;

public partial class ErettsegizzunkContext : DbContext
{
    public ErettsegizzunkContext()
    {
    }

    public ErettsegizzunkContext(DbContextOptions<ErettsegizzunkContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Feladatok> Feladatoks { get; set; }

    public virtual DbSet<Szint> Szints { get; set; }

    public virtual DbSet<Tantargyak> Tantargyaks { get; set; }

    public virtual DbSet<Tema> Temas { get; set; }

    public virtual DbSet<Tipus> Tipus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=localhost;Database=erettsegizzunk;User=root;Password=;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feladatok>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("feladatok");

            entity.HasIndex(e => e.SzintId, "szintId");

            entity.HasIndex(e => e.TantargyId, "tantargyId");

            entity.HasIndex(e => e.TipusId, "tipusId");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Helyese)
                .HasMaxLength(20)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("helyese");
            entity.Property(e => e.Leiras)
                .HasMaxLength(1024)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("leiras");
            entity.Property(e => e.Megoldasok)
                .HasMaxLength(1024)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("megoldasok");
            entity.Property(e => e.SzintId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("szintId");
            entity.Property(e => e.TantargyId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("tantargyId");
            entity.Property(e => e.TipusId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("tipusId");

            entity.HasOne(d => d.Szint).WithMany(p => p.Feladatoks)
                .HasForeignKey(d => d.SzintId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("feladatok_ibfk_3");

            entity.HasOne(d => d.Tantargy).WithMany(p => p.Feladatoks)
                .HasForeignKey(d => d.TantargyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("feladatok_ibfk_1");

            entity.HasOne(d => d.Tipus).WithMany(p => p.Feladatoks)
                .HasForeignKey(d => d.TipusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("feladatok_ibfk_2");

            entity.HasMany(d => d.Temas).WithMany(p => p.Feladatoks)
                .UsingEntity<Dictionary<string, object>>(
                    "FeladatokTema",
                    r => r.HasOne<Tema>().WithMany()
                        .HasForeignKey("TemaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("feladatok_tema_ibfk_2"),
                    l => l.HasOne<Feladatok>().WithMany()
                        .HasForeignKey("FeladatokId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("feladatok_tema_ibfk_1"),
                    j =>
                    {
                        j.HasKey("FeladatokId", "TemaId").HasName("PRIMARY");
                        j.ToTable("feladatok_tema");
                        j.HasIndex(new[] { "TemaId" }, "temaId");
                        j.IndexerProperty<int>("FeladatokId")
                            .HasColumnType("int(11)")
                            .HasColumnName("feladatokId");
                        j.IndexerProperty<int>("TemaId")
                            .HasColumnType("int(11)")
                            .HasColumnName("temaId");
                    });
        });

        modelBuilder.Entity<Szint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("szint");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Nev)
                .HasMaxLength(15)
                .HasColumnName("nev");
        });

        modelBuilder.Entity<Tantargyak>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tantargyak");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Nev)
                .HasMaxLength(255)
                .HasColumnName("nev");
        });

        modelBuilder.Entity<Tema>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tema");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Nev)
                .HasMaxLength(255)
                .HasColumnName("nev");
        });

        modelBuilder.Entity<Tipus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipus");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Nev)
                .HasMaxLength(255)
                .HasColumnName("nev");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
