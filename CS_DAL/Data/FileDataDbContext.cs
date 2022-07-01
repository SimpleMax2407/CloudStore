using CS_DAL.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;

namespace CS_DAL.Data
{
    
    public class FileDataDbContext : DbContext
    {
        public FileDataDbContext() : base() { }
        public FileDataDbContext(DbContextOptions<FileDataDbContext> options) : base(options)  { }
        
        public DbSet<FileDatum> FileData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileDatum>().HasKey(fd => fd.Id);
            modelBuilder.Entity<FileDatum>().Property(fd => fd.UserName).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<FileDatum>().Property(fd => fd.FileName).HasMaxLength(256).IsRequired(); 
            modelBuilder.Entity<FileDatum>().Property(fd => fd.CreationDate).IsRequired();
            modelBuilder.Entity<FileDatum>().Property(fd => fd.EditDate).IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=FileData;Trusted_Connection=True;");
        }
    }
}
