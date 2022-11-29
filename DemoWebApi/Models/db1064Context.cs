using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DemoWebApi.ViewModel;

#nullable disable

namespace DemoWebApi.Models
{
    public partial class db1064Context : DbContext
    {
        public db1064Context()
        {
        }

        public db1064Context(DbContextOptions<db1064Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Dept> Depts { get; set; }
        public virtual DbSet<Emp> Emps { get; set; }
        public virtual DbSet<DeptInfo_VM> DeptInfo_VMs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=DESKTOP-3MP3VAU\\sqlexpress;initial catalog=db1064;trusted_connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Dept>(entity =>
            {
                entity.ToTable("Dept");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Location)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("location");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Emp>(entity =>
            {
                entity.ToTable("Emp");

                entity.HasIndex(e => e.Email, "UQ__Emp__AB6E6164D05B523B")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Deptid).HasColumnName("deptid");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.HasOne(d => d.Dept)
                    .WithMany(p => p.Emps)
                    .HasForeignKey(d => d.Deptid)
                    .HasConstraintName("FK__Emp__deptid__1BFD2C07");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
