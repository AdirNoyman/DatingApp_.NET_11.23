using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseAPIController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }
        // Get all users
        [HttpGet] // api/users
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        // Get a single user
        [HttpGet("{id}")] // api/users/3
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }
    }
}