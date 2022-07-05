using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Domain.Authorization;

using Microsoft.EntityFrameworkCore;

namespace ChurchManagementSystem.Core.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleClaim> RoleClaims { get; set; }
        public virtual DbSet<Claim> Claims { get; set; }
        public virtual DbSet<TokenManager> Tokens { get; set; }
        public virtual DbSet<Template> Templates { get; set; }
     
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FirstName)
                   .IsRequired()
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.LastName)
                   .IsRequired()
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Salt)
                   .IsRequired()
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.HashPassword)
                   .IsRequired()
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.Active)
                    .IsRequired();

                entity.Property(e => e.CannotChangePassword)
                    .IsRequired();

                entity.Property(e => e.CreatedDate)
                   .IsRequired();
                  

                entity.Property(e => e.ModifiedDate)
                   .IsRequired();
                  

            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.Description)
                   .IsRequired()
                   .HasMaxLength(200)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.RoleType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CreatedDate)
                   .IsRequired();

                entity.Property(e => e.ModifiedDate)
                   .IsRequired();
                  
            });

            modelBuilder.Entity<Claim>(entity =>
            {
                entity.HasKey(e => e.ClaimId);

                entity.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.Description)
                   .IsRequired()
                   .HasMaxLength(200)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.CreatedDate)
                   .HasColumnType("timestamp")
                   .IsRequired();


                entity.Property(e => e.ModifiedDate)
                   .IsRequired();
                   
            });

            modelBuilder.Entity<RoleClaim>(entity =>
            {
                entity.HasKey(e => e.RoleClaimId);

                entity.Property(e => e.Active)
                     .IsRequired();
                     
            });

            modelBuilder.Entity<TokenManager>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.Token)
                   .IsRequired()
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.TimeStamp)
                   .HasColumnType("timestamp")
                   .IsRequired();

                entity.Property(e => e.CreatedDate)
                  .IsRequired();

                entity.Property(e => e.Expired)
                    .IsRequired();

                entity.Property(e => e.Used)
                    .IsRequired();
                  
            });

        
         

            //modelBuilder.Entity<TransactionLog>(entity =>
            //{
            //    entity.HasOne(d => d.UserId)
            //        .WithMany(p => p.ConnectionStartCity)
            //        .HasForeignKey(d => d.StartCityId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Verbinding_BeginStad");

            //    entity.HasOne(d => d.EndCity)
            //        .WithMany(p => p.ConnectionEndCity)
            //        .HasForeignKey(d => d.EndCityId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Verbinding_EindStad");
            //});
        }
    }
}