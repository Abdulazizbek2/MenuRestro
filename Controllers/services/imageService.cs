using MenuRestro.Models;
using Microsoft.AspNetCore.Mvc;


public class ImageService : Controller
{

    [HttpPost]
    public async Task<IActionResult> Create(Food food, IFormFile image, RestaurantContext context)
    {
        if (image != null)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            food.ImagePath = "/images/" + image.FileName;
        }

        if (ModelState.IsValid)
        {
            context.Add(food);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(food);
    }
}