namespace Puppy.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Pet> Pets { get; set; } = new List<Pet>();

        public ICollection<Friend> Friends { get; set; } = new List<Friend>();
        public ICollection<Friend> Followers { get; set; } = new List<Friend>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}