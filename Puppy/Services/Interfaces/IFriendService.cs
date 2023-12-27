namespace Puppy.Services.Interfaces;

public interface IFriendService
{
    Task<bool> IsFriend(int userId, int currentUserId);
}