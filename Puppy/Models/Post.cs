using System.ComponentModel.DataAnnotations;

namespace Curs.Models;

public class Post
{
    public int Id { get; set; }
    
    public Author Author { get; set; }
    
    public string Description { get; set; }

    public List<string> Img { get; set; }

    public int Likes { get; set; }

    public int Comments { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime UploadDate { get; set; }

    public User User { get; set; }
    public ICollection<Commentary> Commentaries { get; set; } = new List<Commentary>();
}