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
    public class FruitTreeController : ControllerBase
    {
        private readonly Db _db;

        public FruitTreeController(Db db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FruitTree>>> GetAllPlants()
        {
            string userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User Id is not correct");
            }

            var fruitTrees = await _db.FruitTrees.Where(t => t.UserId == userId).ToListAsync();
            return Ok(fruitTrees);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<FruitTree>> GetTreeById(int Id)
        {
            FruitTree tree = _db.FruitTrees.Find(Id);

            return Ok(tree);
        }

        [HttpPost]
        public async Task<ActionResult<FruitTree>> AddFruitTree(FruitTreeCreation tree)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            FruitTree fullTree = new FruitTree(tree.Name, tree.Species, tree.PlantedAt, tree.Height, Convert.ToInt32(userId));
            _db.FruitTrees.Add(fullTree);
            await _db.SaveChangesAsync();
            return Ok(fullTree);
        }

        [HttpDelete]
        public async Task<ActionResult<FruitTree>> DeleteFruitTree(int Id)
        {
            FruitTree tree = _db.FruitTrees.Find(Id);
            _db.FruitTrees.Remove(tree);
            await _db.SaveChangesAsync();
            return Ok(tree);
        }
    }
}