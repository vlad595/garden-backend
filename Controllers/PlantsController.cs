using System;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PlantsController : ControllerBase
    {
        private readonly Db _db;

        public PlantsController(Db db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlantResponse>>> GetAllPlants()
        {
            string userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User Id is not correct");
            }

            var plants = await _db.Plants
                .Where(u => u.UserId == userId)
                .Select(p => new PlantResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Species = p.Species,
                    PlantedAt = p.PlantedAt,
                    Type = p is FruitTree ? "Tree" : (p is BerryBush ? "Bush" : "Unknown"),
                    Status = p.Status,
                }).ToListAsync();
                
            if (plants.IsNullOrEmpty())
            {
                return NotFound("Plants by this user does not found");
            }
            return plants;
        }
    }
}