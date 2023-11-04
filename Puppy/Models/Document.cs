namespace Curs.Models;

public class Document
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Pet Pet { get; set; }
    public int PetId { get; set; }
    public string[] Imgs { get; set; }
}