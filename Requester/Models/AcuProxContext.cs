using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Requester.Models;

public partial class AcuProxContext : DbContext
{
    public AcuProxContext()
    {
    }

    public AcuProxContext(DbContextOptions<AcuProxContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Log> Logs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=NA-LT-1094;Initial Catalog=AcuProx;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3214EC07C16ABD31");

            entity.ToTable("Log");

            entity.Property(e => e.Cookies).HasMaxLength(2048);
            entity.Property(e => e.Dt)
                .HasColumnType("datetime")
                .HasColumnName("DT");
            entity.Property(e => e.Duration).HasPrecision(3);
            entity.Property(e => e.Headers).IsUnicode(false);
            entity.Property(e => e.Host).HasMaxLength(128);
            entity.Property(e => e.ParsedType)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.Path).HasMaxLength(512);
            entity.Property(e => e.ProcGuid).HasColumnName("ProcGUID");
            entity.Property(e => e.QueryString).HasMaxLength(1024);
            entity.Property(e => e.Site).HasMaxLength(128);
            entity.Property(e => e.StatusCode)
                .HasMaxLength(4)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
