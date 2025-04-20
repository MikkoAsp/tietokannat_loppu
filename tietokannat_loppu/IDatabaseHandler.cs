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
        public List<Localrecipe>? LoadFromDatabase();
        public void UpdateRecipeInDatabase(List<Localrecipe> newRecipes);

        public Task SaveRecipesToDatabaseAsync(Localrecipe localrecipe, TietokannatLoppuContext dbContext);
    }
}
