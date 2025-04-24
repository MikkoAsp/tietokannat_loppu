using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tietokannat_loppu.Entities;

namespace BaseConsoleApp
{
    public class RecipeHandlingManager : RecipeManager
    {
        public RecipeHandlingManager(IDatabaseHandler handler, IAskDetails helper) : base(helper, handler)
        {
        }
        public override void SearchRecipesByIngredients(LocalUser user)
        {
            List<string> searchedIngredients = detailsHelper.AskRecipeIngredients();

            var dbRecipes = dbHandler.LoadFromDatabaseByIngredients(user, searchedIngredients).Result;

            PrintRecipe(dbRecipes);
        }
        public override void SearchRecipesByDishes(LocalUser user)
        {
            Dish? dishOption = (Dish?)detailsHelper.SelectEnumOption<Dish>(0);
            if(dishOption != null)
            {;
                var dbRecipes = dbHandler.LoadFromDatabaseByDish(user, (Dish)dishOption).Result;

                PrintRecipe(dbRecipes);
            }

        }
        public override void SearchRecipesByDiets(LocalUser user)
        {
            Diet? dietOption = (Diet?)detailsHelper.SelectEnumOption<Diet>(0);

            if(dietOption != null)
            {
                var dbRecipes = dbHandler.LoadFromDatabaseByDiet(user, (Diet)dietOption).Result;

                PrintRecipe(dbRecipes);
            }
        }
        public override void UpdateRecipe()
        {
            /*Todo:
             * -Ask user some info about the recipe so we know what recipe to delete
             * -Then delete the recipe
             * -Then Create a new recipe
             * Then Save that recipe to the db
             */

        }
        public override async Task DeleteRecipeWithId(LocalUser localUser)
        {
            //Recipe id user wishes to delete => maybe change this to the recipe name...
            int deletedRecipeId = detailsHelper.AskIntNumber("Enter the recipe id you wish to delete: ");

            await dbHandler.DeleteFromDb(deletedRecipeId, localUser);
        }
    }
}
