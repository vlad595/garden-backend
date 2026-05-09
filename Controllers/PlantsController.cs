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
        private readonly IWebHostEnvironment _env;

        public PlantsController(Db db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
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
                    ImageUrl = p.ImageUrl
                }).ToListAsync();
                
            if (plants.IsNullOrEmpty())
            {
                return NotFound("Plants by this user does not found");
            }
            return plants;
        }

        [HttpPost("{id}/image")]
        public async Task<IActionResult> UploadPlantImage(int id, IFormFile file)
        {
            string userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User Id is not correct");
            }

            var plant = await _db.Plants.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
            
            if (plant == null)
            {
                return NotFound("Plant not found or access denied.");
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Invalid file type. Only JPG and PNG are allowed.");
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "plants");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (!string.IsNullOrEmpty(plant.ImageUrl))
            {
                var oldFilePath = Path.Combine(_env.WebRootPath, plant.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            var fileUrl = $"/images/plants/{fileName}";
            plant.ImageUrl = fileUrl;
            await _db.SaveChangesAsync();

            return Ok(new { Url = fileUrl });
        }
    }
}