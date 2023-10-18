namespace Curs.Models
{
	public class User
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		
		public ICollection<Pet> Pets { get; set; } = new List<Pet>();
		
		public List<User> Friends { get; set; } 

		public ICollection<Post> Posts { get; set; } = new List<Post>();
	}
}
