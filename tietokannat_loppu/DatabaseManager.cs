using System.Text.Json;
using System.Text.RegularExpressions;

namespace BaseConsoleApp
{
    public class DatabaseManager : IDatabaseHandler
    {
        public List<Recipe>? LoadFromDatabase()
        {
            return new List<Recipe>();
        }
        public void SaveRecipesToDatabase(Recipe receivedRecipe)
        {
        
        }
        public void UpdateRecipeInDatabase(List<Recipe> newRecipes)
        {
          
        }
    }
}
