using UserProfileAPI.Models;

namespace UserProfileAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<UserModel> GetUserWithDetailsAsync(int id);
        Task<UserModel> AddUserAsync(UserModel user);
        Task<UserModel> UpdateUserAsync(int id, UserModel user);
        Task<UserModel> DeleteUserAsync(int id);

    }
}
