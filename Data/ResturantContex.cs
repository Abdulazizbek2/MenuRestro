using MenuRestro.Models;
using Microsoft.EntityFrameworkCore;


public class RestaurantContext : DbContext
{
    public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options) { }

    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<TypeFood> TypeFoods { get; set; }

}




