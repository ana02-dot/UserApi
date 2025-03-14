using UserProfileAPI.Dtos;
using UserProfileAPI.Models;

namespace UserProfileAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<UserModel> GetUserWithDetailsAsync(int id);
        Task<UserModel> AddUserAsync(CreateUserDTO userDto);
        Task<UserModel> UpdateUserAsync(int id, UpdateUserDTO updateUserDto);
        Task<UserModel> DeleteUserAsync(int id);

    }
}