using Curs.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Curs.Data
{
	public class AppDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=postgres;Username=root;Password=123");

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
		}

		public DbSet<User> Users { get; set; }

		public DbSet<Curs.Models.Pet> Pet { get; set; } = default!;

		public DbSet<Curs.Models.Post> Post { get; set; } = default!;

		public DbSet<Curs.Models.Commentary> Commentary { get; set; } = default!;
	}
}
