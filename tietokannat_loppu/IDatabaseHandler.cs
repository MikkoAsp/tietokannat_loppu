using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseConsoleApp
{
    public interface IDatabaseHandler
    {
        public List<Recipe>? LoadFromDatabase();
        public void SaveRecipesToDatabase(Recipe receivedRecipe);
        public void UpdateRecipeInDatabase(List<Recipe> newRecipes);
    }
}
