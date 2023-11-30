using Curs.Models;
using Microsoft.EntityFrameworkCore;

namespace Curs.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=cornelius.db.elephantsql.com;Database=qtzdvcyq;Username=qtzdvcyq;Password=jXT76DhO3geh4fq7halb_gCFVayQjShJ");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(c => c.Pets)
                .WithOne(e => e.User);

            modelBuilder.Entity<Pet>()
                .HasOne(e => e.User)
                .WithMany(c => c.Pets);
            modelBuilder.Entity<Post>()
                .HasOne(e => e.User)
                .WithMany(c => c.Posts);

            modelBuilder.Entity<Commentary>()
                .HasOne(e => e.Post)
                .WithMany(c => c.Commentaries);

            modelBuilder.Entity<Friend>()
                .HasOne(e => e.Follower)
                .WithMany(c => c.Friends);
            modelBuilder.Entity<Friend>()
                .HasOne(e => e.User)
                .WithMany(c => c.Followers);
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes);
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes);
            modelBuilder.Entity<Document>()
                .HasOne(d => d.Pet)
                .WithMany(p => p.Documents);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Curs.Models.Pet> Pets { get; set; } = default!;
        public DbSet<Like> Likes { get; set; } = default!;

        public DbSet<Curs.Models.Post> Posts { get; set; } = default!;

        public DbSet<Curs.Models.Commentary> Commentaries { get; set; } = default!;

        public DbSet<Curs.Models.Friend> Friends { get; set; } = default!;
        public DbSet<Document> Documents { get; set; } = default!;
    }
}