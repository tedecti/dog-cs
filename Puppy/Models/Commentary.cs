namespace Puppy.Models;

public class Commentary
{
    public int Id { get; set; }
    public Post Post { get; set; }
    public int PostId { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; }
    public DateTime UploadDate { get; set; }
    

}