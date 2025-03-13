using Microsoft.EntityFrameworkCore;
using UserProfileAPI.Data;
using UserProfileAPI.Interfaces;
using UserProfileAPI.Models;

namespace UserProfileAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dbContext;
        public UserRepository(DataContext context)
        {
            _dbContext = context;
        }

        public async Task<UserModel> GetUserWithDetailsAsync(int id)
        {
            return await _dbContext.Users
                .Include(u => u.PhoneNumbers)
                .Include(u => u.UserRelations)
                    .ThenInclude(ur => ur.RelatedUser)
                .Include(u => u.City)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserModel> AddUserAsync(UserModel user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<UserModel> UpdateUserAsync(int id, UserModel user)
        {
            var existingUser = await _dbContext.Users.FindAsync(id);

            if (existingUser == null)
            {
                return null;
            }
           
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.GenderType = user.GenderType;
            existingUser.PersonalNumber = user.PersonalNumber;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.City = user.City;
            existingUser.PhoneNumbers = user.PhoneNumbers;

            
            if (!string.IsNullOrEmpty(user.ImagePath))
            {
                existingUser.ImagePath = user.ImagePath;
            }

            await _dbContext.SaveChangesAsync();

            return existingUser;
        }

        public async Task<UserModel> DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}
