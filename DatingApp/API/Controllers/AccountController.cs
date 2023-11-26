using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<AppUser>> Register(string username, string password)
        {
            // // Check if username is taken
            // if (await UserExists(username)) return BadRequest("Username is taken");

            // Create new user (using will make sure that when we finish with this class it will be disposed and not take up memory)
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)), // Hash the password
                PasswordSalt = hmac.Key // Salt the password
            };

            // Add user to database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }


    }
}