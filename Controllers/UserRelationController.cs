using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProfileAPI.Data;
using UserProfileAPI.Dtos;
using UserProfileAPI.Filters;
using UserProfileAPI.Interfaces;
using UserProfileAPI.Models;

namespace UserProfileAPI.Controllers
{
    [Route("api/[controller]")]
    
    public class UserRelationController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly IRelationUserRepository _relationUserRepository;

        public UserRelationController(IRelationUserRepository relationRepo, DataContext dbContext)
        {
            _relationUserRepository = relationRepo;
            _dbContext = dbContext;
        }
        /// <summary>
        /// Retrieves a report of user relationships grouped by connection type
        /// </summary>
        /// <returns>A dictionary mapping user IDs to their relationship counts by type</returns>
        [HttpGet("RelationsReport")]
        public async Task<ActionResult<Dictionary<int, Dictionary<string, int>>>> GetUserRelationsReport()
        {
            var report = await _relationUserRepository.GetUserRelationsReportByConnectionType();
            return report == null || !report.Any()
                ? NotFound(new { Message = "დაკავშირებული პირების შესახებ ინფორმაცია არ მოიძებნა" })
                : Ok(report);
        }
        /// <summary>
        /// Creates a new relationship between two users
        /// </summary>
        /// <param name="addUserRelationDto">The data required to establish a user relationship. See <see cref="AddUserRelationDTO"/> for required fields</param>
        /// <returns>The created relationship details</returns>
        [HttpPost]
        public async Task<ActionResult<UserRelationModel>> AddRelatedUser(AddUserRelationDTO addUserRelationDto)
        {
            var user = await _dbContext.Users.FindAsync(addUserRelationDto.UserId);
            var relatedUser = await _dbContext.Users.FindAsync(addUserRelationDto.RelatedUserId);

            if (user == null || relatedUser == null)
                return BadRequest(new { Message = "მომხმარებელი ან მასთან დაკავშირებული მომხმარებელი არ არსებობს" });

            var result = await _relationUserRepository.AddRealtedUser(addUserRelationDto);
            return CreatedAtAction(nameof(GetRelation), new { id = result.Id }, result);

        }
        /// <summary>
        /// Retrieves details of a specific user relationship by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the relationship to retrieve</param>
        /// <returns>The relationship details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserRelationModel>> GetRelation(int id)
        {
            var relation = await _relationUserRepository.GetRelationAsync(id);
            return relation == null
                ? NotFound(new { Message = $"კავშირი ID {id}-ით არ მოიძებნა" })
                : Ok(relation);
        }
        /// <summary>
        /// Deletes a user relationship by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the relationship to delete</param>
        /// <returns>The deleted relationship details</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserRelationModel>> DeleteRelatedUser(int id)
        {
            var result = await _relationUserRepository.DeleteRealtedUser(id);

            return result == null
                ? NotFound(new { Message = $"ურთიერთობა ID {id}-ით არ მოიძებნა" })
                : Ok(result);
        }

    }
}

