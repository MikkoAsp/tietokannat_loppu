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

        public Task SaveRecipesToDatabaseAsync(Localrecipe localrecipe, LocalUser localUser);

        public Task<User> AddUserToDb(string email, string password, string username);

        public Task DeleteFromDb(int recipeId);
    }
}
