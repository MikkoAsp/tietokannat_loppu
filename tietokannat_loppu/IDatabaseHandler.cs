using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseConsoleApp
{
    public interface IDatabaseHandler
    {
        public List<Localrecipe>? LoadFromDatabase();
        public void SaveRecipesToDatabase(Localrecipe receivedRecipe);
        public void UpdateRecipeInDatabase(List<Localrecipe> newRecipes);
    }
}
