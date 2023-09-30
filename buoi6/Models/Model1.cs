using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace buoi6.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Nhansudbcontext")
        {
        }

        public virtual DbSet<Nhanvien> Nhanviens { get; set; }
        public virtual DbSet<Phongban> Phongbans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nhanvien>()
                .Property(e => e.MaNV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Nhanvien>()
                .Property(e => e.MaPB)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Phongban>()
                .Property(e => e.MaPB)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Phongban>()
                .HasMany(e => e.Nhanviens)
                .WithRequired(e => e.Phongban)
                .WillCascadeOnDelete(false);
        }
    }
}
