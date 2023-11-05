using Curs.Models;
using Microsoft.EntityFrameworkCore;

namespace Curs.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=kurs;Username=postgres;Password=123");

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

        public DbSet<Curs.Models.Pet> Pet { get; set; } = default!;
        public DbSet<Like> Like { get; set; } = default!;

        public DbSet<Curs.Models.Post> Post { get; set; } = default!;

        public DbSet<Curs.Models.Commentary> Commentary { get; set; } = default!;

        public DbSet<Curs.Models.Friend> Friend { get; set; } = default!;
        public DbSet<Document> Document { get; set; } = default!;
    }
}