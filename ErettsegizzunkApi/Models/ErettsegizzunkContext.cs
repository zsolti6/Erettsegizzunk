using System;
using System.Collections.Generic;
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

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<SpacedRepetition> SpacedRepetitions { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Theme> Themes { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserStatistic> UserStatistics { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("SERVER=localhost;PORT=3306;DATABASE=erettsegizzunk;USER=root;PASSWORD=;SSL MODE=none;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("level");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(15)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("permission");

            entity.HasIndex(e => e.Level, "level").IsUnique();

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("description");
            entity.Property(e => e.Level)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(1)")
                .HasColumnName("level");
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("name");
        });

        modelBuilder.Entity<SpacedRepetition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("spaced_repetition");

            entity.HasIndex(e => e.TaskId, "taskId");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IntervalDays)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("intervalDays");
            entity.Property(e => e.LastCorrectTime)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp")
                .HasColumnName("lastCorrectTime");
            entity.Property(e => e.NextDueTime)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp")
                .HasColumnName("nextDueTime");
            entity.Property(e => e.TaskId)
                .HasColumnType("int(11)")
                .HasColumnName("taskId");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("userId");

            entity.HasOne(d => d.Task).WithMany(p => p.SpacedRepetitions)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("spaced_repetition_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.SpacedRepetitions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("spaced_repetition_ibfk_1");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("subject");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("task");

            entity.HasIndex(e => e.LevelId, "levelId");

            entity.HasIndex(e => e.SubjectId, "subjectId");

            entity.HasIndex(e => e.TypeId, "typeId");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Answers)
                .HasMaxLength(1024)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("answers");
            entity.Property(e => e.Description)
                .HasMaxLength(1024)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("description");
            entity.Property(e => e.IsCorrect)
                .HasMaxLength(20)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("isCorrect");
            entity.Property(e => e.LevelId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("levelId");
            entity.Property(e => e.PicName)
                .HasMaxLength(18)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("picName");
            entity.Property(e => e.SubjectId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("subjectId");
            entity.Property(e => e.Text)
                .HasMaxLength(1024)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("text");
            entity.Property(e => e.TypeId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("typeId");

            entity.HasOne(d => d.Level).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("task_ibfk_3");

            entity.HasOne(d => d.Subject).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("task_ibfk_1");

            entity.HasOne(d => d.Type).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("task_ibfk_2");

            entity.HasMany(d => d.Themes).WithMany(p => p.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskTheme",
                    r => r.HasOne<Theme>().WithMany()
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("task_theme_ibfk_2"),
                    l => l.HasOne<Task>().WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("task_theme_ibfk_1"),
                    j =>
                    {
                        j.HasKey("TaskId", "ThemeId").HasName("PRIMARY");
                        j.ToTable("task_theme");
                        j.HasIndex(new[] { "ThemeId" }, "themeId");
                        j.IndexerProperty<int>("TaskId")
                            .HasColumnType("int(11)")
                            .HasColumnName("taskId");
                        j.IndexerProperty<int>("ThemeId")
                            .HasColumnType("int(11)")
                            .HasColumnName("themeId");
                    });
        });

        modelBuilder.Entity<Theme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("theme");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("type");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.LoginName, "loginName").IsUnique();

            entity.HasIndex(e => e.PermissionId, "permission");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Email)
                .HasMaxLength(64)
                .HasColumnName("email");
            entity.Property(e => e.Hash)
                .HasMaxLength(64)
                .HasColumnName("HASH");
            entity.Property(e => e.LoginName)
                .HasMaxLength(16)
                .HasColumnName("loginName");
            entity.Property(e => e.Newsletter).HasColumnName("newsletter");
            entity.Property(e => e.PermissionId)
                .HasColumnType("int(11)")
                .HasColumnName("permissionId");
            entity.Property(e => e.ProfilePicturePath)
                .HasMaxLength(64)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("profilePicturePath");
            entity.Property(e => e.Salt)
                .HasMaxLength(64)
                .HasColumnName("SALT");
            entity.Property(e => e.SignupDate)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp")
                .HasColumnName("signupDate");
            entity.Property(e => e.GoogleUser)
                .HasDefaultValueSql("'0'")
                .HasColumnName("googleUser");

            entity.HasOne(d => d.Permission).WithMany(p => p.Users)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("user_ibfk_1");
        });

        modelBuilder.Entity<UserStatistic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_statistics");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.HistorySuccessfulTasks)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("historySuccessfulTasks");
            entity.Property(e => e.HistoryUnsuccessfulTasks)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("historyUnsuccessfulTasks");
            entity.Property(e => e.HungarianSuccessfulTasks)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("hungarianSuccessfulTasks");
            entity.Property(e => e.HungarianUnsuccessfulTasks)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("hungarianUnsuccessfulTasks");
            entity.Property(e => e.MathSuccessfulTasks)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("mathSuccessfulTasks");
            entity.Property(e => e.MathUnsuccessfulTasks)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("mathUnsuccessfulTasks");
            entity.Property(e => e.StatisticsDate)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("date")
                .HasColumnName("statisticsDate");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.UserStatistics)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_statistics_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
