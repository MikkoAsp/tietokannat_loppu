using System;
using System.Collections.Generic;

namespace tietokannat_loppu.Entities;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public int? InstructionsId { get; set; }

    public int? UserId { get; set; }

    public string RecipeName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Instruction> Instructions { get; set; } = new List<Instruction>();

    public virtual Instruction? InstructionsNavigation { get; set; }

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    public virtual User? User { get; set; }
}
