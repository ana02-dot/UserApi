using Microsoft.EntityFrameworkCore;
using UserProfileAPI.Data;
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
            // Get all users with their relations
            var users = await _dbContext.Users
                .Include(u => u.UserRelations)
                .ToListAsync();

            // Create dictionary in format: userId -> (connectionType -> count)
            var report = new Dictionary<int, Dictionary<string, int>>();

            foreach (var user in users)
            {
                // Group the user's relations by connection type and count them
                var connectionTypeCounts = user.UserRelations
                    .GroupBy(r => r.RelationType) // Using Type property
                    .ToDictionary(
                        group => group.Key,
                        group => group.Count()
                    );

                // Add to the report
                report[user.Id] = connectionTypeCounts;
            }

            return report;
        }

        public async Task<UserRelationModel> AddRealtedUser(UserRelationModel userRelation)
        {
            await _dbContext.UserRelations.AddAsync(userRelation);
            await _dbContext.SaveChangesAsync();

            // Return the entity with ID assigned
            return userRelation;
        }

        public async Task<UserRelationModel> UpdateRealtedUser(int id, UserRelationModel userRelation)
        {
            if (id != userRelation.Id)
                return null;

            // Check if relation exists before trying to update
            var exists = await _dbContext.UserRelations.AnyAsync(r => r.Id == id);
            if (!exists)
                return null;

            _dbContext.Entry(userRelation).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                return userRelation;
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
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
