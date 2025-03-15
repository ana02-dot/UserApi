using UserProfileAPI.Dtos;
using UserProfileAPI.Models;

namespace UserProfileAPI.Interfaces
{
    public interface IRelationUserRepository
    {
        Task<UserRelationModel> AddRealtedUser(AddUserRelationDTO addUserRelationDto);
        Task<UserRelationModel> DeleteRealtedUser(int id);
        Task<UserRelationModel> GetRelationAsync(int id);
        Task<Dictionary<int, Dictionary<string, int>>> GetUserRelationsReportByConnectionType();

    }
}
