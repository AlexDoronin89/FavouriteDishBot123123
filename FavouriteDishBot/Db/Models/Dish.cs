using System;
using System.Collections.Generic;

namespace FavouriteDishBot.Db.Models;

public partial class Dish
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string Ingredients { get; set; } = null!;

    public string Recipe { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public int Rating { get; set; }

    public virtual ICollection<DishesList> DishesLists { get; set; } = new List<DishesList>();
}
