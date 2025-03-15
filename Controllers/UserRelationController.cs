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

        [HttpGet("RelationsReport")]
        public async Task<ActionResult<Dictionary<int, Dictionary<string, int>>>> GetUserRelationsReport()
        {
            var report = await _relationUserRepository.GetUserRelationsReportByConnectionType();
            return report == null || !report.Any()
                ? NotFound(new { Message = "დაკავშირებული პირების შესახებ ინფორმაცია არ მოიძებნა" })
                : Ok(report);
        }

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

        [HttpGet("{id}")]
        public async Task<ActionResult<UserRelationModel>> GetRelation(int id)
        {
            var relation = await _relationUserRepository.GetRelationAsync(id);
            return relation == null
                ? NotFound(new { Message = $"კავშირი ID {id}-ით არ მოიძებნა" })
                : Ok(relation);
        }

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

