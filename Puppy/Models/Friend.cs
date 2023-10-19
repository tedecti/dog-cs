namespace Curs.Models;

public class Friend
{
    public int Id { get; set; }
    
    public int User_Id1{ get; set; }
    
    
    public int User_Id2{ get; set; }
    
    public User User{ get; set; }
    
    public string Status{ get; set; }
    
    public DateTime FriendshipDate { get; set; }
}