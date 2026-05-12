using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
namespace Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CareResourcesController : ControllerBase
    {
        private readonly Db _db;

        public CareResourcesController(Db db)
        {
            _db = db;
        }

        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                            ?? User.FindFirst("sub")?.Value;
            
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("Cannot parse User ID from token.");
        }

        private CareResourceResponseDto MapToDto(CareResource resource)
        {
            var dto = new CareResourceResponseDto
            {
                Id = resource.Id,
                Name = resource.Name,
                Quantity = resource.Quantity,
                Price = resource.Price,
                CreatedAt = resource.CreatedAt,
                UserId = resource.UserId
            };

            if (resource is Fertilizer fertilizer)
            {
                dto.ResourceType = "Fertilizer";
                dto.IsOrganic = fertilizer.IsOrganic;
                dto.Nutrients = fertilizer.Nutrients;
            }
            else if (resource is PestControl pestControl)
            {
                dto.ResourceType = "PestControl";
                dto.TargetPest = pestControl.TargetPest;
                dto.WaitingDays = pestControl.WaitingDays;
            }

            return dto;
        }

        [HttpPost("pest-control")]
        public async Task<ActionResult<CareResourceResponseDto>> AddPestControl([FromBody] CreatePestControlDto dto)
        {
            var pestControl = new PestControl
            {
                Name = dto.Name,
                Quantity = dto.Quantity,
                Price = dto.Price,
                TargetPest = dto.TargetPest,
                WaitingDays = dto.WaitingDays,
                CreatedAt = DateTime.UtcNow,
                UserId = GetCurrentUserId()
            };
            
            _db.PestControls.Add(pestControl);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCareResourceById), new { id = pestControl.Id }, MapToDto(pestControl));
        }

        [HttpPost("fertilizer")]
        public async Task<ActionResult<CareResourceResponseDto>> AddFertilizer([FromBody] CreateFertilizerDto dto)
        {
            var fertilizer = new Fertilizer
            {
                Name = dto.Name,
                Quantity = dto.Quantity,
                Price = dto.Price,
                IsOrganic = dto.IsOrganic,
                Nutrients = dto.Nutrients,
                CreatedAt = DateTime.UtcNow,
                UserId = GetCurrentUserId() 
            };

            _db.Fertilizers.Add(fertilizer);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCareResourceById), new { id = fertilizer.Id }, MapToDto(fertilizer));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CareResourceResponseDto>>> GetAllCareResources()
        {
            int currentUserId = GetCurrentUserId();

            var resources = await _db.CareResources
                .Where(r => r.UserId == currentUserId)
                .ToListAsync();
                
            return Ok(resources.Select(MapToDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CareResourceResponseDto>> GetCareResourceById(int id)
        {
            var resource = await _db.CareResources.FindAsync(id);

            if (resource == null)
            {
                return NotFound();
            }

            if (resource.UserId != GetCurrentUserId())
            {
                return Forbid();
            }

            return Ok(MapToDto(resource));
        }

        [HttpPatch("{id}/quantity")]
        public async Task<IActionResult> UpdateQuantity(int id, [FromBody] UpdateQuantityDto dto)
        {
            var resource = await _db.CareResources.FindAsync(id);

            if (resource == null)
            {
                return NotFound($"Resource with ID {id} not found.");
            }

            if (resource.UserId != GetCurrentUserId())
            {
                return Forbid(); 
            }

            if (dto.Quantity < 0)
            {
                return BadRequest("Quantity cannot be negative.");
            }

            resource.Quantity = dto.Quantity;
            await _db.SaveChangesAsync();

            return Ok(MapToDto(resource)); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCareResource(int id)
        {
            var resource = await _db.CareResources.FindAsync(id);

            if (resource == null)
            {
                return NotFound($"Resource with ID {id} not found.");
            }

            if (resource.UserId != GetCurrentUserId())
            {
                return Forbid();
            }

            _db.CareResources.Remove(resource);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}