using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuRestro.Models;
using System.Threading.Tasks;
using System.Linq;

namespace RestaurantMenuApp.Controllers
{
    public class TypeFoodsController : Controller
    {
        private readonly RestaurantContext _context;

        public TypeFoodsController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: TypeFoods
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypeFoods.ToListAsync());
        }

        // GET: TypeFoods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var typeFood = await _context.TypeFoods
                .FirstOrDefaultAsync(m => m.TypeFoodId == id);

            if (typeFood == null)
                return NotFound();

            return View(typeFood);
        }

        // GET: TypeFoods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeFoods/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TypeFood typeFood)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeFood);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeFood);
        }

        // GET: TypeFoods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var typeFood = await _context.TypeFoods.FindAsync(id);
            if (typeFood == null)
                return NotFound();

            return View(typeFood);
        }

        // POST: TypeFoods/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TypeFood typeFood)
        {
            if (id != typeFood.TypeFoodId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeFood);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeFoodExists(typeFood.TypeFoodId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(typeFood);
        }

        // GET: TypeFoods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var typeFood = await _context.TypeFoods
                .FirstOrDefaultAsync(m => m.TypeFoodId == id);
            if (typeFood == null)
                return NotFound();

            return View(typeFood);
        }

        // POST: TypeFoods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typeFood = await _context.TypeFoods.FindAsync(id);
            if (typeFood != null)
            {
                _context.TypeFoods.Remove(typeFood);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TypeFoodExists(int id)
        {
            return _context.TypeFoods.Any(e => e.TypeFoodId == id);
        }
    }
}