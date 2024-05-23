namespace Puppy.Models;

public class ChatRoom
{
    public int Id { get; set; }
    public string RoomId { get; set; }
    public int User1Id { get; set; }
    public int User2Id { get; set; }
    public User User1 { get; set; }
    public User User2 { get; set; }
    public ICollection<ChatMessage> ChatMessages { get; set; }
}