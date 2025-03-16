using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProfileAPI.Data;
using UserProfileAPI.Dtos;
using UserProfileAPI.Interfaces;
using UserProfileAPI.Models;

namespace UserProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _dbcontext;
        private readonly IAuthorization _authRepo;

        public AuthController(DataContext dbcontext, IAuthorization authRepo)
        {
            _dbcontext = dbcontext;
            _authRepo = authRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLog)
        {
            var user = await _dbcontext.Users
        .FirstOrDefaultAsync(u => u.Username == userLog.Username);
            if (user == null || userLog.Password != user.Password)
            {
                return Unauthorized(new { message = "მომხმარებლის სახელი ან პაროლი არასწორია" });
            }

            var token = _authRepo.GenerateToken(userLog);
            return Ok(new
            {
                token,
                user = new { id = user.Id, username = user.Username, firstName = user.FirstName, lastName = user.LastName }
            });
        }
    }
}
