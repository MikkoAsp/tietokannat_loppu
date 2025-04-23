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
        public Task<List<Recipe>?> LoadFromDatabase(LocalUser user);
        public void UpdateRecipeInDatabase(List<Localrecipe> newRecipes);

        public Task SaveRecipesToDatabaseAsync(Localrecipe localrecipe, LocalUser localUser);

        public Task AddUserToDb(LocalUser newUser);
        public  Task DeleteFromDatabase();
    }
}
