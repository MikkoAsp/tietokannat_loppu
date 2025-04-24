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
          var recipeToUpdate = dbContext.Recipes
                .Where(x => x.UserId == localUser.Id)
                .FirstOrDefault(p => p.RecipeId == recipeId);
            if (recipeToUpdate != null)
            {
                Console.WriteLine("Updating recipe: " + recipeToUpdate.RecipeName);
                Console.ReadLine();

                string newRecipeName = helper.AskString("Enter new recipe name: ");
                Diet? newDiet = (Diet?)helper.SelectEnumOption<Diet>(0);
                if (newDiet == null) 
                {
                    Console.WriteLine("Invalid diet option. Please try again.");
                    return;
                }
                Dish? newDish = (Dish?)helper.SelectEnumOption<Dish>(0);
                if (newDish == null)
                {
                    Console.WriteLine("Invalid dish option. Please try again.");
                    return;
                }

                recipeToUpdate.RecipeName = newRecipeName;
                recipeToUpdate.Diet = (Diet)newDiet;
                recipeToUpdate.Dish = (Dish)newDish;


                dbContext.Recipes.Update(recipeToUpdate);
                await dbContext.SaveChangesAsync();
                Console.WriteLine("Item updated successfully");
            }
            else
            {
                Console.WriteLine("Couldn't find recipe with your id");
                Console.ReadLine();
            }
        }

        public async Task DeleteFromDb(int recipeId, LocalUser localUser)
        {
            var itemToDelete = await dbContext.Recipes.Where(x => x.UserId == localUser.Id).FirstOrDefaultAsync(p => p.RecipeId == recipeId);

            if(itemToDelete != null)
            {
                Console.WriteLine("Deleting recipe: " + itemToDelete.RecipeName);
                Console.ReadLine();

                dbContext.Recipes.Remove(itemToDelete);
                await dbContext.SaveChangesAsync();
                Console.WriteLine("Item deleted successfully");
                Console.ReadLine();

            }
            else
            {
                Console.WriteLine("Couldn't find recipe with your id");
                Console.ReadLine();
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
