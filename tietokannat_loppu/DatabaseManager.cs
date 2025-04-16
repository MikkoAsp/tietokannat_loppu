using System.Text.Json;
using System.Text.RegularExpressions;

namespace BaseConsoleApp
{
    public class DatabaseManager : IDatabaseHandler
    {
        public List<Localrecipe>? LoadFromDatabase()
        {
            return new List<Localrecipe>();
        }
        public void SaveRecipesToDatabase(Localrecipe receivedRecipe)
        {
        
        }
        public void UpdateRecipeInDatabase(List<Localrecipe> newRecipes)
        {
          
        }
    }
}
