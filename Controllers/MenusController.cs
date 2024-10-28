using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuRestro.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MenuRestro.Controllers
{
    public class MenusController : Controller
    {
        private readonly RestaurantContext _context;

        public MenusController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: Menus
        public async Task<IActionResult> Index()
        {
            var menus = _context.Menus.Include(m => m.Restaurant);
            return View(await menus.ToListAsync());
        }

        // GET: Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var menu = await _context.Menus
                .Include(m => m.Restaurant)
                .Include(m => m.Foods!) // Загрузка связанных блюд
                    .ThenInclude(f => f.TypeFood!) // Загрузка типа блюда
                .FirstOrDefaultAsync(m => m.MenuId == id);

            if (menu == null)
                return NotFound();

            return View(menu);
        }

        // GET: Menus/Create
        public IActionResult Create()
        {
            ViewBag.RestaurantId = new SelectList(_context.Restaurants, "RestaurantId", "Name");
            return View();
        }

        // POST: Menus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.RestaurantId = new SelectList(_context.Restaurants, "RestaurantId", "Name", menu.RestaurantId);
            return View(menu);
        }

        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
                return NotFound();

            ViewBag.RestaurantId = new SelectList(_context.Restaurants, "RestaurantId", "Name", menu.RestaurantId);
            return View(menu);
        }

        // POST: Menus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Menu menu)
        {
            if (id != menu.MenuId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.MenuId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.RestaurantId = new SelectList(_context.Restaurants, "RestaurantId", "Name", menu.RestaurantId);
            return View(menu);
        }

        // GET: Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var menu = await _context.Menus
                .Include(m => m.Restaurant)
                .FirstOrDefaultAsync(m => m.MenuId == id);

            if (menu == null)
                return NotFound();

            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.MenuId == id);
        }
    }
}