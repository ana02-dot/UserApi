using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProfileAPI.Interfaces;
using UserProfileAPI.Models;

namespace UserProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRelationController : ControllerBase
    {
        private readonly DbContext _dbContext;
        private readonly IRelationUserRepository _relationUserRepository;

        public UserRelationController(IRelationUserRepository relationRepo, DbContext dbContext)
        {
            _relationUserRepository = relationRepo;
            _dbContext = dbContext;
        }

        [HttpGet("RelationsReport")]
        public async Task<ActionResult<Dictionary<int, Dictionary<string, int>>>> GetUserRelationsReport()
        {
            var report = await _relationUserRepository.GetUserRelationsReportByConnectionType();
            return report;
        }

        [HttpPost]
        public async Task<ActionResult<UserRelationModel>> AddRelatedUser(UserRelationModel userRelation)
        {
            var result = await _relationUserRepository.AddRealtedUser(userRelation);
            return Ok(result);

        }

       
        [HttpPut("{id}")]
        public async Task<ActionResult<UserRelationModel>> UpdateRelatedUser(int id, UserRelationModel userRelation)
        {
            if (id != userRelation.Id)
            {
                return BadRequest();
            }

            var result = await _relationUserRepository.UpdateRealtedUser(id, userRelation);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

       
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserRelationModel>> DeleteRelatedUser(int id)
        {
            var result = await _relationUserRepository.DeleteRealtedUser(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

    }
}

