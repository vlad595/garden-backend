using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantsController : ControllerBase
    {
        private readonly Db _db;

        public PlantsController(Db db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> GetAllPlants()
        {
            var plants = await _db.Plants.ToListAsync();
            return Ok(plants);
        }

        [HttpPost]
        public async Task<ActionResult<FruitTree>> AddFruitTree(FruitTree tree)
        {
            _db.FruitTrees.Add(tree);
            await _db.SaveChangesAsync();
            return Ok(tree);
        }
    }
}