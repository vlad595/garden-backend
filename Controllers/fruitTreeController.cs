using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using DTO;

namespace Controllers
{
    [ApiController]
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
            var fruitTrees = await _db.FruitTrees.ToListAsync();
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
            FruitTree fullTree = new FruitTree(tree.Name, tree.Species, tree.PlantedAt, tree.Height);
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