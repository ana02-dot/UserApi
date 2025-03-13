using UserProfileAPI.Models;

namespace UserProfileAPI.Interfaces
{
    public interface IRelationUserRepository
    {
        Task<UserRelationModel> AddRealtedUser(UserRelationModel userRelation);
        Task<UserRelationModel> UpdateRealtedUser(int id, UserRelationModel userRelation);
        Task<UserRelationModel> DeleteRealtedUser(int id);
        Task<Dictionary<int, Dictionary<string, int>>> GetUserRelationsReportByConnectionType();

    }
}
