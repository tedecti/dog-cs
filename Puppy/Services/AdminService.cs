namespace Puppy.Services.Interfaces;

public class AdminService : IAdminService
{
     public Boolean VerifyAdminRole(string Role)
     {
          if (Role == "User")
          {
               return false;
          } 
          return true;
     }
}