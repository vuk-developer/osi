using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OSI.Models;

public partial class OSIContext : DbContext
{
    public string conn = string.Empty;
    public OSIContext()
    {
        IConfigurationBuilder build = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
        IConfiguration _configuration = build.Build();
        var connex = _configuration.GetConnectionString("osi");
        conn = connex;
    }

    public OSIContext(DbContextOptions<OSIContext> options)
        : base(options)
    {
    }

  

    public virtual DbSet<Clanovi> Clanovi { get; set; }

    public virtual DbSet<Knjige> Knjige { get; set; }

 protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    => optionsBuilder.UseSqlServer(conn);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Clanovi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__clanovi__3214EC07F33AD11F");

            entity.ToTable("clanovi");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Id");
            entity.Property(e => e.DatumRodjenja).HasColumnName("datum_rodjenja");
            entity.Property(e => e.Evidencija)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("evidencija");
            entity.Property(e => e.Ime)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ime");
            entity.Property(e => e.Mesto)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mesto");
            entity.Property(e => e.Prezime)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("prezime");
        });

        modelBuilder.Entity<Knjige>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__knjige__3214EC07912C49DC");

            entity.ToTable("knjige");

            entity.Property(e => e.Autor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("autor");
            entity.Property(e => e.ImeKnjige)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ime_knjige");
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("isbn");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Signatura)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("signatura");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
