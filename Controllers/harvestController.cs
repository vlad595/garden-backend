using System;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using DTO;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class HarvestsController : ControllerBase
    {
        private readonly Db _db;

        public HarvestsController(Db db)
        {
            _db = db;
        }

        [HttpGet("{plantId}")]
        public async Task<ActionResult<IEnumerable<HarvestResponse>>> GetAllHarvestsByPlantId([FromRoute]int plantId)
        {
            var harvestsList = await _db.Harvests.Where(harvest => harvest.PlantId == plantId).ToListAsync();
            if (harvestsList == null) return NotFound("Harvests does does not found");
            return Ok(harvestsList);
        }
    
        [HttpPost]
        public async Task<ActionResult<HarvestResponse>> AddHarvestToPlant(HarvestCreate harvest)
        {
            string userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User Id is not correct");
            }

            Plant plant = _db.Plants.FirstOrDefault(p => p.Id == harvest.PlantId);

            if (userId != plant.UserId)
            {
                return BadRequest("Not yours");
            }

            Harvest realHarvest = new (harvest.PlantId, harvest.WeightKg, harvest.HarvestDate, harvest.ProcessingMethod);
            _db.Harvests.Add(realHarvest);
            await _db.SaveChangesAsync();
            return Ok(harvest);
        }

        [HttpPatch("{harvestId}")]
        public async Task<ActionResult<HarvestResponse>> ChangeStatus(int harvestId, ProcessingMethods processingMethod)
        {
            string userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User Id is not correct");
            }

            Harvest harvest = _db.Harvests.FirstOrDefault(h => h.Id == harvestId);
            if (harvest == null)
            {
                return NotFound("Harvest not found"); 
            }

            Plant plant = _db.Plants.FirstOrDefault(p => p.Id == harvest.PlantId);
            if (plant == null)
            {
                return NotFound("Plant not found"); 
            }

            if (userId != plant.UserId)
            {
                return Forbid(); 

            }

            harvest.ProcessingMethod = processingMethod;
            await _db.SaveChangesAsync();
            return Ok(new HarvestResponse(harvest.Id, harvest.HarvestDate, harvest.ProcessingMethod, harvest.WeightKg));
        }
    }
}