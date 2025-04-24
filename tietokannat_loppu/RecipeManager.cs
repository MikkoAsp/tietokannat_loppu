namespace BaseConsoleApp
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using tietokannat_loppu.Entities;

    public abstract class RecipeManager
    {
        protected List<Localrecipe> localRecipes = new();
        protected List<Recipe?> dbRecipes = new();
        public List<Localrecipe> LocalRecipes { get => localRecipes; set => localRecipes = value; }
        protected IAskDetails detailsHelper;
        protected IDatabaseHandler dbHandler;
        public RecipeManager(IAskDetails detailsHelper, IDatabaseHandler databaseHandler)
        {
            this.dbHandler = databaseHandler;
            this.detailsHelper = detailsHelper;
        }
        public void PrintRecipe(List<Recipe?> recipesToPrint)
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
                Console.WriteLine($"ID: {recipe.RecipeId} || Name: {recipe.RecipeName} || Dish: {detailsHelper.FormatEnumDisplayName(recipe.Dish)}{dietString}");

                int ingredientCount = recipe.RecipeIngredients.Count();
                Console.Write("\nIngredients: " + ingredientCount);
                int pointer = 0;
                Console.WriteLine();
                foreach (var ingredient in recipe.RecipeIngredients)
                {
                    pointer++;
                    if (pointer == ingredientCount)
                    {
                        Console.Write($"{ingredient.Ingredient.IngredientName}\n");
                    }
                    else if (pointer == ingredientCount - 1)
                    {
                        Console.Write(ingredient.Ingredient.IngredientName + " and ");
                    }
                    else
                    {
                        Console.Write($"{ingredient.Ingredient.IngredientName}, ");
                    }

                }
                Console.WriteLine("\nIntructions: ");
                int index = 0;
                foreach (var instruction in recipe.Instructions)
                {
                    index++;
                    Console.WriteLine($"{index}. {instruction.CookingInstructions}");
                }
                Console.WriteLine();
            }
        }

        public void ShowAllRecipes(LocalUser user)
        {
            var dbRecipes = dbHandler.LoadAllRecipesFromDb(user).Result;

            if (dbRecipes == null || dbRecipes.Count == 0)
            {
                Console.WriteLine("No recipes added to database");
                return;
            }
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("                      ALL RECIPES");
            Console.WriteLine("------------------------------------------------------------");

            PrintRecipe(dbRecipes);
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
                await dbHandler.SaveRecipesToDatabaseAsync(recipe, user);

                break;
            }
        }
        public abstract void SearchRecipesByIngredients(LocalUser localUser);
        public abstract void SearchRecipesByDishes(LocalUser localUser);
        public abstract void UpdateRecipe();
        public abstract void SearchRecipesByDiets(LocalUser localUser);
        public abstract Task DeleteRecipeWithId(LocalUser localUser);
    }
}
