using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseConsoleApp
{
    public class RecipeHandlingManager : RecipeManager
    {
        public RecipeHandlingManager(IDatabaseHandler handler, IAskDetails helper) : base(helper, handler)
        {
        }
        public override List<Recipe> SearchRecipesByIngredients(List<string> searchedIngredients)
        {
            return allRecipes;
        }
        public override void SearchRecipesByDishes()
        {
            Dish? dishOption = (Dish?)detailsHelper.SelectEnumOption<Dish>(0);
        }
        public override void SearchRecipesByDiets()
        {
            Diet? dietOption = (Diet?)detailsHelper.SelectEnumOption<Diet>(0);
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
        public override void DeleteRecipeWithId()
        {
            //Recipe id user wishes to delete
            int deletedRecipeId = detailsHelper.AskIntNumber("Enter the recipe id you wish to delete: ");

            /*
             Todo: deletion logic here
             */
        }
    }
}
