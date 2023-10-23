using System.ComponentModel.DataAnnotations;

namespace Curs.Models;

public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public string[] Img { get; set; }

    public int Comments { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime UploadDate { get; set; }

    public User User { get; set; }
    public ICollection<Commentary> Commentaries { get; set; } = new List<Commentary>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
}