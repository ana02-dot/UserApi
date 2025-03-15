using Microsoft.EntityFrameworkCore;
using UserProfileAPI.Data;
using UserProfileAPI.Dtos;
using UserProfileAPI.Interfaces;
using UserProfileAPI.Models;

namespace UserProfileAPI.Repositories
{
    public class RelatedUserRepository : IRelationUserRepository
    {
        private readonly DataContext _dbContext;
        public RelatedUserRepository(DataContext context)
        {
            _dbContext = context;
        }
        public async Task<Dictionary<int, Dictionary<string, int>>> GetUserRelationsReportByConnectionType()
        {
            var users = await _dbContext.Users
                 .Include(u => u.UserRelations)
                 .ToListAsync();

            var report = new Dictionary<int, Dictionary<string, int>>();

            foreach (var user in users)
            {
                var connectionTypeCounts = user.UserRelations
                    .GroupBy(r => r.RelationType)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Count()
                    );

                report[user.Id] = connectionTypeCounts;
            }

            return report;
        }

        public async Task<UserRelationModel> AddRealtedUser(AddUserRelationDTO addUserRelationDTO)
        {
            var userRelation = new UserRelationModel
            {
                RelationType = addUserRelationDTO.RelationType,
                UserId = addUserRelationDTO.UserId,
                RelatedUserId = addUserRelationDTO.RelatedUserId
            };

            await _dbContext.UserRelations.AddAsync(userRelation);
            await _dbContext.SaveChangesAsync();
            return userRelation;
        }

        public async Task<UserRelationModel> GetRelationAsync(int id)
        {
            return await _dbContext.UserRelations
                .Include(r => r.User)
                .Include(r => r.RelatedUser)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<UserRelationModel> DeleteRealtedUser(int id)
        {
            var userRelation = await _dbContext.UserRelations.FindAsync(id);

            if (userRelation == null)
            {
                return null;
            }

            _dbContext.UserRelations.Remove(userRelation);
            await _dbContext.SaveChangesAsync();

            return userRelation;
        }
    }
}
