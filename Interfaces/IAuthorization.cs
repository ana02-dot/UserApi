using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserProfileAPI.Dtos;
using UserProfileAPI.Models;

namespace UserProfileAPI.Interfaces
{
    public interface IAuthorization
    {
        string GenerateToken(UserLoginDTO login);
        bool ValidateToken(string token, out ClaimsPrincipal principal);
    }
}