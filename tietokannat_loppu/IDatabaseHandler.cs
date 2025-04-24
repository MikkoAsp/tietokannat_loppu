using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tietokannat_loppu.Entities;

namespace BaseConsoleApp
{
    public interface IDatabaseHandler
    {
        public Task<List<Recipe>?> LoadAllRecipesFromDb(LocalUser user);
        public Task<List<Recipe>?> LoadFromDatabaseByDish(LocalUser user, Dish dish);
        public Task<List<Recipe>?> LoadFromDatabaseByDiet(LocalUser user, Diet diet);
        public Task<List<Recipe>?> LoadFromDatabaseByIngredients(LocalUser user, List<string> ingredients);

        public Task SaveRecipesToDatabaseAsync(Localrecipe localrecipe, LocalUser localUser);

        public Task<User> AddUserToDb(string email, string password, string username);
        public Task UpdateRecipeInDb(int recipeId, LocalUser localUser);
        public Task DeleteFromDb(int recipeId, LocalUser localUser);
    }
}
