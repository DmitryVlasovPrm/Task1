using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi
{
    public partial class ServerContext : DbContext
    {
        private readonly string _connectionString;

        public ServerContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ServerContext(DbContextOptions<ServerContext> options)
            : base(options)
        { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<UserState> UserStates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Login, "UQ__User__5E55825B09A1314E")
                    .IsUnique();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.UserGroup)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserGroupId)
                    .HasConstraintName("FK__User__UserGroupI__71D1E811");

                entity.HasOne(d => d.UserState)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserStateId)
                    .HasConstraintName("FK__User__UserStateI__72C60C4A");
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("UserGroup");

                entity.HasIndex(e => e.Code, "UQ__UserGrou__A25C5AA7B46393DE")
                    .IsUnique();

                entity.Property(e => e.UserGroupId).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserState>(entity =>
            {
                entity.ToTable("UserState");

                entity.HasIndex(e => e.Code, "UQ__UserStat__A25C5AA77C1C2918")
                    .IsUnique();

                entity.Property(e => e.UserStateId).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
