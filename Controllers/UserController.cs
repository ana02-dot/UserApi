using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserProfileAPI.Data;
using UserProfileAPI.Dtos;
using UserProfileAPI.Interfaces;


using UserProfileAPI.Models;
using UserProfileAPI.Repositories;

namespace UserProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly IUserRepository _userRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(DataContext dbContext, IUserRepository userRepo, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _userRepo = userRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("User/{id}")]
        public async Task<ActionResult<UserModel>> GetUser(int id)
        {
            var user = await _userRepo.GetUserWithDetailsAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        [HttpPost("Create/User")]
        public async Task<ActionResult<UserModel>> AddUser(CreateUserDTO userDto)
        {
            var result = await _userRepo.AddUserAsync(userDto);
            return Ok(result);
        }

        [HttpPut("Edit/{id}/User")]
        public async Task<ActionResult<UserModel>> UpdateUser(int id, UpdateUserDTO updateUserDto)
        {
            var result = await _userRepo.UpdateUserAsync(id, updateUserDto);
            if (result == null) return NotFound("User not found");

            return Ok(result);
        }

        [HttpDelete("Delete/{id}/User")]
        public async Task<ActionResult<UserModel>> DeleteUser(int id)
        {
            var result = await _userRepo.DeleteUserAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("upload/{id}/image")]
        public async Task<ActionResult> UploadUserImage(int id, IFormFile image)
        {
            var user = await _userRepo.GetUserWithDetailsAsync(id);
            if (user == null) return NotFound("User not found");
            if (image == null || image.Length == 0) return BadRequest("No image provided");

            // Ensure wwwroot path is set
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath ?? "wwwroot", "images", "users");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            // Delete old image if exists
            if (!string.IsNullOrEmpty(user.ImagePath))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath ?? "wwwroot", user.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
            }

            // Generate a unique file name
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save new image
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

         

            return Ok(new { imagePath = user.ImagePath });
        }
    }
}

