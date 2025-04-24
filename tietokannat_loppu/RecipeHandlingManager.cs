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
        public override async Task UpdateRecipeInDb(LocalUser localUser)
        {

            int idRecipeToUpdate = detailsHelper.AskIntNumber("Enter the recipe id you wish to update: ");
            await dbHandler.UpdateRecipeInDb(idRecipeToUpdate, localUser);


        }
        public override async Task DeleteRecipeWithId(LocalUser localUser)
        {
            //Recipe id user wishes to delete => maybe change this to the recipe name...
            int deletedRecipeId = detailsHelper.AskIntNumber("Enter the recipe id you wish to delete: ");

            await dbHandler.DeleteFromDb(deletedRecipeId, localUser);
        }
    }
}
