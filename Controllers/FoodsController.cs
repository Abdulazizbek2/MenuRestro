using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MenuRestro.Models;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RestaurantMenuApp.Controllers
{
    public class FoodsController : Controller
    {
        private readonly RestaurantContext _context;

        public FoodsController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: Foods
        public async Task<IActionResult> Index()
        {
            var foods = _context.Foods.Include(f => f.TypeFood).Include(f => f.Restaurant);
            return View(await foods.ToListAsync());
        }

        // GET: Foods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var food = await _context.Foods
                .Include(f => f.TypeFood)
                .Include(f => f.Restaurant)
                .FirstOrDefaultAsync(m => m.FoodId == id);

            if (food == null)
                return NotFound();

            return View(food);
        }

        // GET: Foods/Create
        public IActionResult Create()
        {
            ViewBag.RestaurantId = new SelectList(_context.Restaurants, "RestaurantId", "Name");
            ViewBag.TypeFoodId = new SelectList(_context.TypeFoods, "TypeFoodId", "Name");
            return View();
        }

        // POST: Foods/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Food food, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                // Путь для сохранения изображения в wwwroot/images
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);

                // Создание директории, если она не существует
                if (!Directory.Exists(Path.GetDirectoryName(imagePath)))
                {
                    var directory = Path.GetDirectoryName(imagePath);
                    if (directory != null)
                    {
                        Directory.CreateDirectory(directory);
                    }
                }

                // Сохранение файла
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Сохранение пути изображения в модели
                food.ImagePath = "/images/" + image.FileName;
            }

            if (ModelState.IsValid || image == null)
            {
                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine("Загружается файл: " + (image != null ? image.FileName : "Нет файла"));
            Console.WriteLine("Сохраненный путь: " + food.ImagePath);

            // Повторная передача ViewBag для выпадающих списков
            ViewBag.RestaurantId = new SelectList(_context.Restaurants, "RestaurantId", "Name", food.RestaurantId);
            ViewBag.TypeFoodId = new SelectList(_context.TypeFoods, "TypeFoodId", "Name", food.TypeFoodId);
            return View(food);
        }

        // GET: Foods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var food = await _context.Foods.FindAsync(id);
            if (food == null)
                return NotFound();

            ViewBag.RestaurantId = new SelectList(_context.Restaurants, "RestaurantId", "Name", food.RestaurantId);
            ViewBag.TypeFoodId = new SelectList(_context.TypeFoods, "TypeFoodId", "Name", food.TypeFoodId);
            return View(food);
        }

        // POST: Foods/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Food food, IFormFile image)
        {
            if (id != food.FoodId)
                return NotFound();

            if (image != null && image.Length > 0)
            {
                // Путь для сохранения изображения в wwwroot/images
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);

                // Создание директории, если она не существует
                if (!Directory.Exists(Path.GetDirectoryName(imagePath)))
                {
                    var directory = Path.GetDirectoryName(imagePath);
                    if (directory != null)
                    {
                        Directory.CreateDirectory(directory);
                    }
                }

                // Сохранение файла
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Обновление пути изображения в модели
                food.ImagePath = "/images/" + image.FileName;
            }

            if (ModelState.IsValid || image == null)
            {
                try
                {
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.FoodId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.RestaurantId = new SelectList(_context.Restaurants, "RestaurantId", "Name", food.RestaurantId);
            ViewBag.TypeFoodId = new SelectList(_context.TypeFoods, "TypeFoodId", "Name", food.TypeFoodId);
            return View(food);
        }

        // GET: Foods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Console.WriteLine("Искаемый ид: " + id);
            if (id == null)
                return NotFound();

            var food = await _context.Foods
                .Include(f => f.TypeFood)
                .Include(f => f.Restaurant)
                .FirstOrDefaultAsync(m => m.FoodId == id);
            Console.WriteLine("Искаемый блюдо: " + food);
            if (food == null)
                return NotFound();

            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var food = await _context.Foods.FindAsync(id);


            if (food != null)
            {
                _context.Foods.Remove(food);
                await _context.SaveChangesAsync();
            }

            // Удаление файла изображения, если он существует
            if (!string.IsNullOrEmpty(food?.ImagePath))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", food.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }


            }

            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.FoodId == id);
        }
    }
}