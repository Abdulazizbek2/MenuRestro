namespace MenuRestro.Models;

public class Restaurant
{
    public int RestaurantId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    // Свойства навигации
    public ICollection<Menu>? Menus { get; set; }
}

public class Menu
{
    public int MenuId { get; set; }
    public string? Name { get; set; }
    public int RestaurantId { get; set; }

    // Свойства навигации
    public Restaurant? Restaurant { get; set; }
    public ICollection<Food>? Foods { get; set; }
}

public class Food
{
    public int FoodId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int TypeFoodId { get; set; }
    public int RestaurantId { get; set; }
    public string? ImagePath { get; set; }

    // Свойства навигации
    public TypeFood? TypeFood { get; set; }
    public Restaurant? Restaurant { get; set; }
}

public class TypeFood
{
    public int TypeFoodId { get; set; }
    public string? Name { get; set; }

    // Свойства навигации
    public ICollection<Food>? Foods { get; set; }
}