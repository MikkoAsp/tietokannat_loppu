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
        public DatabaseManager(TietokannatLoppuContext contex)
        {
            dbContext = contex;
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
        public async Task<List<Recipe>?> LoadFromDatabase(LocalUser user)
        {
            Console.WriteLine("Looking for recipes with id: " + user.Id);
            var list = await dbContext.Recipes.Where(x => x.UserId == user.Id).Select(x => new
            {
                RecipeName = x.RecipeName,
                Dish = x.Dish,
                Diet = x.Diet,
                Ingredients = x.RecipeIngredients.Select(x => x.Ingredient.IngredientName).ToList(),
                Instructions = x.Instructions.OrderBy(x => x.Step).ToList()
            }).ToListAsync();


            foreach(var item in list)
            {
                Console.WriteLine("Name: " + item.RecipeName);
                Console.WriteLine("Diet:" + item.Diet);
                Console.WriteLine("Dish: " + item.Dish);
                Console.WriteLine("Ingredients: " + item.Ingredients.Count);

                foreach(var ing in item.Ingredients)
                {
                    Console.WriteLine(ing);
                }
                Console.WriteLine("Instructions: ");
                foreach(var ins in item.Instructions)
                {
                    Console.WriteLine("Step: " + ins.Step);
                    Console.WriteLine("-" + ins.CookingInstructions);
                }
                Console.WriteLine();
            }
            Console.ReadLine();
            return null;
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


        public void UpdateRecipeInDatabase(LocalUser localUser)
        {
          
        }

        public async Task DeleteFromDb(int recipeId)
        {
            var itemToDelete = await dbContext.Recipes.FirstOrDefaultAsync(p => p.RecipeId == recipeId);

            Console.WriteLine("Deleting recipe: " + itemToDelete.RecipeName);
            Console.ReadLine();

            if(itemToDelete != null)
            {
                dbContext.Recipes.Remove(itemToDelete);
                dbContext.SaveChanges();
                Console.WriteLine("Item deleted");
                Console.ReadLine();
            }

        }
    }
}
