using Curs.Models;
using Microsoft.EntityFrameworkCore;

namespace Curs.Data
{
    public class AppDbContext : DbContext
    {
        private static string Get()
        {
            var uri = new Uri(
                "postgres://qtzdvcyq:jXT76DhO3geh4fq7halb_gCFVayQjShJ@cornelius.db.elephantsql.com/qtzdvcyq");
            var db = uri.AbsolutePath.Trim('/');
            var user = uri.UserInfo.Split(':')[0];
            var passwd = uri.UserInfo.Split(':')[1];
            var port = uri.Port > 0 ? uri.Port : 5432;
            var connStr = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}",
                uri.Host, db, user, passwd, port);
            Console.WriteLine(connStr);
            return connStr;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(Get());

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