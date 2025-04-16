using System;
using System.Collections.Generic;

namespace tietokannat_loppu.Entities;

public partial class RecipeIngredient
{
    public int RecipeId { get; set; }

    public int IngredientId { get; set; }

    public decimal Quantity { get; set; }

    public string? UnitType { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
