using System;
using System.Collections.Generic;

namespace FavouriteDishBot.Db.Models;

public partial class DishesList
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public long ChatId { get; set; }

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
