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
                .Include(u => u.UserRelations)
                .ThenInclude(ur => ur.RelatedUser)
                .Include(u => u.City)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserModel> AddUserAsync(CreateUserDTO userDto)
        {
            var city = await _dbContext.Cities
        .FirstOrDefaultAsync(c => c.Name == userDto.CityName);

            if (city == null)
            {
                city = new CityModel { Name = userDto.CityName };
                _dbContext.Cities.Add(city);
                await _dbContext.SaveChangesAsync();
            }

            var user = new UserModel
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                GenderType = userDto.GenderType,
                PersonalNumber = userDto.PersonalNumber,
                DateOfBirth = userDto.DateOfBirth,
                Number = userDto.Number,
                PhoneNumberType = userDto.PhoneNumberType,
                CityId = city.Id
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<UserModel> UpdateUserAsync(int id, UpdateUserDTO updateUserDto)
        {
            var existingUser = await _dbContext.Users
        .Include(u => u.City)
        .FirstOrDefaultAsync(u => u.Id == id);

            if (existingUser == null) return null;

        
            if (!string.IsNullOrEmpty(updateUserDto.FirstName)) existingUser.FirstName = updateUserDto.FirstName;
            if (!string.IsNullOrEmpty(updateUserDto.LastName)) existingUser.LastName = updateUserDto.LastName;
            if (!string.IsNullOrEmpty(updateUserDto.GenderType)) existingUser.GenderType = updateUserDto.GenderType;
            if (updateUserDto.DateOfBirth.HasValue) existingUser.DateOfBirth = updateUserDto.DateOfBirth.Value;
            if (!string.IsNullOrEmpty(updateUserDto.Number)) existingUser.Number = updateUserDto.Number;
            if (!string.IsNullOrEmpty(updateUserDto.PhoneNumberType)) existingUser.PhoneNumberType = updateUserDto.PhoneNumberType;

            
            if (!string.IsNullOrEmpty(updateUserDto.CityName) && updateUserDto.CityName != existingUser.City.Name)
            {
                var city = await _dbContext.Cities.FirstOrDefaultAsync(c => c.Name == updateUserDto.CityName);
                if (city == null)
                {
                    city = new CityModel { Name = updateUserDto.CityName };
                    _dbContext.Cities.Add(city);
                    await _dbContext.SaveChangesAsync();
                }
                existingUser.CityId = city.Id;
            }

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
