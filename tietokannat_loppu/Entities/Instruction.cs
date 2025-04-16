using System;
using System.Collections.Generic;

namespace tietokannat_loppu.Entities;

public partial class Instruction
{
    public int InstructionsId { get; set; }

    public string CookingInstructions { get; set; } = null!;

    public int Step { get; set; }

    public int? RecipeId { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
