using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserProfileAPI.Interfaces;


using UserProfileAPI.Models;
using UserProfileAPI.Repositories;

namespace UserProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DbContext _dbContext;
        private readonly IUserRepository _userRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(DbContext dbContext, UserRepository userRepo, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _userRepo = userRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUser(int id)
        {
            var user = await _userRepo.GetUserWithDetailsAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        
        [HttpPost("RegisterUser")]
        public async Task<ActionResult<UserModel>> AddUser(UserModel user)
        {
            var result = await _userRepo.AddUserAsync(user);

            return Ok(result);
          
        }

      
        [HttpPut("{id}")]
        public async Task<ActionResult<UserModel>> UpdateUser(int id, UserModel user)
        {
            
            if (id != user.Id)
            {
                return BadRequest();
            }

            var result = await _userRepo.UpdateUserAsync(id, user);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

       
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserModel>> DeleteUser(int id)
        {
            
            var result = await _userRepo.DeleteUserAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        
        [HttpPost("{id}/image")]
        public async Task<ActionResult> UploadUserImage(int id, IFormFile image)
        {
            // ფიზიკური პირის სურათის ატვირთვა/ცვლილება
            var user = await _userRepo.GetUserWithDetailsAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (image == null || image.Length == 0)
            {
                return BadRequest("No image provided");
            }

            // Create directory if not exists
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "users");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Delete old image if exists
            if (!string.IsNullOrEmpty(user.ImagePath) && System.IO.File.Exists(user.ImagePath))
            {
                System.IO.File.Delete(user.ImagePath);
            }

            // Save new image
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Update user image path
            user.ImagePath = "/images/users/" + uniqueFileName;
            await _userRepo.UpdateUserAsync(id, user);

            return Ok(new { imagePath = user.ImagePath });
        }
    }
}

