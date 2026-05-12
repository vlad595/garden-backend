using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Models;
using DTO;
using Data;

namespace Controllers // Заміни на свій неймспейс
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly Db _db; 

        public StatisticsController(Db context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<ActionResult<GardenStatisticsDto>> GetStatisticsAsync()
        {
            // Отримуємо ID поточного авторизованого користувача з токена
            // Якщо у тебе інший спосіб отримання UserId, зміни цей рядок
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Користувач не авторизований");
            }

            var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

            // 1. Статистика садку (тільки для поточного користувача)
            var userPlants = _db.Plants.Where(p => p.UserId == userId);

            var totalPlants = await userPlants.CountAsync();
            var sickPlants = await userPlants.CountAsync(p => p.Status == PlantStatus.Sick);
            var treatedPlants = await userPlants.CountAsync(p => p.Status == PlantStatus.Treated);

            // 2. Статистика плодів (врожаю)
            // Шукаємо врожаї тільки тих рослин, які належать користувачу
            var userHarvests = _db.Harvests.Where(h => h.Plant.UserId == userId);

            var harvestsLastMonth = await userHarvests
                .Where(h => h.HarvestDate >= oneMonthAgo)
                .ToListAsync();

            var harvestsLastMonthCount = harvestsLastMonth.Count;
            var totalHarvestWeightLastMonthKg = harvestsLastMonth.Sum(h => h.WeightKg);

            var frozenFruitsWeightKg = await userHarvests
                .Where(h => h.ProcessingMethod == ProcessingMethods.Freezing)
                .SumAsync(h => h.WeightKg);

            var fruitsForConservationWeightKg = await userHarvests
                .Where(h => h.ProcessingMethod == ProcessingMethods.Conservation) // Для джему/варення
                .SumAsync(h => h.WeightKg);

            var soldFruitsWeightLastMonthKg = harvestsLastMonth
                .Where(h => h.ProcessingMethod == ProcessingMethods.Sell)
                .Sum(h => h.WeightKg);

            // УВАГА: У моделі Harvest немає поля Price (Ціна). 
            // Якщо прибуток потрібно рахувати, доведеться додати ціну продажу у модель Harvest
            // або створити окрему таблицю Sales. Поки що залишаємо 0 або костиль.
            decimal totalMonthlyProfit = 0; 

            // Найбільш плодовита рослина за загальною вагою врожаю
            var mostFruitfulPlantId = await userHarvests
                .GroupBy(h => h.PlantId)
                .OrderByDescending(g => g.Sum(h => h.WeightKg))
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            string mostFruitfulPlantName = "Немає даних";
            if (mostFruitfulPlantId != 0)
            {
                mostFruitfulPlantName = await _db.Plants
                    .Where(p => p.Id == mostFruitfulPlantId)
                    .Select(p => p.Name)
                    .FirstOrDefaultAsync() ?? "Невідома рослина";
            }

            // 3. Статистика засобів догляду
            var userCareResources = _db.CareResources.Where(c => c.UserId == userId);

            var totalCareProducts = await userCareResources.CountAsync();
            
            // Завдяки спадкуванню в EF Core ми можемо використовувати OfType<T>()
            var fertilizerCount = await userCareResources.OfType<Fertilizer>().CountAsync();
            var pestControlCount = await userCareResources.OfType<PestControl>().CountAsync();

            // Витрати: сума цін всіх засобів
            // Якщо Price — це ціна за 1 одиницю, а Quantity — кількість, тоді Sum(c => c.Price * (decimal)c.Quantity)
            // Спочатку витягуємо лише колонку Price з бази даних у пам'ять
            var prices = await userCareResources.Select(c => c.Price).ToListAsync();

            // Потім рахуємо суму вже засобами самого C# (LINQ to Objects)
            var totalExpenses = prices.Sum();

            var dto = new GardenStatisticsDto
            {
                TotalPlants = totalPlants,
                SickPlants = sickPlants,
                TreatedPlants = treatedPlants,

                TotalHarvestWeightLastMonthKg = totalHarvestWeightLastMonthKg,
                HarvestsLastMonthCount = harvestsLastMonthCount,
                FrozenFruitsWeightKg = frozenFruitsWeightKg,
                SoldFruitsWeightLastMonthKg = soldFruitsWeightLastMonthKg,
                TotalMonthlyProfit = totalMonthlyProfit,
                FruitsForConservationWeightKg = fruitsForConservationWeightKg,
                MostFruitfulPlantName = mostFruitfulPlantName,

                TotalCareProducts = totalCareProducts,
                FertilizerCount = fertilizerCount,
                PestControlCount = pestControlCount,
                TotalExpenses = totalExpenses
            };

            return Ok(dto);
        }
    }
}