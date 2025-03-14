using Microsoft.EntityFrameworkCore;
using UserProfileAPI.Data;
using UserProfileAPI.Dtos;
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
        .Include(u => u.UserRelations) // დაკავშირებული ფიზიკური პირები
            .ThenInclude(ur => ur.RelatedUser)
        .Include(u => u.City) // ქალაქის ინფორმაცია
        .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserModel> AddUserAsync(CreateUserDTO userDto)
        {
            var user = new UserModel
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                GenderType = userDto.GenderType,
                PersonalNumber = userDto.PersonalNumber,
                DateOfBirth = userDto.DateOfBirth,
                PhoneNumberType = userDto.PhoneNumberType,
                Number = userDto.Number,
                CityId = userDto.CityId
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<UserModel> UpdateUserAsync(int id, UpdateUserDTO updateUserDto)
        {
            var existingUser = await _dbContext.Users.FindAsync(id);
            if (existingUser == null) return null;

            if (!string.IsNullOrEmpty(updateUserDto.FirstName)) existingUser.FirstName = updateUserDto.FirstName;
            if (!string.IsNullOrEmpty(updateUserDto.LastName)) existingUser.LastName = updateUserDto.LastName;
            if (!string.IsNullOrEmpty(updateUserDto.GenderType)) existingUser.GenderType = updateUserDto.GenderType;
            if (!string.IsNullOrEmpty(updateUserDto.PersonalNumber)) existingUser.PersonalNumber = updateUserDto.PersonalNumber;
            if (updateUserDto.DateOfBirth.HasValue) existingUser.DateOfBirth = updateUserDto.DateOfBirth.Value;
            if (!string.IsNullOrEmpty(updateUserDto.Number)) existingUser.Number = updateUserDto.Number;


            await _dbContext.SaveChangesAsync();
            return existingUser;
        }

        public async Task<UserModel> DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return null;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}
