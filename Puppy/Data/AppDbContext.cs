using Microsoft.EntityFrameworkCore;
using Puppy.Models;

namespace Puppy.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("dogString"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>();
            modelBuilder.Entity<Complaint>()
                .HasOne(e => e.User)
                .WithMany(e => e.Complaints);
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
            modelBuilder.Entity<ChatRoom>()
                .HasKey(c => c.RoomId);
            modelBuilder.Entity<ChatRoom>()
                .HasMany(c => c.ChatMessages)
                .WithOne(m => m.ChatRoom)
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChatRoom>()
                .HasOne(c => c.User1)
                .WithMany()
                .HasForeignKey(c => c.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatRoom>()
                .HasOne(c => c.User2)
                .WithMany()
                .HasForeignKey(c => c.User2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Admin> Admin { get; set; }
        public DbSet<Complaint> Complaint { get; set; }
        public DbSet<User> Users { get; set; } = default!;

        public DbSet<Pet> Pet { get; set; } = default!;
        public DbSet<Like?> Like { get; set; } = default!;

        public DbSet<Post> Post { get; set; } = default!;

        public DbSet<Commentary> Commentary { get; set; } = default!;

        public DbSet<Friend?> Friend { get; set; } = default!;
        public DbSet<Document> Document { get; set; } = default!;
        public DbSet<ChatMessage?> ChatMessage { get; set; } = default!;
        public DbSet<ChatRoom?> ChatRoom { get; set; } = default!;
    }
}