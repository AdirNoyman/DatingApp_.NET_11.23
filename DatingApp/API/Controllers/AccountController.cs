using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")] // POST to api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
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

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreatedToken(user)
            };

        }

        [HttpPost("login")] // POST to api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // Find user in database and make sure on the way that is only one user with that username
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            // If user does not exist, return Unauthorized
            if (user == null) return Unauthorized("Invalid username 🤨");

            // Create new instance of HMACSHA512 (with users password and key (salt))
            using var hmac = new HMACSHA512(user.PasswordSalt);

            // Hash the password sent through the login
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            // Compare computed hash with password hash in database
            for (int i = 0; i < computedHash.Length; i++)
            {
                // If hashes do not match
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password 🤨");
            }

            // If hashes match
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreatedToken(user)
            };

        }


        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }


    }
}