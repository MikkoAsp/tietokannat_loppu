namespace BaseConsoleApp
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using tietokannat_loppu.Entities;

    public abstract class RecipeManager
    {
        protected List<Localrecipe> allRecipes = new();
        public List<Localrecipe> AllRecipes { get => allRecipes; set => allRecipes = value; }
        protected IAskDetails detailsHelper;
        protected IDatabaseHandler databaseHandler;
        public RecipeManager(IAskDetails detailsHelper, IDatabaseHandler databaseHandler)
        {
            this.databaseHandler = databaseHandler;
            this.detailsHelper = detailsHelper;
        }
        public void PrintRecipe(List<Localrecipe> recipesToPrint)
        {
            if (recipesToPrint.Count() == 0)
            {
                Console.WriteLine("No recipes found");
                return;
            }
            Console.WriteLine("Amount of recipes: " + recipesToPrint.Count());

            foreach (var recipe in recipesToPrint)
            {
                string dietString = recipe.Diet != Diet.None ? $" || Diet: {detailsHelper.FormatEnumDisplayName(recipe.Diet)}" : "";
                Console.WriteLine($"Name: {recipe.Name} || Dish: {detailsHelper.FormatEnumDisplayName(recipe.Dish)}{dietString}");

                int ingredientCount = recipe.Ingredients.Count();
                Console.Write("\nIngredients: " + ingredientCount);
                int pointer = 0;
                Console.WriteLine();
                foreach (var ingredient in recipe.Ingredients)
                {
                    pointer++;
                    if (pointer == ingredientCount)
                    {
                        Console.Write($"{ingredient}\n");
                    }
                    else if (pointer == ingredientCount - 1)
                    {
                        Console.Write(ingredient + " ja ");
                    }
                    else
                    {
                        Console.Write($"{ingredient}, ");
                    }

                }
                Console.WriteLine("\nIntructions: ");
                int index = 0;
                foreach (var instruction in recipe.Instructions)
                {
                    index++;
                    Console.WriteLine($"{index}. {instruction}");
                }
                Console.WriteLine();
            }
        }

        public void ShowAllRecipes(LocalUser user)
        {
            //Todo: load recipes from db and save them to a list of allRecipes

            var recipes = databaseHandler.LoadFromDatabase(user);

            foreach (var recipe in recipes.Result)
            {
                Console.WriteLine(recipe.RecipeName);
            }


            Console.ReadLine();
            Console.WriteLine();
            if (allRecipes.Count() == 0)
            {
                Console.WriteLine("No recipes added.");
                return;
            }
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("                      ALL RECIPES");
            Console.WriteLine("------------------------------------------------------------");

            PrintRecipe(allRecipes);
            Console.WriteLine("------------------------------------------------------------");
        }
        public async Task AddNewRecipe(LocalUser user)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("                      ADD NEW RECIPE");
                Console.WriteLine("------------------------------------------------------------");

                string recipeName = detailsHelper.AskString("Enter recipe name: ");
                Dish? dish = (Dish?)detailsHelper.SelectEnumOption<Dish>(0);
                if (dish == null)
                {
                    Console.WriteLine("Returning Back To Menu...");
                    break;
                }
                List<string> addedIngredients = detailsHelper.AskRecipeIngredients();
                List<string> addedInstructions = detailsHelper.AskRecipeInstructions();
                Diet? diet = (Diet?)detailsHelper.SelectEnumOption<Diet>(0);
                if (diet == null)
                {
                    Console.WriteLine("Returning Back To Menu...");
                    break;
                }

                Localrecipe recipe = new Localrecipe(recipeName, (Dish)dish, addedIngredients, addedInstructions, (Diet)diet);
                detailsHelper.InfoUser(recipe);

                //TODO: save recipe to database
                await databaseHandler.SaveRecipesToDatabaseAsync(recipe, user);

                break;
            }
        }
        public abstract List<Localrecipe> SearchRecipesByIngredients(List<string> searchedIngredients);
        public abstract void SearchRecipesByDishes();
        public abstract void UpdateRecipe();
        public abstract void SearchRecipesByDiets();
        public abstract Task DeleteRecipeWithId();
    }
}
