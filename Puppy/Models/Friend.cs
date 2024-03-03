namespace Puppy.Models;

public class Friend
{
    public int Id { get; set; }
    
    public int UserId{ get; set; }
    public int FollowerId{ get; set; }
    
    public User User{ get; set; }
    public User Follower{ get; set; }
}