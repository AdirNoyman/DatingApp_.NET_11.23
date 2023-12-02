using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")] // POST to api/account/register
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            // Check if username is taken
            // if (await UserExists(username)) return BadRequest("Username is taken");

            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken 🤨");

            // Create new user (using will make sure that when we finish with this class it will be disposed and not take up memory)
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), // Hash the password
                PasswordSalt = hmac.Key // Salt the password
            };

            // Add user to database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;

        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }


    }
}