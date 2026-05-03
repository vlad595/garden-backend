using System;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class BerryBushController : ControllerBase
    {
        private readonly Db _db; 

        public BerryBushController(Db db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BerryBush>>> GetAllBerryBushes()
        {
            string userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User Id is not correct");
            }

            var berryBushes = await _db.BerryBushes.Where(bush => bush.UserId == userId).ToListAsync();
            return Ok(berryBushes);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<BerryBush>> GetBerryBush(int Id)
        {
            BerryBush bush = _db.BerryBushes.Find(Id);
            if (bush != null) {
                return Ok(bush);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<BerryBush>> PostBerryBush(BerryBushCreation berryBush)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            BerryBush fullBush = new BerryBush(berryBush.Name, berryBush.Species, berryBush.PlantedAt, berryBush.TrellisNeeds, Convert.ToInt32(userId), berryBush.Status);
            _db.BerryBushes.Add(fullBush);
            await _db.SaveChangesAsync();
            return Ok(fullBush);
        }

        [HttpDelete]
        public async Task<ActionResult<BerryBush>> DeleteBerryBush(int Id)
        {
            string userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User Id is not correct");
            }

            BerryBush bush = _db.BerryBushes.Find(Id);

            if (userId != bush.UserId)
            {
                return BadRequest("Not acceptable");
            }
            
            _db.BerryBushes.Remove(bush);
            await _db.SaveChangesAsync();
            return Ok(bush);
        }
    }
}