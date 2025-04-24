using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using tietokannat_loppu.Entities;

namespace BaseConsoleApp
{
    public class DatabaseManager : IDatabaseHandler
    {
        TietokannatLoppuContext dbContext;
        IAskDetails helper;
        public DatabaseManager(TietokannatLoppuContext contex, IAskDetails helper)
        {
            dbContext = contex;
            this.helper = helper;
        }
        public async Task<User> AddUserToDb(string email, string password, string username)
        {
            var userToSave = new User
            {
                Username = username,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                Password = password
            };

            dbContext.Users.Add(userToSave);
            await dbContext.SaveChangesAsync();
            Console.WriteLine("New user created with id " + userToSave.UserId);
            Console.ReadLine();
            return userToSave;
        }
        public async Task<List<Recipe>?> LoadAllRecipesFromDb(LocalUser user)
        {
            Console.WriteLine("Looking for recipes with id: " + user.Id);

            var listOfRecipes = await dbContext.Recipes
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredients => recipeIngredients.Ingredient)
                .Include(recipe => recipe.Instructions)
                .Where(recipe => recipe.UserId == user.Id)
                .ToListAsync();

            foreach(var item in listOfRecipes)
            {
                item.Instructions = item.Instructions.OrderBy(instruction => instruction.Step).ToList();
            }
            return listOfRecipes;
        }
        public async Task SaveRecipesToDatabaseAsync(Localrecipe localrecipe, LocalUser localUser)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Email == localUser.Email);

            if(user != null)
            {
                var recipeToSave = new Recipe
                {
                    RecipeName = localrecipe.Name,
                    Dish = localrecipe.Dish,
                    Diet = localrecipe.Diet,
                    CreatedAt = DateTime.UtcNow,
                    UserId = user.UserId
                };

                // Save recipe first
                dbContext.Recipes.Add(recipeToSave);
                await dbContext.SaveChangesAsync();

                int step = 1;

                foreach (var instructionText in localrecipe.Instructions)
                {
                    var instruction = new Instruction
                    {
                        CookingInstructions = instructionText,
                        Step = step++,
                        RecipeId = recipeToSave.RecipeId
                    };
                    dbContext.Instructions.Add(instruction);
                }

                foreach (var ingredientName in localrecipe.Ingredients)
                {
                    var ingredient = await dbContext.Ingredients
                        .FirstOrDefaultAsync(i => i.IngredientName == ingredientName);
                    if (ingredient == null)
                    {
                        ingredient = new Ingredient { IngredientName = ingredientName };
                        dbContext.Ingredients.Add(ingredient);
                        await dbContext.SaveChangesAsync(); // Save to get ID
                    }

                    var recipeIngredient = new RecipeIngredient
                    {
                        RecipeId = recipeToSave.RecipeId,
                        IngredientId = ingredient.IngredientId,
                        Quantity = 1,
                        UnitType = "pcs"
                    };
                    dbContext.RecipeIngredients.Add(recipeIngredient);
                }

                await dbContext.SaveChangesAsync(); // Save instructions and ingredients
            }
            else
            {
                Console.WriteLine("Could find your user!");
            }
        }

        public async Task UpdateRecipeInDb(int recipeId, LocalUser localUser)
        {
          var recipeToUpdate = await dbContext.Recipes
                .Include(recipe => recipe.Instructions)
                .Where(x => x.UserId == localUser.Id)
                .FirstOrDefaultAsync(recipe => recipe.RecipeId == recipeId);

            if (recipeToUpdate != null)
            {
                bool changedData = false;

                Console.WriteLine("\nOld instructions: ");

                foreach (var item in recipeToUpdate.Instructions)
                {
                    Console.WriteLine(item.Step + ". " + item.CookingInstructions);
                }
                while (true)
                {
                    int stepNum = helper.AskIntNumberWithMaxMinRange("Enter instruction number to edit or enter 0 to not edit: ", 0, recipeToUpdate.Instructions.Count);

                    if (stepNum == 0)
                    {
                        break;
                    }
                    else
                    {
                        foreach(var item in recipeToUpdate.Instructions)
                        {
                            if(item.Step == stepNum)
                            {
                                string newText = helper.AskString("New instructions for step " + item.Step + ": ");
                                item.CookingInstructions = newText;
                                Console.WriteLine("Item edited edit another?");
                            }
                        }
                    }
                }

                Console.WriteLine("\nOld recipe name: " + recipeToUpdate.RecipeName);
                string answer = helper.AskString("Change name? (y/n): ").ToLower();

                if(answer == "y")
                {
                    changedData = true;
                    string newRecipeName = helper.AskString("\nEnter new recipe name: ");
                    recipeToUpdate.RecipeName = newRecipeName;
                }
               
                Console.WriteLine("\nOld recipe Diet: " + recipeToUpdate.Diet);

                answer = helper.AskString("Change diet? (y/n): ").ToLower();

                if(answer == "y")
                {
                    Diet? newDiet = (Diet?)helper.SelectEnumOption<Diet>(0);
                    if (newDiet == null)
                    {
                        Console.WriteLine("0 Entered -> Returning back to menu.");
                        return;
                    }
                    changedData = true;
                    recipeToUpdate.Diet = (Diet)newDiet;
                }
                
                Console.WriteLine("\nOld recipe dish: " + recipeToUpdate.Dish);
                answer = helper.AskString("Change dish? (y/n): ").ToLower();

                if(answer == "y")
                {
                    Dish? newDish = (Dish?)helper.SelectEnumOption<Dish>(0);
                    if (newDish == null)
                    {
                        Console.WriteLine("Invalid dish option. Please try again.");
                        return;
                    }
                    changedData = true;
                    recipeToUpdate.Dish = (Dish)newDish;
                }

                if (changedData)
                {
                    dbContext.Recipes.Update(recipeToUpdate);
                    await dbContext.SaveChangesAsync();
                    Console.WriteLine("Item updated successfully");
                }
                else
                {
                    Console.WriteLine("Nothing changed. Returning to menu.");
                }
            }
            else
            {
                Console.WriteLine("Couldn't find recipe with your id");
            }
        }

        public async Task DeleteFromDb(int recipeId, LocalUser localUser)
        {
            var itemToDelete = await dbContext.Recipes.Where(x => x.UserId == localUser.Id).FirstOrDefaultAsync(p => p.RecipeId == recipeId);

            if(itemToDelete != null)
            {
                string answer = helper.AskString("You are about to delete recipe: " + itemToDelete.RecipeName + " is this fine? (y/n) :").ToLower();

                if(answer == "y")
                {
                    dbContext.Recipes.Remove(itemToDelete);
                    await dbContext.SaveChangesAsync();
                    Console.WriteLine("Item deleted successfully");
                }
                else
                {
                    Console.WriteLine("Returning back to menu...");
                }
            }
            else
            {
                Console.WriteLine("Couldn't find recipe with your id");
            }
        }

        public async Task<List<Recipe>?> LoadFromDatabaseByDish(LocalUser user, Dish dish)
        {
            var results = await dbContext.Recipes
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredients => recipeIngredients.Ingredient)
                .Include(recipe => recipe.Instructions)
                .Where(recipe => recipe.UserId == user.Id && recipe.Dish == dish)
                .ToListAsync();

            return results;
        }
        public async Task<List<Recipe>?> LoadFromDatabaseByDiet(LocalUser user, Diet diet)
        {
            var results = await dbContext.Recipes
            .Include(recipe => recipe.RecipeIngredients)
            .ThenInclude(recipeIngredients => recipeIngredients.Ingredient)
            .Include(recipe => recipe.Instructions)
            .Where(recipe => recipe.UserId == user.Id && recipe.Diet == diet)
            .ToListAsync();

            return results;
        }

        public async Task<List<Recipe>?> LoadFromDatabaseByIngredients(LocalUser user, List<string> ingredients)
        {
            var results = await dbContext.Recipes
            .Include(recipe => recipe.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .Include(recipe => recipe.Instructions)
            .Where(recipe => recipe.UserId == user.Id &&
            recipe.RecipeIngredients.Any(ri => ingredients.Contains(ri.Ingredient.IngredientName)))
            .ToListAsync();

            return results;
        }
    }
}
