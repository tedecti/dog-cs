namespace Puppy.Models;

public class Complaint
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Commentary { get; set; }
    public string Status { get; set; }
    public User User { get; set; }
    public Post Post { get; set; }
}