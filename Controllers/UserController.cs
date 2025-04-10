using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserProfileAPI.Data;
using UserProfileAPI.Dtos;
using UserProfileAPI.Filters;
using UserProfileAPI.Interfaces;


using UserProfileAPI.Models;
using UserProfileAPI.Repositories;

namespace UserProfileAPI.Controllers
{
    [Route("api/[controller]")]

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

        /// <summary>
        /// Retrieves detailed information about a specific user by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve</param>
        /// <returns>Returns the user's detailed information</returns>
        [HttpGet("User/{id}")]
        public async Task<ActionResult<UserModel>> GetUser(int id)
        {
            var user = await _userRepo.GetUserWithDetailsAsync(id);
            return user == null
                ? NotFound(new { Message = "მომხმარებელი არ მოიძებნა" })
                : Ok(user);
        }
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userDto">The data required to create a new user. See <see cref="CreateUserDTO"/> for required fields.</param>
        /// <returns>The created user's details</returns>
        [HttpPost("Create/User")]
        public async Task<ActionResult<UserModel>> AddUser(CreateUserDTO userDto)
        {
            var createdUser = await _userRepo.AddUserAsync(userDto);
            return Ok(new { Message = "მომხმარებელი წარმატებით დაემატა", createdUser });
        }

        /// <summary>
        /// Updates an existing user's information
        /// </summary>
        /// <param name="id">The unique identifier of the user to update</param>
        /// <param name="updateUserDto">The updated user data. See <see cref="UpdateUserDTO"/>for editable fields </param>
        /// <returns>The updated user's details</returns>
        [HttpPut("Edit/{id}/User")]
        public async Task<ActionResult<UserModel>> UpdateUser(int id, UpdateUserDTO updateUserDto)
        {
            var updatedUser = await _userRepo.UpdateUserAsync(id, updateUserDto);
            return updatedUser == null
                ? NotFound(new { Message = "მომხმარებელი არ მოიძებნა" })
                : Ok(updatedUser);
        }
        /// <summary>
        /// Deletes a user by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete</param>
        /// <returns>The deleted user's details</returns>
        [HttpDelete("Delete/{id}/User")]
        public async Task<ActionResult<UserModel>> DeleteUser(int id)
        {
            var deletedUser = await _userRepo.DeleteUserAsync(id);
            return deletedUser == null
                ? NotFound(new { Message = "მომხმარებელი არ მოიძებნა" })
                : Ok(new { Message = $"მომხმარებელი წარმატებით წაიშალა" });
        }

        /// <summary>
        /// Uploads or replaces a user's image.
        /// </summary>
        /// <param name="id">The unique identifier of the user whose image is being uploaded.</param>
        /// <param name="image">The image file to upload (e.g., JPG, PNG). Must not exceed 10MB.</param>
        /// <returns>A success message</returns>
        [HttpPost("upload/{id}/image")]
        public async Task<ActionResult> UploadUserImage(int id, IFormFile image)
        {
            var user = await _userRepo.GetUserWithDetailsAsync(id);
            if (user == null) return NotFound("User not found");
            if (image == null || image.Length == 0) return BadRequest("No image provided");


            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath ?? "wwwroot", "images", "users");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);


            if (!string.IsNullOrEmpty(user.ImagePath))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath ?? "wwwroot", user.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            user.ImagePath = $"/images/users/{uniqueFileName}";
            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = $"ფოტო წარმატებით დაემატა" });
        }
        /// <summary>
        /// Retrieves user's list by their searchTerm.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns>relevant user's list</returns>
        [HttpGet("Search")]
        public async Task<ActionResult<List<UserModel>>> QuickSearch(string searchTerm)
        { 
            var results = await _userRepo.GetUsersAsyncWithQuickSearch(searchTerm);
            return results.IsNullOrEmpty()
              ? NotFound(new {Message = "იუზერის მონაცემები ვერ მოიძებნა"}) : Ok(results);
        }

    }
}

